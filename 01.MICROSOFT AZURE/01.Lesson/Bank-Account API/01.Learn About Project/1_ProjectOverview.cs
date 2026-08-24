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
    // So, We create a BankDTO class with only those three properties: BankId, BankName, and BranchCode.
    #endregion

    #region --- 05. DATA ---
    // The ApplicationDbContext is the main EF Core class that manages database interaction.
    // Inside the Data folder, create a class named ApplicationDbContext.cs
    #endregion
}

