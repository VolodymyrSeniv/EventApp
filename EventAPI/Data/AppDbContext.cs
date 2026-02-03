using Microsoft.EntityFrameworkCore;
using EventAPI.Models; // Используем наши перенесенные модели

namespace EventAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Здесь мы говорим базе: "У нас есть такие таблицы"
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Group> Groups { get; set; }
}