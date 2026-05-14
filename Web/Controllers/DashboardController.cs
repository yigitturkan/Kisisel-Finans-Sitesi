using Business.Services;
using Core.Concrete.DTOs;
using Data.Context;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;

        public DashboardController()
        {
            _dashboardService = new DashboardService();
        }

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var income = await _dashboardService.GetTotalIncomeAsync(userId);
            var expense = await _dashboardService.GetTotalExpenseAsync(userId);
            var user = await _dashboardService.GetUserAsync(userId);

            // =========================
            // EN ÇOK HARCANAN KATEGORİ
            // =========================

            string topCategory = "Bu ay harcama yapılmamış";

            using (var db = new ApplicationDbContext())
            {
                // Bu ayın giderleri
                var monthlyExpenses = db.Expenses
                    .Where(x =>
                        x.UserId == userId &&
                        !x.IsDeleted &&
                        x.CreatedDate.Month == DateTime.Now.Month &&
                        x.CreatedDate.Year == DateTime.Now.Year)
                    .ToList();

                // Eğer gider varsa kategori hesapla
                if (monthlyExpenses.Any())
                {
                    var topExpenseCategory = monthlyExpenses
                        .GroupBy(x =>
                            string.IsNullOrWhiteSpace(x.Category)
                            ? "Diğer"
                            : x.Category)
                        .Select(g => new
                        {
                            Category = g.Key,
                            Total = g.Sum(x => x.Amount)
                        })
                        .OrderByDescending(x => x.Total)
                        .FirstOrDefault();

                    if (topExpenseCategory != null)
                    {
                        topCategory = topExpenseCategory.Category;
                    }
                }
            }

            // =========================
            // DTO
            // =========================

            var dto = new DashboardDto
            {
                TotalIncome = income,

                TotalExpense = expense,

                Balance = income - expense,

                RecentTransactionCount = 5,

                TopExpenseCategory = topCategory,

                RecentTransactions = new List<RecentTransactionDto>
        {
            new RecentTransactionDto
            {
                Description = "Toplam Gelir",
                Amount = income,
                Type = "Gelir",
                Date = DateTime.Now
            },

            new RecentTransactionDto
            {
                Description = "Toplam Gider",
                Amount = expense,
                Type = "Gider",
                Date = DateTime.Now
            }
        }
            };

            // =========================
            // KULLANICI BİLGİSİ
            // =========================

            if (user != null)
            {
                string fullNm =
                    (user.FirstName + " " + user.LastName).Trim();

                ViewBag.UserName =
                    !string.IsNullOrEmpty(fullNm)
                    ? fullNm
                    : user.UserName;

                ViewBag.ProfileImage =
                    user.ProfilePicturePath;
            }

            return View(dto);
        }
        // GET: /Dashboard/Reports?year=2025&month=4   (month=0 → tüm yıl)
        public ActionResult Reports(int? year, int? month)
        {
            var userId = User.Identity.GetUserId();
            var trCulture = new CultureInfo("tr-TR");

            using (var db = new ApplicationDbContext())
            {
                // ── Kullanıcı adı
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    string fullNm = (user.FirstName + " " + user.LastName).Trim();
                    ViewBag.UserName = !string.IsNullOrEmpty(fullNm) ? fullNm : user.UserName;
                    ViewBag.ProfileImage = user.ProfilePicturePath;
                }

                // ── Tüm veriyi çek (memory'de filtrele — EF date fonksiyonları sorun çıkarabilir)
                var allIncomes = db.Incomes.Where(x => x.UserId == userId && !x.IsDeleted).ToList();
                var allExpenses = db.Expenses.Where(x => x.UserId == userId && !x.IsDeleted).ToList();

                // ── Mevcut yıllar
                var years = allIncomes.Select(x => x.CreatedDate.Year)
                    .Union(allExpenses.Select(x => x.CreatedDate.Year))
                    .Distinct().OrderByDescending(y => y).ToList();
                if (!years.Any()) years.Add(DateTime.Now.Year);

                int selYear = year ?? DateTime.Now.Year;
                int selMonth = month ?? DateTime.Now.Month;

                // ── Seçili dönem filtresi
                var pInc = selMonth == 0
                    ? allIncomes.Where(x => x.CreatedDate.Year == selYear).ToList()
                    : allIncomes.Where(x => x.CreatedDate.Year == selYear && x.CreatedDate.Month == selMonth).ToList();

                var pExp = selMonth == 0
                    ? allExpenses.Where(x => x.CreatedDate.Year == selYear).ToList()
                    : allExpenses.Where(x => x.CreatedDate.Year == selYear && x.CreatedDate.Month == selMonth).ToList();

                // ── Son 12 ay özeti
                var monthly = new List<MonthSummary>();
                var now = DateTime.Now;
                for (int i = 11; i >= 0; i--)
                {
                    var d = now.AddMonths(-i);
                    monthly.Add(new MonthSummary
                    {
                        Year = d.Year,
                        Month = d.Month,
                        MonthLabel = d.ToString("MMM yy", trCulture),
                        TotalIncome = allIncomes.Where(x => x.CreatedDate.Year == d.Year && x.CreatedDate.Month == d.Month).Sum(x => (decimal?)x.Amount) ?? 0,
                        TotalExpense = allExpenses.Where(x => x.CreatedDate.Year == d.Year && x.CreatedDate.Month == d.Month).Sum(x => (decimal?)x.Amount) ?? 0
                    });
                }

                // ── Kategori dağılımı
                decimal totalPeriodExp = pExp.Sum(x => (decimal?)x.Amount) ?? 0;
                var categories = pExp
                    .GroupBy(x => string.IsNullOrWhiteSpace(x.Category) ? "Diğer" : x.Category)
                    .Select(g => new CategorySummary
                    {
                        Category = g.Key,
                        Total = g.Sum(x => x.Amount),
                        Count = g.Count(),
                        Percent = totalPeriodExp > 0 ? Math.Round((double)(g.Sum(x => x.Amount) / totalPeriodExp) * 100, 1) : 0
                    })
                    .OrderByDescending(x => x.Total)
                    .ToList();

                // ── İşlem listesi
                var txList = new List<ReportTransaction>();
                txList.AddRange(pInc.Select(x => new ReportTransaction
                {
                    Date = x.CreatedDate,
                    Description = x.Description ?? "",
                    Category = x.Source ?? "Gelir",
                    Amount = x.Amount,
                    IsIncome = true
                }));
                txList.AddRange(pExp.Select(x => new ReportTransaction
                {
                    Date = x.CreatedDate,
                    Description = x.Description ?? "",
                    Category = string.IsNullOrWhiteSpace(x.Category) ? "Diğer" : x.Category,
                    Amount = x.Amount,
                    IsIncome = false
                }));
                txList = txList.OrderByDescending(x => x.Date).ToList();

                // ── Period label
                string[] monthNames = { "", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
                                         "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
                string periodLabel = selMonth == 0
                    ? $"{selYear} Yılı"
                    : $"{monthNames[selMonth]} {selYear}";

                var vm = new ReportViewModel
                {
                    SelectedYear = selYear,
                    SelectedMonth = selMonth,
                    PeriodIncome = pInc.Sum(x => (decimal?)x.Amount) ?? 0,
                    PeriodExpense = totalPeriodExp,
                    AllTimeIncome = allIncomes.Sum(x => (decimal?)x.Amount) ?? 0,
                    AllTimeExpense = allExpenses.Sum(x => (decimal?)x.Amount) ?? 0,
                    MonthlyHistory = monthly,
                    CategoryBreakdown = categories,
                    Transactions = txList,
                    AvailableYears = years,
                    PeriodLabel = periodLabel
                };

                return View(vm);
            }
        }
    }
}