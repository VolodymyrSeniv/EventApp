using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public partial class EventListViewModel : BaseViewModel
    {
        private readonly EventService eventService;
        public ObservableCollection<Event> Events { get; private set; } = new();
        public EventListViewModel(EventService eventService) 
        {
            Title = "Event List";
            this.eventService = eventService;
        }

        [RelayCommand]
        async Task GetEventList() 
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                if (Events.Any()) Events.Clear();
                var events = eventService.GetEvents();
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
            }
        }
    }
}
