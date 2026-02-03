using MauiAppB.Models;

namespace MauiAppB.Views;

public partial class EventDetailsPage : ContentPage
{
    private Event _currentEvent;

    public EventDetailsPage(Event selectedEvent)
    {
        InitializeComponent();
        _currentEvent = selectedEvent;
        // ЭТА СТРОЧКА ВАЖНА ДЛЯ ОТОБРАЖЕНИЯ ФОТО
        BindingContext = _currentEvent; 
    }

    private void SaveChanges()
    {
        App.EventService.UpdateEvent(_currentEvent);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnUpdateEventClicked(object sender, EventArgs e)
    {
        SettingsOverlay.IsVisible = false;

        // Navigate to CreateEventPage passing the current event object
        // This triggers the Edit Mode constructor
        await Navigation.PushAsync(new CreateEventPage(_currentEvent));
    }

    // === ЛОГИКА ВРЕМЕНИ ===
    private void OnConfirmTimeClicked(object sender, EventArgs e)
    {
        if (RbTimeYes.IsChecked) _currentEvent.TimeAnswer = "Tak";
        else if (RbTimeNo.IsChecked) _currentEvent.TimeAnswer = "Proszę inny termin";
        else return;

        _currentEvent.IsTimeConfirmed = true;

        // Ręczne odświeżenie UI (ponieważ BindingContext to zwykły model, a nie ViewModel)
        BindingContext = null;
        BindingContext = _currentEvent;

        SaveChanges();
    }

    private void OnChangeTimeClicked(object sender, EventArgs e)
    {
        _currentEvent.IsTimeConfirmed = false;
        BindingContext = null;
        BindingContext = _currentEvent;
        SaveChanges();
    }

    // === ЛОГИКА ЕДЫ ===
    private void OnConfirmFoodClicked(object sender, EventArgs e)
    {
        if (RbFoodYes.IsChecked) _currentEvent.FoodAnswer = "Zgadzam się";
        else if (RbFoodLater.IsChecked) _currentEvent.FoodAnswer = "Możemy po";
        else return;

        _currentEvent.IsFoodConfirmed = true;
        BindingContext = null;
        BindingContext = _currentEvent;
        SaveChanges();
    }

    private void OnChangeFoodClicked(object sender, EventArgs e)
    {
        _currentEvent.IsFoodConfirmed = false;
        RbFoodYes.IsChecked = false;
        RbFoodLater.IsChecked = false;
    }
    private void OnSettingsClicked(object sender, EventArgs e)
    {
        SettingsOverlay.IsVisible = true;
    }
    private void OnCloseSettingsClicked(object sender, EventArgs e)
    {
        SettingsOverlay.IsVisible = false;
    }

    // Удалить группу
    private async void OnDeleteEventClicked(object sender, EventArgs e)
    {
        // 1. Wyświetlenie potwierdzenia użytkownikowi
        bool answer = await DisplayAlert("Usuń wydarzenie", "Czy na pewno chcesz usunąć to wydarzenie?", "Tak", "Nie");

        if (answer)
        {
            try
            {
                // 2. Ukrycie menu ustawień
                SettingsOverlay.IsVisible = false;

                // 3. KLUCZOWE: Usunięcie z bazy danych przy użyciu ID bieżącego wydarzenia
                App.EventService.DeleteEvent(_currentEvent.Id);

                // 4. Powrót do poprzedniej strony (listy wydarzeń)
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", "Nie udało się usunąć wydarzenia: " + ex.Message, "OK");
            }
        }
    }
}