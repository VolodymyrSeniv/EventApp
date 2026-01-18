using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services;
using MauiApp1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public partial class EventListViewModel : BaseViewModel
    {
        [ObservableProperty]
        Event newEvent = new Event { Date = DateTime.Now };
        public ObservableCollection<Event> Events { get; private set; } = new();
        public EventListViewModel(EventService eventService)
        {
            Title = "Events";
            Task.Run(async () => await GetEventList());
        }

        [ObservableProperty]
        bool isRefreshing;


        [RelayCommand]
        async Task GetEventList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                if (Events.Any()) Events.Clear();
                var events = App.EventService.GetEvents();
                foreach (var ev in events)
                {
                    Events.Add(ev);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get events: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Failed to retrieve a list of events", "OK");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }
        [RelayCommand]
        async Task GetEventDetails(int id)
        {
            if (id == 0) return;
            await Shell.Current.GoToAsync($"{nameof(EventDetailPage)}?Id={id}", true);

        }

        [RelayCommand]
        async Task AddEvent()
        {
            // 1. Create and show the popup
            var popup = new EventFormPopup();

            // 2. Wait here until the user clicks "Save" or "Cancel" in the popup
            var result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            // 3. Check if we received valid data back (result is null if they clicked Cancel)
            if (popup.EventResult is Event eventik)
            {
                // 4. Save to Database
                App.EventService.AddEvent(eventik);

                // 5. Show success message
                await Shell.Current.DisplayAlert("Success", "Event created successfully", "Ok");

                // 6. Update the UI List
                Events.Add(eventik);

                // OR keep your original reload if you prefer:
                await GetEventList();
            }
        }

        [RelayCommand]
        async Task DeleteEvent(int id) 
        {
            if (id == 0) 
            {
                await Shell.Current.DisplayAlert("Invalid Record", "Please try again", "Ok");
                return;
            }
            var result = App.EventService.DeleteEvent(id);
            if (result == 0) await Shell.Current.DisplayAlert("Invalid Data", "Please insert valid data", "Ok");
            else 
            {
                await Shell.Current.DisplayAlert("Deletion Successful", "Record Removeds Successfully", "Ok");
                await GetEventList();
            }

        }
    }
}
