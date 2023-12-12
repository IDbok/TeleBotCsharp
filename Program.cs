using System;
using System.Runtime.Intrinsics.Arm;
using System.Threading;
using TeleBot.DbCommunication;
using TeleBot.DbCommunication.MySqlConnector;
using TeleBot.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static TeleBot.DbCommunication.DbProcessing;

namespace TeleBot
{
    internal class Program
    {
        private static List<string> output = new();
        static ITelegramBotClient _botClient;

        static async Task Main() //static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            OutputMessage mes = (string somemessage) => GetOutput(somemessage, ref output); //SendOutputMessage(string somemessage, ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)(somemessage, ref outputMessage);
            DbProcessing.OutputMessageHendler(mes);//; outputMessage = (string somemessage) => Console.WriteLine(somemessage);//;

            string token = ""; // Замените на токен вашего бота
            _botClient = new TelegramBotClient(token);

            using (var db = new MyBotDbContext())
            {
                db.Database.EnsureCreated();
            }

            using var cts = new CancellationTokenSource();

            // Настройка приемника обновлений
            var receiverOptions = new ReceiverOptions
            {
                //AllowedUpdates = Array.Empty<UpdateType>() // Получать все типы обновлений
                AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
                {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                UpdateType.EditedMessage, // Измененные сообщения
                UpdateType.CallbackQuery // Inline кнопки (добавляются в UpdateHandler)
                },
                // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
                // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
                ThrowPendingUpdates = true,
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.WriteLine("Press any key to exit" );

            //await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно

            Console.ReadLine();
            cts.Cancel();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                // Проверяем, есть ли сообщение
                // todo - create ability to get message from callbackquery
                // todo - create ability to get edited message and handle it
                if (update.Type == UpdateType.EditedMessage) Console.WriteLine($"Edited message({update.EditedMessage.MessageId}): {update.EditedMessage.Text}");
                
                //await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, response, replyMarkup: rkm);
                if (update.Type != UpdateType.Message) return;
                if (update.Message.Type != MessageType.Text) return;
                var updateType = false;
                var message = update.Message;
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;
                
                var textMessage = new Models.TextMessage(message);
                var user = textMessage.User;

                output.Clear();
                DbProcessing.AddMessage(textMessage);
                
                // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                Console.WriteLine($"{user.FirstName} ({user.UserId}) написал сообщение: {messageText}");
                
                // Отправка ответного сообщения
                await botClient.SendTextMessageAsync(
                    chatId,
                    string.Join("\n",output), 
                    //replyToMessageId: update.Message.MessageId, //  отвечает за "ответ" на сообщение
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        private static void GetOutput(string somemessage, ref List<string> output) 
        {
            output.Add(somemessage);
        }
    }
}