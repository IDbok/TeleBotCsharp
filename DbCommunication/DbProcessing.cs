using TeleBot.Models;

namespace TeleBot.DbCommunication
{
    public static class DbProcessing
    {
        // create an delegate to list of output messages
        public delegate void OutputMessage(string message);
        public static OutputMessage? outputMessage;
        public static void OutputMessageHendler(OutputMessage newMethod)
        {
            outputMessage += newMethod;
        }
        public static void SaveMessageToDB(TextMessage message)
        {
            
            using (var context = new MyBotDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == message.UserId);

                if (user != null)
                {
                    // todo - make authorisation process
                    user.Messages.Add(message);
                }
                else
                {
                    // User does not exist, create new user and add message
                    user = new User
                    {
                        UserId = message.UserId,
                        Username = message.User.Username,
                        FirstName = message.User.FirstName,
                        Messages = new List<TextMessage> { message }
                    };
                    context.Users.Add(user);
                }
                
                context.SaveChanges();
            }
        }
        public static void SaveExpenseToDB(Expense expense)
        {
            using (var context = new MyBotDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == expense.UserId);

                if (user != null)
                {
                    // todo - make authorisation process
                    if (expense.Bisness == 1)
                        user.BusnessExpenses.Add((BusinessExpense)expense);
                    else
                        user.PersonalExpenses.Add((PersonalExpense)expense);
                }
                else
                {
                    // User does not exist, create new user and add message
                    user = new User
                    {
                        UserId = expense.UserId,
                        Username = expense.User.Username,
                        FirstName = expense.User.FirstName,
                    };
                    if (expense.Bisness == 1)
                        user.BusnessExpenses = new List<BusinessExpense> { (BusinessExpense)expense };
                    else
                        user.PersonalExpenses = new List<PersonalExpense> { (PersonalExpense)expense };
                    context.Users.Add(user);
                }

                context.SaveChanges();
                // create message with expense category and sum

                outputMessage?.Invoke( $"Расход записан.\n"+expense.ToString());
            }
        }


        public static void AddMessage(TextMessage message)
        {
            if (DataProcessing.CheckIfExpense(message, out Expense expense))
                SaveExpenseToDB(expense);
            
            SaveMessageToDB(message);
            
        }


    }
}
