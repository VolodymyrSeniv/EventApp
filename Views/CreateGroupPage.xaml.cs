using MauiAppB.Models;
using MauiAppB.Services;
using MauiAppB.ViewModels;
using Microsoft.Maui.Controls.Shapes;

namespace MauiAppB.Views;

public partial class CreateGroupPage : ContentPage
{
    private List<string> addedUsers = new List<string>();
    private readonly GroupListViewModel _viewModel;
    private string selectedPhotoPath = "group_placeholder.png";

    public CreateGroupPage(GroupListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Ensure we are in create mode by default
        _viewModel.SetCreateMode();
    }
    public void InitializeForEdit(Group group)
    {
        _viewModel.SetEditMode(group);

        // Update the photo preview manually in Code-behind
        if (!string.IsNullOrEmpty(group.PhotoUrl))
        {
            selectedPhotoPath = group.PhotoUrl;
            GroupImagePreview.Source = ImageSource.FromFile(selectedPhotoPath);
            GroupImagePreview.IsVisible = true;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync();
            if (result != null)
            {
                selectedPhotoPath = result.FullPath;
                GroupImagePreview.Source = ImageSource.FromFile(selectedPhotoPath);
                GroupImagePreview.IsVisible = true;
                await DisplayAlert("Sukces", "Zdjęcie zostało wybrane", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Nie udało się wybrać zdjęcia: {ex.Message}", "OK");
        }
    }

    private void OnAddMemberClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text?.Trim();

        if (string.IsNullOrEmpty(username))
        {
            DisplayAlert("Błąd", "Wpisz nazwę użytkownika!", "OK");
            return;
        }

        if (addedUsers.Contains(username))
        {
            DisplayAlert("Info", "Ten użytkownik jest już dodany.", "OK");
            return;
        }

        addedUsers.Add(username);

        // Tworzenie wizualnego elementu listy
        var userGrid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = GridLength.Auto } },
            Padding = new Thickness(10)
        };

        var userBorder = new Border
        {
            Stroke = Colors.Gray,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            BackgroundColor = Colors.White,
            Content = userGrid,
            Margin = new Thickness(0, 0, 0, 5)
        };

        userGrid.Add(new Label { Text = username, TextColor = Colors.Black, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(10, 0, 0, 0) });

        var removeBtn = new Button { Text = "✕", TextColor = Colors.Red, BackgroundColor = Colors.Transparent, WidthRequest = 40 };
        removeBtn.Clicked += (s, args) => {
            MembersStack.Children.Remove(userBorder);
            addedUsers.Remove(username);
        };

        userGrid.Add(removeBtn, 1);
        MembersStack.Children.Add(userBorder);
        UsernameEntry.Text = string.Empty;
    }
}