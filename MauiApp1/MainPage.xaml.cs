namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void LoginClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                LoginBtn.Text = $"Clicked {count} time login";
            else
                LoginBtn.Text = $"Clicked {count} times login";

            SemanticScreenReader.Announce(LoginBtn.Text);
        }
        private void RegisterClicked(object? sender, EventArgs e) 
        {
            count++;

            if (count == 1)
                RegisterBtn.Text = $"Clicked {count} time register";
            else
                RegisterBtn.Text = $"Clicked {count} times register";

            SemanticScreenReader.Announce(RegisterBtn.Text);
        }
    }
}
