using HarrysAccountsPackage.Data;
using HarrysAccountsPackage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol;

namespace HarrysAccountsPackage.Controllers
{
    public class InvoicesController : Controller
    {
        HttpClient client = new HttpClient();

        private readonly ApplicationDbContext _db;

        public InvoicesController(ApplicationDbContext db)
        {
            _db = db;
        }
        // Get invoices for an account by searching for invoices that have the provided AccountCode property
        public async Task<ActionResult<List<Invoices>>> GetAccountInvoices(string? AccountCode)
        {
            var invoices = new List<Invoices>();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7266/api/Invoice/{AccountCode}");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                invoices = await response.Content.ReadFromJsonAsync<List<Invoices>>();
                if (invoices.Count > 0)
                {
                    return View(invoices);
                }
            }
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
        public async Task<ActionResult<Invoices>> Post(Invoices obj)
        {
            var Invoice = obj;

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7266/api/Invoice");
            request.Content = JsonContent.Create(Invoice);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SalesLedger","Account");
            }
            else
            {
                return View();
            }
        }
    }
}
