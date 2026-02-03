
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppB.Models;

// Добавляем INotifyPropertyChanged
public class User : BaseEntity
{
    // Вместо public string Name { get; set; } пишем развернуто

    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set 
        { 
            _firstName = value; 
            OnPropertyChanged(); // <--- ЭТА СТРОКА ОБНОВЛЯЕТ ЭКРАН
        }
    }

    private string _email;
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set { _lastName = value; OnPropertyChanged(); }
    }

    private string _photoUrl;
    public string PhotoUrl
    {
        get => _photoUrl;
        set { _photoUrl = value; OnPropertyChanged(); }
    }

    private string _phoneNumber;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set { _phoneNumber = value; OnPropertyChanged(); }
    }

    private string _bio;
    public string Bio
    {
        get => _bio;
        set { _bio = value; OnPropertyChanged(); }
    }

    private string _username;
    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    private string _status;
    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    // Стандартный код для уведомления интерфейса
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}