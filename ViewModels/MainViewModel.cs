namespace QuieroLazos.ViewModels
{
    public partial class MainViewModel : ObservableRecipient, IRecipient<TropaUpdated>
    {
        public MainViewModel()
        {
            _stateBar = "Cargando Tropas...";
            Tropas = [];
            GetTropasCommand.Execute(null);
        }

        private State _state;

        [ObservableProperty]
        private string _stateBar;

        public static ObservableCollection<Tropa> Tropas { get; private set; }

        public static ListCollectionView Printers => new(PrinterSettings.InstalledPrinters.Cast<string>().ToList());

        public void Receive(TropaUpdated message)
        {
            StateBar = (++_state).ToString();
        }

        [RelayCommand]
        private async Task GetTropas()
        {
            Tropas.Clear();
            using (var connection = new SqlConnection("#DATABASE-CONNECTION_STRING#"))
            {
                connection.Open();
                using var command = new SqlCommand("SELECT * FROM [RunfoApps].[lazos].[GetTropas]", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var tropa = reader.GetInt32(0).ToString();
                    Tropas.Add(new Tropa(tropa));
                }
            }

            _state = new(Tropas.Count);
            StateBar = _state.ToString();
            IsActive = true;
        }
    }
}
