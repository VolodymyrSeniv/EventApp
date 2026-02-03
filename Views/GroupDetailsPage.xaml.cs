using System.Collections.ObjectModel;
using MauiAppB.Models;
using MauiAppB.ViewModels;

namespace MauiAppB.Views;

public partial class GroupDetailsPage : ContentPage
{
    public ObservableCollection<Event> Events { get; set; } = new();

    public GroupDetailsPage(GroupDetailsViewModel groupDetailsViewModel)
    {
        InitializeComponent();
        BindingContext = groupDetailsViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as GroupDetailsViewModel;
        if (vm != null)
        {
            vm.LoadEvents(); // Wywołuje odświeżenie z bazy
        }
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
            selectedEvent.IsActionButtonsVisible = true;
        }
    }
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
        var selectedEvent = e.CurrentSelection.FirstOrDefault() as Event;
        if (selectedEvent == null) return;
        ((CollectionView)sender).SelectedItem = null;
        await Navigation.PushAsync(new EventDetailsPage(selectedEvent));
    }
private async void OnCreateEventClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as GroupDetailsViewModel;
        if (vm?.Groupka != null)
        {
            // Przekazujemy ID aktualnej grupy do konstruktora
            await Navigation.PushAsync(new CreateEventPage(vm.Groupka.Id));
        }
    }
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
        SettingsOverlay.IsVisible = false;

        var vm = BindingContext as GroupDetailsViewModel;
        if (vm?.Groupka != null)
        {
            await vm.DeleteGroupCommand.ExecuteAsync(vm.Groupka.Id);
        }
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
    private async void OnUpdateGroupClicked(object sender, EventArgs e)
    {
        SettingsOverlay.IsVisible = false;
        var detailsVm = BindingContext as GroupDetailsViewModel;

        if (detailsVm?.Groupka != null)
        {
            var listViewModel = Handler.MauiContext.Services.GetService<GroupListViewModel>();
            var editPage = new CreateGroupPage(listViewModel);
            editPage.InitializeForEdit(detailsVm.Groupka);
            await Navigation.PushAsync(editPage);
        }
    }
}