namespace CommunityHallManager.Models;

/// <summary>
/// Loại hoạt động: họp dân, tập văn nghệ, thể thao, hội nghị...
/// </summary>
public class ActivityType
{
    public int ActivityId { get; set; }

    public string ActivityName { get; set; } = null!;

    public string? Description { get; set; }

    /// <summary>Thời lượng mặc định (giờ) khi đặt lịch</summary>
    public int DefaultDurationHours { get; set; } = 2;

    // Trang thai: "DangSuDung", "TamDung"
    public string Status { get; set; } = "DangSuDung";

    public DateTime CreatedDate { get; set; }

    public ICollection<BookingRequest> Bookings { get; set; } = new List<BookingRequest>();
}
