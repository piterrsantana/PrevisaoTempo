using Microsoft.Extensions.DependencyInjection;

namespace PrevisaoTempo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // Tamanho inicial da janela no Windows/Desktop
            window.Width = 400;
            window.Height = 700;

            // Define o tamanho mínimo para o usuário não diminuir demais
            window.MinimumWidth = 400;
            window.MinimumHeight = 700;

            // Define o tamanho máximo se quiser travar e impedir que expanda
            window.MaximumWidth = 400;
            window.MaximumHeight = 700;

            return window;
        }
    }
}