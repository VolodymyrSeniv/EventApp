using System.Text.Json.Serialization;

namespace EventAPI.Models;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Time { get; set; } // Można zmienić na DateTime, ale string też zadziała
    public string? Location { get; set; }

    // KLUCZ OBCY (Wskazuje, do jakiej grupy należy event)
    public int GroupId { get; set; }
    
    [JsonIgnore]
    public Group Group { get; set; }
    public string? Link { get; set; }

    // Kto bierze udział?
    public List<User> Participants { get; set; } = new();
}