using ATM_Machine_BE.Models;
using Microsoft.AspNetCore.Mvc;

namespace ATM_Machine_BE.Services
{
    /// <summary>
    /// Service interface for managing customer accounts.
    /// </summary>
    public interface IAccountService
    {
        Task<int> GetAccountBalance(int accoundId);
        Task<Account> UpdateBalance(int accoundId, int amount);
    }
}
