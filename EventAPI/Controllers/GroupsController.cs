using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventAPI.Data;
using EventAPI.Models;

namespace EventAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly AppDbContext _context;

    public GroupsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Groups
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        // Включаем (Include) список участников, чтобы видеть, кто в группе
        return await _context.Groups
            .Include(g => g.Members)
            .ToListAsync();
    }

    // POST: api/Groups
    [HttpPost("create/{userId}")]
    public async Task<ActionResult<Group>> CreateGroup(int userId, Group group)
    {
        // 1. Сначала ищем пользователя, который создает группу
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound("Пользователь не найден");
        }

        // 2. Добавляем этого пользователя в список участников новой группы
        // (Entity Framework сам поймет, что нужно заполнить таблицу связей)
        group.Members.Add(user);

        // 3. Сохраняем группу
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGroups), new { id = group.Id }, group);
    }

    // POST: api/Groups/1/join/5
    // Специальный метод: Добавить пользователя (userId) в группу (groupId)
    [HttpPost("{groupId}/join/{userId}")]
    public async Task<IActionResult> AddUserToGroup(int groupId, int userId)
    {
        // 1. Ищем группу
        var group = await _context.Groups
            .Include(g => g.Members) // Важно загрузить текущих членов
            .FirstOrDefaultAsync(g => g.Id == groupId);

        // 2. Ищем пользователя
        var user = await _context.Users.FindAsync(userId);

        if (group == null || user == null)
        {
            return NotFound("Группа или пользователь не найдены");
        }

        // 3. Добавляем пользователя в список членов группы
        if (!group.Members.Contains(user))
        {
            group.Members.Add(user);
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = $"Пользователь {user.Username} добавлен в группу {group.Name}" });
    }
}