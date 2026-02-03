using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Maps; // Для карт
using Microsoft.Maui.Maps;          // Для карт
using Microsoft.Maui.Devices.Sensors; // Для геолокации/адресов
using MauiAppB.Services;
using MauiAppB.Models;

namespace MauiAppB.Views;

public partial class CreateEventPage : ContentPage
{
    private int _groupId;
    private string _selectedPhotoPath = null;
    private Location _selectedLocation; // Хранит координаты выбранной точки

    public CreateEventPage(int groupId)
    {
        InitializeComponent();
        _groupId = groupId;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // === 1. ЛОГИКА КАРТ (НОВОЕ) ===

    // Поиск по названию (кнопка на клавиатуре "Search" или Enter)
    private async void OnSearchLocationPressed(object sender, EventArgs e)
    {
        string query = LocationSearchBar.Text;
        if (string.IsNullOrWhiteSpace(query)) return;

        try
        {
            // Ищем координаты по тексту
            var locations = await Geocoding.Default.GetLocationsAsync(query);
            var location = locations?.FirstOrDefault();

            if (location != null)
            {
                UpdateMapAndAddress(location);
            }
            else
            {
                await DisplayAlert("Błąd", "Nie znaleziono takiego miejsca.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", "Problem z wyszukiwaniem: " + ex.Message, "OK");
        }
    }

    // Клик по карте (ставит пин)
    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        UpdateMapAndAddress(e.Location);
    }

    // Общий метод обновления карты и текста адреса
    private async void UpdateMapAndAddress(Location location)
    {
        _selectedLocation = location;

        // 1. Двигаем камеру
        LocationMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.5)));

        // 2. Ставим пин
        LocationMap.Pins.Clear();
        var pin = new Pin
        {
            Label = "Miejsce wydarzenia",
            Location = location,
            Type = PinType.Place
        };
        LocationMap.Pins.Add(pin);

        // 3. Пытаемся получить красивый адрес (Reverse Geocoding)
        try
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);
            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                // Формируем строку: "Улица Дом, Город"
                string niceAddress = $"{placemark.Thoroughfare} {placemark.SubThoroughfare}, {placemark.Locality}";
                
                // Убираем лишние пробелы и запятые, если данных мало
                niceAddress = niceAddress.Trim(',', ' '); 
                if(string.IsNullOrEmpty(niceAddress)) niceAddress = placemark.Locality ?? "Wybrana lokalizacja";

                LocationLabel.Text = niceAddress; 
                pin.Address = niceAddress;
                LocationSearchBar.Text = niceAddress; // Записываем в поле поиска для красоты
            }
        }
        catch
        {
            // Если не удалось найти адрес, пишем координаты
            LocationLabel.Text = $"{location.Latitude:F4}, {location.Longitude:F4}";
        }
    }

    // === 2. ФОТО И ОПРОСЫ (КАК БЫЛО) ===

    private async void OnPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null)
            {
                _selectedPhotoPath = photo.FullPath;
                SelectedImage.Source = ImageSource.FromFile(_selectedPhotoPath);
                SelectedImage.IsVisible = true;
                PhotoPlaceholder.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Не удалось выбрать фото", "OK");
        }
    }

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
            Content = newEntry
        };
        PollOptionsStack.Children.Add(newBorder);
    }

    private void OnDeleteOptionClicked(object sender, EventArgs e)
    {
        try
        {
            if (PollOptionsStack.Children.Count > 2)
            {
                PollOptionsStack.Children.RemoveAt(PollOptionsStack.Children.Count - 1);
            }
            else
            {
                DisplayAlert("Info", "Muszą być co najmniej dwie opcje!", "OK");
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Błąd", ex.Message, "OK");
        }
    }

    // === 3. СОЗДАНИЕ СОБЫТИЯ ===

    private async void OnCreateClicked(object sender, EventArgs e)
{
    // ШАГ 1
    await DisplayAlert("Debug 1", "Начало метода. Проверяем имя...", "OK");

    if (string.IsNullOrWhiteSpace(NameEntry.Text))
    {
        await DisplayAlert("Błąd", "Wpisz nazwę wydarzenia!", "OK");
        return;
    }

    ((Button)sender).IsEnabled = false;

    try
    {
        // ШАГ 2
        await DisplayAlert("Debug 2", "Собираем данные...", "OK");

        var apiService = new ApiService();
        
        string name = NameEntry.Text;
        string description = DescriptionEntry.Text ?? "";
        
        // --- ИСПРАВЛЕНИЕ: Добавили ?? DateTime.Now, чтобы не было ошибки CS0266 ---
        DateTime safeDate = EventDatePicker.Date ?? DateTime.Now;
        TimeSpan safeTime = EventTimePicker.Time ?? TimeSpan.Zero;
        
        string datePart = $"{safeDate.Day:00}.{safeDate.Month:00}.{safeDate.Year}";
        string timePart = $"{safeTime.Hours:00}:{safeTime.Minutes:00}";
        string fullTime = $"{datePart} {timePart}";

        string location = LocationLabel.Text; 
        if (string.IsNullOrWhiteSpace(location) || location.Contains("Nie wybrano"))
        {
            location = "Online"; 
        }

        string link = LinkEntry.Text ?? "";

        // ШАГ 3 (Убрали BaseUrlDebug, чтобы не было ошибки CS1061)
        await DisplayAlert("Debug 3", "Данные готовы. Отправляем...", "OK"); 

        // ОТПРАВКА
        bool isSuccess = await apiService.CreateEvent(name, description, fullTime, location, link, _groupId);

        // ШАГ 4
        await DisplayAlert("Debug 4", $"Ответ получен! Успех: {isSuccess}", "OK");

        if (isSuccess)
        {
            await DisplayAlert("Sukces", "Всё получилось!", "OK");
            await Navigation.PopAsync();
        }
    }
    catch (Exception ex)
    {
        // Покажем полную ошибку, если приложение упадет
        await DisplayAlert("CRASH", $"Ошибка: {ex.Message}", "OK");
    }
    finally
    {
        ((Button)sender).IsEnabled = true;
    }
}
}