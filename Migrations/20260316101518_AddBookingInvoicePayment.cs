using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityHallManager.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingInvoicePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Room",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "SanSang",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "AVAILABLE");

            migrationBuilder.AlterColumn<string>(
                name: "RoomType",
                table: "Room",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "HoiTruong",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "HALL");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Community_Hall",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "DangHoatDong",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "ACTIVE");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerName",
                table: "Community_Hall",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Activity_Type",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DefaultDurationHours = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "DangSuDung"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_Type", x => x.ActivityId);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    EquipmentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EquipmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPricePerUse = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "SanSang"),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipment_Community_Hall_HallId",
                        column: x => x.HallId,
                        principalTable: "Community_Hall",
                        principalColumn: "HallId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Account",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "CanBo"),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "DangHoatDong"),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Account", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Booking_Request",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: true),
                    RequesterName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RequesterPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RequesterUnit = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedParticipants = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "ChoDuyet"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking_Request", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Request_Activity_Type_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity_Type",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Booking_Request_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Request_User_Account_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "User_Account",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Booking_Request_User_Account_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User_Account",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Usage_Invoice",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    TotalHours = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    RoomFee = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    EquipmentFee = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    OtherFee = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    InvoiceStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "ChuaThanhToan"),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usage_Invoice", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Usage_Invoice_Booking_Request_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking_Request",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usage_Invoice_Community_Hall_HallId",
                        column: x => x.HallId,
                        principalTable: "Community_Hall",
                        principalColumn: "HallId");
                    table.ForeignKey(
                        name: "FK_Usage_Invoice_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_Usage_Invoice_User_Account_ProcessedBy",
                        column: x => x.ProcessedBy,
                        principalTable: "User_Account",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    PayerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "TienMat"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentRefCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ConfirmedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Usage_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Usage_Invoice",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_User_Account_ConfirmedBy",
                        column: x => x.ConfirmedBy,
                        principalTable: "User_Account",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Request_ActivityId",
                table: "Booking_Request",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Request_ApprovedBy",
                table: "Booking_Request",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Request_CreatedBy",
                table: "Booking_Request",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Request_RoomId",
                table: "Booking_Request",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Request_StartDateTime",
                table: "Booking_Request",
                column: "StartDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Request_Status",
                table: "Booking_Request",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_HallId",
                table: "Equipment",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ConfirmedBy",
                table: "Payment",
                column: "ConfirmedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InvoiceId",
                table: "Payment",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentDate",
                table: "Payment",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Usage_Invoice_BookingId",
                table: "Usage_Invoice",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usage_Invoice_HallId",
                table: "Usage_Invoice",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Usage_Invoice_InvoiceStatus",
                table: "Usage_Invoice",
                column: "InvoiceStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Usage_Invoice_ProcessedBy",
                table: "Usage_Invoice",
                column: "ProcessedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Usage_Invoice_RoomId",
                table: "Usage_Invoice",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Account_Username",
                table: "User_Account",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Usage_Invoice");

            migrationBuilder.DropTable(
                name: "Booking_Request");

            migrationBuilder.DropTable(
                name: "Activity_Type");

            migrationBuilder.DropTable(
                name: "User_Account");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Room",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "AVAILABLE",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "SanSang");

            migrationBuilder.AlterColumn<string>(
                name: "RoomType",
                table: "Room",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "HALL",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "HoiTruong");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Community_Hall",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "ACTIVE",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "DangHoatDong");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerName",
                table: "Community_Hall",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
