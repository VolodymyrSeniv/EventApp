using MauiAppB.Models;
using SQLite;
using System.Collections.Generic;

namespace MauiAppB.Models;

[Table("groups")]
public class Group: BaseEntity
{
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string StatusText { get; set; }
        public string StatusColor { get; set; }
        public int NotificationCount { get; set; }

        [Ignore] // Populated manually via Join Table
        public List<User> Members { get; set; } = new();
        [Ignore]
        public List<Event> Events { get; set; } = new();
}