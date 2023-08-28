using ATM_Machine_BE.Models;

namespace ATM_Machine_BE.Services
{
    /// <summary>
    /// Service interface for managing transactions.
    /// </summary>
    public interface ITransactionService
    {
        Task<int> Deposit(int accountId, int amount);
        Task<int> Withdrawal(int accountId, int amount);
        ATMMachine GetMachine();
    }
}
