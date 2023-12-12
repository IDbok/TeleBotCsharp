using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBot.Models
{
    public class PersonalExpense : Expense
    {
        public override int ExpenseId { get; set; }
    }
}
