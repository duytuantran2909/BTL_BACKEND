namespace CommunityHallManager.Models;

public class CommunityHall
{
    public int HallId { get; set; }

    // Tên nhà văn hóa
    public string HallName { get; set; } = null!;

    // Địa chỉ
    public string Address { get; set; } = null!;

    // Người quản lý (cán bộ phụ trách)
    public string? ManagerName { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    // Trang thai: "DangHoatDong", "TamDung", "NgungHoatDong"
    public string Status { get; set; } = "DangHoatDong";

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}