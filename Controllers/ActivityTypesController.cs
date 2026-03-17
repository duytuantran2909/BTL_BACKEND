using CommunityHallManager.Data;
using CommunityHallManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunityHallManager.Controllers;

[ApiController]
[Route("api/activity-types")]
public class ActivityTypesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ActivityTypesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityType>>> GetAll()
    {
        var list = await _context.ActivityTypes.ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActivityType>> GetById(int id)
    {
        var item = await _context.ActivityTypes.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ActivityType>> Create(ActivityType activityType)
    {
        activityType.CreatedDate = DateTime.UtcNow;
        _context.ActivityTypes.Add(activityType);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = activityType.ActivityId }, activityType);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ActivityType dto)
    {
        var item = await _context.ActivityTypes.FindAsync(id);
        if (item == null) return NotFound();

        item.ActivityName = dto.ActivityName;
        item.Description = dto.Description;
        item.DefaultDurationHours = dto.DefaultDurationHours;
        item.Status = dto.Status;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ActivityTypes.FindAsync(id);
        if (item == null) return NotFound();

        _context.ActivityTypes.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
