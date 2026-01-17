using MauiAppB.Models;

namespace MauiAppB.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        LoadUserProfile();
    }

    private void LoadUserProfile()
    {
        var myProfile = new User
        {
            FirstName = "Ola",
            LastName = "Nowak", // Добавили фамилию
            PhotoUrl = "profil.png",
            Status = "online",
            PhoneNumber = "+48 111 111 111",
            Bio = "Kocham koty",
            Username = "@Olaaaa"
        };
        BindingContext = myProfile;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void OnEditClicked(object sender, EventArgs e)
    {
        // Получаем текущего пользователя из контекста страницы
        var currentUser = BindingContext as User;

        if (currentUser != null)
        {
            // Переходим на страницу редактирования и передаем туда пользователя
            await Navigation.PushAsync(new EditProfilePage(currentUser));
        }
    }
}