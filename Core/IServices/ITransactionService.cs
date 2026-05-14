using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ITransactionService
    {
        Task AddAsync(Transaction transaction);
        Task<decimal> GetTotalIncome(string userId);
        Task<decimal> GetTotalExpense(string userId);
    }
}
