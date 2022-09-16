using HarrysAccountsPackage.Data;
using HarrysAccountsPackage.Models;
using Microsoft.AspNetCore.Mvc;

namespace HarrysAccountsPackage.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get accounts homepage
        public IActionResult Index()
        {
            return View();
        }

        // Get an Account by AccountCode, should redirect if AccountCode is empty, AccountCode does not exist in database or the Accounts table in database is empty.
        public IActionResult GetByAccountCode(string? AccountCode)
        {
            // Check if nothing was entered into the customer enquiry input field.
            if (AccountCode == null)
            {
                return RedirectToAction("GetAccountCodeList");
            }
            // Check if account code exists in the database.
            var account = _db.Accounts.FirstOrDefault(x => x.AccountCode == AccountCode);
            if (account == null)
            {
                return RedirectToAction("GetAccountCodeList");
            }
            // Check if Accounts database is empty.
            var accountsEmpty  = from m in _db.Accounts select m;
            if (accountsEmpty.Count() == 0)
            {
                RedirectToAction("GetAccountCodeListEmpty");
            }
            // Return found account
            return View(account);
        }

        // Get a list of accounts to choose from if redirected here.
        public IActionResult GetAccountCodeList()
        // Get list of accounts in the Accounts table in database.
        {
            var accountCodeList = _db.Accounts.ToList();
            // Return found list with the view.
            return View(accountCodeList);
        }

        // Get Sales Ledger Homepage.
        public IActionResult SalesLedger()
        {
            return View();
        }

        // Get view to enter account details for posting.
        public IActionResult Post()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Post account object to database.
        public IActionResult Post(Account obj)
        {
            // Check if an account with provided AccountCode property exists.
            var accExists = from m in _db.Accounts select m;
            accExists = accExists.Where(x => x.AccountCode == obj.AccountCode);

            bool accCondition = false;

            if (accExists.Count() > 0)
            {
                accCondition = true;
            }
            else
            {
                accCondition = false;
            }
            if (accCondition == false)
            {
                if (ModelState.IsValid)
                {
                    _db.Accounts.Add(obj);
                    _db.SaveChanges();
                    return RedirectToAction("SalesLedger");
                }
            } else
            // Redirect if an Account with provided account code already exists to prevent duplication
            {
                ModelState.AddModelError("Custom Error", "An Account with that Account Code already exists, Account Code must be unique.");
                return View(obj);
            }
            return View();
        }
    }
}
