using System;
using System.Collections.Generic;
using OrdenaYA.models;
using System.Linq;
using Xamarin.Forms;
using OrdenaYaVersion1.Models;

namespace OrdenaYaVersion1.Views.UnirMesa
{	
	public partial class UnirMesa : ContentPage
	{
        public UnirMesa()
        {
            InitializeComponent();

            // Obtener las mesas de la base de datos
            var mesas = App.Database.Table<Mesa>().ToList();
            var mesasOcupadas = mesas.Where(m => m.Ocupada).ToList();

            // Verificar si hay mesas ocupadas
            if (mesasOcupadas.Any())
            {
                // Llenar los Picker con las mesas ocupadas
                MesaOcupada1Picker.ItemsSource = mesasOcupadas;
                MesaOcupada2Picker.ItemsSource = mesasOcupadas;
            }
            else
            {
                // Agregar una opción al Picker que diga "No hay mesas"
                MesaOcupada1Picker.Items.Add("No hay mesas");
                MesaOcupada2Picker.Items.Add("No hay mesas");

                // Hacer los Picker no seleccionables
                MesaOcupada1Picker.IsEnabled = false;
                MesaOcupada2Picker.IsEnabled = false;
            }
        }

            private async void OnAceptarClicked(object sender, EventArgs e)
        {
            var mesa1 = (Mesa)MesaOcupada1Picker.SelectedItem;
            var mesa2 = (Mesa)MesaOcupada2Picker.SelectedItem;

            if (mesa1 == null || mesa2 == null)
            {
                await DisplayAlert("Error", "Por favor, selecciona dos mesas ocupadas.", "OK");
                return;
            }

            if (mesa1 == mesa2)
            {
                await DisplayAlert("Error", "Por favor, selecciona dos mesas distintas.", "OK");
                return;
            }

            // Obtener las comandas asociadas a las mesas desde la base de datos
            var comanda1 = App.Database.Table<Comanda>().FirstOrDefault(c => c.Id == mesa1.ComandaId);
            var comanda2 = App.Database.Table<Comanda>().FirstOrDefault(c => c.Id == mesa2.ComandaId);

            if (comanda1 != null && comanda2 != null)
            {
                // Asegurarse de que las comandas tienen productos antes de intentar unirlas
                if (comanda1.Productos != null && comanda2.Productos != null)
                {
                    // Unir las comandas
                    comanda2.Productos.AddRange(comanda1.Productos);
                }

                // Actualizar la comanda en la base de datos
                App.Database.Update(comanda2);

                // Obtener los objetos ProductoComanda asociados a la comanda original
                var productosComanda = App.Database.Table<ProductoComanda>().Where(pc => pc.ComandaId == comanda1.Id).ToList();

                // Actualizar el ComandaId de cada ProductoComanda para que apunte a la nueva comanda
                foreach (var productoComanda in productosComanda)
                {
                    productoComanda.ComandaId = comanda2.Id;
                    App.Database.Update(productoComanda); // Actualizar en la base de datos
                }


                // Liberar la segunda mesa
                mesa1.LiberarMesa();
                App.Database.Update(mesa1);

                await DisplayAlert("Éxito", $"Las comandas han sido unidas en la mesa {mesa2.Numero}. La mesa {mesa1.Numero} ahora está libre.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se encontraron las comandas asociadas a las mesas seleccionadas.", "OK");
            }

            await Navigation.PopToRootAsync();
        }

        public async void OnAyudaClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Ayuda", "Esta es la mesa que se va a quedar libre.", "OK");
        }

    }
}

