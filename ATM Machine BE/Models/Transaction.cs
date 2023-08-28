using System.Security.Principal;

namespace ATM_Machine_BE.Models
{
    /// <summary>
    /// Represents a transaction.
    /// </summary>
    public class Transaction
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public int AccountId { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
