namespace MauiApp1.Views;
using System;
using MauiApp1.ViewModels;
public partial class EventDetailPage : ContentPage
{
	public EventDetailPage(EventDetailViewModel eventDetailViewModel)
	{
        InitializeComponent();
		BindingContext = eventDetailViewModel;
    }

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
    }
}