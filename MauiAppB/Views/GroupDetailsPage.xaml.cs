using System.Collections.ObjectModel;
using MauiAppB.Models;
using MauiAppB.Services; // Подключаем сервис
using Microsoft.Maui.Controls;
using System.Linq;

namespace MauiAppB.Views;

public partial class GroupDetailsPage : ContentPage
{
    public ObservableCollection<Event> Events { get; set; } = new();
    
    // ID группы, которую мы сейчас просматриваем
    private int _currentGroupId;

    public GroupDetailsPage(Group group)
    {
        InitializeComponent();
        
        _currentGroupId = group.Id; // Запоминаем ID группы
        BindingContext = this;      // Привязываем данные
    }

    // Этот метод запускается каждый раз, когда вы открываете страницу (или возвращаетесь назад)
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadGroupEvents();
    }

    private async Task LoadGroupEvents()
    {
        try
        {
            var apiService = new ApiService();
            
            // 1. Получаем ВСЕ ивенты с сервера
            var allEvents = await apiService.GetEvents();

            // 2. Фильтруем: оставляем только те, у которых GroupId совпадает с текущей группой
            var myGroupEvents = allEvents.Where(e => e.GroupId == _currentGroupId).ToList();
            // 3. Показываем на экране (предполагаем, что у тебя есть CollectionView с именем EventsCollection)
            // Если у тебя другое имя списка в XAML, поменяй EventsCollection на него.
            // EventsCollection.ItemsSource = myGroupEvents; 
            
            // Если у тебя просто список в коде, то делай с myGroupEvents что нужно
            Events.Clear();
            foreach (var evt in myGroupEvents)
            {
                // Ставим заглушки, если данных нет
                if (string.IsNullOrEmpty(evt.ImageUrl)) evt.ImageUrl = "free.png";
                if (string.IsNullOrEmpty(evt.Location)) evt.Location = "Online";

                // Добавляем в список на экране
                Events.Add(evt);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Не удалось загрузить события", "OK");
        }
    }

    // private async Task LoadEventsFromServer()
    // {
    //     var apiService = new ApiService();
        
    //     // 1. Скачиваем ВСЕ ивенты с сервера
    //     var allEvents = await apiService.GetEvents();

    //     // 2. Очищаем экран
    //     Events.Clear();

    //     // 3. Фильтруем: берем только те, у которых GroupId совпадает с нашей группой
    //     var myGroupEvents = allEvents.Where(e => e.GroupId == _currentGroupId);

    //     // 4. Добавляем их на экран
    //     foreach (var evt in myGroupEvents)
    //     {
    //         // Если картинки нет, ставим заглушку
    //         if (string.IsNullOrEmpty(evt.ImageUrl)) evt.ImageUrl = "free.png";
            
    //         // Если локации нет, ставим заглушку
    //         if (string.IsNullOrEmpty(evt.Location)) evt.Location = "Online";

    //         Events.Add(evt);
    //     }
    // }

    private async void OnCreateEventClicked(object sender, EventArgs e)
    {
        // Передаем ID группы на страницу создания
        await Navigation.PushAsync(new CreateEventPage(_currentGroupId));
    }

    private async void OnEventSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedEvent = e.CurrentSelection.FirstOrDefault() as Event;
        if (selectedEvent == null) return;

        ((CollectionView)sender).SelectedItem = null;
        
        // Переход к деталям ивента (если такая страница есть)
        // await Navigation.PushAsync(new EventDetailsPage(selectedEvent));
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // --- Меню настроек (удаление группы и т.д.) ---
    private void OnSettingsClicked(object sender, EventArgs e)
    {
        SettingsOverlay.IsVisible = true;
    }

    private void OnCloseSettingsClicked(object sender, EventArgs e)
    {
        SettingsOverlay.IsVisible = false;
    }

    private async void OnDeleteGroupClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Usuń grupę", "Czy na pewno?", "Tak", "Nie");
        if (answer)
        {
            // Тут можно добавить запрос к API на удаление
            SettingsOverlay.IsVisible = false;
            await Navigation.PopAsync();
        }
    }
    
    // Кнопки "Пойду / Не пойду"
    private void OnDecisionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedEvent = button?.BindingContext as Event;
        if (selectedEvent != null)
        {
            selectedEvent.IsActionButtonsVisible = false;
        }
    }

    private void OnChangeDecisionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedEvent = button?.BindingContext as Event;
        if (selectedEvent != null)
        {
            selectedEvent.IsActionButtonsVisible = true;
        }
    }
}