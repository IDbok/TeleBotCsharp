using Telegram.Bot.Types;

namespace TeleBot.Models
{
    public class TextMessage
    {
        public int TextMessageId { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EditDate { get; set; }

        public TextMessage()
        {
            
        }
        public TextMessage(Message message)
        {
            User = new User
            {
                UserId = message.From.Id,
                Username = message.From.Username,
                FirstName = message.From.FirstName,
                LastName = message.From.LastName
            };
            UserId = User.UserId;
            ChatId = message.Chat.Id;
            Text = message.Text;
            Date = message.Date;
            EditDate = message.EditDate;
        }
    }
}
