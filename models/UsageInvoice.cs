using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityHallManager.Models;

/// <summary>
/// Hóa đơn sử dụng (tính tiền theo giờ + phí khác).
/// </summary>
public class UsageInvoice
{
    public int InvoiceId { get; set; }

    public int BookingId { get; set; }

    // Normalized: HallId/RoomId are derived from Booking -> Room -> Hall
    [NotMapped]
    public int HallId => Booking?.Room?.HallId ?? 0;

    [NotMapped]
    public int RoomId => Booking?.RoomId ?? 0;

    public decimal TotalHours { get; set; }
    public decimal RoomFee { get; set; }
    public decimal EquipmentFee { get; set; }
    public decimal OtherFee { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }

    // Trang thai: "ChuaThanhToan", "DaThanhToan", "DaHuy"
    public string InvoiceStatus { get; set; } = "ChuaThanhToan";

    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }

    public BookingRequest? Booking { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

