using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppB.Models;
using MauiAppB.Services;
using MauiAppB.Views;
using SQLite;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiAppB.ViewModels
{
    public partial class EventListViewModel : BaseViewModel
    {
        public readonly EventService eventService;

        // Kolekcja bindowana do CollectionView w XAML
        public ObservableCollection<Event> Events { get; private set; } = new();

        [ObservableProperty]
        private int _groupId;

        [ObservableProperty]
        string newEventName;

        [ObservableProperty]
        int currentEventId;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        string selectedPhotoPath = "group_placeholder.png";

        public EventListViewModel(EventService eventService)
        {
            this.eventService = eventService;
            Title = "Wydarzenia grupy";
        }

        // Metoda wywoływana, gdy chcemy załadować wydarzenia dla konkretnej grupy
        [RelayCommand]
        public async Task GetEventsAsync()
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;

                // Pobieramy dane z serwisu używając wygenerowanego GroupId
                var events = eventService.GetEventsForGroup(GroupId);

                Events.Clear();
                foreach (var evt in events)
                {
                    Events.Add(evt);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Błąd: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Błąd", "Nie udało się pobrać listy", "OK");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task GoToDetailsAsync(Event selectedEvent)
        {
            if (selectedEvent == null) return;

            // Nawigacja do strony szczegółów wydarzenia
            await Application.Current.MainPage.Navigation.PushAsync(new EventDetailsPage(selectedEvent));
        }

        //[RelayCommand]
        //private async Task AddEventAsync()
        //{
        //    // Nawigacja do kreatora wydarzeń, przekazujemy ID grupy
        //    await Application.Current.MainPage.Navigation.PushAsync(new CreateEventPage(GroupId));

        //    if (string.IsNullOrWhiteSpace(newEventName))
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Błąd", "Wpisz nazwę grupy!", "OK");
        //        return;
        //    }

        //    try
        //    {
        //        IsLoading = true;

        //        var eventData = new Event
        //        {
        //            // If CurrentGroupId is 0, SQLite will auto-generate a new ID.
        //            // If it is >0, we keep it so SQLite knows which row to update.
        //            Id = CurrentEventId,
        //            Name = NewEventName,
        //            Description = Description,
        //            ImageUrl = SelectedPhotoPath,
        //        };

        //        if (CurrentEventId == 0)
        //        {
        //            // === CREATE MODE ===
        //            eventService.AddEvent(eventData);
        //            await Application.Current.MainPage.DisplayAlert("Sukces", "Grupa utworzona!", "OK");
        //        }
        //        else
        //        {
        //            // === EDIT MODE ===
        //            //App.GroupService.UpdateGroup(eventData);
        //            await Application.Current.MainPage.DisplayAlert("Sukces", "Grupa zaktualizowana!", "OK");
        //        }

        //        await GetEventsAsync(); // Refresh list
        //        await Application.Current.MainPage.Navigation.PopAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Error: {ex.Message}");
        //        await Application.Current.MainPage.DisplayAlert("Błąd", "Operacja nieudana", "OK");
        //    }
        //    finally
        //    {
        //        IsLoading = false;
        //    }
        //}

        [RelayCommand]
        private async Task DeleteEventAsync(Event eventToDelete)
        {
            if (eventToDelete == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert("Usuwanie", $"Czy na pewno chcesz usunąć {eventToDelete.Name}?", "Tak", "Nie");

            if (confirm)
            {
                try
                {
                    eventService.DeleteEvent(eventToDelete.Id);
                    Events.Remove(eventToDelete);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd", "Nie udało się usunąć wydarzenia", "OK");
                }
            }
        }
    }
}