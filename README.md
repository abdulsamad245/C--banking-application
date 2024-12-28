# Banking Application

## Project Overview
This Banking Application is a console-based system that simulates basic banking operations. It allows for customer account management, transactions, and balance tracking.

## Features Implemented
1. Customer Management
   - Create new customers with unique account numbers based on the given rule
   - Delete customers (only if their account balances are zero)
   - List all customers

2. Account Management
   - Each customer has a savings account and a current account
   - Deposit and withdraw funds from accounts
   - View transaction history for each account

3. Transaction Handling
   - Record all transactions (deposits and withdrawals)
   - Store transaction history in separate files for each account

4. Data Persistence
   - Save customer information to a file (db/customers.txt)
   - Save transaction history for each account in separate files

5. Security
   - Generate unique account numbers and PINs for each customer
   - Authenticate customers using their credentials

6. Bank Employee Operations
   - Perform transactions on behalf of customers
   - View and manage customer accounts

## Technical Details
- The application is built using C# and .NET
- File-based storage is used for data persistence
- Unit tests are implemented to ensure functionality

## How to Run
1. Ensure you have .NET 6.0 or later installed
2. Clone the repository
3. Navigate to the project directory
4. Run `dotnet build` to build the project
5. Run `dotnet run --project Banking.Console` to start the application

## Running Tests
To run the unit tests, use the command `dotnet test` in the project directory.

