using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Models;

namespace MauiApp1.ViewModels;

[QueryProperty(nameof(Eventik), "Event")]
public partial class EventDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    Event eventik;
}