using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Banking.Domain.Interfaces;
using Banking.Domain.Models;

namespace Banking.Domain.Services
{
    // Provides banking operations for managing customers and their accounts
    public class BankService
    {
        private const string CustomersFile = "database/customers.txt";
        private List<ICustomer> customers = new List<ICustomer>();

        public BankService()
        {
            LoadCustomers();
        }

        // Creates a new customer or returns an existing one
        public ICustomer CreateCustomer(string firstName, string lastName, string email)
        {
            var existingCustomer = customers.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (existingCustomer != null)
            {
                Console.WriteLine("Customer already exists. Returning existing customer.");
                return existingCustomer;
            }

            var customer = new Customer(firstName, lastName, email);
            customers.Add(customer);
            SaveCustomers();
            return customer;
        }

        // Deletes a customer and their associated transaction files
        public bool DeleteCustomer(string accountNumber)
        {
            var customer = customers.FirstOrDefault(c =>
                c.AccountNumber == accountNumber &&
                c.SavingsAccount.Balance == 0 && c.CurrentAccount.Balance == 0);

            if (customer != null)
            {
                customers.Remove(customer);
                SaveCustomers();
                DeleteTransactionFiles(accountNumber);
                return true;
            }
            return false;
        }

        // Deletes transaction files associated with a customer
        private void DeleteTransactionFiles(string accountNumber)
        {
            try
            {
                File.Delete(Path.Combine("database", "savings", $"{accountNumber}.txt"));
                File.Delete(Path.Combine("database", "current", $"{accountNumber}.txt"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting transaction files: {ex.Message}");
            }
        }

        // Retrieves a customer based on their credentials
        public ICustomer GetCustomer(string firstName, string lastName, string accountNumber, string pin)
        {
            return customers.FirstOrDefault(c =>
                c.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                c.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase) &&
                c.AccountNumber == accountNumber &&
                c.Pin == pin);
        }

        // Returns a list of all customers
        public List<ICustomer> GetAllCustomers()
        {
            return new List<ICustomer>(customers);
        }

        // Loads customer data from the file system
        public void LoadCustomers()
        {
            try
            {
                if (File.Exists(CustomersFile))
                {
                    var lines = File.ReadAllLines(CustomersFile);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('\t');
                        if (parts.Length == 6)
                        {
                            var customer = new Customer(
                                parts[0],
                                parts[1],
                                parts[2],
                                decimal.Parse(parts[4]),
                                decimal.Parse(parts[5])
                            );
                            customers.Add(customer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading customers: {ex.Message}");
                throw new IOException("Failed to load customers.", ex);
            }
        }

        // Saves customer data to the file system
        private void SaveCustomers()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CustomersFile));
                File.WriteAllLines(CustomersFile, customers.Select(c =>
                    $"{c.FirstName}\t{c.LastName}\t{c.Email}\t{c.AccountNumber}\t{c.SavingsAccount.Balance}\t{c.CurrentAccount.Balance}"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving customers: {ex.Message}");
                throw new IOException("Failed to save customers.", ex);
            }
        }

        // Updates customer balance after a transaction
        public void UpdateCustomerBalance(ICustomer customer)
        {
            var existingCustomer = customers.FirstOrDefault(c => c.AccountNumber == customer.AccountNumber);
            if (existingCustomer != null)
            {
                customers.Remove(existingCustomer);
                customers.Add(customer);
                SaveCustomers();
            }
        }
    }
}

