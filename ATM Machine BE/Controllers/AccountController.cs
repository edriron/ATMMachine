using ATM_Machine_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ATM_Machine_BE.Services;

namespace ATM_Machine_BE.Controllers
{
    /// <summary>
    /// API endpoints for managing customer accounts.
    /// This endpoint allows viewing and updating account's balance, as requested.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Retrieves the account balance for a specific account.
        /// </summary>
        /// <param name="accoundId">The ID of the account to retrieve the balance for.</param>
        /// <returns>
        ///   <para>Returns an HTTP response with the account balance if the account is found.</para>
        ///   <para>If the account is not found, return '404 Not Found'.</para>
        /// </returns>
        [HttpGet("{accoundId}")]
        public async Task<ActionResult<int>> GetAccountBalance(int accoundId)
        {
            try 
            {
                var result = await _accountService.GetAccountBalance(accoundId);
                return Ok(result);
            }
            catch (AccountNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Updates the account balance by adding the specified amount.
        /// </summary>
        /// <param name="accountId">The ID of the account to update.</param>
        /// <param name="amount">The amount (positive or negative) to add to the account balance.</param>
        /// <returns>
        ///   <para>Returns an HTTP response with the updated account details if the account is found.</para>
        ///   <para>If the account is not found, return '404 Not Found'.</para>
        /// </returns>
        [HttpPut]
        public async Task<ActionResult<Account>> UpdateBalance(int accoundId, int amount)
        {
            if (amount == 0)
                return BadRequest("Amount cannot be zero");
            try
            {
                var result = await _accountService.UpdateBalance(accoundId, amount);
                return Ok(result);
            }
            catch (AccountNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
