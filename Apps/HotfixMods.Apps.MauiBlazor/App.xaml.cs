namespace HotfixMods.Apps.MauiBlazor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            const int newWidth = 1920;
            const int newHeight = 1020;

            return new Window(new MainPage())
            {
                Width = newWidth,
                Height = newHeight
            };
        }
    }
}
