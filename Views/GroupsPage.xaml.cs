using System.Collections.ObjectModel;
using MauiAppB.Models;
using MauiAppB.ViewModels;
using Microsoft.Maui.Controls;
using System.Linq;

namespace MauiAppB.Views;

public partial class GroupsPage : ContentPage
{

    public GroupsPage(GroupListViewModel groupListViewModel)
    {
        InitializeComponent();
        BindingContext = groupListViewModel;
    }

    //private async void OnGroupSelected(object sender, SelectionChangedEventArgs e)
    //{
    //    var selectedGroup = e.CurrentSelection.FirstOrDefault() as Group;
    //    if (selectedGroup == null) return;

    //    // Navigate to details page passing the selected group
    //    var viewModel = new GroupDetailsViewModel(selectedGroup);
    //    await Navigation.PushAsync(new GroupDetailsPage(selectedGroup));

    //    // Deselect item
    //    ((CollectionView)sender).SelectedItem = null;
    //}

    private async void OnProfileClicked(object sender, EventArgs e) 
    {
         // Переход на страницу профиля
         await Navigation.PushAsync(new ProfilePage());
    }
    private async void OnCreateGroupClicked(object sender, EventArgs e)
    {
        // Pobieramy stronę z kontenera - DI automatycznie wstrzyknie GroupService
        var createGroupPage = Handler.MauiContext.Services.GetService<CreateGroupPage>();

        if (createGroupPage != null)
        {
            await Navigation.PushAsync(createGroupPage);
        }
        else
        {
            // Alternatywa, jeśli DI nie jest w pełni skonfigurowane dla stron:
            // await Navigation.PushAsync(new CreateGroupPage(App.GroupService));
        }
    }
}