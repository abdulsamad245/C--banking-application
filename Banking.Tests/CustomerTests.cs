using Xunit;
using Banking.Domain.Models;
using System;

namespace Banking.Tests
{
    public class CustomerTests
    {
        // Test for ensuring that a customer creates both savings and current accounts correctly
        [Fact]
        public void Customer_CreatesAccountsCorrectly()
        {
            var customer = new Customer("John", "Doe", "john@example.com");

            Assert.NotNull(customer.SavingsAccount);
            Assert.NotNull(customer.CurrentAccount);
            Assert.Equal(customer.SavingsAccount.AccountNumber, customer.CurrentAccount.AccountNumber);
        }

        // Test for verifying that deposit and withdrawal operations work as expected
        [Fact]
        public void Account_DepositAndWithdrawWork()
        {
            var customer = new Customer("Jane", "Smith", "jane@example.com");
            var account = customer.SavingsAccount;

            var balance = account.Balance;
            account.Deposit(100);
            account.Withdraw(balance + 100);

            account.Deposit(100);
            Assert.Equal(100, account.Balance);

            bool withdrawResult = account.Withdraw(50);
            Assert.True(withdrawResult);
            Assert.Equal(50, account.Balance);

            withdrawResult = account.Withdraw(100);
            Assert.False(withdrawResult);
            Assert.Equal(50, account.Balance);
        }
    }
}
