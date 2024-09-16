using System;
using System.Collections.Generic;
using Firebase.Database;
using System.Threading.Tasks;
using OrdenaYA.models;
using Xamarin.Forms;
using Firebase.Database.Query;
using System.Collections;

namespace OrdenaYaVersion1.Views.Mesas
{
    public partial class Mesas : ContentPage
    {
        //Lista para almacenar las mesas
        private List<Mesa> mesas;

        public Mesas()
        {
            InitializeComponent();

            //Obtener todas las mesas
            mesas = App.Database.Table<Mesa>().ToList();
            ActualizarListas();
        }

        private void ActualizarListas()
        {
            foreach (var mesa in mesas)
            {
                var button = new Button
                {
                    Text = mesa.Numero.ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = mesa.Ocupada ? Color.Red : Color.Green,
                    Command = new Command(() => OnMesaSelected(mesa))
                };
                if (mesa.Ocupada)
                {
                    MesasOcupadasStackLayout.Children.Add(button);
                }
                else
                {
                    MesasLibresStackLayout.Children.Add(button);
                }
            }
        }

        private async void OnMesaSelected(Mesa mesaSeleccionada)
        {
            if (!mesaSeleccionada.Ocupada)
            {
                // Si la mesa no está ocupada, crear una nueva comanda
                Comanda nuevaComanda = new Comanda(mesaSeleccionada.Numero, new List<Producto>());

                // Navegar a la página de TomarComanda sin ocupar la mesa ni guardar la comanda en la base de datos
                var tomarComandaPage = new TomarComanda.TomarComanda(nuevaComanda);
                await Navigation.PushAsync(tomarComandaPage);
            }
            else
            {
                // Si la mesa está ocupada, mostrar un mensaje de error
                await DisplayAlert("Mesa Ocupada", "Lo siento, esta mesa ya está ocupada. No puedes tomar una comanda en una mesa ocupada.", "OK");
            }
        }

    }
}

