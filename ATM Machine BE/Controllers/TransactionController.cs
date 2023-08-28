using ATM_Machine_BE.Models;
using ATM_Machine_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace ATM_Machine_BE.Controllers
{
    /// <summary>
    /// API endpoints for processing transactions.
    /// This endpoint allows to make transactions for spesific accounts, including despoit and withdrawal, as requested.
    /// In addition, this endpoint allows to view the machine state via GetMachine.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Deposits the specified amount into the account.
        /// </summary>
        /// <param name="accountId">The ID of the account to deposit into.</param>
        /// <param name="amount">The amount to deposit.</param>
        /// <returns>
        ///   <para>Returns an HTTP response with the updated account details if the deposit is successful.</para>
        ///   <para>If the account is not found, return '404 Not Found'. If the amount is negative, return '400 Bad Request'.</para>
        /// </returns>
        [HttpPut]
        public async Task<ActionResult<int>> Deposit(int accountId, int amount)
        {
            if (amount <= 0)
                return BadRequest("Amount must be positive");

            try
            {
                var result = await _transactionService.Deposit(accountId, amount);
                return Ok(result);
            }
            catch (AccountNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Withdraws the specified amount from the account.
        /// </summary>
        /// <param name="accountId">The ID of the account to withdraw from.</param>
        /// <param name="amount">The amount to withdraw.</param>
        /// <returns>
        ///   <para>Returns an HTTP response with the updated account details if the withdrawal is successful.</para>
        ///   <para>If the account is not found, return '404 Not Found'. If the amount is negative, return '400 Bad Request'.</para>
        /// </returns>
        [HttpPut]
        public async Task<ActionResult<Account>> Withdrawal(int accountId, int amount)
        {
            if (amount <= 0)
                return BadRequest("Amount must be positive");

            try
            {
                var result = await _transactionService.Withdrawal(accountId, amount);
                return Ok(result);
            }
            catch (AccountNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InsufficientBalanceException e)
            {
                return BadRequest(e.Message);
            }
            catch (InsufficientBillsException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the details of the ATM machine.
        /// </summary>
        /// <returns>
        ///   <para>Returns an HTTP response with the details of the ATM machine if successful.</para>
        ///   <para>If the ATM machine is not found, return '404 Not Found'.</para>
        /// </returns>
        [HttpGet]
        public ActionResult<ATMMachine> GetMachine()
        {
            var result = _transactionService.GetMachine();
            return Ok(result);
        }
    }

}
