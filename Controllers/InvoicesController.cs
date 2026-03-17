using CommunityHallManager.Data;
using CommunityHallManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public InvoicesController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/invoices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsageInvoice>>> GetAll()
    {
        var list = await _context.UsageInvoices
            .Include(i => i.Booking)
            .ThenInclude(b => b!.Room)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
        return Ok(list);
    }

    // GET api/invoices/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsageInvoice>> GetById(int id)
    {
        var inv = await _context.UsageInvoices
            .Include(i => i.Booking)
            .ThenInclude(b => b!.Room)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);

        if (inv == null) return NotFound();
        return Ok(inv);
    }

    public class GenerateInvoiceDto
    {
        public int BookingId { get; set; }
        public decimal EquipmentFee { get; set; } = 0;
        public decimal OtherFee { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public int DueDays { get; set; } = 7;
    }

    // POST api/invoices/generate
    //  totalHours * basePrice + equipmentFee + otherFee - discount
    [HttpPost("generate")]
    public async Task<ActionResult<UsageInvoice>> Generate(GenerateInvoiceDto dto)
    {
        var booking = await _context.BookingRequests
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.BookingId == dto.BookingId);

        if (booking == null) return BadRequest("Booking khong ton tai.");
        if (booking.Status != "DaDuyet" && booking.Status != "DaHoanThanh")
            return BadRequest("Chi tao hoa don khi booking DaDuyet hoac DaHoanThanh.");
        var existed = await _context.UsageInvoices.AnyAsync(i => i.BookingId == dto.BookingId);
        if (existed) return BadRequest("Booking nay da co hoa don.");
        if (booking.Room == null) return BadRequest("Phong khong ton tai.");
        var duration = booking.EndDateTime - booking.StartDateTime;
        if (duration.TotalMinutes <= 0) return BadRequest("Thoi gian booking khong hop le.");

        // Làm tròn 0.5 giờ 
        var totalHoursRaw = (decimal)duration.TotalMinutes / 60m;
        var totalHours = Math.Ceiling(totalHoursRaw * 2m) / 2m;
        var roomFee = totalHours * booking.Room.BasePricePerHour;
        var totalAmount = roomFee + dto.EquipmentFee + dto.OtherFee - dto.DiscountAmount;
        if (totalAmount < 0) totalAmount = 0;

        var inv = new UsageInvoice
        {
            BookingId = booking.BookingId,
            TotalHours = totalHours,
            RoomFee = roomFee,
            EquipmentFee = dto.EquipmentFee,
            OtherFee = dto.OtherFee,
            DiscountAmount = dto.DiscountAmount,
            TotalAmount = totalAmount,
            InvoiceStatus = "ChuaThanhToan",
            InvoiceDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.Date.AddDays(dto.DueDays <= 0 ? 7 : dto.DueDays)
        };

        _context.UsageInvoices.Add(inv);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = inv.InvoiceId }, inv);
    }

    // PUT api/invoices/5/cancel
    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var inv = await _context.UsageInvoices.FindAsync(id);
        if (inv == null) return NotFound();

        inv.InvoiceStatus = "DaHuy";
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/invoices/5
    // Chi cho xoa khi chua phat sinh thanh toan (tranh mat lich su giao dich).
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var inv = await _context.UsageInvoices
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);

        if (inv == null) return NotFound();

        if (inv.Payments.Any())
            return BadRequest("Hoa don da co thanh toan, khong the xoa.");

        _context.UsageInvoices.Remove(inv);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

