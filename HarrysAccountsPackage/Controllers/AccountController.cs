using HarrysAccountsPackage.Data;
using HarrysAccountsPackage.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HarrysAccountsPackage.Controllers
{
    public class AccountController : Controller
    {
        HttpClient client = new HttpClient();

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
        public async Task<ActionResult<Account>> GetByAccountCode(string? AccountCode)
        {
            if (AccountCode == null)
            {
                return RedirectToAction("GetAccountCodeList");
            }
            var account = new Account();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7266/api/Account/{AccountCode}");
            
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                account = await response.Content.ReadFromJsonAsync<Account>();
            }
            return View(account);
        }

        // Get a list of accounts to choose from if redirected here.
        public async Task<ActionResult<List<Account>>> GetAccountCodeList()
        {
            var accounts = new List<Account>();
    
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7266/api/Account");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                accounts = await response.Content.ReadFromJsonAsync<List<Account>>();
            }

            return View(accounts);
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
        public async Task<ActionResult<Account>> Post(Account obj)
        {
            var Account = obj;

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7266/api/Account");
            request.Content = JsonContent.Create(Account);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return View("SalesLedger");
            } else
            {
                return View();
            }
        }
    }
}
