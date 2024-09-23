using Microsoft.Extensions.DependencyInjection;
using QuieroLazos.ViewModels;
using System.Windows;

namespace QuieroLazos
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = App.Current.ServiceProvider.GetService<MainViewModel>();
        }
    }
}
