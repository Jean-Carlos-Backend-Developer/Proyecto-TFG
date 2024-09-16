using System;
using System.Collections.Generic;
using OrdenaYA.models;
using OrdenaYaVersion1.Models;
using Xamarin.Forms;

namespace OrdenaYaVersion1.Views.PagarMesa
{
    public partial class PagarMesa : ContentPage
    {
        Comanda comanda;
        public PagarMesa(Comanda comanda)
        {
            InitializeComponent();
            this.Title = "Total mesa " + comanda.NumeroMesa;
            this.comanda = comanda;

            double total = 0;

            // Obtener los productos de la comanda desde ProductoComanda
            var productosComanda = App.Database.Table<ProductoComanda>()
                .Where(pc => pc.ComandaId == comanda.Id)
                .ToList();

            // Verificar si la comanda tiene productos
            if (productosComanda.Count > 0) // Verificar si la lista tiene elementos
            {
                foreach (var productoComanda in productosComanda)
                {
                    // Obtener el producto asociado desde la base de datos
                    var producto = App.Database.Get<Producto>(productoComanda.ProductoId);

                    total += producto.Precio * productoComanda.Cantidad; // Calcular el total correctamente

                    var frame = new Frame
                    {
                        Padding = 10,
                        BackgroundColor = Color.White, // Cambiamos el color de fondo a blanco
                        CornerRadius = 10,
                        Content = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                        {
                            new Label
                            {
                                Text = $"{producto.Nombre} {producto.Precio.ToString("F2")}€ x {productoComanda.Cantidad}",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                FontSize = 20,
                                TextColor = Color.FromHex("#333333")
                            },
                            new Label
                            {
                                Text = $"{(producto.Precio * productoComanda.Cantidad).ToString("F2")}" + " €",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                FontSize = 20
                            }
                        }
                        }
                    };

                    layoutComanda.Children.Add(frame); // Agregar el Frame al layout
                }
            }
            else
            {
                // Manejar el caso en que la comanda no tenga productos
                var labelSinProductos = new Label
                {
                    Text = "No hay productos en esta comanda.",
                    HorizontalOptions = LayoutOptions.Center
                };
                layoutComanda.Children.Add(labelSinProductos);
            }

            var lblTotal = new Label
            {
                Text = "Total " + total.ToString("F2") + "€",
                FontSize = 32,
                HorizontalOptions = LayoutOptions.Center
            };

            layoutComanda.Children.Add(lblTotal); // Agregar el label del total al final
        }

        public async void OnPagarClicked(object sender, EventArgs e)
        {
            // Mostrar un ActionSheet para elegir el método de pago
            var action = await DisplayActionSheet($"Cobrar y cerrar mesa {comanda.NumeroMesa}", "Cancelar", null, "Efectivo", "Tarjeta");

            switch (action)
            {
                case "Efectivo":
                case "Tarjeta":

                    // Mostrar un mensaje de que la mesa ha sido pagada correctamente
                    await DisplayAlert("Éxito", "Mesa pagada correctamente.", "OK");

                    // Liberar la mesa y guardarla en la base de datos
                    var mesa = App.Database.Table<Mesa>().FirstOrDefault(m => m.Numero == comanda.NumeroMesa);
                    if (mesa != null)
                    {
                        mesa.LiberarMesa();
                        App.Database.Update(mesa);
                    }

                    // Eliminar la comanda de la base de datos
                    App.Database.Delete(comanda);

                    // Navegar de vuelta al inicio de la aplicación
                    await Navigation.PopToRootAsync();
                    break;
            }
        }
    }

}

