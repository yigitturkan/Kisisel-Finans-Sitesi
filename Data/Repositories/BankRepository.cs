using Core.Abstract.Repositories;
using Core.Concrete.Entities;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class BankRepository : Repository<Bank>, IBankRepository
    {
        public BankRepository(ApplicationDbContext context) : base(context) { }

        public List<Bank> GetUserBanksWithDetails(string userId) =>
            _context.Banks
                .Where(x => x.UserId == userId)
                .Include(x => x.BankAccounts)
                .Include(x => x.CreditCards)
                .ToList();
    }
}
