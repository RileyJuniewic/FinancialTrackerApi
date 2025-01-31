# Juni's Financial Tracker API

![GitHub repo size](https://img.shields.io/github/repo-size/RileyJuniewic/FinancialTrackerApi)
![GitHub stars](https://img.shields.io/github/stars/RileyJuniewic/FinancialTrackerApi?style=social)

## Description
A Web API written with ASP.NET that provides CRUD operations for managing personal finances. Users can edit transactions with recalculation consistency—any changes to past transactions automatically update balances without breaking data integrity.

The API uses JWT authentication for secure access and leverages Dapper ORM for efficient database interactions, ensuring high performance.

## Technologies Used
- ASP.NET Core
- C#
- Dapper
- Microsoft SQL Server

## Installation
### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- A database configured with FinancialTracker procedures

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/RileyJuniewic/FinancialTrackerApi.git
   ```
2. Navigate to the project directory:
   ```bash
   cd FinancialTrackerApi
   ```
3. Install dependencies:
   ```bash
   dotnet restore
   ```
4. Set up your database and update `appsettings.json` accordingly.

5. Run the application:
   ```bash
   dotnet run
   ```

# API Endpoints

## User Controller

The UserController handles user management actions such as retrieving, creating, updating, and deleting users.

| HTTP Method | Endpoint                    | Description                      | Auth Required |
|-------------|-----------------------------|----------------------------------|---------------|
| POST        | `/api/User/Login`           | Log user in                      | No            |
| POST        | `/api/User/Register`        | Create a new user                | No            |
| POST        | `/api/User/Logout`          | Log user out                     | Yes           |
| GET         | `/api/User/RefreshJwtToken` | Refresh jwt authentication token | Yes           |

## SavingsAccount Controller

The SavingsAccountController handles user savings account-related actions such as retrieving transactions, opening savings accounts,
and viewing financial data.

| HTTP Method | Endpoint                              | Description                                                            | Auth Required |
|-------------|---------------------------------------|------------------------------------------------------------------------|---------------|
| POST        | `/api/SavingsAccount/OpenAccount`     | Open a new savings account                                             | Yes           |
| POST        | `/api/SavingsAccount/CloseAccount`    | Close a savings account                                                | Yes           |
| POST        | `/api/SavingsAccount/NewTransaction`  | Create a new transaction                                               | Yes           |
| POST        | `/api/SavingsAccount/Transfer`        | Create a transfer transaction                                          | Yes           |
| GET         | `/api/SavingsAccount`                 | Get savings account                                                    | Yes           |
| GET         | `/api/SavingsAccount/Accounts`        | Get user savings accounts                                              | Yes           |
| GET         | `/api/SavingsAccount/Transactions`    | Get user transactions                                                  | Yes           |
| GET         | `/api/SavingsAccount/TransactionSums` | Get the sums of transfers in, transfers out, deposits, and withdrawals | Yes           |
| PATCH       | `/api/SavingsAccount/ChangeName`      | Change savings account name                                            | Yes           |
| PUT         | `/api/SavingsAccount/EditTransaction` | Edit a transaction                                                     | Yes           |

## Contact
For questions or suggestions, feel free to reach out:
- GitHub: [RileyJuniewic](https://github.com/RileyJuniewic)

