using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Banking.Domain.Interfaces;

namespace Banking.Domain.Models
{
    public abstract class Account : IAccount
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }
        protected List<Transaction> Transactions { get; } = new List<Transaction>();
        protected string TransactionFilePath { get; }

        // Constructor for the Account class
        protected Account(string accountNumber, string accountType, decimal initialBalance = 0)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;

            // Update file path to use the database directory
            TransactionFilePath = Path.Combine("database", accountType, $"{AccountNumber}.txt");
            LoadTransactions();
        }

        // Deposits money into the account
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive.");
            }
            Balance += amount;
            RecordTransaction("Lodgement", amount);
        }

        // Withdraws money from the account
        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive.");
            }
            if (Balance >= amount)
            {
                Balance -= amount;
                RecordTransaction("Withdrawal", amount);
                return true;
            }
            return false;
        }

        // Returns the transaction history of the account
        public List<Transaction> GetTransactionHistory()
        {
            return new List<Transaction>(Transactions);
        }

        // Records a new transaction
        protected void RecordTransaction(string action, decimal amount)
        {
            var transaction = new Transaction
            {
                Date = DateTime.Now,
                Action = action,
                Amount = amount,
                Balance = Balance
            };
            Transactions.Add(transaction);
            SaveTransaction(transaction);
        }

        // Saves a transaction to the transaction file
        protected void SaveTransaction(Transaction transaction)
        {
            try
            {
                // Ensure the directory exists before saving
                Directory.CreateDirectory(Path.GetDirectoryName(TransactionFilePath));
                File.AppendAllText(TransactionFilePath, transaction.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transaction: {ex.Message}");
                throw new IOException("Failed to save transaction.", ex);
            }
        }

        // Loads transactions from the transaction file
        protected void LoadTransactions()
        {
            try
            {
                if (File.Exists(TransactionFilePath))
                {
                    var lines = File.ReadAllLines(TransactionFilePath);
                    Transactions.Clear();

                    foreach (var line in lines)
                    {
                        var parts = line.Split('\t');

                        // Validate transaction file format
                        if (parts.Length != 4)
                        {
                            Console.WriteLine($"Malformed line in transaction file: {line}");
                            continue;
                        }

                        // Parse date using TryParseExact
                        if (DateTime.TryParseExact(parts[0], "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                        {
                            Transactions.Add(new Transaction
                            {
                                Date = date,
                                Action = parts[1],
                                Amount = decimal.Parse(parts[2]),
                                Balance = decimal.Parse(parts[3])
                            });
                        }
                        else
                        {
                            Console.WriteLine($"Invalid date format in line: {line}");
                        }
                    }

                    // Update balance if transactions exist
                    if (Transactions.Any())
                    {
                        Balance = Transactions.Last().Balance;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
                throw new IOException("Failed to load transactions.", ex);
            }
        }
    }

    // Represents a savings account
    public class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance = 0)
            : base(accountNumber, "savings", initialBalance) { }
    }

    // Represents a current account
    public class CurrentAccount : Account
    {
        public CurrentAccount(string accountNumber, decimal initialBalance = 0)
            : base(accountNumber, "current", initialBalance) { }
    }

}
