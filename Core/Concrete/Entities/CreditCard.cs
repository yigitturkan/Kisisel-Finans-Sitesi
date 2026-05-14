using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Entities
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public decimal CardLimit { get; set; }
        public decimal CurrentDebt { get; set; } // Güncel Borç
        public int ClosingDay { get; set; } // Hesap kesim günü (1-31)
        public DateTime? DueDate { get; set; } // Son ödeme tarihi
        public double InterestRate { get; set; } // Gecikme faizi vb.
        public string UserId { get; set; } // Bunu ekle
        public int BankId { get; set; }
        public virtual Bank Bank { get; set; }
    }
}
