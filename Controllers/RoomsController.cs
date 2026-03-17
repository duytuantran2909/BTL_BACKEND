using CommunityHallManager.Data;
using CommunityHallManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RoomsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetAll()
    {
        var rooms = await _context.Rooms.Include(r => r.Hall).ToListAsync();
        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Room>> GetById(int id)
    {
        var room = await _context.Rooms.Include(r => r.Hall).FirstOrDefaultAsync(r => r.RoomId == id);
        if (room == null) return NotFound();
        return Ok(room);
    }

    [HttpGet("by-hall/{hallId:int}")]
    public async Task<ActionResult<IEnumerable<Room>>> GetByHall(int hallId)
    {
        var rooms = await _context.Rooms.Where(r => r.HallId == hallId).ToListAsync();
        return Ok(rooms);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> Create(Room room)
    {
        room.CreatedDate = DateTime.UtcNow;
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = room.RoomId }, room);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Room dto)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return NotFound();

        room.RoomName = dto.RoomName;
        room.RoomType = dto.RoomType;
        room.Capacity = dto.Capacity;
        room.BasePricePerHour = dto.BasePricePerHour;
        room.Status = dto.Status;
        room.HallId = dto.HallId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return NotFound();

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
