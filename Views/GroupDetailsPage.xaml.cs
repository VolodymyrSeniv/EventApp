using System.Collections.ObjectModel;
using MauiAppB.Models;

namespace MauiAppB.Views;

public partial class GroupDetailsPage : ContentPage
{
    public ObservableCollection<Event> Events { get; set; } = new();

    public GroupDetailsPage(Group group)
    {
        InitializeComponent();
       Events = new ObservableCollection<Event>(group.Events);
        if (Events.Count == 0) 
        {
             // LoadFakeEvents(); // Only if needed
        }

        BindingContext = this; 
        EventsCollection.ItemsSource = Events;
    }

    public GroupDetailsPage()
    {
        InitializeComponent();
        LoadFakeEvents();
        EventsCollection.ItemsSource = Events;
    }

    private void LoadFakeEvents()
    {
        Events.Clear();
        Events.Add(new Event
        {
            Name = "Kabacki Las",
            Description = "Wycieczka lasem",
            Time = "Piątek 18:00",
            ImageUrl = "free.png",
            IsActionButtonsVisible = true
        });
        Events.Add(new Event
        {
            Name = "Zakopane",
            Description = "Góry i narty",
            Time = "Wtorek 10:00",
            ImageUrl = "free.png",
            IsActionButtonsVisible = false
        });
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

   private void OnChangeDecisionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedEvent = button?.BindingContext as Event;

        if (selectedEvent != null)
        {
            // Show the choice buttons again
            selectedEvent.IsActionButtonsVisible = true;
        }
    }

    // Existing methods (OnDecisionClicked, OnBackClicked, etc.) remain the same
    private void OnDecisionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedEvent = button?.BindingContext as Event;
        if (selectedEvent != null)
        {
            selectedEvent.IsActionButtonsVisible = false;
        }
    }

    // === ЛОГИКА ПЕРЕКЛЮЧЕНИЯ ВКЛАДОК ===
    // private void OnTabClicked(object sender, TappedEventArgs e)
    // {
    //     // 1. Получаем параметр (Photos, Videos, Events, Links)
    //     string tabName = e.Parameter as string;

    //     // 2. Сбрасываем ВСЕ вкладки в "неактивное" состояние (серые, без линии)
    //     ResetTabs();

    //     // 3. Активируем нужную
    //     switch (tabName)
    //     {
    //         case "Photos":
    //             LblPhotos.Opacity = 1.0;
    //             LinePhotos.IsVisible = true;
    //             EventsCollection.IsVisible = false;
    //             OtherContent.IsVisible = true; // Показываем "Tu nic nie ma"
    //             break;
    //         case "Videos":
    //             LblVideos.Opacity = 1.0;
    //             LineVideos.IsVisible = true;
    //             EventsCollection.IsVisible = false;
    //             OtherContent.IsVisible = true;
    //             break;
//             case "Events":
//                 LblEvents.Opacity = 1.0;
//                 LineEvents.IsVisible = true;
//                 EventsCollection.IsVisible = true; // Показываем список ивентов
//                 OtherContent.IsVisible = false;
//                 break;
//             case "Links":
//                 LblLinks.Opacity = 1.0;
//                 LineLinks.IsVisible = true;
//                 EventsCollection.IsVisible = false;
//                 OtherContent.IsVisible = true;
//                 break;
//                 // Добавь этот case:
// case "Files":
//     LblFiles.Opacity = 1.0;
//     LineFiles.IsVisible = true;
//     EventsCollection.IsVisible = false;
//     OtherContent.IsVisible = true;
//     break;
//         }
//     }
private async void OnEventSelected(object sender, SelectionChangedEventArgs e)
    {
        // 1. Проверяем, что нажали на ивент
        var selectedEvent = e.CurrentSelection.FirstOrDefault() as Event;
        if (selectedEvent == null) return;

        // 2. Снимаем выделение (чтобы не горело серым)
        ((CollectionView)sender).SelectedItem = null;

        // 3. Переходим на страницу деталей ИВЕНТА
        await Navigation.PushAsync(new EventDetailsPage(selectedEvent));
    }
//     private void ResetTabs()
//     {
//         // Делаем все тексты полупрозрачными
//         LblPhotos.Opacity = 0.5;
//         LblVideos.Opacity = 0.5;
//         LblEvents.Opacity = 0.5;
//         LblLinks.Opacity = 0.5;

//         // Скрываем все линии
//         LinePhotos.IsVisible = false;
//         LineVideos.IsVisible = false;
//         LineEvents.IsVisible = false;
//         LineLinks.IsVisible = false;
//         LblFiles.Opacity = 0.5;
// LineFiles.IsVisible = false;
//     }
private async void OnCreateEventClicked(object sender, EventArgs e)
    {
        // Переход на страницу создания
        await Navigation.PushAsync(new CreateEventPage());
    }
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
    
}