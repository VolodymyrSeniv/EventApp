using MauiAppB.Models;

namespace MauiAppB.Views;

public partial class EditProfilePage : ContentPage
{
    public EditProfilePage(User user)
    {
        InitializeComponent();
        // Привязываем переданного пользователя, чтобы поля заполнились его данными
        BindingContext = user;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        // Просто возвращаемся без сохранения (хотя изменения в Entry уже могли примениться к объекту в памяти)
        await Navigation.PopAsync();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Тут можно добавить логику сохранения на сервер
        // Пока просто возвращаемся назад, имитируя успешное сохранение
        await DisplayAlert("Sukces", "Dane zostały zaktualizowane", "OK");
        await Navigation.PopAsync();
    }

    private async void OnChangePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            // 1. Открываем галерею
            var result = await MediaPicker.Default.PickPhotoAsync();

            if (result != null)
            {
                // 2. Если фото выбрано, получаем текущего пользователя
                var user = BindingContext as User;
                
                if (user != null)
                {
                    // 3. Обновляем путь к фото (в реальном приложении тут загрузка на сервер)
                    // Для теста просто берем путь к файлу на телефоне
                    user.PhotoUrl = result.FullPath;
                    
                    // Если UI nie обновляется сразу, может потребоваться:
                    // OnPropertyChanged(nameof(PhotoUrl)); в модели
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", "Nie udało się wybrać zdjęcia: " + ex.Message, "OK");
        }
    }
}