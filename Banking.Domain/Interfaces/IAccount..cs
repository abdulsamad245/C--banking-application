using System.Collections.Generic;
using Banking.Domain.Models;

namespace Banking.Domain.Interfaces
{
    // Interface representing a bank account
    public interface IAccount
    {
        string AccountNumber { get; }
        decimal Balance { get; }
        void Deposit(decimal amount);
        bool Withdraw(decimal amount);
        List<Transaction> GetTransactionHistory();
    }
}

