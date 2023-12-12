using Microsoft.EntityFrameworkCore;
//using Telegram.Bot.Types;
using TeleBot.Models;

namespace TeleBot.DbCommunication
{
    public class MyBotDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TextMessage> Messages { get; set; } = null!;

        //public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<PersonalExpense> PersonalExpenses { get; set; } = null!;
        public DbSet<BusinessExpense> BusinessExpenses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;

        public MyBotDbContext()
        {
            Database.EnsureCreated();
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=telebot_db_v3;",
                new MySqlServerVersion(new Version(5,7,24)));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>().UseTpcMappingStrategy();
        }

    }
    
}
    

