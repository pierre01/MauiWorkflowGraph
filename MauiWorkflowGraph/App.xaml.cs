namespace MauiWorkflowGraph
{
    public partial class App : Application
    {
        const double newWidth = 800d;
        const double newHeight = 900d;

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            //app.Services
            var window = new Window(new AppShell());

            // Center the window.X and Y on the main display
            window.X = (DeviceDisplay.MainDisplayInfo.Width - newWidth) / 2;
            window.Y = (DeviceDisplay.MainDisplayInfo.Height - newHeight) / 2;

            // Lock the window size
            window.Width = newWidth;
            window.Height = newHeight;

            return window;
        }

    }
}