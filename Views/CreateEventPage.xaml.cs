using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors; // For Location
using Microsoft.Maui.Maps;
using MauiAppB.Models;
namespace MauiAppB.Views;

public partial class CreateEventPage : ContentPage
{
    private int _targetGroupId;
    private Event _eventToEdit;
    public CreateEventPage(int groupId)
	{
		InitializeComponent();
        _targetGroupId = groupId;
        TitleLabel.Text = "Stwórz wydarzenie";
    }

    public CreateEventPage(Event eventToEdit)
    {
        InitializeComponent();
        _eventToEdit = eventToEdit;
        _targetGroupId = eventToEdit.GroupId;

        // Fill existing data into fields
        NameEntry.Text = eventToEdit.Name;
        DescriptionEntry.Text = eventToEdit.Description;
        EventDatePicker.Date = eventToEdit.Date;
        EventTimePicker.Time = eventToEdit.Time;
        LocationLabel.Text = eventToEdit.Location;

        CreateButton.Text = "Zapisz zmiany";
        TitleLabel.Text = "Edytuj wydarzenie";
    }

    private Location _selectedLocation;
    // 1. Wyszukiwanie po wpisaniu nazwy (np. "Restauracja Sphinx")
    private async void OnSearchLocationPressed(object sender, EventArgs e)
    {
        string query = LocationSearchBar.Text;
        if (string.IsNullOrWhiteSpace(query)) return;

        try
        {
            // Zamiana nazwy/adresu na współrzędne
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

    // 2. Metoda aktualizująca mapę i etykietę adresu
    private async void UpdateMapAndAddress(Location location)
    {
        _selectedLocation = location;

        // Przesuń mapę do znalezionego miejsca
        LocationMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.5)));

        // Dodaj pinezkę
        LocationMap.Pins.Clear();
        var pin = new Pin
        {
            Label = "Miejsce wydarzenia",
            Location = location,
            Type = PinType.Place
        };
        LocationMap.Pins.Add(pin);

        // REVERSE GEOCODING: Pobierz ludzką nazwę adresu
        try
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);
            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                // Budujemy ładny adres: np. "Restauracja, ul. Nowa 5, Warszawa"
                string niceAddress = $"{placemark.Thoroughfare} {placemark.SubThoroughfare}, {placemark.Locality}";
                LocationLabel.Text = $"Wybrany adres: {niceAddress}";

                // Opcjonalnie: Zaktualizuj Label pinezki na nazwę ulicy
                pin.Address = niceAddress;
            }
        }
        catch
        {
            LocationLabel.Text = $"Wybrana lokalizacja: {location.Latitude:F4}, {location.Longitude:F4}";
        }
    }

    // 3. Zaktualizuj też OnMapClicked, aby używało tej samej logiki adresu
    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        UpdateMapAndAddress(e.Location);
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
            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                await DisplayAlert("Błąd", "Wpisz nazwę wydarzenia!", "OK");
                return;
            }

            var eventData = _eventToEdit ?? new Event();
            eventData.Name = NameEntry.Text;
            eventData.Description = DescriptionEntry.Text;
            eventData.GroupId = _targetGroupId;
            eventData.Date = EventDatePicker.Date;
            eventData.Time = EventTimePicker.Time;
            eventData.Location = LocationLabel.Text;

            if (_eventToEdit == null)
            {
                // CREATE MODE
                App.EventService.AddEvent(eventData);
                await DisplayAlert("Sukces", "Wydarzenie utworzone!", "OK");
            }
            else
            {
                // EDIT MODE
                App.EventService.UpdateEvent(eventData);
                await DisplayAlert("Sukces", "Wydarzenie zaktualizowane!", "OK");
            }

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", ex.Message, "OK");
        }
    }
}