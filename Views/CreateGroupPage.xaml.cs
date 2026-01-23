using Microsoft.Maui.Controls.Shapes;
namespace MauiAppB.Views;

public partial class CreateGroupPage : ContentPage
{
    private List<string> addedUsers = new List<string>();
	public CreateGroupPage()
	{
        
		InitializeComponent();
	}
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnPhotoClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Foto", "Wybierz zdjęcie grupy", "OK");
    }

    // ЛОГИКА: Добавить участника в список
    private void OnAddMemberClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text?.Trim();

        // 1. Проверка на пустоту
        if (string.IsNullOrEmpty(username))
        {
            DisplayAlert("Błąd", "Wpisz nazwę użytkownika!", "OK");
            return;
        }

        // 2. Проверка, не добавлен ли уже такой
        if (addedUsers.Contains(username))
        {
            DisplayAlert("Info", "Ten użytkownik jest już dodany.", "OK");
            return;
        }

        // 3. Добавляем в список данных
        addedUsers.Add(username);

        // 4. СОЗДАЕМ ВИЗУАЛЬНЫЙ ЭЛЕМЕНТ (Сетка с именем и кнопкой удаления)
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
        
        // Красивая рамка вокруг
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

        // Имя пользователя
        var nameLabel = new Label
        {
            Text = username,
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10, 0, 0, 0)
        };

        // Кнопка удаления (Х)
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

        // Привязываем удаление этого конкретного элемента
        removeButton.Clicked += (s, args) => 
        {
            MembersStack.Children.Remove(userBorder); // Удаляем с экрана
            addedUsers.Remove(username); // Удаляем из списка данных
        };

        // Собираем всё вместе
        userGrid.Add(nameLabel, 0, 0); // Колонка 0
        userGrid.Add(removeButton, 1, 0); // Колонка 1

        // Добавляем в вертикальный список на экране
        MembersStack.Children.Add(userBorder);

        // Очищаем поле ввода
        UsernameEntry.Text = string.Empty;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(GroupNameEntry.Text))
        {
            await DisplayAlert("Błąd", "Wpisz nazwę grupy!", "OK");
            return;
        }

        // Тут логика создания группы с списком addedUsers
        string message = $"Grupa '{GroupNameEntry.Text}' utworzona! Uczestnicy: {addedUsers.Count}";
        await DisplayAlert("Sukces", message, "OK");
        
        await Navigation.PopAsync();
    }

}