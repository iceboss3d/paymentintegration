using System;

namespace PaymentIntegration.Models
{
    public class TransactionModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public int Amount { get; set; }
        public string TrxRef { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
