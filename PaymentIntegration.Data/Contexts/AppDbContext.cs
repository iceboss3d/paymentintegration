using Microsoft.EntityFrameworkCore;
using PaymentIntegration.Models.Domain;

namespace PaymentIntegration.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
