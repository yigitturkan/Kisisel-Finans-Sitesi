using Core.Abstract.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Entities
{
    public class Expense : BaseEntity
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
