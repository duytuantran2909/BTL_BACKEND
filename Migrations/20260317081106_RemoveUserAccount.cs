using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityHallManager.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Request_User_Account_ApprovedBy",
                table: "Booking_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Request_User_Account_CreatedBy",
                table: "Booking_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_Account_ConfirmedBy",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Usage_Invoice_User_Account_ProcessedBy",
                table: "Usage_Invoice");

            migrationBuilder.DropTable(
                name: "User_Account");

            migrationBuilder.DropIndex(
                name: "IX_Usage_Invoice_ProcessedBy",
                table: "Usage_Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Payment_ConfirmedBy",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Booking_Request_ApprovedBy",
                table: "Booking_Request");

            migrationBuilder.DropIndex(
                name: "IX_Booking_Request_CreatedBy",
                table: "Booking_Request");

            migrationBuilder.DropColumn(
                name: "ProcessedBy",
                table: "Usage_Invoice");

            migrationBuilder.DropColumn(
                name: "ConfirmedBy",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Booking_Request");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Booking_Request");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessedBy",
                table: "Usage_Invoice",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmedBy",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedBy",
                table: "Booking_Request",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Booking_Request",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User_Account",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "CanBo"),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "DangHoatDong"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Account", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usage_Invoice_ProcessedBy",
                table: "Usage_Invoice",
                column: "ProcessedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ConfirmedBy",
                table: "Payment",
                column: "ConfirmedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Request_ApprovedBy",
                table: "Booking_Request",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Request_CreatedBy",
                table: "Booking_Request",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_Account_Username",
                table: "User_Account",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Request_User_Account_ApprovedBy",
                table: "Booking_Request",
                column: "ApprovedBy",
                principalTable: "User_Account",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Request_User_Account_CreatedBy",
                table: "Booking_Request",
                column: "CreatedBy",
                principalTable: "User_Account",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_Account_ConfirmedBy",
                table: "Payment",
                column: "ConfirmedBy",
                principalTable: "User_Account",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Usage_Invoice_User_Account_ProcessedBy",
                table: "Usage_Invoice",
                column: "ProcessedBy",
                principalTable: "User_Account",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
