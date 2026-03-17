using CommunityHallManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/reports/revenue
    // Báo cáo doanh thu theo tháng (đơn giản)
    [HttpGet("revenue")]
    public async Task<IActionResult> Revenue()
    {
        // EF Core khong translate duoc string format trong query SQL,
        // nen chi group/sum trong DB, sau do format chuoi o client.
        var rows = await _context.UsageInvoices
            .Where(i => i.InvoiceStatus != "DaHuy")
            .GroupBy(i => new { i.InvoiceDate.Year, i.InvoiceDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TongHoaDon = g.Count(),
                TongTien = g.Sum(x => x.TotalAmount)
            })
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ToListAsync();

        var data = rows.Select(x => new
        {
            Thang = $"{x.Year:D4}-{x.Month:D2}",
            x.TongHoaDon,
            x.TongTien
        });

        return Ok(data);
    }

    // GET api/reports/usage
    // Tần suất sử dụng theo tháng (đơn giản)
    [HttpGet("usage")]
    public async Task<IActionResult> Usage()
    {
        var rows = await _context.BookingRequests
            .Where(b => b.Status == "DaDuyet" || b.Status == "DaHoanThanh")
            .GroupBy(b => new { b.StartDateTime.Year, b.StartDateTime.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TongLuotDat = g.Count()
            })
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ToListAsync();

        var data = rows.Select(x => new
        {
            Thang = $"{x.Year:D4}-{x.Month:D2}",
            x.TongLuotDat
        });

        return Ok(data);
    }
}

