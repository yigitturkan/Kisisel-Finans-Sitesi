using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class IncomeService
    {
        public void Add(Income model)
        {
            using (var context = new Data.Context.ApplicationDbContext())
            {
                context.Incomes.Add(model);
                context.SaveChanges();
            }
        }
    }
}
