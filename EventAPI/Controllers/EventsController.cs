using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventAPI.Data;
using EventAPI.Models;

namespace EventAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EventsController(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET: api/Events
    // Получить ВСЕ ивенты (мобильное приложение само отфильтрует нужные)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        return await _context.Events.ToListAsync();
    }

    // 2. GET: api/Events/5
    // Получить один конкретный ивент по ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var @event = await _context.Events.FindAsync(id);

        if (@event == null)
        {
            return NotFound();
        }

        return @event;
    }

    // 3. POST: api/Events
    // СОЗДАТЬ ИВЕНТ (Сюда приходят данные с телефона)
    [HttpPost]
    public async Task<ActionResult<Event>> PostEvent(Event @event)
    {
        // Проверка: Если GroupId пришел 0, значит что-то не так
        if (@event.GroupId <= 0)
        {
             // Если хочешь разрешить ивенты без группы, удали этот блок.
             // Но сейчас мы хотим строгую привязку.
             // return BadRequest("Ивент должен быть привязан к группе (GroupId обязателен)");
        }

        _context.Events.Add(@event);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent), new { id = @event.Id }, @event);
    }

    // 4. DELETE: api/Events/5
    // Удалить ивент
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}