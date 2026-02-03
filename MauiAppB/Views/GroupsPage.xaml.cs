using System.Collections.ObjectModel;
using MauiAppB.Models;
using MauiAppB.Services;
using Microsoft.Maui.Controls;
using System.Linq;

namespace MauiAppB.Views;

public partial class GroupsPage : ContentPage
{
    public ObservableCollection<Group> Groups { get; set; } = new();

    public GroupsPage()
    {
        InitializeComponent();
      
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadGroupsFromServer();
    }

    private async Task LoadGroupsFromServer()
    {
        var apiService = new ApiService();
        var realGroups = await apiService.GetGroups();

        Groups.Clear();

        foreach (var group in realGroups)
        {
            // === ВАЖНОЕ ИСПРАВЛЕНИЕ ===
            // Если с сервера пришла пустота (null), ставим стандартную.
            // А если там есть путь (даже длинный), оставляем его как есть!
            if (string.IsNullOrEmpty(group.PhotoUrl)) 
            {
                group.PhotoUrl = "profil.png";
            }
            // ===========================

            Groups.Add(group);
        }
    }

    private async void OnGroupSelected(object sender, SelectionChangedEventArgs e)
    {
        // 1. Получаем группу, на которую нажали
        var selectedGroup = e.CurrentSelection.FirstOrDefault() as Group;
        
        // Если нажатие было ложным (null), ничего не делаем
        if (selectedGroup == null) return;

        // 2. Снимаем выделение (чтобы строка не оставалась серой/синей)
        ((CollectionView)sender).SelectedItem = null;

        // 3. === ВАЖНО: ПЕРЕХОДИМ НА СТРАНИЦУ ДЕТАЛЕЙ ===
        // Мы передаем selectedGroup в конструктор следующей страницы
        await Navigation.PushAsync(new GroupDetailsPage(selectedGroup));
    }

    private async void OnProfileClicked(object sender, EventArgs e) 
    {
         await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnCreateGroupClicked(object sender, EventArgs e)
    {
        // Переходим на страницу создания
        await Navigation.PushAsync(new CreateGroupPage());
    }
}