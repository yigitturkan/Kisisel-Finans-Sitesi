using Core.Abstract.IRepositories;
using Core.Concrete.Entities;
using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(ApplicationDbContext context) : base(context) { }

        public List<Expense> GetByUserId(string userId) =>
            _context.Expenses.Where(x => x.UserId == userId && !x.IsDeleted).ToList();

        public decimal GetTotalByUserId(string userId) =>
            _context.Expenses
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Sum(x => (decimal?)x.Amount) ?? 0;
    }
}
