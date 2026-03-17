namespace CommunityHallManager.Models;

/// <summary>
/// Thiết bị cộng đồng (âm thanh, đèn, ghế, micro...)
/// </summary>
public class Equipment
{
    public int EquipmentId { get; set; }

    public int HallId { get; set; }

    public string EquipmentName { get; set; } = null!;

    // Loai: "AmThanh", "Den", "Micro", "Ghe", "Ban", "Khac"
    public string? EquipmentType { get; set; }

    public int Quantity { get; set; } = 1;

    /// <summary>Giá thuê 1 lần sử dụng (VNĐ)</summary>
    public decimal UnitPricePerUse { get; set; } = 0;

    // Trang thai: "SanSang", "DangSuDung", "Hong", "DangBaoTri"
    public string Status { get; set; } = "SanSang";

    public DateTime? LastMaintenanceDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public CommunityHall? Hall { get; set; }
}
