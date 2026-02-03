namespace EventAPI.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set; }
    
    // RELACJE
    public List<User> Members { get; set; } = new(); // Lista członków
    public List<Event> Events { get; set; } = new(); // Lista wydarzeń w tej grupie
}