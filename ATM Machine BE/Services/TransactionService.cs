using ATM_Machine_BE.Data;
using ATM_Machine_BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ATM_Machine_BE.Services
{
    /// <summary>
    /// Service for managing transactions.
    /// How to test: After adding accounts, deposit money to an account and check that that balance has been updated.
    ///              Check machine's bills, then withdrawal an amount- if success, check tbhe machine again to see what bills have been given
    ///              to the account, and see that the account's balance has been updated as well.
    ///              
    ///              After each transaction, use GetAllTransactions to get the list of all transactions
    ///              to test that the currect details are recorded.
    /// </summary>
    public class TransactionService : ITransactionService
    {
        private const int DefaultBillCount = 5;
        private static List<ATMMachine> machines = new List<ATMMachine>
        {
            new ATMMachine
            {
                Id = 1,
                Bill200 = DefaultBillCount,
                Bill100 = DefaultBillCount,
                Bill50 = DefaultBillCount,
                Bill20 = DefaultBillCount
            }
        };

        private readonly DataContext _context;

        public TransactionService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the (first) ATM machine.
        /// </summary>
        /// <returns></returns>
        public ATMMachine GetMachine()
        {
            return machines[0];
        }

        /// <summary>
        /// Performs a deposit operation for a specified account.
        /// </summary>
        /// <param name="accountId">The ID of the account to deposit into.</param>
        /// <param name="amount">The amount to deposit.</param>
        /// <returns>The new balance if the deposit is successful.</returns>
        /// /// <exception cref="AccountNotFoundException">Thrown when account is not found.</exception>
        public async Task<int> Deposit(int accountId, int amount)
        {
            // Find the account (whose ID is id) in the db
            var accountInDb = await _context.Accounts.FindAsync(accountId);
            if (accountInDb == null)
                throw new AccountNotFoundException("Account was not found");

            // Update the account's balance according the the given amount
            accountInDb.Balance += amount;

            // Create new Transaction obj, set it's properties, then save to db
            Transaction transaction = new Transaction();
            transaction.TransactionType = TransactionType.Deposit;
            transaction.AccountId = accountInDb.Id;
            transaction.Amount = amount;
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            return accountInDb.Balance;
        }

        /// <summary>
        /// Performs a withdrawal operation for a specified account.
        /// </summary>
        /// <param name="accountId">The ID of the account to withdraw from.</param>
        /// <param name="amount">The amount to withdraw.</param>
        /// <returns>The new balance if the withdrawal is successful.</returns>
        /// /// <exception cref="AccountNotFoundException">Thrown when account is not found.</exception>
        /// /// <exception cref="InsufficientBalanceException">Thrown when amount is greater than the account's balance.</exception>
        /// /// <exception cref="AccountNotFoundException">Thrown when there aren't enough bills in the machine.</exception>
        public async Task<int> Withdrawal(int accountId, int amount)
        {
            // Find the account (whose ID is id) in the db
            var accountInDb = await _context.Accounts.FindAsync(accountId);

            if (accountInDb == null)
                throw new AccountNotFoundException("Account was not found");

            if (accountInDb.Balance < amount)
                throw new InsufficientBalanceException("Insufficient balance");

            // Get the machine and calculate how many (if possible) bills required of each type to sum to amount
            ATMMachine machine = machines.First();
            int[] coins = { 200, 100, 50, 20 };
            int[] counts = { machine.Bill200, machine.Bill100, machine.Bill50, machine.Bill20 };
            var billsCount = CalculateBillsCount(amount, coins, counts);

            if (billsCount == null)
                throw new InsufficientBillsException("Not enough bills in the machine");

            // Update the bills count according to calculation
            machine.Bill200 -= billsCount[0];
            machine.Bill100 -= billsCount[1];
            machine.Bill50 -= billsCount[2];
            machine.Bill20 -= billsCount[3];

            // Update account's balance
            accountInDb.Balance -= amount;

            // Create new Transaction obj, set it's properties, then save to db
            Transaction transaction = new Transaction();
            transaction.TransactionType = TransactionType.Withdrawl;
            transaction.AccountId = accountInDb.Id;
            transaction.Amount = amount;
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            return accountInDb.Balance;
        }

        /// <summary>
        /// Calculates the required bills for a withdrawal operation.
        /// The algorithm is similar to the coin problem with limited coins - solved with dymanic programming.
        /// </summary>
        /// <param name="amount">The amount to withdraw.</param>
        /// <param name="coins">The available coins.</param>
        /// <param name="counts">The available counts of each coin.</param>
        /// <returns>An array representing the count of each coin required that sum up exactly to amount, or null if not possible.</returns>
        public static int[]? CalculateBillsCount(int amount, int[] coins, int[] counts)
        {
            // Init an array to store the min number of coins needed for each sum from 0 to amount
            int[] dp = new int[amount + 1];
            Array.Fill(dp, int.MaxValue);
            dp[0] = 0;

            for (int i = 0; i < coins.Length; i++)  // for each coin
            {
                int coin = coins[i];
                for (int j = 0; j < counts[i]; j++)  // and for each count of coin i
                {
                    // Update dp[s] for all s >= coin
                    for (int s = amount; s >= coin; s--)  
                    {
                        int remainder = s - coin;
                        if (remainder >= 0 && dp[remainder] != int.MaxValue) // If not neg && min num of coins to make remainder != init
                        {
                            // If using the current coin leads to fewer coins than the previously min -> update dp[s]
                            if (1 + dp[remainder] < dp[s])
                                dp[s] = 1 + dp[remainder];
                        }
                    }
                }
            }

            // Not possible combination found
            if (dp[amount] == int.MaxValue)
                return null;

            // Calc the count of used coins for each coin.
            int[] usedCoins = new int[coins.Length];
            int remainingSum = amount;

            for (int i = coins.Length - 1; i >= 0; i--)
            {
                int coin = coins[i];
                while (remainingSum >= coin && dp[remainingSum] == 1 + dp[remainingSum - coin])
                {
                    usedCoins[i]++;
                    remainingSum -= coin;
                }
            }

            return usedCoins;
        }
    }
}
