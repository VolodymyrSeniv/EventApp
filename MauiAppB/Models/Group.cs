using System.Collections.Generic;

namespace MauiAppB.Models;

public class Group
{
    public int Id { get; set; } // Added ID just in case
    public string Name { get; set; }
    public string PhotoUrl { get; set; }
    public string StatusText { get; set; }
    public string StatusColor { get; set; }
    public int NotificationCount { get; set; }
    public bool HasNotifications => NotificationCount > 0;

    public List<User> Members { get; set; } = new();
    
    // IMPORTANT: Each group has its own list of events
    public List<Event> Events { get; set; } = new List<Event>();
}