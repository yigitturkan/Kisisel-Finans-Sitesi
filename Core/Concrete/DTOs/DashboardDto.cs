using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.DTOs
{
    public class DashboardDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }

        // View'da hata veren eksik kısımlar:
        public int RecentTransactionCount { get; set; }
        public string TopExpenseCategory { get; set; }
        public List<RecentTransactionDto> RecentTransactions { get; set; } = new List<RecentTransactionDto>();
    }

    public class RecentTransactionDto
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // "Gelir" veya "Gider"
        public DateTime Date { get; set; }
    }
}
