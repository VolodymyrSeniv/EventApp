using MauiAppB.Services;
using System.Net.Http.Json;
using System.Diagnostics;

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
        var newUser = new MauiAppB.Models.User
        {
            FirstName = FirstNameEntry.Text,
            LastName = LastNameEntry.Text,
            Username = UserNameEntry.Text,
            PhoneNumber = PhoneEntry.Text,
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text,
            Status = "online",
            Bio = "Here is your bio!",
            PhotoUrl = "https://example.com/default-photo.png"
        };

        try
        {
            var authService = Handler.MauiContext.Services.GetService<AuthService>();
            var (success, error) = await authService.RegisterAsync(newUser);

            if (success)
            {
                await DisplayAlert("Sukces", "Konto zarejestrowane!", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                // Show the detailed error so you can diagnose the root cause
                Debug.WriteLine($"Register failed: {error}");
                await DisplayAlert("Błąd", $"Nie udało się zarejestrować.\n\nReason:\n{error}", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"OnNextClicked unexpected: {ex}");
            await DisplayAlert("Błąd", $"Network error: {ex.Message}", "OK");
        }
    }
}