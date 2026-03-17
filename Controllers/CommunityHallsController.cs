using CommunityHallManager.Data;
using CommunityHallManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Controllers;

[ApiController]
[Route("api/halls")]
public class CommunityHallsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommunityHallsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommunityHall>>> GetAll()
    {
        var list = await _context.CommunityHalls.ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommunityHall>> GetById(int id)
    {
        var hall = await _context.CommunityHalls
            .Include(h => h.Rooms)
            .Include(h => h.Equipments)
            .FirstOrDefaultAsync(h => h.HallId == id);

        if (hall == null) return NotFound();
        return Ok(hall);
    }

    [HttpPost]
    public async Task<ActionResult<CommunityHall>> Create(CommunityHall hall)
    {
        hall.CreatedDate = DateTime.UtcNow;
        _context.CommunityHalls.Add(hall);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = hall.HallId }, hall);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CommunityHall dto)
    {
        var hall = await _context.CommunityHalls.FindAsync(id);
        if (hall == null) return NotFound();

        hall.HallName = dto.HallName;
        hall.Address = dto.Address;
        hall.ManagerName = dto.ManagerName;
        hall.PhoneNumber = dto.PhoneNumber;
        hall.Email = dto.Email;
        hall.Status = dto.Status;
        hall.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var hall = await _context.CommunityHalls.FindAsync(id);
        if (hall == null) return NotFound();

        _context.CommunityHalls.Remove(hall);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
