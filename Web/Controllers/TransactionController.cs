using Business.Services;
using Core.Concrete.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly TransactionService _service;
        private readonly IncomeService _incomeService;
        private readonly ExpenseService _expenseService;

        public TransactionController()
        {
            var context = new Data.Context.ApplicationDbContext();

            _service = new TransactionService(context);

            _incomeService = new IncomeService();
            _expenseService = new ExpenseService();
        }

        // =========================
        // GELİR LİSTESİ
        // =========================
        public ActionResult Incomes()
        {
            var userId = User.Identity.GetUserId();
            var context = new Data.Context.ApplicationDbContext();

            var incomes = context.Incomes
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            var user = context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                string fullName = (user.FirstName + " " + user.LastName).Trim();

                ViewBag.UserName =
                    !string.IsNullOrEmpty(fullName)
                    ? fullName
                    : user.UserName;

                ViewBag.ProfileImage = user.ProfilePicturePath;
            }

            return View(incomes);
        }

        // =========================
        // GİDER LİSTESİ
        // =========================
        public ActionResult Expenses()
        {
            var userId = User.Identity.GetUserId();
            var context = new Data.Context.ApplicationDbContext();

            var expenses = context.Expenses
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            var user = context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                string fullName = (user.FirstName + " " + user.LastName).Trim();

                ViewBag.UserName =
                    !string.IsNullOrEmpty(fullName)
                    ? fullName
                    : user.UserName;

                ViewBag.ProfileImage = user.ProfilePicturePath;
            }

            return View(expenses);
        }

        // =========================
        // GELİR EKLE - GET
        // =========================
        public ActionResult AddIncome()
        {
            var userId = User.Identity.GetUserId();
            var context = new Data.Context.ApplicationDbContext();

            var user = context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                string fullName = (user.FirstName + " " + user.LastName).Trim();

                ViewBag.UserName =
                    !string.IsNullOrEmpty(fullName)
                    ? fullName
                    : user.UserName;

                ViewBag.ProfileImage = user.ProfilePicturePath;
            }

            return View();
        }

        // =========================
        // GELİR EKLE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddIncome(Income model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.UserId = User.Identity.GetUserId();
                    model.CreatedDate = DateTime.Now;

                    _incomeService.Add(model);

                    TempData["Success"] = "Gelir başarıyla eklendi!";

                    return RedirectToAction("Incomes");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Hata oluştu: " + ex.Message);
                }
            }

            return View(model);
        }

        // =========================
        // GİDER EKLE - GET
        // =========================
        public ActionResult AddExpense()
        {
            var userId = User.Identity.GetUserId();
            var context = new Data.Context.ApplicationDbContext();

            var user = context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                string fullName = (user.FirstName + " " + user.LastName).Trim();

                ViewBag.UserName =
                    !string.IsNullOrEmpty(fullName)
                    ? fullName
                    : user.UserName;

                ViewBag.ProfileImage = user.ProfilePicturePath;
            }

            return View();
        }

        // =========================
        // GİDER EKLE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExpense(Expense model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.UserId = User.Identity.GetUserId();
                    model.CreatedDate = DateTime.Now;

                    _expenseService.Add(model);

                    TempData["Success"] = "Gider başarıyla eklendi!";

                    return RedirectToAction("Expenses");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Hata oluştu: " + ex.Message);
                }
            }

            return View(model);
        }

        // =========================
        // GELİR SİL
        // =========================
        public ActionResult DeleteIncome(int id)
        {
            var context = new Data.Context.ApplicationDbContext();

            var income = context.Incomes
                .FirstOrDefault(x => x.Id == id);

            if (income != null)
            {
                income.IsDeleted = true;

                context.SaveChanges();

                TempData["Success"] = "Gelir başarıyla silindi!";
            }

            return RedirectToAction("Incomes");
        }

        // =========================
        // GİDER SİL
        // =========================
        public ActionResult DeleteExpense(int id)
        {
            var context = new Data.Context.ApplicationDbContext();

            var expense = context.Expenses
                .FirstOrDefault(x => x.Id == id);

            if (expense != null)
            {
                expense.IsDeleted = true;

                context.SaveChanges();

                TempData["Success"] = "Gider başarıyla silindi!";
            }

            return RedirectToAction("Expenses");
        }
    }
}