using Microsoft.EntityFrameworkCore;
//using Telegram.Bot.Types;
using TeleBot.Models;

namespace TeleBot.DbCommunication
{
    public class MyBotDbContext : DbContext
    {
        public delegate void OutputMessage(string message);
        public static OutputMessage? outputMessage;
        public static void OutputMessageHendler(OutputMessage newMethod)
        {
            outputMessage += newMethod;
        }
        public static void SetDbPasswod(string password)
        {
            DbPass = password;
        }
        public static void SetDbVersion(int major, int minor, int patch)
        {
            dbVersion[0] = major;
            dbVersion[1] = minor;
            dbVersion[2] = patch;
        }
        public static void SetDbVersion(string version)
        {
            // create from string version array of int
            string[] versionArray = version.Split('.');
            if (versionArray.Length != 3)
            {
                throw new System.Exception("Wrong version format");
            }
            for (int i = 0; i < versionArray.Length; i++)
            {
                dbVersion[i] = int.Parse(versionArray[i]);
            }
        }
        private static int[] dbVersion = new int[3];
        private static string DbPass;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TextMessage> Messages { get; set; } = null!;

        //public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<PersonalExpense> PersonalExpenses { get; set; } = null!;
        public DbSet<BusinessExpense> BusinessExpenses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;

        public MyBotDbContext()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"server=localhost;user=root;password={DbPass};database=telebot_db_v3;",
                new MySqlServerVersion(new Version(dbVersion[0], dbVersion[1], dbVersion[2])),//8,0,35//5,7,24
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure();
                });
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>().UseTpcMappingStrategy();
        }

    }
    
}
    

