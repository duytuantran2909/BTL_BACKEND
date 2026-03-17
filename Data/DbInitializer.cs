using CommunityHallManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Data;

public static class DbInitializer
{
    public static async Task ResetMigrateAndSeedAsync(AppDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        await SeedIfEmptyAsync(context);
    }

    public static async Task SeedIfEmptyAsync(AppDbContext context)
    {
        await SeedHallsAndRoomsIfEmptyAsync(context);
        await SeedActivityTypesIfEmptyAsync(context);
        await SeedBookingsInvoicesPaymentsIfEmptyAsync(context);
    }

    private static async Task SeedHallsAndRoomsIfEmptyAsync(AppDbContext context)
    {
        if (await context.CommunityHalls.AnyAsync())
            return;

        var halls = new List<CommunityHall>
        {
            new()
            {
                HallName = "Nhà văn hóa Trung tâm",
                Address = "Số 1, Đường Chính, Xã A",
                ManagerName = "Nguyễn Văn An",
                PhoneNumber = "0901000001",
                Status = "DangHoatDong",
                Rooms =
                {
                    new Room { RoomName = "Hội trường lớn", RoomType = "HoiTruong", Capacity = 300, BasePricePerHour = 300_000m, Status = "SanSang" },
                    new Room { RoomName = "Phòng họp 1", RoomType = "PhongHop", Capacity = 40, BasePricePerHour = 120_000m, Status = "SanSang" },
                    new Room { RoomName = "Phòng họp 2", RoomType = "PhongHop", Capacity = 30, BasePricePerHour = 100_000m, Status = "SanSang" }
                }
            },
            new()
            {
                HallName = "Nhà văn hóa Thôn Đông",
                Address = "Thôn Đông, Xã A",
                ManagerName = "Trần Thị Bình",
                PhoneNumber = "0901000002",
                Status = "DangHoatDong",
                Rooms =
                {
                    new Room { RoomName = "Hội trường thôn", RoomType = "HoiTruong", Capacity = 120, BasePricePerHour = 180_000m, Status = "SanSang" },
                    new Room { RoomName = "Sân thể thao", RoomType = "SanTheThao", Capacity = 200, BasePricePerHour = 150_000m, Status = "SanSang" }
                }
            },
            new()
            {
                HallName = "Nhà văn hóa Thôn Tây",
                Address = "Thôn Tây, Xã A",
                ManagerName = "Lê Văn Cường",
                PhoneNumber = "0901000003",
                Status = "DangHoatDong",
                Rooms =
                {
                    new Room { RoomName = "Phòng sinh hoạt cộng đồng", RoomType = "Khac", Capacity = 60, BasePricePerHour = 110_000m, Status = "SanSang" }
                }
            }
        };

        context.CommunityHalls.AddRange(halls);
        await context.SaveChangesAsync();
    }

    private static async Task SeedActivityTypesIfEmptyAsync(AppDbContext context)
    {
        if (await context.ActivityTypes.AnyAsync())
            return;

        var now = DateTime.UtcNow;
        context.ActivityTypes.AddRange(
            new ActivityType { ActivityName = "Họp dân", Description = "Họp tổ dân phố / họp thôn", DefaultDurationHours = 2, Status = "DangSuDung", CreatedDate = now },
            new ActivityType { ActivityName = "Hội nghị", Description = "Hội nghị chuyên đề", DefaultDurationHours = 3, Status = "DangSuDung", CreatedDate = now },
            new ActivityType { ActivityName = "Văn nghệ", Description = "Tập luyện / biểu diễn", DefaultDurationHours = 2, Status = "DangSuDung", CreatedDate = now },
            new ActivityType { ActivityName = "Thể thao", Description = "Giao lưu thể thao", DefaultDurationHours = 2, Status = "DangSuDung", CreatedDate = now }
        );

        await context.SaveChangesAsync();
    }

