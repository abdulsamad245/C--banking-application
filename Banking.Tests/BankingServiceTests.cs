using Xunit;
using Banking.Domain.Services;
using Banking.Domain.Interfaces;
using System.Linq;
using Xunit.Abstractions;
using Banking.Domain.Models;
using System.IO;

namespace Banking.Tests
{
    public class BankServiceTests
    {
        private readonly ITestOutputHelper _output;

        public BankServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }

        // Test for adding a customer to the list when creating a new customer
        [Fact]
        public void CreateCustomer_AddsCustomerToList()
        {
            var bankService = new BankService();
            int initialCount = bankService.GetAllCustomers().Count;

            bankService.CreateCustomer("Alice", "Johnson", "alice@example.com");

            Assert.Equal(initialCount + 1, bankService.GetAllCustomers().Count);
            bankService.DeleteCustomer("aj-12-01-10");
        }

        // Test for deleting a customer when their balance is zero
        [Fact]
        public void DeleteCustomer_RemovesCustomerWithZeroBalance()
        {
            var bankService = new BankService();
            bankService.CreateCustomer("Bob", "Williams", "bob@example.com");
            var accountNumber = "bw-11-02-23";

            bool result = bankService.DeleteCustomer(accountNumber);

            Assert.True(result);
        }

        // Test for failing to delete a customer when their balance is non-zero
        [Fact]
        public void DeleteCustomer_FailsForNonZeroBalance()
        {
            var bankService = new BankService();
            bankService.CreateCustomer("Charlie", "Brown", "charlie@example.com");

            var customer = bankService.GetCustomer("Charlie", "Brown", "cb-12-03-02", "0302");
            customer.SavingsAccount.Deposit(100);

            var accountNumber = "cb-12-03-02";

            bool result = bankService.DeleteCustomer(accountNumber);

            Assert.False(result);
        }
    }
}
