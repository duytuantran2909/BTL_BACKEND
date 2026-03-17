using CommunityHallManager.Data;
using CommunityHallManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingRequest>>> GetAll()
    {
        var list = await _context.BookingRequests
            .Include(b => b.Room)
            .Include(b => b.ActivityType)
            .OrderByDescending(b => b.StartDateTime)
            .ToListAsync();
        return Ok(list);
    }

    // GET api/bookings/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingRequest>> GetById(int id)
    {
        var booking = await _context.BookingRequests
            .Include(b => b.Room)
            .Include(b => b.ActivityType)
            .Include(b => b.Invoice)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null) return NotFound();
        return Ok(booking);
    }

    // POST api/bookings
    [HttpPost]
    public async Task<ActionResult<BookingRequest>> Create(BookingRequest booking)
    {
        if (booking.RoomId <= 0)
            return BadRequest("RoomId khong hop le.");

        if (string.IsNullOrWhiteSpace(booking.RequesterName))
            return BadRequest("RequesterName la bat buoc.");

        if (booking.StartDateTime == default || booking.EndDateTime == default)
            return BadRequest("StartDateTime/EndDateTime khong hop le. Dinh dang khuyen nghi: yyyy-MM-ddTHH:mm (vi du: 2026-03-20T18:00).");

        if (booking.EndDateTime <= booking.StartDateTime)
            return BadRequest("Thoi gian ket thuc phai lon hon thoi gian bat dau.");

        var roomExists = await _context.Rooms.AnyAsync(r => r.RoomId == booking.RoomId);
        if (!roomExists)
            return BadRequest("Phong khong ton tai. Hay tao phong truoc khi dat lich.");

        if (booking.ActivityId.HasValue)
        {
            var activityExists = await _context.ActivityTypes.AnyAsync(a => a.ActivityId == booking.ActivityId.Value);
            if (!activityExists)
                return BadRequest("Loai hoat dong (ActivityId) khong ton tai.");
        }

        // Check trung lich: giao nhau khi (start < existingEnd) && (end > existingStart)
        var hasConflict = await _context.BookingRequests.AnyAsync(b =>
            b.RoomId == booking.RoomId &&
            (b.Status == "ChoDuyet" || b.Status == "DaDuyet") &&
            booking.StartDateTime < b.EndDateTime &&
            booking.EndDateTime > b.StartDateTime
        );

        if (hasConflict)
            return BadRequest("Phong da co lich trong khoang thoi gian nay.");

        booking.CreatedAt = DateTime.UtcNow;
        booking.Status = string.IsNullOrWhiteSpace(booking.Status) ? "ChoDuyet" : booking.Status;

        _context.BookingRequests.Add(booking);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
           
            return BadRequest("Khong the tao booking do du lieu khong hop le (kiem tra RoomId/ActivityId va cac truong bat buoc).");
        }
        return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
    }

    // PUT api/bookings/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookingRequest dto)
    {
        var booking = await _context.BookingRequests.FindAsync(id);
        if (booking == null) return NotFound();

        if (dto.EndDateTime <= dto.StartDateTime)
            return BadRequest("Thoi gian ket thuc phai lon hon thoi gian bat dau.");

        // Check trung lich 
        var hasConflict = await _context.BookingRequests.AnyAsync(b =>
            b.BookingId != id &&
            b.RoomId == dto.RoomId &&
            (b.Status == "ChoDuyet" || b.Status == "DaDuyet") &&
            dto.StartDateTime < b.EndDateTime &&
            dto.EndDateTime > b.StartDateTime
        );

        if (hasConflict)
            return BadRequest("Phong da co lich trong khoang thoi gian nay.");

        booking.RoomId = dto.RoomId;
        booking.ActivityId = dto.ActivityId;
        booking.RequesterName = dto.RequesterName;
        booking.RequesterPhone = dto.RequesterPhone;
        booking.RequesterUnit = dto.RequesterUnit;
        booking.StartDateTime = dto.StartDateTime;
        booking.EndDateTime = dto.EndDateTime;
        booking.ExpectedParticipants = dto.ExpectedParticipants;
        booking.Notes = dto.Notes;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var booking = await _context.BookingRequests.FindAsync(id);
        if (booking == null) return NotFound();

        booking.Status = "DaDuyet";
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/reject")]
    public async Task<IActionResult> Reject(int id, [FromQuery] string? reason)
    {
        var booking = await _context.BookingRequests.FindAsync(id);
        if (booking == null) return NotFound();

        booking.Status = "TuChoi";
        if (!string.IsNullOrWhiteSpace(reason))
            booking.Notes = reason;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PUT api/bookings/5/complete
    [HttpPut("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        var booking = await _context.BookingRequests.FindAsync(id);
        if (booking == null) return NotFound();

        booking.Status = "DaHoanThanh";
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _context.BookingRequests.FindAsync(id);
        if (booking == null) return NotFound();

        booking.Status = "DaHuy";
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE hard
    // Xoa cứng booking (chi cho phep khi booking chua co hoa don).
    [HttpDelete("{id:int}/hard")]
    public async Task<IActionResult> HardDelete(int id)
    {
        var booking = await _context.BookingRequests
            .Include(b => b.Invoice)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking == null) return NotFound();

        if (booking.Invoice != null)
            return BadRequest("Booking da co hoa don, khong the xoa.");

        _context.BookingRequests.Remove(booking);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

