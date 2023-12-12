using System.ComponentModel.DataAnnotations.Schema;

namespace TeleBot.Models
{
    public class Currency
    {
        public const int defaultCurrencyId = 3;
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Expense> Expenses { get; set; }
        // todo - add currency symbols
        // todo - add currency to db
        [NotMapped]
        public static Dictionary<int, List<string>> CurrencyCodeDic = new Dictionary<int, List<string>> 
        {
            { 1, new List<string> { "rub","рубль", "руб", "руб.", "рублей", "рубля", "рублей", "р", "р." } },
            { 2, new List<string> { "usd","доллар", "долларов", "доллара", "доллара", "долларов", "дол", "дол." } },
            { 3, new List<string> { "gel", "лар", "лари", "ларри", "л", "л.", "g" } }
        };
        public static bool GetCurrencyId(string cur, out int curId) 
        
        {
            curId = -1;
            // check if cur is in CurrencyCodeDic
            foreach (var item in CurrencyCodeDic)
            {
                if (item.Value.Contains(cur.ToLower()))
                {
                    curId = item.Key;
                    return true;
                }
            }
            return false;
        }
    }
}
