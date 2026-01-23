using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppB.Models;

public class Event : INotifyPropertyChanged
{
    // === ОСНОВНЫЕ ПОЛЯ ===
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string Time { get; set; }
   public string Date { get; set; }
   public string Location { get; set; }
    public List<User> Participants { get; set; } = new();

    // === 1. ЛОГИКА СПИСКА (Кнопки "Będę/Nie będę") ===
    // Мы вернули это поле, чтобы ошибки исчезли
    private bool _isActionButtonsVisible = true;
    public bool IsActionButtonsVisible
    {
        get => _isActionButtonsVisible;
        set { _isActionButtonsVisible = value; OnPropertyChanged(); }
    }

    // === 2. ЛОГИКА ДЕТАЛЕЙ (Опросы) ===
    
    // Опрос про Время
    private bool _isTimeConfirmed;
    public bool IsTimeConfirmed
    {
        get => _isTimeConfirmed;
        set { _isTimeConfirmed = value; OnPropertyChanged(); }
    }

    private string _timeAnswer;
    public string TimeAnswer
    {
        get => _timeAnswer;
        set { _timeAnswer = value; OnPropertyChanged(); }
    }

    // Опрос про Еду
    private bool _isFoodConfirmed;
    public bool IsFoodConfirmed
    {
        get => _isFoodConfirmed;
        set { _isFoodConfirmed = value; OnPropertyChanged(); }
    }

    private string _foodAnswer;
    public string FoodAnswer
    {
        get => _foodAnswer;
        set { _foodAnswer = value; OnPropertyChanged(); }
    }

    // === УВЕДОМЛЕНИЯ ИНТЕРФЕЙСА ===
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}