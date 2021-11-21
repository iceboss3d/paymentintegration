using Microsoft.EntityFrameworkCore;
using PaymentIntegration.Data.Contexts;
using PaymentIntegration.Data.Repositories.Abstractions;
using PaymentIntegration.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentIntegration.Data.Repositories.Implementation
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions.Select(t => t).ToListAsync();
        }

        public async Task<Transaction> GetByReferenceAsync(string reference)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.TrxRef == reference);
        }

        public async Task SaveTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
