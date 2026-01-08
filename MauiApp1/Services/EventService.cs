using System;
using System.Collections.Generic;
using System.Text;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class EventService
    {
        public List<Event> GetEvents(){
            return new List<Event>() {
                new Event
                {
                    Id = 1,
                    Title = "Tech Conference 2026",
                    Date = new DateTime(2026, 3, 15, 9, 0, 0),
                    Location = "San Francisco, CA",
                    Description = "A deep dive into the latest AI and .NET MAUI developments.",
                    ImageUrl = "tech_event.png" // Local resource
                },
                new Event
                {
                    Id = 2,
                    Title = "Summer Music Festival",
                    Date = new DateTime(2026, 7, 10, 16, 30, 0),
                    Location = "Central Park, NY",
                    Description = "Enjoy live performances from top artists under the summer sun.",
                    ImageUrl = "https://picsum.photos/id/10/400/250" // Web URL
                },
                new Event
                {
                    Id = 3,
                    Title = "Community Charity Run",
                    Date = new DateTime(2026, 10, 05, 7, 0, 0),
                    Location = "Austin, TX",
                    Description = "Join us for a 5k run to support local youth education programs.",
                    ImageUrl = "marathon.png"
                },
                new Event
                {
                    Id = 4,
                    Title = "Art Gallery Opening",
                    Date = new DateTime(2026, 11, 20, 18, 0, 0),
                    Location = "Chicago, IL",
                    Description = "An evening of modern art, networking, and refreshments.",
                    ImageUrl = "https://picsum.photos/id/24/400/250"
                }
            };
        }
    }
}
