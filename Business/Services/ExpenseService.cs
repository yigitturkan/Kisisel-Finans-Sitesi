using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ExpenseService
    {
        public void Add(Expense model)
        {
            using (var context = new Data.Context.ApplicationDbContext())
            {
                context.Expenses.Add(model);
                context.SaveChanges();
            }
        }
    }
}
