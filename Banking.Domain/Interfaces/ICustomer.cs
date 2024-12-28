namespace Banking.Domain.Interfaces
{
    // Interface representing a bank customer
    public interface ICustomer
    {
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string Pin { get; }
        string AccountNumber { get; }
        IAccount SavingsAccount { get; }
        IAccount CurrentAccount { get; }
    }
}

