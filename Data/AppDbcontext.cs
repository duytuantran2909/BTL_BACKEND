using CommunityHallManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<CommunityHall> CommunityHalls => Set<CommunityHall>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Equipment> Equipments => Set<Equipment>();
    public DbSet<ActivityType> ActivityTypes => Set<ActivityType>();
    public DbSet<BookingRequest> BookingRequests => Set<BookingRequest>();
    public DbSet<UsageInvoice> UsageInvoices => Set<UsageInvoice>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Bảng Nha van hoa
        modelBuilder.Entity<CommunityHall>(e =>
        {
            e.ToTable("Community_Hall");
            e.HasKey(x => x.HallId);

            e.Property(x => x.HallName)
                .HasMaxLength(150)
                .IsRequired();

            e.Property(x => x.Address)
                .HasMaxLength(255)
                .IsRequired();

            e.Property(x => x.ManagerName)
                .HasMaxLength(100);

            e.Property(x => x.Status)
                .HasMaxLength(30)
                .HasDefaultValue("DangHoatDong");

            e.Property(x => x.CreatedDate)
                .HasDefaultValueSql("SYSDATETIME()");
        });

        // Bảng Phong
        modelBuilder.Entity<Room>(e =>
        {
            e.ToTable("Room");
            e.HasKey(x => x.RoomId);

            e.Property(x => x.RoomName)
                .HasMaxLength(150)
                .IsRequired();

            e.Property(x => x.RoomType)
                .HasMaxLength(50)
                .HasDefaultValue("HoiTruong");

            e.Property(x => x.Status)
                .HasMaxLength(30)
                .HasDefaultValue("SanSang");

            e.Property(x => x.BasePricePerHour)
                .HasColumnType("decimal(12,2)");

            e.Property(x => x.CreatedDate)
                .HasDefaultValueSql("SYSDATETIME()");

            e.HasOne(x => x.Hall)
             .WithMany(h => h.Rooms)
             .HasForeignKey(x => x.HallId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Bảng Thiet bi
        modelBuilder.Entity<Equipment>(e =>
        {
            e.ToTable("Equipment");
            e.HasKey(x => x.EquipmentId);

            e.Property(x => x.EquipmentName).HasMaxLength(150).IsRequired();
            e.Property(x => x.EquipmentType).HasMaxLength(50);
            e.Property(x => x.Status).HasMaxLength(30).HasDefaultValue("SanSang");
            e.Property(x => x.UnitPricePerUse).HasColumnType("decimal(12,2)");
            e.Property(x => x.CreatedDate).HasDefaultValueSql("SYSDATETIME()");

            e.HasOne(x => x.Hall)
             .WithMany(h => h.Equipments)
             .HasForeignKey(x => x.HallId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Bảng Loai hoat dong
        modelBuilder.Entity<ActivityType>(e =>
        {
            e.ToTable("Activity_Type");
            e.HasKey(x => x.ActivityId);

            e.Property(x => x.ActivityName).HasMaxLength(150).IsRequired();
            e.Property(x => x.Description).HasMaxLength(255);
            e.Property(x => x.Status).HasMaxLength(30).HasDefaultValue("DangSuDung");
            e.Property(x => x.CreatedDate).HasDefaultValueSql("SYSDATETIME()");
        });

        // Bảng Dat lich
        modelBuilder.Entity<BookingRequest>(e =>
        {
            e.ToTable("Booking_Request");
            e.HasKey(x => x.BookingId);

            e.Property(x => x.RequesterName).HasMaxLength(150).IsRequired();
            e.Property(x => x.RequesterPhone).HasMaxLength(20);
            e.Property(x => x.RequesterUnit).HasMaxLength(150);
            e.Property(x => x.Status).HasMaxLength(30).HasDefaultValue("ChoDuyet");
            e.Property(x => x.Notes).HasMaxLength(255);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSDATETIME()");

            e.HasOne(x => x.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.ActivityType)
                .WithMany(a => a.Bookings)
                .HasForeignKey(x => x.ActivityId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasIndex(x => x.RoomId);
            e.HasIndex(x => x.StartDateTime);
            e.HasIndex(x => x.Status);
        });

        // Bảng Hoa don
        modelBuilder.Entity<UsageInvoice>(e =>
        {
            e.ToTable("Usage_Invoice");
            e.HasKey(x => x.InvoiceId);

            e.Property(x => x.TotalHours).HasColumnType("decimal(10,2)");
            e.Property(x => x.RoomFee).HasColumnType("decimal(12,2)");
            e.Property(x => x.EquipmentFee).HasColumnType("decimal(12,2)");
            e.Property(x => x.OtherFee).HasColumnType("decimal(12,2)");
            e.Property(x => x.DiscountAmount).HasColumnType("decimal(12,2)");
            e.Property(x => x.TotalAmount).HasColumnType("decimal(12,2)");

            e.Property(x => x.InvoiceStatus).HasMaxLength(30).HasDefaultValue("ChuaThanhToan");
            e.Property(x => x.InvoiceDate).HasDefaultValueSql("SYSDATETIME()");

            e.HasOne(x => x.Booking)
                .WithOne(b => b.Invoice)
                .HasForeignKey<UsageInvoice>(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => x.InvoiceStatus);
        });

        // Bảng Thanh toan
        modelBuilder.Entity<Payment>(e =>
        {
            e.ToTable("Payment");
            e.HasKey(x => x.PaymentId);

            e.Property(x => x.PayerName).HasMaxLength(150).IsRequired();
            e.Property(x => x.AmountPaid).HasColumnType("decimal(12,2)");
            e.Property(x => x.PaymentMethod).HasMaxLength(30).HasDefaultValue("TienMat");
            e.Property(x => x.PaymentRefCode).HasMaxLength(100);
            e.Property(x => x.Notes).HasMaxLength(255);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("SYSDATETIME()");

            e.HasOne(x => x.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => x.InvoiceId);
            e.HasIndex(x => x.PaymentDate);
        });
    }
}