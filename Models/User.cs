namespace TeleBot.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<TextMessage> Messages { get; set; } = new();
        //public List<Expense> Expenses { get; set; } = new();
        public List<PersonalExpense> PersonalExpenses { get; set; } = new();
        public List<BusinessExpense> BusnessExpenses { get; set; } = new();
    }
}
