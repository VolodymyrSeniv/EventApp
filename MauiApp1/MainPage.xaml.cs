using MauiApp1.ViewModels;
using System;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {

        public MainPage(EventListViewModel eventListViewModel)
        {
            InitializeComponent();
            BindingContext = eventListViewModel;
        }
    }
}
