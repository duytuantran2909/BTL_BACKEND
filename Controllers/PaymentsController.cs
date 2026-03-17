using CommunityHallManager.Data;
using CommunityHallManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PaymentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/payments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetAll()
    {
        var list = await _context.Payments
            .Include(p => p.Invoice)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
        return Ok(list);
    }

    // GET api/payments/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Payment>> GetById(int id)
    {
        var p = await _context.Payments
            .Include(x => x.Invoice)
            .FirstOrDefaultAsync(x => x.PaymentId == id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    // POST api/payments
    [HttpPost]
    public async Task<ActionResult<Payment>> Create(Payment payment)
    {
        var inv = await _context.UsageInvoices.FirstOrDefaultAsync(i => i.InvoiceId == payment.InvoiceId);
        if (inv == null) return BadRequest("Hoa don khong ton tai.");

        if (inv.InvoiceStatus == "DaHuy") return BadRequest("Hoa don da huy, khong the thanh toan.");

        if (payment.AmountPaid <= 0) return BadRequest("So tien thanh toan phai > 0.");

        var paidSum = await _context.Payments
            .Where(p => p.InvoiceId == payment.InvoiceId)
            .SumAsync(p => (decimal?)p.AmountPaid) ?? 0m;

        if (paidSum + payment.AmountPaid > inv.TotalAmount)
            return BadRequest("So tien thanh toan vuot qua tong tien hoa don.");

        payment.PaymentDate = payment.PaymentDate == default ? DateTime.UtcNow : payment.PaymentDate;
        payment.CreatedAt = DateTime.UtcNow;
        _context.Payments.Add(payment);

        // Nếu trả đủ thì cập nhật trạng thái hóa đơn
        if (paidSum + payment.AmountPaid == inv.TotalAmount)
            inv.InvoiceStatus = "DaThanhToan";

        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = payment.PaymentId }, payment);
    }

    // DELETE api/payments/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
        if (payment == null) return NotFound();

        var inv = await _context.UsageInvoices.FirstOrDefaultAsync(i => i.InvoiceId == payment.InvoiceId);

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();

        if (inv != null && inv.InvoiceStatus != "DaHuy")
        {
            var paidSum = await _context.Payments
                .Where(p => p.InvoiceId == inv.InvoiceId)
                .SumAsync(p => (decimal?)p.AmountPaid) ?? 0m;

            inv.InvoiceStatus = paidSum >= inv.TotalAmount ? "DaThanhToan" : "ChuaThanhToan";
            await _context.SaveChangesAsync();
        }

        return NoContent();
    }
}

