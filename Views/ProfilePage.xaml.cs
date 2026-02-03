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
    //private async void OnCreateEventClicked(object sender, EventArgs e)
    //{
    //    // Переход на страницу создания
    //    await Navigation.PushAsync(new CreateEventPage());
    //}
    private void OnSettingsClicked(object sender, EventArgs e)
    {
        SettingsOverlay.IsVisible = true;
    }

    // Закрыть меню (при клике на фон)
    private void OnCloseSettingsClicked(object sender, EventArgs e)
    {
        SettingsOverlay.IsVisible = false;
    }

    // Удалить группу
    private async void OnDeleteGroupClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Usuń grupę", "Czy na pewno?", "Tak", "Nie");
        if (answer)
        {
            SettingsOverlay.IsVisible = false;
            await Navigation.PopAsync(); // Уходим с страницы
        }
    }
    private async void OnDeleteProfileClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Wyloguj", "Czy na pewno chcesz się wylogować?", "Tak", "Nie");

        if (answer)
        {
            SettingsOverlay.IsVisible = false;
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}