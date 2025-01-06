using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Helpers;
using System.IO;

namespace QuieroLazos.Models
{
    public partial class Tropa(string number, int cantidad) : ObservableObject
    {
        public string Number => number;

        private TropaState _state;

        private int _garron = 1;
        private int _garronSet = 0;
        private int _cantidad = cantidad;

        private static List<Producto> _productos = [
                new Producto("CORAZON"),
                new Producto("HIGADO"),
                new Producto("LENGUA"),
                new Producto("LIBRILLO"),
                new Producto("MONDONGO"),
                new Producto("RIÑON", "RI%A5ON")
            ];

        public TropaState State
        {
            get => _state;
            private set
            {
                SetProperty(ref _state, value);
                StateColor = _state switch
                {
                    TropaState.Available => "#99FF99",
                    TropaState.ToClose => "#FF9999",
                    TropaState.Closed => "#9c9c9c",
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
        private void PrintTropa(object param)
        {
            var values = (object[])param;
            var printer = (string)values[0];
            var garron = int.Parse((string)values[2]);

            if (garron != _garronSet)
            {
                _garronSet = garron;
                _garron = 1;
            }

            if (_cantidad == 1)
            {
                State = TropaState.ToClose;
            }

            if (State != TropaState.Closed)
            {
                var path = State switch
                {
                    TropaState.Available => ".\\Etiquetas\\lazo.e01",
                    TropaState.ToClose => ".\\Etiquetas\\lazo_black.e01",
                    _ => "",
                };

                if (State == TropaState.ToClose)
                {
                    foreach (var p in _productos)
                    {
                        var content = File.ReadAllText(path)
                            .Replace("[@GARRON@]", (garron + _garron).ToString(), StringComparison.CurrentCultureIgnoreCase)
                            .Replace("[@CORTE@]", p.Traduccion, StringComparison.CurrentCultureIgnoreCase)
                            .Replace("[@TROPA@]", Number, StringComparison.CurrentCultureIgnoreCase);
                        PrinterHelper.SendStringToPrinter(printer, content, $"Tropa {Number}");
                    }
                }
                else
                {
                    var content = File.ReadAllText(path)
                        .Replace("[@GARRON@]", (garron + _garron++).ToString(), StringComparison.CurrentCultureIgnoreCase)
                        .Replace("[@CORTE@]", "MONDONGO", StringComparison.CurrentCultureIgnoreCase)
                        .Replace("[@TROPA@]", Number, StringComparison.CurrentCultureIgnoreCase);
                    PrinterHelper.SendStringToPrinter(printer, content, $"Tropa {Number}");
                }

                _cantidad--;
            }

            if (_cantidad == 0)
            {
                State = TropaState.Closed;
            }
        }
    }
}