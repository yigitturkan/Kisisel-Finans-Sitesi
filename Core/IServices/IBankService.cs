using Core.Concrete.Entities;
using System.Collections.Generic;

namespace Core.IServices
{
    public interface IBankService
    {
        void AddBank(Bank bank);
        void AddBankAccount(BankAccount account);
        void AddCreditCard(CreditCard card);
        List<Bank> GetUserBanks(string userId);
        void DeleteBank(int id);
        void DeleteBankAccount(int id);
        void DeleteCreditCard(int id);
        BankAccount GetAccountById(int id);
        void UpdateAccount(BankAccount account);
        CreditCard GetCreditCardById(int id);
        void UpdateCreditCard(CreditCard card);
    }
}