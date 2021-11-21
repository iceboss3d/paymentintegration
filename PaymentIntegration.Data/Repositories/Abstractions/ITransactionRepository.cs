using PaymentIntegration.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentIntegration.Data.Repositories.Abstractions
{
    public interface ITransactionRepository
    {
        Task SaveTransactionAsync(Transaction transaction);
        Task<ICollection<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetByReferenceAsync(string reference);
        Task UpdateTransaction(Transaction transaction);
    }
}
