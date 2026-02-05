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

        public void SetEditMode(Group groupToEdit)
        {
            CurrentGroupId = groupToEdit.Id;
            NewGroupName = groupToEdit.Name;
            SelectedPhotoPath = groupToEdit.PhotoUrl;

            PageTitle = "Edytuj grupę";
            ButtonText = "Zapisz zmiany";
        }

        public void SetCreateMode()
        {
            CurrentGroupId = 0;
            NewGroupName = string.Empty;
            SelectedPhotoPath = "group_placeholder.png";
            AddedUsers.Clear();

            PageTitle = "Stwórz grupę";
            ButtonText = "Utwórz grupę";
        }

        [ObservableProperty]
        bool isRefreshing;

        public GroupListViewModel(GroupService groupService)
        {
            Title = "Groups";
            this.groupService = groupService;
            MainThread.BeginInvokeOnMainThread(async () => await GetGroupsList());
        }

        [RelayCommand]
        public async Task GetGroupsList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                // Retrieve the logged-in user's ID
                int currentUserId = Preferences.Get("UserId", 0);

                if (currentUserId == 0)
                {
                    Debug.WriteLine("BŁĄD: Brak ID użytkownika w preferencjach!");
                    return;
                }

                // Use the instance-based service instead of App.GroupService for consistency
                var groups = groupService.GetGroups(currentUserId);

                Groups.Clear();
                foreach (var group in groups)
                {
                    Groups.Add(group);
                }
                Debug.WriteLine($"GetGroupsList: loaded {Groups.Count} groups for user {currentUserId}");
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

            var detailsVM = IPlatformApplication.Current.Services.GetService<GroupDetailsViewModel>();

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
                int currentUserId = Preferences.Get("UserId", 0);

                var groupData = new Group
                {
                    Id = CurrentGroupId,
                    Name = NewGroupName,
                    PhotoUrl = SelectedPhotoPath,
                    StatusText = "Active",
                    StatusColor = "#2ECC71",
                    NotificationCount = 0,
                    CreatorId = currentUserId // Explicitly assign the current user as owner
                };

                if (CurrentGroupId == 0) // 0 means NEW Group
                {
                    groupService.AddGroup(groupData);
                    await Application.Current.MainPage.DisplayAlert("Sukces", "Grupa utworzona!", "OK");
                }
                else
                {
                    groupService.UpdateGroup(groupData);
                    await Application.Current.MainPage.DisplayAlert("Sukces", "Grupa zaktualizowana!", "OK");
                }

                SetCreateMode();
                await GetGroupsList(); // Refresh the list before navigating back
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}