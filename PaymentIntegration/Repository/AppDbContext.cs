using Microsoft.EntityFrameworkCore;
using PaymentIntegration.Models;

namespace PaymentIntegration.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TransactionModel> Transactions { get; set; }
    }
}
