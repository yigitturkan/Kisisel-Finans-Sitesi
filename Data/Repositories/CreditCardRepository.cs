using Core.Abstract.Repositories;
using Core.Concrete.Entities;
using Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public class CreditCardRepository : Repository<CreditCard>, ICreditCardRepository
    {
        public CreditCardRepository(ApplicationDbContext context) : base(context) { }

        public List<CreditCard> GetByBankId(int bankId) =>
            _context.CreditCards.Where(x => x.BankId == bankId).ToList();

        public List<CreditCard> GetByUserId(string userId) =>
            _context.CreditCards.Where(x => x.UserId == userId).ToList();
    }
}
