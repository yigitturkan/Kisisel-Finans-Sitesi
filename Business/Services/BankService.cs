using Core.Concrete.Entities;
using Core.IServices;
using Data.Context;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Business.Services
{
    public class BankService : IBankService
    {
        private readonly ApplicationDbContext _context;
        public BankService(ApplicationDbContext context) { _context = context; }

        public void AddBank(Bank bank)
        {
            _context.Banks.Add(bank);
            _context.SaveChanges();
        }

        public void AddBankAccount(BankAccount account)
        {
            _context.BankAccounts.Add(account);
            if (account.IsIncludedInTotal)
            {
                _context.Incomes.Add(new Income
                {
                    UserId = account.UserId,
                    Amount = account.Balance,
                    Description = $"{account.AccountName} Hesap Açılış Bakiyesi",
                    Source = "Banka Hesabı"
                });
            }
            _context.SaveChanges();
        }

        public void AddCreditCard(CreditCard card)
        {
            if (card == null) return;
            if (string.IsNullOrEmpty(card.UserId))
            {
                var bank = _context.Banks.Find(card.BankId);
                if (bank != null) card.UserId = bank.UserId;
            }
            _context.CreditCards.Add(card);
            _context.SaveChanges();
        }

        public void DeleteBank(int id)
        {
            var bank = _context.Banks.Find(id);
            if (bank != null)
            {
                var accounts = _context.BankAccounts.Where(x => x.BankId == id).ToList();
                _context.BankAccounts.RemoveRange(accounts);
                var cards = _context.CreditCards.Where(x => x.BankId == id).ToList();
                _context.CreditCards.RemoveRange(cards);
                _context.Banks.Remove(bank);
                _context.SaveChanges();
            }
        }

        public void DeleteBankAccount(int id)
        {
            var account = _context.BankAccounts.Find(id);
            if (account != null) { _context.BankAccounts.Remove(account); _context.SaveChanges(); }
        }

        public void DeleteCreditCard(int id)
        {
            var card = _context.CreditCards.Find(id);
            if (card != null) { _context.CreditCards.Remove(card); _context.SaveChanges(); }
        }

        public BankAccount GetAccountById(int id) => _context.BankAccounts.Find(id);
        public CreditCard GetCreditCardById(int id) => _context.CreditCards.Find(id);

        public List<Bank> GetUserBanks(string userId) =>
            _context.Banks
                .Where(x => x.UserId == userId)
                .Include(x => x.BankAccounts)
                .Include(x => x.CreditCards)
                .ToList();

        public void UpdateAccount(BankAccount account)
        {
            _context.Entry(account).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void UpdateCreditCard(CreditCard card)
        {
            _context.Entry(card).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}