using ATM_Machine_BE.Data;
using ATM_Machine_BE.Models;

namespace ATM_Machine_BE.Services
{
    /// <summary>
    /// Service for managing account operations.
    /// How to run: Using SQLExpress, with SSMS you can view and edit records of tables.
    ///             Run Entity migration to create the db:
    ///             dotnet ef migrations add InitialCreate
    ///             dotnet ef database update
    /// Note: You can use EntityFramework In-Memory db as well to easily test the functionalities.
    /// 
    /// How to test: Add account(s), then check account balance, update the account balance and recheck to see the value changes.S
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly DataContext _context;

        public AccountService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the account balance for a specific account.
        /// </summary>
        /// <param name="accoundId">The ID of the account to retrieve the balance for.</param>
        /// <returns>The account balance if the account exists.</returns>
        /// /// <exception cref="AccountNotFoundException">Thrown when account is not found.</exception>
        public async Task<int> GetAccountBalance(int accoundId)
        {
            // Find the account (whose ID is id) in the db
            var accountInDb = await _context.Accounts.FindAsync(accoundId);
            if (accountInDb == null)
                throw new AccountNotFoundException("Account was not found");

            return accountInDb.Balance;
        }

        /// <summary>
        /// Updates the account balance for a specific account.
        /// </summary>
        /// <param name="accoundId">The ID of the account to update.</param>
        /// <param name="amount">The amount to add to the account balance (can be positive or negative).</param>
        /// <returns>The updated account if the account exists.</returns>
        /// <exception cref="AccountNotFoundException">Thrown when account is not found.</exception>
        public async Task<Account> UpdateBalance(int accoundId, int amount)
        {
            // Find the account (whose ID is id) in the db
            var accountInDb = await _context.Accounts.FindAsync(accoundId);
            if (accountInDb == null)
                throw new AccountNotFoundException("Account was not found");

            // Update the account's balance according the the given amount, then save
            accountInDb.Balance += amount;

            await _context.SaveChangesAsync();

            return accountInDb;
        }
    }
}
