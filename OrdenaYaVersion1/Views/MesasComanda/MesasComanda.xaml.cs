using System;
using System.Collections.Generic;
using System.Linq;
using OrdenaYA.models;
using Xamarin.Forms;

namespace OrdenaYaVersion1.Views.MesasComanda
{
    public partial class MesasComanda : ContentPage
    {
        public MesasComanda()
        {
            InitializeComponent();

            // Obtener las mesas de la base de datos
            var mesas = App.Database.Table<Mesa>().ToList();

            // Filtrar las mesas ocupadas
            var mesasOcupadas = mesas.Where(m => m.Ocupada).ToList();

            // Si no hay mesas ocupadas, mostrar un mensaje
            if (!mesasOcupadas.Any())
            {
                txtTexto.Text = "No hay mesas para pagar.";
            }
            else
            {
                // Añadir las mesas ocupadas al FlexLayout
                foreach (var mesa in mesasOcupadas)
                {
                    var button = new Button
                    {
                        Text = mesa.Numero.ToString(),
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = mesa.Ocupada ? Color.Red : Color.Green,
                        Command = new Command(() => OnMesaSelected(mesa))
                    };
                    layoutMesasConComanda.Children.Add(button);
                }
            }
        }

        private async void OnMesaSelected(Mesa mesaSeleccionada)
        {
            //Obtener la comanda de la mesa seleccionada
            var comanda = App.Database.Table<Comanda>().FirstOrDefault(c => c.NumeroMesa == mesaSeleccionada.Numero);

            // Navegar a la ventana de PagarMesa pasando la comanda
            await Navigation.PushAsync(new PagarMesa.PagarMesa(comanda));
        }
    }

}

