namespace ATM_Machine_BE.Models
{
    public class ServiceErrorException : Exception
    {
        public ServiceErrorException(string message) : base(message)
        {

        }
    }

    public class AccountNotFoundException : ServiceErrorException
    {
        public AccountNotFoundException(string message) : base(message)
        {

        }
    }

    public class InsufficientBalanceException : ServiceErrorException
    {
        public InsufficientBalanceException(string message) : base(message)
        {

        }
    }

    public class InsufficientBillsException : ServiceErrorException
    {
        public InsufficientBillsException(string message) : base(message)
        {

        }
    }
}
