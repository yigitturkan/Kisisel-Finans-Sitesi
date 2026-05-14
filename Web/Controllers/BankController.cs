using Business.Services;
using Core.Concrete.Entities;
using Core.IServices;
using Data.Context;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        private readonly IBankService _bankService;

        public BankController()
        {
            var context = new ApplicationDbContext();

            _bankService = new BankService(context);
        }

        // =========================
        // ANA SAYFA
        // =========================
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var banks = _bankService.GetUserBanks(userId);

            using (var db = new ApplicationDbContext())
            {
                // =========================
                // TOPLAM GELİR
                // =========================
                ViewBag.TotalIncome = db.Incomes
                    .Where(x =>
                        x.UserId == userId &&
                        !x.IsDeleted)
                    .Sum(x => (decimal?)x.Amount) ?? 0;

                // =========================
                // TOPLAM GİDER
                // =========================
                ViewBag.TotalExpense = db.Expenses
                    .Where(x =>
                        x.UserId == userId &&
                        !x.IsDeleted)
                    .Sum(x => (decimal?)x.Amount) ?? 0;

                // =========================
                // KULLANICI BİLGİSİ
                // =========================
                var user = db.Users
                    .FirstOrDefault(x => x.Id == userId);

                if (user != null)
                {
                    string fullName =
                        (user.FirstName + " " + user.LastName).Trim();

                    ViewBag.UserName =
                        !string.IsNullOrEmpty(fullName)
                        ? fullName
                        : user.UserName;

                    ViewBag.ProfileImage =
                        user.ProfilePicturePath;
                }
            }

            return View(banks);
        }

        // =========================
        // BANKA EKLE
        // =========================
        [HttpPost]
        public ActionResult AddBank(Bank bank)
        {
            bank.UserId = User.Identity.GetUserId();

            _bankService.AddBank(bank);

            return RedirectToAction("Index");
        }

        // =========================
        // HESAP EKLE
        // =========================
        [HttpPost]
        public ActionResult AddAccount(BankAccount account)
        {
            account.UserId = User.Identity.GetUserId();

            _bankService.AddBankAccount(account);

            return RedirectToAction("Index");
        }

        // =========================
        // KREDİ KARTI EKLE
        // =========================
        [HttpPost]
        public ActionResult AddCreditCard(CreditCard card)
        {
            if (card.BankId > 0)
            {
                card.UserId = User.Identity.GetUserId();

                _bankService.AddCreditCard(card);
            }

            return RedirectToAction("Index");
        }

        // =========================
        // BANKA SİL
        // =========================
        [HttpPost]
        public ActionResult DeleteBank(int id)
        {
            _bankService.DeleteBank(id);

            return RedirectToAction("Index");
        }

        // =========================
        // HESAP SİL
        // =========================
        [HttpPost]
        public ActionResult DeleteAccount(int id)
        {
            _bankService.DeleteBankAccount(id);

            return RedirectToAction("Index");
        }

        // =========================
        // KART SİL
        // =========================
        [HttpPost]
        public ActionResult DeleteCard(int id)
        {
            _bankService.DeleteCreditCard(id);

            return RedirectToAction("Index");
        }

        // =========================
        // BAKİYE GÜNCELLE
        // =========================
        [HttpPost]
        public ActionResult UpdateAccountBalance(int accountId, decimal amount)
        {
            var account = _bankService.GetAccountById(accountId);

            if (account != null)
            {
                account.Balance = amount;

                _bankService.UpdateAccount(account);
            }

            return RedirectToAction("Index");
        }

        // =========================
        // KREDİ KARTI GÜNCELLE
        // =========================
        [HttpPost]
        public ActionResult UpdateCreditCard(CreditCard card)
        {
            if (card.Id > 0)
            {
                card.UserId = User.Identity.GetUserId();

                _bankService.UpdateCreditCard(card);
            }

            return RedirectToAction("Index");
        }

        // =========================
        // KREDİ KARTI BORÇ ÖDE
        // =========================
        [HttpPost]
        public ActionResult PayCreditCardDebt(
            int cardId,
            int fromAccountId,
            decimal amount)
        {
            using (var db = new ApplicationDbContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var card =
                            db.CreditCards.Find(cardId);

                        if (card != null)
                        {
                            // Kart borcunu azalt
                            card.CurrentDebt -= amount;

                            // Hesaptan ödeme yapıldıysa
                            if (fromAccountId != -1)
                            {
                                var acc =
                                    db.BankAccounts.Find(fromAccountId);

                                if (acc != null)
                                {
                                    acc.Balance -= amount;
                                }
                            }

                            db.SaveChanges();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}