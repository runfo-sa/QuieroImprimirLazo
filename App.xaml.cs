using Microsoft.Extensions.DependencyInjection;
using QuieroLazos.ViewModels;
using System.Windows;

namespace QuieroLazos
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider ServiceProvider => ConfigureServices();

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Services

            // Viewmodels
            services.AddTransient<MainViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
