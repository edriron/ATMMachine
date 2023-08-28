namespace ATM_Machine_BE.Models
{
    /// <summary>
    /// Represents a customer account.
    /// </summary>
    public class Account
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public string ClientName { get; set; } = string.Empty;
    }
}
