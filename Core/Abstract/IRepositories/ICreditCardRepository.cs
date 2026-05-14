using Core.Concrete.Entities;
using System.Collections.Generic;

namespace Core.Abstract.Repositories
{
    public interface ICreditCardRepository : IRepository<CreditCard>
    {
        List<CreditCard> GetByBankId(int bankId);
        List<CreditCard> GetByUserId(string userId);
    }

}
