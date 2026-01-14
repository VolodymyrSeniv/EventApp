using MauiApp1.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MauiApp1
{
    public partial class App : Application
    {
        public static EventService EventService { get; private set; } 
        public App(EventService eventService)
        {
            InitializeComponent();
            MainPage = new AppShell();
            EventService = eventService;
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return base.CreateWindow(activationState);
        }
    }
}