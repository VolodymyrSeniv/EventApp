using Microsoft.Maui.Controls.Shapes;
using MauiAppB.Services; // Не забудь

namespace MauiAppB.Views;

public partial class CreateGroupPage : ContentPage
{
    private List<string> addedUsers = new List<string>();
    private string? _selectedPhotoPath = null; // Переменная для хранения пути к фото

    public CreateGroupPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // === ЛОГИКА ВЫБОРА ФОТО ===
    private async void OnPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            // Открываем галерею телефона
            var photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                // Сохраняем путь к выбранному файлу
                // (В реальном приложении тут нужно загружать фото на сервер, 
                // но пока мы просто берем имя файла)
                _selectedPhotoPath = photo.FullPath;

                // Показываем фото на экране
                SelectedImage.Source = ImageSource.FromFile(_selectedPhotoPath);
                SelectedImage.IsVisible = true;       // Показываем картинку
                PhotoPlaceholder.IsVisible = false;   // Скрываем текст "Dodaj zdjęcie"
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось выбрать фото: {ex.Message}", "OK");
        }
    }

    // ЛОГИКА: Добавить участника в список (без изменений)
    private void OnAddMemberClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text?.Trim();

        if (string.IsNullOrEmpty(username)) return;

        if (addedUsers.Contains(username))
        {
            DisplayAlert("Info", "Ten użytkownik jest już dodany.", "OK");
            return;
        }

        addedUsers.Add(username);

        // Создаем визуальный элемент
        var userGrid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection 
            { 
                new ColumnDefinition { Width = GridLength.Star }, 
                new ColumnDefinition { Width = GridLength.Auto } 
            },
            BackgroundColor = Colors.White,
            Padding = new Thickness(10),
        };
        
        var userBorder = new Border
        {
            Stroke = Colors.Gray,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            BackgroundColor = Colors.White,
            Padding = 0,
            Content = userGrid,
            HeightRequest = 45,
            Margin = new Thickness(0, 0, 0, 5)
        };

        var nameLabel = new Label
        {
            Text = username,
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10, 0, 0, 0)
        };

        var removeButton = new Button
        {
            Text = "✕",
            TextColor = Colors.Red,
            BackgroundColor = Colors.Transparent,
            FontSize = 16,
            WidthRequest = 40,
            HeightRequest = 40,
            VerticalOptions = LayoutOptions.Center
        };

        removeButton.Clicked += (s, args) => 
        {
            MembersStack.Children.Remove(userBorder);
            addedUsers.Remove(username);
        };

        userGrid.Add(nameLabel, 0, 0);
        userGrid.Add(removeButton, 1, 0);
        MembersStack.Children.Add(userBorder);
        UsernameEntry.Text = string.Empty;
    }

    // === ОБНОВЛЕННАЯ ЛОГИКА СОЗДАНИЯ ===
    private async void OnCreateClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(GroupNameEntry.Text))
        {
            await DisplayAlert("Błąd", "Wpisz nazwę grupy!", "OK");
            return;
        }

        if (App.CurrentUser == null)
        {
            await DisplayAlert("Błąd", "Nie jesteś zalogowany!", "OK");
            return;
        }

        ((Button)sender).IsEnabled = false;

        try
        {
            var apiService = new ApiService();
            int creatorId = App.CurrentUser.Id;
            string groupName = GroupNameEntry.Text;
            
            // Если фото не выбрано, ставим стандартное
            // Если выбрано — пока просто отправляем имя файла (чтобы работало без сложного сервера загрузки)
            string photoUrl = _selectedPhotoPath ?? "profil.png";

            // Передаем photoUrl в метод создания
            bool isSuccess = await apiService.CreateGroup(groupName, creatorId, photoUrl);

            if (isSuccess)
            {
                await DisplayAlert("Sukces", $"Grupa '{groupName}' została utworzona!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Błąd", "Nie udało się połączyć z serwerem.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            ((Button)sender).IsEnabled = true;
        }
    }
}