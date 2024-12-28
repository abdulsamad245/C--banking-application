using System;
using Banking.Domain.Interfaces;
using Banking.Domain.Services;
using System.Linq;

namespace Banking.Console
{
    class Program
    {
        private static BankService bankService = new BankService();
        private static string bankEmployeePin = "A1234";

        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    System.Console.WriteLine("Welcome to the Banking Application");
                    System.Console.WriteLine("1. Bank Employee");
                    System.Console.WriteLine("2. Customer");
                    System.Console.WriteLine("3. Exit");
                    System.Console.Write("Choose an option: ");

                    string choice = System.Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            BankEmployeeMenu();
                            break;
                        case "2":
                            CustomerMenu();
                            break;
                        case "3":
                            return;
                        default:
                            System.Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Displays and handles the bank employee menu
        static void BankEmployeeMenu()
        {
            string pin = GetPinFromUser("Enter PIN: ");

            if (pin != bankEmployeePin)
            {
                System.Console.WriteLine("Invalid PIN. Access denied.");
                return;
            }

            while (true)
            {
                System.Console.WriteLine("\nBank Employee Menu");
                System.Console.WriteLine("1. Create Customer");
                System.Console.WriteLine("2. Delete Customer");
                System.Console.WriteLine("3. List Customers");
                System.Console.WriteLine("4. Create Transaction");
                System.Console.WriteLine("5. Return to Main Menu");
                System.Console.Write("Choose an option: ");

                string choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateCustomer();
                        break;
                    case "2":
                        DeleteCustomer();
                        break;
                    case "3":
                        ListCustomers();
                        break;
                    case "4":
                        CreateTransaction();
                        break;
                    case "5":
                        return;
                    default:
                        System.Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        // Creates a new customer
        static void CreateCustomer()
        {
            string firstName = GetStringFromUser("Enter First Name: ");
            string lastName = GetStringFromUser("Enter Last Name: ");
            string email = GetStringFromUser("Enter Email: ");

            try
            {
                var customer = bankService.CreateCustomer(firstName, lastName, email);
                System.Console.WriteLine($"Customer created successfully. PIN: {customer.Pin}");
                System.Console.WriteLine($"Account Number: {customer.AccountNumber}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error creating customer: {ex.Message}");
            }
        }

        // Deletes an existing customer
        static void DeleteCustomer()
        {
            string accountNumber = GetStringFromUser("Enter Account Number: ");

            if (bankService.DeleteCustomer(accountNumber))
            {
                System.Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                System.Console.WriteLine("Unable to delete customer. Check if account balances are zero.");
            }
        }

        // Lists all customers
        static void ListCustomers()
        {
            var customers = bankService.GetAllCustomers();
            foreach (var customer in customers)
            {
                System.Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}");
                System.Console.WriteLine($"Email: {customer.Email}");
                System.Console.WriteLine($"Account Number: {customer.AccountNumber}");
                System.Console.WriteLine($"Savings Balance: {customer.SavingsAccount.Balance:C}");
                System.Console.WriteLine($"Current Balance: {customer.CurrentAccount.Balance:C}");
                System.Console.WriteLine();
            }
        }

        // Creates a new transaction for a customer
        static void CreateTransaction()
        {
            string accountNumber = GetStringFromUser("Enter Account Number: ");
            string accountType = GetStringFromUser("Enter 'S' for Savings or 'C' for Current account: ").ToUpper();
            string transactionType = GetStringFromUser("Enter 'D' for Deposit or 'W' for Withdraw: ").ToUpper();
            decimal amount = GetDecimalFromUser("Enter amount: ");
            if (amount <= 0) return;

            var customers = bankService.GetAllCustomers();
            var customer = customers.FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (customer == null)
            {
                System.Console.WriteLine("Customer not found.");
                return;
            }

            var account = accountType == "S" ? customer.SavingsAccount : customer.CurrentAccount;

            try
            {
                if (transactionType == "D")
                {
                    account.Deposit(amount);
                    System.Console.WriteLine("Deposit successful.");
                }
                else if (transactionType == "W")
                {
                    if (account.Withdraw(amount))
                    {
                        System.Console.WriteLine("Withdrawal successful.");
                    }
                    else
                    {
                        System.Console.WriteLine("Insufficient funds.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid transaction type.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error processing transaction: {ex.Message}");
            }
        }

        // Displays and handles the customer menu
        static void CustomerMenu()
        {
            string firstName = GetStringFromUser("Enter First Name: ");
            string lastName = GetStringFromUser("Enter Last Name: ");
            string accountNumber = GetStringFromUser("Enter Account Number: ");
            string pin = GetPinFromUser("Enter PIN: ");

            var customer = bankService.GetCustomer(firstName, lastName, accountNumber, pin);

            if (customer == null)
            {
                System.Console.WriteLine("Invalid credentials. Access denied.");
                return;
            }

            while (true)
            {
                System.Console.WriteLine($"\nWelcome, {customer.FirstName} {customer.LastName}");
                System.Console.WriteLine("1. View Transaction History");
                System.Console.WriteLine("2. Deposit");
                System.Console.WriteLine("3. Withdraw");
                System.Console.WriteLine("4. Return to Main Menu");
                System.Console.Write("Choose an option: ");

                string choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewTransactionHistory(customer);
                        break;
                    case "2":
                        PerformTransaction(customer, true);
                        break;
                    case "3":
                        PerformTransaction(customer, false);
                        break;
                    case "4":
                        return;
                    default:
                        System.Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        // Displays the transaction history for a customer's account
        static void ViewTransactionHistory(ICustomer customer)
        {
            string accountType = GetStringFromUser("Enter 'S' for Savings or 'C' for Current account: ").ToUpper();

            var account = accountType == "S" ? customer.SavingsAccount : customer.CurrentAccount;
            var transactions = account.GetTransactionHistory();

            System.Console.WriteLine($"Transaction History for {accountType} Account:");
            foreach (var transaction in transactions)
            {
                System.Console.WriteLine(transaction);
            }
        }

        // Performs a deposit or withdrawal transaction for a customer
        static void PerformTransaction(ICustomer customer, bool isDeposit)
        {
            string accountType = GetStringFromUser("Enter 'S' for Savings or 'C' for Current account: ").ToUpper();
            decimal amount = GetDecimalFromUser("Enter amount: ");
            if (amount <= 0) return;

            var account = accountType == "S" ? customer.SavingsAccount : customer.CurrentAccount;

            try
            {
                if (isDeposit)
                {
                    account.Deposit(amount);
                    System.Console.WriteLine("Deposit successful.");
                }
                else
                {
                    if (account.Withdraw(amount))
                    {
                        System.Console.WriteLine("Withdrawal successful.");
                    }
                    else
                    {
                        System.Console.WriteLine("Insufficient funds.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error processing transaction: {ex.Message}");
            }
        }

        // Gets a string input from the user
        static string GetStringFromUser(string prompt)
        {
            System.Console.Write(prompt);
            return System.Console.ReadLine();
        }

        // Gets a decimal input from the user
        static decimal GetDecimalFromUser(string prompt)
        {
            System.Console.Write(prompt);
            if (decimal.TryParse(System.Console.ReadLine(), out decimal amount))
            {
                return amount;
            }
            System.Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            return 0;
        }

        // Gets a PIN input from the user
        static string GetPinFromUser(string prompt)
        {
            System.Console.Write(prompt);
            return System.Console.ReadLine();
        }
    }
}

