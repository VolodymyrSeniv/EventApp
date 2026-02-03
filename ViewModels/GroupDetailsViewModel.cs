using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppB.Models;
using System.Collections.ObjectModel;
using MauiAppB.Services;
using MauiAppB.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace MauiAppB.ViewModels
{
    public partial class GroupDetailsViewModel: BaseViewModel, IQueryAttributable
    {
        private readonly EventService _eventService;

        [ObservableProperty]
        Group groupka;

        [ObservableProperty]
        int id;

        [ObservableProperty]
        string editName;

        [ObservableProperty]
        bool isRefreshing;

        // KOLEKCJA WYDARZEŃ TEJ GRUPY
        public ObservableCollection<Event> GroupEvents { get; } = new();

        public GroupDetailsViewModel(EventService eventService)
        {
            // Zakładam, że masz dostęp do serwisu tak jak do GroupService
            _eventService = eventService;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Id"))
            {
                int receivedId = Convert.ToInt32(query["Id"]);
                Id = receivedId;
                Groupka = App.GroupService.GetGroup(Id);

                // Przypisujemy dane do pól edycji (podobnie jak Make = car.Make)
                EditName = Groupka.Name;
                LoadEvents();
            }
        }

        [RelayCommand]
        public void LoadEvents()
        {
            Debug.WriteLine($"Ładowanie wydarzeń dla grupy o ID: {Id}"); // Sprawdź to w konsoli!
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                GroupEvents.Clear();

                // Jeśli Id tu wynosi 0, to wynik zawsze będzie pusty
                var events = _eventService.GetEventsForGroup(Id);

                foreach (var evt in events)
                {
                    GroupEvents.Add(evt);
                }
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        // PRZEJŚCIE DO TWORZENIA NOWEGO WYDARZENIA
        [RelayCommand]
        async Task GoToCreateEvent()
        {
            var createPage = new CreateEventPage(Id);
            await Application.Current.MainPage.Navigation.PushAsync(createPage);

        }

        [RelayCommand]
        async Task DeleteGroup(int id)
        {
            if (IsLoading) return;

            if (id == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", "Nieprawidłowy ID grupy", "Ok");
                return;
            }

            try
            {
                IsLoading = true;
                var result = App.GroupService.DeleteGroup(id);

                if (result > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Sukces", "Grupa została usunięta", "Ok");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd", "Grupa już nie istnieje lub nie została znaleziona", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", $"Wystąpił błąd: {ex.Message}", "Ok");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        async Task SaveGroup()
        {
            if (string.IsNullOrWhiteSpace(EditName))
            {
                await Application.Current.MainPage.DisplayAlert("Info", "Proszę podać nazwę grupy", "Ok");
                return;
            }

            // Aktualizujemy obiekt modelu danymi z pól edycji
            Groupka.Name = EditName;

            try
            {
                IsLoading = true;

                // Wywołanie usługi bazy danych (odpowiednik App.CarDatabaseService.UpdateCar)
                var result = App.GroupService.UpdateGroup(Groupka);

                if (result > 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Sukces", "Grupa zaktualizowana", "Ok");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd", "Nie udało się zapisać zmian", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
