using CommunityToolkit.Maui.Views;
using MauiApp1.Models;

namespace MauiApp1.Views;

public partial class EventFormPopup : Popup
{
    // 1. Create a public property to store the result
    public Event? EventResult { get; private set; }

    public EventFormPopup()
    {
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // Just close without setting the result
        await CloseAsync();
    }

    // 2. Change 'void' to 'async void' so we can await
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            TitleEntry.PlaceholderColor = Colors.Red;
            return;
        }

        var newEvent = new Event
        {
            Title = TitleEntry.Text,
            Location = LocationEntry.Text,
            Description = DescEntry.Text,
            Date = (DateTime)EventDatePicker.Date,
            ImageUrl = "default_event.png"
        };

        // 3. Store the result in the property
        EventResult = newEvent;

        // 4. Call the Async method (with no parameters, as per your definition)
        await CloseAsync();
    }
}