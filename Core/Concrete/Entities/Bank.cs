using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Entities
{
    public class Bank
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string UserId { get; set; } // Hangi kullanıcıya ait olduğu

        // İlişkiler
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<CreditCard> CreditCards { get; set; }
    }
}
