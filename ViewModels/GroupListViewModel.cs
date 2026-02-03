using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppB.Models;
using MauiAppB.Services;
using MauiAppB.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiAppB.ViewModels
{
    public partial class GroupListViewModel : BaseViewModel
    {
        public readonly GroupService groupService;
        public ObservableCollection<Group> Groups { get; private set; } = new();

        [ObservableProperty]
        string newGroupName;

        [ObservableProperty]
        int currentGroupId; // 0 = Create Mode, >0 = Edit Mode

        [ObservableProperty]
        string pageTitle = "Stwórz grupę"; // Default title

        [ObservableProperty]
        string buttonText = "Utwórz grupę"; // Default button text

        [ObservableProperty]
        string selectedPhotoPath = "group_placeholder.png";

        public ObservableCollection<string> AddedUsers { get; } = new();

        // NEW METHOD: Call this when opening the page for editing
        public void SetEditMode(Group groupToEdit)
        {
            CurrentGroupId = groupToEdit.Id;
            NewGroupName = groupToEdit.Name;
            SelectedPhotoPath = groupToEdit.PhotoUrl;

            // Change UI text
            PageTitle = "Edytuj grupę";
            ButtonText = "Zapisz zmiany";
        }

        // NEW METHOD: Call this when opening the page for creating
        public void SetCreateMode()
        {
            CurrentGroupId = 0;
            NewGroupName = string.Empty;
            SelectedPhotoPath = "group_placeholder.png";
            AddedUsers.Clear();

            // Reset UI text
            PageTitle = "Stwórz grupę";
            ButtonText = "Utwórz grupę";
        }

        [ObservableProperty]
        bool isRefreshing;

        public GroupListViewModel(GroupService groupService)
        {
            Title = "Groups";
            this.groupService = groupService;
            // Bezpieczniejsze wywołanie początkowe
            MainThread.BeginInvokeOnMainThread(async () => await GetGroupsList());
        }

        [RelayCommand]
        async Task GetGroupsList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;

                // Pobieramy dane przez wstrzyknięty serwis
                var groups = groupService.GetGroups();

                Groups.Clear();
                foreach (var group in groups)
                {
                    Groups.Add(group);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get groups: {ex.Message}");
                // Używamy MainPage zamiast Shell.Current
                await Application.Current.MainPage.DisplayAlert("Error!", "Failed to retrieve a list of groups", "OK");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GetGroupDetails(int id)
        {
            if (id == 0) return;

            var selectedGroup = groupService.GetGroup(id);
            if (selectedGroup == null) return;

            // Pobieramy VM z DI
            var detailsVM = IPlatformApplication.Current.Services.GetService<GroupDetailsViewModel>();

            // KLUCZOWE: Musisz ustawić Id przed wywołaniem LoadEvents
            detailsVM.Id = selectedGroup.Id;
            detailsVM.Groupka = selectedGroup;

            var detailsPage = new GroupDetailsPage(detailsVM);
            await Application.Current.MainPage.Navigation.PushAsync(detailsPage);
        }

        [RelayCommand]
        async Task AddGroup()
        {
            if (string.IsNullOrWhiteSpace(NewGroupName))
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", "Wpisz nazwę grupy!", "OK");
                return;
            }

            try
            {
                IsLoading = true;

                var groupData = new Group
                {
                    // If CurrentGroupId is 0, SQLite will auto-generate a new ID.
                    // If it is >0, we keep it so SQLite knows which row to update.
                    Id = CurrentGroupId,
                    Name = NewGroupName,
                    PhotoUrl = SelectedPhotoPath,
                    StatusText = "Active",
                    StatusColor = "#2ECC71",
                    NotificationCount = 0
                };

                if (CurrentGroupId == 0)
                {
                    // === CREATE MODE ===
                    groupService.AddGroup(groupData);
                    await Application.Current.MainPage.DisplayAlert("Sukces", "Grupa utworzona!", "OK");
                }
                else
                {
                    // === EDIT MODE ===
                    App.GroupService.UpdateGroup(groupData);
                    await Application.Current.MainPage.DisplayAlert("Sukces", "Grupa zaktualizowana!", "OK");
                }

                // Cleanup
                SetCreateMode(); // Reset form
                await GetGroupsList(); // Refresh list
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Błąd", "Operacja nieudana", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}