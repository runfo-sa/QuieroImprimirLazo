using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Helpers;
using System.IO;

namespace QuieroLazos.Models
{
    public partial class Tropa(string number) : ObservableObject
    {
        public string Number => number;

        private TropaState _state;
        public TropaState State
        {
            get => _state;
            private set
            {
                SetProperty(ref _state, value);
                StateColor = _state switch
                {
                    TropaState.Available => "#99FF99",
                    TropaState.Open => "#FF9999",
                    TropaState.Closed => "#99CCFF",
                    _ => throw new NotImplementedException()
                };
                if (State == TropaState.Closed)
                {
                    WeakReferenceMessenger.Default.Send(new TropaUpdated());
                }
            }
        }

        private string _stateColor = "#99FF99";
        public string StateColor
        {
            get => _stateColor;
            set => SetProperty(ref _stateColor, value);
        }

        [RelayCommand]
        private void PrintTropa(string printer)
        {
            if (State != TropaState.Closed)
            {
                var path = State switch
                {
                    TropaState.Available => ".\\Etiquetas\\lazo.e01",
                    TropaState.Open => ".\\Etiquetas\\lazo_black.e01",
                    _ => "",
                };
                var content = File.ReadAllText(path).Replace("[@TROPA@]", Number, StringComparison.CurrentCultureIgnoreCase);
                PrinterHelper.SendStringToPrinter(printer, content, $"Tropa {Number}");

                State++;
            }
        }
    }
}
