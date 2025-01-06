using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Data.SqlClient;
using QuieroLazos.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Data;

namespace QuieroLazos.ViewModels
{
    public partial class MainViewModel : ObservableRecipient, IRecipient<TropaUpdated>
    {
        private State _state;

        [ObservableProperty]
        private string _stateBar;

        [ObservableProperty]
        private string _garronInicial = "0";

        public static ObservableCollection<Tropa> Tropas { get; private set; } = [];

        public static ListCollectionView Printers { get; set; } = new(GetPrinters());

        public MainViewModel()
        {
            _stateBar = "Cargando Tropas...";
            GetTropasCommand.Execute(null);
        }

        private static List<string> GetPrinters()
        {
            return PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        }

        public void Receive(TropaUpdated message)
        {
            StateBar = (++_state).ToString();
        }

        [RelayCommand]
        private async Task GetTropas()
        {
            Tropas.Clear();
            using (var connection = new SqlConnection(""))
            {
                await connection.OpenAsync();
                using var command = new SqlCommand("SELECT * FROM [RunfoApps].[lazos].[GetTropas]", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var tropa = reader.GetInt32(0).ToString();
                    var cant = reader.GetInt16(1);
                    Tropas.Add(new Tropa(tropa, cant));
                }
            }

            _state = new(Tropas.Count);
            StateBar = _state.ToString();
            IsActive = true;
        }
    }
}