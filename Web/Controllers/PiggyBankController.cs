using Business;
using Data.Context;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using System;
using Core.Concrete.Entities;

namespace Web.Controllers
{
    [Authorize]
    public class PiggyBankController : Controller
    {
        // 1. HATA BURADA: Bu iki satırın class'ın en üstünde olduğundan emin ol!
        private readonly PiggyBankManager _manager;
        private readonly ApplicationDbContext _context;

        // 2. HATA BURADA: Constructor (Yapıcı Metot) içinde bunları başlatman lazım
        public PiggyBankController()
        {
            _context = new ApplicationDbContext();
            _manager = new PiggyBankManager(_context); // Manager'a context'i gönderiyoruz
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var totalIncome = _context.Incomes.Where(x => x.UserId == userId && !x.IsDeleted).Sum(x => (decimal?)x.Amount) ?? 0;
            var totalExpense = _context.Expenses.Where(x => x.UserId == userId && !x.IsDeleted).Sum(x => (decimal?)x.Amount) ?? 0;

            ViewBag.MainBalance = totalIncome - totalExpense;

            var pikkies = _context.PiggyBanks.Where(x => x.UserId == userId).ToList();
            return View(pikkies);
        }

        [HttpPost]
        public ActionResult Add(int id, decimal amount)
        {
            var userId = User.Identity.GetUserId();
            // Artık _manager burada hata vermez, çünkü yukarıda tanımladık
            _manager.AddMoney(id, amount, userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Withdraw(int id, decimal amount)
        {
            var userId = User.Identity.GetUserId();
            // Geri al (Para çek) metodu
            _manager.WithdrawMoney(id, amount, userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(PiggyBank model)
        {
            model.UserId = User.Identity.GetUserId();
            model.CreatedDate = DateTime.Now;
            _context.PiggyBanks.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}