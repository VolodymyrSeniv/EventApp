using MauiAppB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace MauiAppB.Services;

public class MockDataService : IDataService
{
    public async Task<List<Group>> GetMyGroupsAsync()
    {
        await Task.Delay(500); // Simulate internet

        var me = new User { Id = 1, FirstName = "Anya", LastName = "Student", PhotoUrl = "profil.png" };
        var friend = new User { Id = 2, FirstName = "Ivan", LastName = "Backend", PhotoUrl = "profil.png" };

        var partyEvent = new Event
        {
            Id = 1,
            Name = "Coursework Submission",
            Description = "Celebrating project submission!",
            Date = DateTime.Now.AddDays(10),
            Time = "Friday 18:00", // Added Time for design
            ImageUrl = "free.png", 
            Participants = new List<User> { me, friend },
            IsActionButtonsVisible = true
        };

        return new List<Group>
        {
            new Group
            {
                Id = 1,
                Name = "Basketball",
                PhotoUrl = "profil.png",
                StatusText = "Active event 🔴",
                StatusColor = "#FF3B30",
                NotificationCount = 1,
                Members = new List<User> { me, friend },
                Events = new List<Event> { partyEvent }
            },
            new Group
            {
                Id = 2,
                Name = "WPAM",
                PhotoUrl = "profil.png",
                StatusText = "All events finished ✅",
                StatusColor = "#34C759",
                NotificationCount = 0,
                Members = new List<User> { me }
            },
            new Group
            {
                Id = 3,
                Name = "Kings 👑",
                PhotoUrl = "profil.png",
                StatusText = "Active event 🔴",
                StatusColor = "#FF3B30",
                NotificationCount = 2
            }
        };
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        await Task.Delay(1000);
        return true;
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        await Task.Delay(1000);
        return true;
    }
}