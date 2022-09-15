using HarrysAccountsPackage.Models;
using Microsoft.EntityFrameworkCore;

namespace HarrysAccountsPackage.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Invoices> Invoices { get; set; } 
    }
}
