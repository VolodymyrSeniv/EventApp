using Microsoft.Maui.Controls.Shapes;
namespace MauiAppB.Views;

public partial class CreateEventPage : ContentPage
{
	public CreateEventPage()
	{
		InitializeComponent();
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // private async void OnAddPhotoClicked(object sender, EventArgs e)
    // {
    //     await DisplayAlert("Foto", "Galeria...", "OK");
    // }
    
    private async void OnAddOptionClicked(object sender, EventArgs e)
    {
        var newEntry = new Entry
        {
            Placeholder = $"Opcja {PollOptionsStack.Children.Count + 1}",
            PlaceholderColor = Colors.Gray,
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center
        };
        var newBorder = new Border
        {
            Stroke = Colors.Gray,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            BackgroundColor = Colors.White,
            HeightRequest = 45,
            Padding = new Thickness(15, 0),
            Content = newEntry // Кладем поле внутрь рамки
        };
        PollOptionsStack.Children.Add(newBorder);
    }
    // Кнопка удаления последней опции
    private void OnDeleteOptionClicked(object sender, EventArgs e)
    {
        try
        {
            // Проверяем: если опций больше 2, удаляем последнюю
            if (PollOptionsStack.Children.Count > 2)
            {
                // Удаляем элемент с последнего индекса
                PollOptionsStack.Children.RemoveAt(PollOptionsStack.Children.Count - 1);
            }
            else
            {
                // Если опций 2 или меньше — показываем сообщение
                DisplayAlert("Info", "Muszą być co najmniej dwie opcje!", "OK");
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Błąd", ex.Message, "OK");
        }
    }
    // Безопасный метод создания
    private async void OnCreateClicked(object sender, EventArgs e)
    {
        try
        {
            // 1. Проверяем, доступны ли поля (для отладки)
            if (NameEntry == null) throw new Exception("Поле NameEntry не найдено!");
            
            // 2. Собираем данные (пока просто для теста)
            string name = NameEntry.Text;
            string description = DescriptionEntry.Text;

            // 3. Выводим успех
            await DisplayAlert("Sukces", $"Wydarzenie '{name}' zostało utworzone!", "OK");
            
            // 4. Возвращаемся назад
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            // ЕСЛИ ОШИБКА ЕСТЬ - ОНА ПОЯВИТСЯ ТУТ, А НЕ ВЫЛЕТИТ
            await DisplayAlert("Ошибка (Błąd)", ex.ToString(), "OK");
        }
    }
}