using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IDashboardService
    {
        Task<dynamic> GetUserAsync(string userId);
        Task<decimal> GetTotalIncomeAsync(string userId);
        Task<decimal> GetTotalExpenseAsync(string userId);
    }
}