    private static async Task SeedBookingsInvoicesPaymentsIfEmptyAsync(AppDbContext context)
    {
        if (await context.BookingRequests.AnyAsync() ||
            await context.UsageInvoices.AnyAsync() ||
            await context.Payments.AnyAsync())
            return;

        var rooms = await context.Rooms.OrderBy(r => r.RoomId).ToListAsync();
        if (!rooms.Any())
            return;

        var activities = await context.ActivityTypes.OrderBy(a => a.ActivityId).ToListAsync();
        var a1 = activities.FirstOrDefault()?.ActivityId;
        var a2 = activities.Skip(1).FirstOrDefault()?.ActivityId;

        // Tao booking o nhieu thang de bao cao doanh thu co so lieu
        // (dung DateTime.UtcNow lam moc, lui ve 2 thang).
        var baseMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var m0 = baseMonth.AddMonths(-2); // thang -2
        var m1 = baseMonth.AddMonths(-1); // thang -1
        var m2 = baseMonth;              // thang hien tai

        var bookings = new List<BookingRequest>
        {
            NewBooking(rooms[0].RoomId, a1, "Tổ dân phố 1", "0902000001", "Tổ 1", m0.AddDays(5).AddHours(8),  m0.AddDays(5).AddHours(11), 120, "DaHoanThanh"),
            NewBooking(rooms[1].RoomId, a2, "UBND xã",     "0902000002", "UBND", m0.AddDays(12).AddHours(13), m0.AddDays(12).AddHours(15), 35,  "DaDuyet"),
            NewBooking(rooms[2].RoomId, a1, "Đoàn thanh niên", "0902000003", "ĐTN", m1.AddDays(3).AddHours(18), m1.AddDays(3).AddHours(20), 50, "DaHoanThanh"),
            NewBooking(rooms[3].RoomId, a2, "Hội phụ nữ",   "0902000004", "HPN",  m1.AddDays(18).AddHours(7),  m1.AddDays(18).AddHours(10), 80, "DaHoanThanh"),
            NewBooking(rooms[4].RoomId, a1, "CLB bóng đá",  "0902000005", "CLB",  m2.AddDays(7).AddHours(16),  m2.AddDays(7).AddHours(18), 60, "DaDuyet"),
            NewBooking(rooms[0].RoomId, a2, "Hội cựu chiến binh", "0902000006", "CCB", m2.AddDays(15).AddHours(8), m2.AddDays(15).AddHours(12), 150, "DaHoanThanh"),
        };

        context.BookingRequests.AddRange(bookings);
        await context.SaveChangesAsync();

        // Tao hoa don tu booking (don gian: totalHours * basePrice + phi khac - giam gia)
        var roomPrice = rooms.ToDictionary(r => r.RoomId, r => r.BasePricePerHour);
        var invoices = new List<UsageInvoice>();
        foreach (var b in bookings)
        {
            var totalHoursRaw = (decimal)(b.EndDateTime - b.StartDateTime).TotalMinutes / 60m;
            var totalHours = Math.Ceiling(totalHoursRaw * 2m) / 2m;
            var roomFee = totalHours * roomPrice[b.RoomId];
            var equipFee = b.RoomId == rooms[4].RoomId ? 50_000m : 0m;
            var otherFee = 20_000m;
            var discount = b.ExpectedParticipants >= 120 ? 30_000m : 0m;
            var total = roomFee + equipFee + otherFee - discount;
            if (total < 0) total = 0;

            var invDate = b.EndDateTime; // gan theo thoi diem su dung
            invoices.Add(new UsageInvoice
            {
                BookingId = b.BookingId,
                TotalHours = totalHours,
                RoomFee = roomFee,
                EquipmentFee = equipFee,
                OtherFee = otherFee,
                DiscountAmount = discount,
                TotalAmount = total,
                InvoiceStatus = "ChuaThanhToan",
                InvoiceDate = invDate,
                DueDate = invDate.Date.AddDays(7)
            });
        }

        context.UsageInvoices.AddRange(invoices);
        await context.SaveChangesAsync();

        // Tao thanh toan: mot so thanh toan du, mot so thanh toan mot phan
        var payments = new List<Payment>();
        foreach (var inv in invoices)
        {
            var booking = bookings.First(b => b.BookingId == inv.BookingId);

            if (booking.Status == "DaHoanThanh")
            {
                // thanh toan du
                payments.Add(new Payment
                {
                    InvoiceId = inv.InvoiceId,
                    PayerName = booking.RequesterName,
                    AmountPaid = inv.TotalAmount,
                    PaymentMethod = "ChuyenKhoan",
                    PaymentDate = inv.InvoiceDate.AddHours(1),
                    PaymentRefCode = "SEED",
                    Notes = "Seed demo",
                    CreatedAt = DateTime.UtcNow
                });
                inv.InvoiceStatus = "DaThanhToan";
            }
            else
            {
                // thanh toan 1 phan (50%)
                var half = Math.Round(inv.TotalAmount * 0.5m, 2);
                if (half > 0)
                {
                    payments.Add(new Payment
                    {
                        InvoiceId = inv.InvoiceId,
                        PayerName = booking.RequesterName,
                        AmountPaid = half,
                        PaymentMethod = "TienMat",
                        PaymentDate = inv.InvoiceDate.AddHours(2),
                        PaymentRefCode = "SEED",
                        Notes = "Seed demo (partial)",
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        context.Payments.AddRange(payments);
        await context.SaveChangesAsync();
    }

    private static BookingRequest NewBooking(
        int roomId,
        int? activityId,
        string requesterName,
        string requesterPhone,
        string requesterUnit,
        DateTime startUtc,
        DateTime endUtc,
        int expectedParticipants,
        string status)
        => new()
        {
            RoomId = roomId,
            ActivityId = activityId,
            RequesterName = requesterName,
            RequesterPhone = requesterPhone,
            RequesterUnit = requesterUnit,
            StartDateTime = DateTime.SpecifyKind(startUtc, DateTimeKind.Utc),
            EndDateTime = DateTime.SpecifyKind(endUtc, DateTimeKind.Utc),
            ExpectedParticipants = expectedParticipants,
            Status = status,
            Notes = "Seed demo",
            CreatedAt = DateTime.UtcNow
        };
}

