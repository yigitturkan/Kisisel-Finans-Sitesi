using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountName { get; set; } // Örn: Maaş Hesabı, Birikim
        public decimal Balance { get; set; } // Mevcut Bakiye
        public bool IsIncludedInTotal { get; set; } // "Ana paraya eklensin mi?" butonu

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int BankId { get; set; }
        public virtual Bank Bank { get; set; }
    }
}
