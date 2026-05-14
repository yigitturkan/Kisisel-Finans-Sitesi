using Core.Abstract.Bases;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public TransactionType Type { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
