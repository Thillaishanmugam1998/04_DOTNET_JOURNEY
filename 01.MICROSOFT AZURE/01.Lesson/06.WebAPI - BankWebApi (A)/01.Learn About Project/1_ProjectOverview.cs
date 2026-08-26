using Microsoft.AspNetCore.Hosting.Server;
using System.Security.Principal;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bank_Account_API._01.Learn_About_Project
{

    #region --- 01. PROJECT OVERVIEW ---
    //Bank-Account API(3 tables)
    //Banks – BankId, BankName, BranchCode, Address, IFSCCode
    //Accounts – AccountId, AccountNumber, AccountHolderName, Balance, AccountType, BankId(FK → Banks)
    //Transactions – TransactionId, AccountId(FK → Accounts), TransactionType(Credit/Debit), Amount, TransactionDate, Description
    #endregion

    #region --- 02. PROJECT STRUCTURE ---
    /*
     * 01.Models
     * 02.DTOs
     * 03.Data
     * 04.Repositories
     * 05.Services
     * 06.Controllers
     * 07.Program.cs
     */


    //Run these commands into Package Manager Console:
    //------------------------------------------------
    //Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.0
    //Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.0

    //Why these packages?
    //Microsoft.EntityFrameworkCore.SqlServer allows EF Core to work with SQL Server.
    //Microsoft.EntityFrameworkCore.Tools give us migration commands such as Add-Migration and Update-Database.
    #endregion

    #region --- 03. MODELS ---
    // Models - In this folder, we create a table for each class. Each class represents a table in the database.
    // The properties of the class represent the columns of the table.

    // 1.Inside the Models folder, create a class named Banks.cs
    // 2.Inside the Models folder, create a class named Accounts.cs
    // 3.Inside the Models folder, create a class named Transactions.cs
    #endregion

    #region --- 04. DTOs ---
    // DTO means Data Transfer Object.DTOs are used to control what data enters and leaves the API.
    // Instead of exposing the database entity directly everywhere, we use DTOs for cleaner API contracts.

    //Example 
    // Select * from Banks
    // Instead of returning the entire Banks entity, we can create a BankDTO that only includes the necessary fields.
    // Select BankId, BankName, BranchCode from Banks;
    // So, We create a BankDTO class with only those three properties: BankId, BankName, and BankCode.
    #endregion

    #region --- 05. DATA ---
    // The ApplicationDbContext is the main EF Core class that manages database interaction.
    // Inside the Data folder, create a class named ApplicationDbContext.cs
    #endregion

    #region --- 06. SERVICES ---
    // Services are where the business logic of the application resides.
    // They interact with repositories to fetch or manipulate data.

    // BAL(Business Access Layer) = Services
    // Controller ---> BAL 
    // Controller ---> Service 
    #endregion

    #region --- 07. REPOSITORIES ---
    // Repositories are responsible for data access.
    // They interact with the database context to perform CRUD operations.

    // DAL(Data Access Layer) = Repositories
    // Controller ---> BAL ---> DAL
    // Controller ---> Service ---> Repository
    #endregion

    #region --- 08. CONTROLLERS ---
    // Controllers are the entry point for API requests.
    #endregion

    #region --- 09. DATABASE CREATION & EF CORE MIGRATIONS ---
    /*
     * How to Create the Database and Apply Migrations Step-by-Step:
     * -------------------------------------------------------------
     * 
     * Step 1: Open Package Manager Console (PMC)
     *         In Visual Studio, go to: Tools -> NuGet Package Manager -> Package Manager Console.
     * 
     * Step 2: Set the Default Project
     *         In the Package Manager Console, set the 'Default project' dropdown to 'Bank-Account API'.
     * 
     * Step 3: Run Add-Migration Command
     *         Run the following command in the Package Manager Console to generate the migration files:
     *         Add-Migration InitialCreate
     * 
     * Step 4: Run Update-Database Command
     *         Run the following command in the Package Manager Console to create the database and tables:
     *         Update-Database
     * 
     * Step 5: Verify in SQL Server Management Studio (SSMS)
     *         Open SSMS, connect to SQL Server (Data Source=RAJESH), and check that:
     *         - The database 'BankAccountDb' is created.
     *         - The tables (Banks, Accounts, Transactions) are created and seeded with initial data.
     * 
     * Alternative (Using .NET Core CLI via Command Prompt / PowerShell):
     * -----------------------------------------------------------
     * Run these commands from the solution directory:
     * 1. dotnet ef migrations add InitialCreate --project "Bank-Account API"
     * 2. dotnet ef database update --project "Bank-Account API"
     */
    #endregion
}
