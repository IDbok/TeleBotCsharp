using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleBot.Models
{
    public class BusinessExpense : Expense
    {
        public override int ExpenseId { get; set; }
        public string? Notes { get; set; }
    }
}
