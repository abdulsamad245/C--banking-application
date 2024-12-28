using System;

namespace Banking.Domain.Models
{
    // Represents a financial transaction in a bank account
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

        // Returns a string representation of the transaction
        public override string ToString()
        {
            return $"{Date:dd-MM-yyyy}\t{Action}\t{Amount:F2}\t{Balance:F2}";
        }
    }
}

