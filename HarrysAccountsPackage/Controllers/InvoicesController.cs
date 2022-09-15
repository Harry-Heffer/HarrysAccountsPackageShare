using HarrysAccountsPackage.Data;
using HarrysAccountsPackage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol;

namespace HarrysAccountsPackage.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public InvoicesController(ApplicationDbContext db)
        {
            _db = db;
        }
        // Get invoices for an account by searching for invoices that have the provided AccountCode property
        public IActionResult GetAccountInvoices(string? AccountCode)
        {
            var invoices = from m in _db.Invoices select m;
            if(!String.IsNullOrEmpty(AccountCode))
            {
                invoices = invoices.Where(x => x.AccountCode == AccountCode);
            }
            if (invoices.Count() > 0) 
            {
                return View(invoices);
            }
            // Redirect if no invoices with provided AccountCode property exist in the Invoices table in the database.
            return RedirectToAction("GetAccountInvoicesEmpty");
        }
        // Redirect here if no invoices found for an account.
        public IActionResult GetAccountInvoicesEmpty()
        {
            return View();
        }
        public IActionResult Post()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Post Invoice object.
        // Provides the object an account name based on AccountCode used when posting, this is to ensure spelling mistakes when posting an invoice AccountName
        // do not cause storage issues in the database.
        public IActionResult Post(Invoices obj)
        {
            if (ModelState.IsValid)
            {
                // Check that account code exists
                var accExists = from m in _db.Accounts select m;
                accExists = accExists.Where(x => x.AccountCode == obj.AccountCode);

                bool accCondition = false;

                if (accExists.Count() > 0)
                {
                    accCondition = true;
                } else
                {
                    accCondition = false;
                }
                // Check if Invoice Number is duplicated with selected AccountCode
                var invExists = from m in _db.Invoices select m;
                invExists = invExists.Where(x => x.InvoiceNumber == obj.InvoiceNumber).Where(x => x.AccountCode == obj.AccountCode);

                bool invCondition = false;

                if (invExists.Count() > 0)
                {
                    invCondition = true;
                }
                else
                {
                    invCondition = false;
                }

                if (accCondition == true && invCondition == false)
                {
                    var name = from m in _db.Accounts select m;
                    name = name.Where(x => x.AccountCode == obj.AccountCode);
                    obj.AccountName = name.First().AccountName;
                    _db.Invoices.Add(obj);
                    _db.SaveChanges();
                    return RedirectToAction("SalesLedger", "Account");
                } 
                else if (accCondition == false)
                {
                    ModelState.AddModelError("Customer Error", "The Account Code entered does not exist.");
                    return View(obj);
                }
                if (invCondition == true)
                {
                    ModelState.AddModelError("Customer Error", "Duplicate Invoice Number");
                }
            }
            return View();
        }
    }
}
