using Core.Concrete.Entities;
using System.Collections.Generic;

namespace Core.IServices
{
    public interface IPiggyBankService
    {
        void AddMoney(int piggyBankId, decimal amount); // Ana paradan kumbaraya
        void WithdrawMoney(int piggyBankId, decimal amount); // Kumbaradan ana paraya
        List<PiggyBank> GetAll(); // Kumbaraları listeleme
    }
}
