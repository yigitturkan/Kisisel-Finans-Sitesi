using Business.Services;
using Core.Concrete.Entities;
using Data.Context;
using System;
using System.Linq;

namespace Business
{
    public class PiggyBankManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IncomeService _incomeService;
        private readonly ExpenseService _expenseService;
        public PiggyBankManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public PiggyBankManager()
        {
            _context = new ApplicationDbContext();
            _incomeService = new IncomeService();
            _expenseService = new ExpenseService();
        }

        public void AddMoney(int piggyId, decimal amount, string userId)
        {
            var piggy = _context.PiggyBanks.Find(piggyId);

            // Senin sisteminde bakiye kontrolü
            var totalIncome = _context.Incomes.Where(x => x.UserId == userId && !x.IsDeleted).Sum(x => (decimal?)x.Amount) ?? 0;
            var totalExpense = _context.Expenses.Where(x => x.UserId == userId && !x.IsDeleted).Sum(x => (decimal?)x.Amount) ?? 0;
            var currentBalance = totalIncome - totalExpense;

            if (piggy != null && currentBalance >= amount)
            {
                // Kumbaraya para atmayı bir 'Expense' (Gider) olarak kaydediyoruz
                var expense = new Core.Concrete.Entities.Expense
                {
                    UserId = userId,
                    Amount = amount,
                    Description = piggy.Name + " Kumbarasına Para Aktarıldı",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                _context.Expenses.Add(expense);
                piggy.CurrentAmount += amount;
                _context.SaveChanges();
            }
        }
        public void WithdrawMoney(int piggyId, decimal amount, string userId)
        {
            var piggy = _context.PiggyBanks.Find(piggyId);
            if (piggy != null && piggy.CurrentAmount >= amount)
            {
                // Kumbaradan para çekmek bir "GELİR"dir (Cebine para giriyor)
                var income = new Income
                {
                    UserId = userId,
                    Amount = amount,
                    Description = piggy.Name + " Kumbarasından Nakit Alım",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                _context.Incomes.Add(income);
                piggy.CurrentAmount -= amount;
                _context.SaveChanges();
            }
        }
    }
}