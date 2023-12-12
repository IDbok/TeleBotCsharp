using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TeleBot.DbCommunication;
using TeleBot.Models;
using Telegram.Bot.Types;

namespace TeleBot
{
    public class DataProcessing
    {
        public static bool CheckHashTag(ref List<string> messageInList, out string hashTag) 
        {
            hashTag = "";
            char specificChar = '#';

            int index = messageInList.FindIndex(s => s.Contains(specificChar)); // -1 if not found

            if (index != -1)
            {
                hashTag = messageInList[index].Replace(specificChar.ToString(), string.Empty);
                messageInList.RemoveAt(index);
                return true;
            }

            return false;
        }
        private static bool CheckCurrency(ref List<string> messageInList, out int currencyId)
        {
            currencyId = -1;
            
            for (int i = 0;i < messageInList.Count; i++)
            {
                if (Currency.GetCurrencyId(messageInList[i], out currencyId))
                {
                    messageInList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public static bool CheckCategory(ref List<string> messageInList, out int categoryId)
        {
            categoryId = -1;
            for (int i = 0; i < messageInList.Count; i++)
            {
                if (Category.GetCategoryId(messageInList[i], out categoryId))
                {
                    messageInList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public static bool CheckIfExpense(TextMessage message, out Expense expense)
        {
            expense = new PersonalExpense();

            double sum = 0;
            int  curId, catId;
            string text;

            var messageInList = message.Text.Split(' ').ToList();
            messageInList.RemoveAll(string.IsNullOrWhiteSpace);
            if (messageInList.Count < 2) return false;

            CheckHashTag(ref messageInList, out string hashTag);
            text = string.Join(" ", messageInList);
            if (hashTag == "салон")
            {  expense = new BusinessExpense(); expense.Bisness = 1; }

            int index = messageInList.FindIndex(s => double.TryParse(s, out sum));
            if (index == -1) return false;
            messageInList.RemoveAt(index);

            CheckCurrency(ref messageInList, out curId);
            CheckCategory(ref messageInList, out catId);
            //var random = new Random();
            //expense.UserId = random.Next(1, 10000000);
            expense.User = message.User;
            expense.UserId = message.User.UserId;
            expense.Sum = sum;
            expense.CurrencyId = curId != -1 ? curId : Currency.defaultCurrencyId;
            expense.CategoryId = catId != -1 ? catId : Category.defaultCategoryId;
            expense.Description = text;
            expense.Date = message.Date;

            return true;

        }
        
        public static void AddExpense(TextMessage message)
        {

            //using (var db = new MyBotDbContext())
            //{
            //    var expense = new Expense
            //    {
            //        Date = message.Date,
            //        Sum = 0,
            //        CurrencyId = Currency.GetCurrencyId(message),
            //        CategoryId = Category.Food,
            //        Description = "test"
            //    };
            //    db.Expenses.Add(expense);
            //    db.SaveChanges();
            //}
        }
    }
}
