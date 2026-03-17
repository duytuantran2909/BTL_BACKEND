namespace CommunityHallManager.Models;

/// <summary>
/// Phiếu đăng ký/đặt lịch sử dụng phòng.
/// </summary>
public class BookingRequest
{
    public int BookingId { get; set; }

    public int RoomId { get; set; }
    public int? ActivityId { get; set; }

    public string RequesterName { get; set; } = null!;
    public string? RequesterPhone { get; set; }
    public string? RequesterUnit { get; set; }

    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    public int? ExpectedParticipants { get; set; }

    // Trang thai: "ChoDuyet", "DaDuyet", "TuChoi", "DaHuy", "DaHoanThanh"
    public string Status { get; set; } = "ChoDuyet";

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public Room? Room { get; set; }
    public ActivityType? ActivityType { get; set; }

    public UsageInvoice? Invoice { get; set; }
}

