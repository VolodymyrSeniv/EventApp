using MauiAppB.Models;
using MauiAppB.Services;    
namespace MauiAppB.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnNextClicked(object sender, EventArgs e)
{
    // 1. Walidacja wstępna - czy pola nie są puste?
    if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || 
        string.IsNullOrWhiteSpace(NameEntry.Text) || 
        string.IsNullOrWhiteSpace(SurnameEntry.Text) ||
        string.IsNullOrWhiteSpace(EmailEntry.Text)) // <--- Ważne!
    {
        await DisplayAlert("Błąd", "Wypełnij wszystkie pola!", "OK");
        return;
    }

    var newUser = new User
    {
        FirstName = NameEntry.Text,
        LastName = SurnameEntry.Text,
        Username = UsernameEntry.Text,
        Email = EmailEntry.Text,          // <--- Upewnij się, że to tu jest!
        PhoneNumber = PhoneEntry.Text ?? "123456789", // Zabezpieczenie przed nullem
        
        // Domyślne wartości, których serwer może wymagać:
       
        PhotoUrl = "default_user.png",
        Bio = "Nowy użytkownik"
    };

    var apiService = new ApiService();
    bool isSuccess = await apiService.RegisterUser(newUser);

    if (isSuccess)
    {
        await DisplayAlert("Sukces", "Zarejestrowano pomyślnie!", "OK");
        await Navigation.PopAsync();
    }
    else
    {
        // Jeśli nadal błąd, wyświetlamy ogólny komunikat
        await DisplayAlert("Błąd", "Serwer odrzucił dane (400 Bad Request). Sprawdź czy wpisałeś wszystkie dane.", "OK");
    }
}
}