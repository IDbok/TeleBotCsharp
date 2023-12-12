using System.ComponentModel.DataAnnotations.Schema;

namespace TeleBot.Models
{
    public class Category
    {
        public const int defaultCategoryId = 17;
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Expense> Expenses { get; set; }
        [NotMapped]
        public static Dictionary<int, List<string>> CategoryCodeDic = new Dictionary<int, List<string>>
        {
            { 1, new List<string>{"Квартплата" } },
            { 2, new List<string>{"Коммуналка" } },
            { 3, new List<string>{"Дом" } },
            { 4, new List<string>{"Связь" } },
            { 5, new List<string>{"Продукты" } },
            { 6, new List<string>{"Подписки" } },
            { 7, new List<string>{"Спорт" } },
            { 8, new List<string>{"Такси" } },
            { 9, new List<string>{"Развлечения" } },
            { 10, new List<string>{"Одежда" } },
            { 11, new List<string>{"Фастфуд" } },
            { 12, new List<string>{"Здоровье" } },
            { 13, new List<string>{"Подарки" } },
            { 14, new List<string>{"Личное" } },
            { 15, new List<string>{"Благотворительность"} },
            { 16, new List<string>{"Путешествия"} },
            { 17, new List<string>{"Прочее"} }

        };
        public static bool GetCategoryId(string cat, out int categoryId)
        {
            categoryId = -1;
            // todo - fistly check of List of Categories contains string

            foreach (var item in CategoryCodeDic)
            {
                if (item.Value.Contains(FirstCharToUpper(cat)))
                {
                    categoryId = item.Key;
                    return true;
                }
            }
            return false;
        }
        private static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return "тут пусто";//new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
