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

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // === ЛОГИКА ВРЕМЕНИ ===
    private void OnConfirmTimeClicked(object sender, EventArgs e)
    {
        if (RbTimeYes.IsChecked) _currentEvent.TimeAnswer = "Tak";
        else if (RbTimeNo.IsChecked) _currentEvent.TimeAnswer = "Proszę inny termin";
        else return; // Если ничего не выбрано

        _currentEvent.IsTimeConfirmed = true;
    }

    private void OnChangeTimeClicked(object sender, EventArgs e)
    {
        // Скрываем ответ, показываем вопросы
        _currentEvent.IsTimeConfirmed = false;
        
        // Сбрасываем выбор (галочки)
        RbTimeYes.IsChecked = false;
        RbTimeNo.IsChecked = false;
    }

    // === ЛОГИКА ЕДЫ ===
    private void OnConfirmFoodClicked(object sender, EventArgs e)
    {
        if (RbFoodYes.IsChecked) _currentEvent.FoodAnswer = "Zgadzam się";
        else if (RbFoodLater.IsChecked) _currentEvent.FoodAnswer = "Możemy po";
        else return;

        _currentEvent.IsFoodConfirmed = true;
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
    private async void OnDeleteGroupClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Usuń wydarzenie", "Czy na pewno?", "Tak", "Nie");
        if (answer)
        {
            SettingsOverlay.IsVisible = false;
            await Navigation.PopAsync(); // Уходим с страницы
        }
    }
}