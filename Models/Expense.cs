namespace TeleBot.Models
{
    public class Expense
    {
        public virtual int ExpenseId { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; } // todo - create method to get date from message if contains "факт: date"
        public double Sum { get; set; }
        public int Bisness { get; set; } = 0; // 0 - personal, 1 - bisness

        public int CurrencyId { get; set; }
        public Currency? Currency { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Description { get; set; }
        //public string Comment { get; set; }
        public override string ToString()
        {
            return $"{Category.CategoryCodeDic[this.CategoryId][0]}: " +
                $"{this.Sum} {Currency.CurrencyCodeDic[this.CurrencyId][0]}";
        }
    }
}
