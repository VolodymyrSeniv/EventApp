using System.Text.Json.Serialization; // Ważne dla API

namespace EventAPI.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhotoUrl { get; set; }
    public string PhoneNumber { get; set; }
    public string Bio { get; set; }
    public string Username { get; set; }
    // public string Status { get; set; }

    // RELACJE (User należy do wielu grup)
    [JsonIgnore] // Zapobiega pętli przy pobieraniu danych
    public List<Group> Groups { get; set; } = new(); 
    
    // Lista eventów, w których bierze udział (opcjonalnie)
    [JsonIgnore]
    public List<Event> Events { get; set; } = new();
}