using Core.Abstract.IRepositories;
using Core.Concrete.Entities;
using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class IncomeRepository : Repository<Income>, IIncomeRepository
    {
        public IncomeRepository(ApplicationDbContext context) : base(context) { }

        public List<Income> GetByUserId(string userId) =>
            _context.Incomes.Where(x => x.UserId == userId && !x.IsDeleted).ToList();

        public decimal GetTotalByUserId(string userId) =>
            _context.Incomes
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Sum(x => (decimal?)x.Amount) ?? 0;
    }
}
