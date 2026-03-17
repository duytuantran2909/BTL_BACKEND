namespace CommunityHallManager.Models;

/// <summary>
/// Thanh toán hóa đơn.
/// </summary>
public class Payment
{
    public int PaymentId { get; set; }

    public int InvoiceId { get; set; }

    public string PayerName { get; set; } = null!;

    public decimal AmountPaid { get; set; }

    // Phuong thuc: "TienMat", "ChuyenKhoan", "Online"
    public string PaymentMethod { get; set; } = "TienMat";

    public DateTime PaymentDate { get; set; }

    public string? PaymentRefCode { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public UsageInvoice? Invoice { get; set; }
}

