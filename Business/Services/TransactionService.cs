using Core.Concrete.Entities;
using Core.IServices;
using Data.Context;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction t)
        {
            _context.Transactions.Add(t);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalIncome(string userId)
        {
            return await _context.Transactions
                .Where(x => x.UserId == userId
                         && x.Type == TransactionType.Income)
                .SumAsync(x => x.Amount);
        }

        public async Task<decimal> GetTotalExpense(string userId)
        {
            return await _context.Transactions
                .Where(x => x.UserId == userId
                         && x.Type == TransactionType.Expense)
                .SumAsync(x => x.Amount);
        }
    }
}
