using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bank_Account_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IFSCCode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountHolderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "BankId", "Address", "BankName", "BranchCode", "IFSCCode" },
                values: new object[,]
                {
                    { 1, "Mumbai, Maharashtra", "State Bank of India", "SBI001", "SBIN0000001" },
                    { 2, "Mumbai, Maharashtra", "HDFC Bank", "HDFC01", "HDFC0000001" },
                    { 3, "Mumbai, Maharashtra", "ICICI Bank", "ICICI1", "ICIC0000001" },
                    { 4, "Mumbai, Maharashtra", "Axis Bank", "AXIS01", "UTIB0000001" },
                    { 5, "New Delhi, Delhi", "Punjab National Bank", "PNB001", "PUNB0000001" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "AccountHolderName", "AccountNumber", "AccountType", "Balance", "BankId" },
                values: new object[,]
                {
                    { 1, "Thillai Shanmugam", "100010001001", "Savings", 50000.00m, 1 },
                    { 2, "Tamilvani", "200020002002", "Savings", 75000.00m, 2 },
                    { 3, "Dharshini", "300030003003", "Current", 60000.00m, 3 },
                    { 4, "Abinaya", "400040004004", "Savings", 90000.00m, 4 }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountId", "Amount", "Description", "TransactionDate", "TransactionType" },
                values: new object[,]
                {
                    { 1, 1, 10000.00m, "Initial Deposit", new DateTime(2026, 8, 20, 10, 0, 0, 0, DateTimeKind.Utc), "Credit" },
                    { 2, 1, 2000.00m, "ATM Withdrawal", new DateTime(2026, 8, 22, 14, 30, 0, 0, DateTimeKind.Utc), "Debit" },
                    { 3, 2, 15000.00m, "Salary Credit", new DateTime(2026, 8, 21, 11, 15, 0, 0, DateTimeKind.Utc), "Credit" },
                    { 4, 3, 5000.00m, "Online Shopping", new DateTime(2026, 8, 23, 9, 45, 0, 0, DateTimeKind.Utc), "Debit" },
                    { 5, 4, 25000.00m, "Funds Transfer", new DateTime(2026, 8, 24, 16, 0, 0, 0, DateTimeKind.Utc), "Credit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BankId",
                table: "Accounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
