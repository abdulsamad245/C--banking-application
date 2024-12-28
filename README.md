# Banking Application

## Project Overview

The Banking Application is a console-based simulation of a banking system designed to perform basic banking operations such as account management, transaction handling, and balance tracking. This application supports two user roles: Bank Employee and Customer, with specific functionalities available to each role. 

The application allows bank employees to create new customer accounts, perform transactions on behalf of customers, view customer information, and manage account details. Customers can log in using their credentials, view their transaction history, and perform deposits or withdrawals from their savings and current accounts. All data is stored in text files for persistence.

## Features Implemented

### 1. Customer Management
   - Create new customers: Customers are created with a unique account number and PIN, generated based on their first and last names. The account details are stored in a file (`customers.txt`).
   - Delete customers: Customers can only be deleted if their account balances are zero. The deletion operation ensures that no funds are lost when a customer is removed.
   - List all customers: A functionality to display all customer accounts, including their savings and current account balances, allowing the bank employee to see all accounts in the system.

### 2. Account Management
   - Savings and Current Accounts: Each customer has two types of accounts: a savings account and a current account. These accounts are initialized when the customer is created.
   - Deposit and Withdrawal: Customers can deposit or withdraw funds from either their savings or current accounts. The system ensures that balances cannot go negative.
   - Transaction History: Every transaction (deposit or withdrawal) is logged and saved to separate transaction files for each account (e.g., `js-8-10-19-savings.txt`, `js-8-10-19-current.txt`), including the date, type of transaction, amount, and the resulting balance.

### 3. Transaction Handling
   - Record Transactions: All transactions made on accounts (both deposit and withdrawal) are recorded. These records include the transaction date, action (deposit or withdrawal), amount transferred, and the final balance after the transaction.
   - Separate Files for Each Account: Each account has its own transaction history file. These files are updated every time a transaction is made, and the details are saved with tab-delimited values for easy reading.

### 4. Data Persistence
   - Customer Information: Customer details, including account numbers, first names, last names, email addresses, and PINs, are saved in a file called `customers.txt`. This file is created when the program first runs and is updated whenever customers are added or removed.
   - Transaction History: Transaction history is saved in separate text files for each account (e.g., `js-8-10-19-savings.txt` for savings accounts and `js-8-10-19-current.txt` for current accounts). This ensures that all actions on an account are preserved for future reference.

### 5. Security
   - Account Number and PIN Generation: Account numbers and PINs are generated using the initials of the customer and the positions of their initials in the alphabet. The format is `xx-nn-yy-zz`, where:
     - `xx` is the initials of the customer,
     - `nn` is the length of the full name,
     - `yy` and `zz` are the alphabetical positions of the first and second initials, respectively.
   - Authentication: Customers are required to enter their full name, account number, and PIN to log in. Bank employees use a fixed PIN (`A1234`) to access their operations.

### 6. Bank Employee Operations
   - Create and Delete Customers: Bank employees can create new customer accounts and delete accounts (only if the balance is zero).
   - View and Manage Accounts: Bank employees can view a list of customers and their account balances and perform transactions (deposit and withdrawal) on behalf of the customers.
   - Perform Transactions: Employees can deposit and withdraw money into/from customer accounts, updating the balances accordingly and recording the transaction history.

### 7. Customer Operations
   - Login: Customers must enter their full name, account number, and PIN to access their account.
   - Transaction History: Customers can view the history of their transactions for both savings and current accounts.
   - Deposit and Withdraw: Customers can add money to their savings or current accounts or withdraw funds, with the system ensuring that the account balance never becomes negative.

### 8. File Naming and Storage
   - File Naming: Account files and transaction history files follow a strict naming convention. For example, if a customer's name is "Joe Smith," their account file will be named `js-8-10-19` (with `js` being the initials, `8` the length of the full name, and `10` and `19` being the alphabetical positions of `J` and `S`, respectively). The account files are stored as `xx-nn-yy-zz-savings.txt` and `xx-nn-yy-zz-current.txt`.
   - Transaction File Format: Transaction records are stored with tab-separated values containing the transaction date, action (deposit/withdrawal), amount, and balance.

## Technical Details

- Programming Language: The application is built using C# with the .NET framework.
- File-Based Storage: All customer data and transaction histories are stored in text files for data persistence. This allows the application to persist data even after it is closed and reopened.
- Unit Testing: Unit tests are implemented to ensure that key functionalities (such as customer creation, transactions, and account management) work as expected.

## How to Run

1. Pre-Requisites: Ensure you have .NET 6.0 or later installed on your system.
   
2. Clone the Repository: Clone the project repository from GitHub using the following command:
   ```bash
   git clone <repository_url>
   ```

3. Navigate to the Project Directory: Use the terminal or command prompt to navigate to the project directory.

4. Build the Project: Run the following command to build the application:
   ```bash
   dotnet build
   ```

5. Run the Application: Use the following command to start the application:
   ```bash
   dotnet run --project Banking.Console
   ```

6. Login as a Bank Employee: To log in as a bank employee, enter the PIN `A1234`. You can then manage customer accounts and perform transactions.

7. Login as a Customer: To log in as a customer, enter your first name, last name, account number, and PIN. After logging in, you can perform operations on your account.

## Running Tests

1. Run Unit Tests: To run unit tests and verify the functionality of the application, use the following command in the project directory:
   ```bash
   dotnet test
   ```

2. Test Coverage: The unit tests verify essential features such as:
   - Customer creation
   - Deposit and withdrawal functionality
   - Transaction history recording
   - Account management

## Challenges and Solutions

- File Management: Handling file-based storage was challenging due to the need to generate unique filenames for customer accounts and transaction history files. This was resolved by using a consistent naming convention based on customer initials and account information.
  
- Security: Ensuring that only valid customers could log in and perform transactions required a robust authentication system. This was achieved by validating the entered credentials (first name, last name, account number, and PIN) during the login process.

- Transaction Recording: Recording each transaction accurately, especially with the need for a consistent file format, required careful attention to detail. The application ensures that every transaction is stored with a timestamp and the transaction type (deposit or withdrawal).
