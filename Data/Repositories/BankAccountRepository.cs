using Core.Abstract.Repositories;
using Core.Concrete.Entities;
using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(ApplicationDbContext context) : base(context) { }

        public List<BankAccount> GetByBankId(int bankId) =>
            _context.BankAccounts.Where(x => x.BankId == bankId).ToList();

        public List<BankAccount> GetByUserId(string userId) =>
            _context.BankAccounts.Where(x => x.UserId == userId).ToList();
    }
}
