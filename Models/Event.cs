using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppB.Models;

[Table("events")]
public class Event: BaseEntity
{
    // === ОСНОВНЫЕ ПОЛЯ ===
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public TimeSpan? Time { get; set; }
    public DateTime? Date { get; set; }
    public string Location { get; set; }

    [Indexed]
    public int GroupId { get; set; }

    [Ignore] // Populated manually via Join Table
    public List<User> Participants { get; set; } = new();

    // === 1. ЛОГИКА СПИСКА (Кнопки "Będę/Nie będę") ===
    // Мы вернули это поле, чтобы ошибки исчезли
    private bool _isActionButtonsVisible = true;
    public bool IsActionButtonsVisible
    {
        get => _isActionButtonsVisible;
        set => SetProperty(ref _isActionButtonsVisible, value);
    }

    // === 2. ЛОГИКА ДЕТАЛЕЙ (Опросы) ===
    
    // Опрос про Время
    private bool _isTimeConfirmed;
    public bool IsTimeConfirmed
    {
        get => _isTimeConfirmed;
        set => SetProperty(ref _isTimeConfirmed, value);
    }

    private string _timeAnswer;
    public string TimeAnswer
    {
        get => _timeAnswer;
        set => SetProperty(ref _timeAnswer, value);
    }

    // Опрос про Еду
    private bool _isFoodConfirmed;
    public bool IsFoodConfirmed
    {
        get => _isFoodConfirmed;
        set => SetProperty(ref _isFoodConfirmed, value);
    }

    private string _foodAnswer;
    public string FoodAnswer
    {
        get => _foodAnswer;
        set => SetProperty(ref _foodAnswer, value);
    }
}