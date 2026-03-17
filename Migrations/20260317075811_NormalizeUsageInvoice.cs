using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityHallManager.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeUsageInvoice : Migration
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
                name: "FK_Usage_Invoice_Community_Hall_HallId",
                table: "Usage_Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Usage_Invoice_Room_RoomId",
                table: "Usage_Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Usage_Invoice_HallId",
                table: "Usage_Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Usage_Invoice_RoomId",
                table: "Usage_Invoice");

            migrationBuilder.DropColumn(
                name: "HallId",
                table: "Usage_Invoice");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Usage_Invoice");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Request_User_Account_ApprovedBy",
                table: "Booking_Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Request_User_Account_CreatedBy",
                table: "Booking_Request");

            migrationBuilder.AddColumn<int>(
                name: "HallId",
                table: "Usage_Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Usage_Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Usage_Invoice_HallId",
                table: "Usage_Invoice",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Usage_Invoice_RoomId",
                table: "Usage_Invoice",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Request_User_Account_ApprovedBy",
                table: "Booking_Request",
                column: "ApprovedBy",
                principalTable: "User_Account",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Request_User_Account_CreatedBy",
                table: "Booking_Request",
                column: "CreatedBy",
                principalTable: "User_Account",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Usage_Invoice_Community_Hall_HallId",
                table: "Usage_Invoice",
                column: "HallId",
                principalTable: "Community_Hall",
                principalColumn: "HallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usage_Invoice_Room_RoomId",
                table: "Usage_Invoice",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");
        }
    }
}
