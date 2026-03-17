using CommunityHallManager.Data;
using CommunityHallManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Controllers;

[ApiController]
[Route("api/equipments")]
public class EquipmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EquipmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipment>>> GetAll()
    {
        var list = await _context.Equipments.Include(e => e.Hall).ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Equipment>> GetById(int id)
    {
        var eq = await _context.Equipments.Include(e => e.Hall).FirstOrDefaultAsync(e => e.EquipmentId == id);
        if (eq == null) return NotFound();
        return Ok(eq);
    }

    [HttpGet("by-hall/{hallId:int}")]
    public async Task<ActionResult<IEnumerable<Equipment>>> GetByHall(int hallId)
    {
        var list = await _context.Equipments.Where(e => e.HallId == hallId).ToListAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<Equipment>> Create(Equipment equipment)
    {
        equipment.CreatedDate = DateTime.UtcNow;
        _context.Equipments.Add(equipment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = equipment.EquipmentId }, equipment);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Equipment dto)
    {
        var eq = await _context.Equipments.FindAsync(id);
        if (eq == null) return NotFound();

        eq.EquipmentName = dto.EquipmentName;
        eq.EquipmentType = dto.EquipmentType;
        eq.Quantity = dto.Quantity;
        eq.UnitPricePerUse = dto.UnitPricePerUse;
        eq.Status = dto.Status;
        eq.LastMaintenanceDate = dto.LastMaintenanceDate;
        eq.HallId = dto.HallId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eq = await _context.Equipments.FindAsync(id);
        if (eq == null) return NotFound();

        _context.Equipments.Remove(eq);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
