using System.Collections.ObjectModel;
using MauiAppB.Models;
using Microsoft.Maui.Controls;
using System.Linq;

namespace MauiAppB.Views;

public partial class GroupsPage : ContentPage
{
    public ObservableCollection<Group> Groups { get; set; } = new();

    public GroupsPage()
    {
        InitializeComponent();
        LoadFakeGroups(); // Load data
        BindingContext = this;
    }

    private void LoadFakeGroups()
    {
        Groups.Clear();

        // --- GROUP 1: Rowerzyści ---
        var group1 = new Group
        {
            Name = "Rowerzyści",
            PhotoUrl = "profil.png",
            StatusText = "Jest aktualne wydarzenie 🔴"
        };
        
        // Add event to group 1
        group1.Events.Add(new Event
        {
            Name = "Kabacki Las",
            Time = "Piątek 18:00",
            Description = "Wycieczka lasem",
            ImageUrl = "free.png",
            IsActionButtonsVisible = true
        });

        // --- GROUP 2: WPAM ---
        var group2 = new Group
        {
            Name = "WPAM",
            PhotoUrl = "profil.png",
            StatusText = "Wszystkie wydarzenia są zakończone ✅"
        };

        // Add event to group 2
        group2.Events.Add(new Event
        {
            Name = "Zakopane",
            Time = "Wtorek 10:00",
            Description = "Góry i narty",
            ImageUrl = "free.png",
            IsActionButtonsVisible = false
        });
        
        var group3 = new Group
        {
            Name = "Kings 👑",
            PhotoUrl = "profil.png",
            StatusText = "Jest aktualne wydarzenie 🔴"
        };

        Groups.Add(group1);
        Groups.Add(group2);
        Groups.Add(group3);
    }

    private async void OnGroupSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedGroup = e.CurrentSelection.FirstOrDefault() as Group;
        if (selectedGroup == null) return;

        // Navigate to details page passing the selected group
        await Navigation.PushAsync(new GroupDetailsPage(selectedGroup));

        // Deselect item
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void OnProfileClicked(object sender, EventArgs e) 
    {
         // Переход на страницу профиля
         await Navigation.PushAsync(new ProfilePage());
    }
    private async void OnCreateGroupClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateGroupPage());
    }
}