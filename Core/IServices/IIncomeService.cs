using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IIncomeService
    {
        void Add(Income model);
        List<Income> GetByUserId(string userId);
        decimal GetTotalByUserId(string userId);
        void Delete(int id);
    }

    public interface IExpenseService
    {
        void Add(Expense model);
        List<Expense> GetByUserId(string userId);
        decimal GetTotalByUserId(string userId);
        void Delete(int id);
    }
}
