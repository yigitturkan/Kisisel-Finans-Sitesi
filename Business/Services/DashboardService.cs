using Core.IServices;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.Services
{
    public class DashboardService : IDashboardService
    {
        // GetUserAsync: Kullanıcının adını soyadını getirmek için kullanılır
        public async Task<dynamic> GetUserAsync(string userId)
        {
            using (var _db = new ApplicationDbContext())
            {
                // AspNetUsers tablosundan kullanıcıyı bulur
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                return user;
            }
        }

        // GetTotalIncomeAsync: Kullanıcının toplam gelirini hesaplar
        public async Task<decimal> GetTotalIncomeAsync(string userId)
        {
            using (var _db = new ApplicationDbContext())
            {
                // UserId eşleşen ve silinmemiş gelirleri bulup miktarını toplar
                var total = await _db.Incomes
                    .Where(x => x.UserId == userId && !x.IsDeleted)
                    .SumAsync(x => (decimal?)x.Amount); // decimal? kullanarak null gelirse hata almasını engelliyoruz

                return total ?? 0; // Eğer hiç gelir yoksa 0 döner
            }
        }

        // GetTotalExpenseAsync: Kullanıcının toplam giderini hesaplar
        public async Task<decimal> GetTotalExpenseAsync(string userId)
        {
            using (var _db = new ApplicationDbContext())
            {
                // UserId eşleşen ve silinmemiş giderleri bulup miktarını toplar
                var total = await _db.Expenses
                    .Where(x => x.UserId == userId && !x.IsDeleted)
                    .SumAsync(x => (decimal?)x.Amount);

                return total ?? 0; // Eğer hiç gider yoksa 0 döner
            }
        }

        // GetTopExpenseCategoryAsync: En çok harcama yapılan kategoriyi döner
        public async Task<string> GetTopExpenseCategoryAsync(string userId)
        {
            using (var _db = new ApplicationDbContext())
            {
                var topCategory = await _db.Expenses
                    .Where(x => x.UserId == userId && !x.IsDeleted && x.Category != null)
                    .GroupBy(x => x.Category)
                    .OrderByDescending(g => g.Sum(x => x.Amount))
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync();

                return topCategory ?? "Henüz veri yok";
            }
        }
    }
}