using System;
using Banking.Domain.Interfaces;

namespace Banking.Domain.Models
{
    // Represents a bank customer
    public class Customer : ICustomer
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Pin { get; }
        public string AccountNumber { get; }
        public IAccount SavingsAccount { get; }
        public IAccount CurrentAccount { get; }

        public Customer(string firstName, string lastName, string email, decimal savingsBalance = 0, decimal currentBalance = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Invalid email format.");
            }

            AccountNumber = GenerateAccountNumber();
            Pin = GeneratePin(AccountNumber);

            SavingsAccount = new SavingsAccount(AccountNumber, savingsBalance);
            CurrentAccount = new CurrentAccount(AccountNumber, currentBalance);
        }

        // Generates a unique account number based on customer details
        private string GenerateAccountNumber()
        {
            string initials = $"{FirstName[0]}{LastName[0]}".ToLower();
            int nameLength = FirstName.Length + LastName.Length;
            int firstInitialPosition = char.ToUpper(FirstName[0]) - 'A' + 1;
            int lastInitialPosition = char.ToUpper(LastName[0]) - 'A' + 1;

            return $"{initials}-{nameLength}-{firstInitialPosition:D2}-{lastInitialPosition:D2}";
        }

        // Generates a PIN based on the account number
        private string GeneratePin(string accountNumber)
        {
            string[] parts = accountNumber.Split('-');
            return $"{parts[2]}{parts[3]}";
        }
    }
}

