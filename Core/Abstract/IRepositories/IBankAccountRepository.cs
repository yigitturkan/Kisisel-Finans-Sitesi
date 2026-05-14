using Core.Concrete.Entities;
using System.Collections.Generic;

namespace Core.Abstract.Repositories
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        List<BankAccount> GetByBankId(int bankId);
        List<BankAccount> GetByUserId(string userId);
    }

}
