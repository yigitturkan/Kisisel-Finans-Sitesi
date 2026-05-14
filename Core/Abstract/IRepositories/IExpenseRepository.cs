using Core.Abstract.Repositories;
using Core.Concrete.Entities;
using System.Collections.Generic;

namespace Core.Abstract.IRepositories
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        List<Expense> GetByUserId(string userId);
        decimal GetTotalByUserId(string userId);
    }
}
