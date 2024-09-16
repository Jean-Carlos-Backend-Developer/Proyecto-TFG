using System;
using System.Collections.Generic;
using System.Linq;
using OrdenaYA.models;
using OrdenaYaVersion1.Models;
using Xamarin.Forms;

namespace OrdenaYaVersion1.Views.TraspasarMesa
{	
	public partial class TraspasarMesa : ContentPage
	{	
		public TraspasarMesa ()
		{
			InitializeComponent ();

            // Obtener las mesas de la base de datos
            var mesas = App.Database.Table<Mesa>().ToList();
            var mesasOcupadas = mesas.Where(m => m.Ocupada).ToList();
            var mesasLibres = mesas.Where(m => !m.Ocupada).ToList();

            // Verificar si hay mesas ocupadas
            if (mesasOcupadas.Any())
            {
                // Llenar los Picker con las mesas libres y ocupadas
                MesaOrigenPicker.ItemsSource = mesasOcupadas;
                MesaDestinoPicker.ItemsSource = mesasLibres;
            }
            else
            {
                // Agregar una opción al Picker que diga "No hay mesas"
                MesaOrigenPicker.Items.Add("No hay mesas");
                MesaOrigenPicker.SelectedIndex = 0; // Seleccionar esta opción por defecto
                MesaOrigenPicker.IsEnabled = false; // Hacer el Picker no seleccionable
            }
        }

        private async void OnAceptarClicked(object sender, EventArgs e)
        {
            var mesaOrigen = (Mesa)MesaOrigenPicker.SelectedItem;
            var mesaDestino = (Mesa)MesaDestinoPicker.SelectedItem;

            if (mesaOrigen == null || mesaDestino == null)
            {
                await DisplayAlert("Error", "Por favor, selecciona una mesa de origen y una mesa de destino.", "OK");
                return;
            }

            // Obtener la comanda asociada a la mesa de origen desde la base de datos
            var comanda = App.Database.Table<Comanda>().FirstOrDefault(c => c.Id == mesaOrigen.ComandaId);

            if (comanda != null)
            {
                // Transferir la comanda a la mesa destino
                mesaDestino.OcuparMesa(comanda);
                comanda.NumeroMesa = mesaDestino.Numero; //Actualizar el id de la comanda 

                // Actualizar la mesa de origen en la base de datos
                mesaOrigen.LiberarMesa();
                App.Database.Update(mesaOrigen);

                // Actualizar la mesa de destino y la comanda en la base de datos
                App.Database.Update(mesaDestino);
                App.Database.Update(comanda); // Asegurarse de actualizar la comanda en la base de datos

                // Obtener los objetos ProductoComanda asociados a la comanda original
                var productosComanda = App.Database.Table<ProductoComanda>().Where(pc => pc.ComandaId == comanda.Id).ToList();

                // Actualizar el ComandaId de cada ProductoComanda para que apunte a la nueva comanda
                foreach (var productoComanda in productosComanda)
                {
                    productoComanda.ComandaId = comanda.Id;
                    App.Database.Update(productoComanda); // Actualizar en la base de datos
                }

                await DisplayAlert("Éxito", $"La comanda ha sido transferida de la mesa {mesaOrigen.Numero} a la mesa {mesaDestino.Numero}.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se encontró la comanda asociada a la mesa de origen.", "OK");
            }

            await Navigation.PopToRootAsync();
        }
    }
}

