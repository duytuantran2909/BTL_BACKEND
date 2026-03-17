namespace CommunityHallManager.Models;

public class Room
{
    public int RoomId { get; set; }

    public int HallId { get; set; }

    // Ten phong / hoi truong
    public string RoomName { get; set; } = null!;

    // Loai phong: "HoiTruong", "PhongHop", "SanTheThao", "Khac"
    public string RoomType { get; set; } = "HoiTruong";

    public int? Capacity { get; set; }

    public decimal BasePricePerHour { get; set; }

    // Trang thai: "SanSang", "KhongSuDung", "DangBaoTri"
    public string Status { get; set; } = "SanSang";

    public DateTime CreatedDate { get; set; }

    public CommunityHall? Hall { get; set; }

    public ICollection<BookingRequest> Bookings { get; set; } = new List<BookingRequest>();
}