using System;
using System.Collections.Generic;
using OrdenaYA.models;
using OrdenaYaVersion1.Models;
using Xamarin.Forms;

namespace OrdenaYaVersion1.Views.PreviaComanda
{	
	public partial class PreviaComanda : ContentPage
	{
		Comanda comanda;
        public PreviaComanda(List<Producto> productos, int mesa)
        {
            InitializeComponent();
            this.Title = "Total mesa " + mesa;

            double total = 0;

            //Crear objeto comanda para pasarlo a la nueva ventana
            comanda = new Comanda(mesa, productos);
            foreach (var producto in productos)
            {
                total += producto.GetTotal(); //Sumar el total de cada producto

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
                                Text = $"{producto.Nombre} {producto.Precio.ToString("F2")}€ x {producto.Cantidad}",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                FontSize = 20,
                                TextColor = Color.FromHex("#333333")
                            },
                            new Label
                            {
                                Text = $"{producto.GetTotal().ToString("F2")}" + " €",
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                FontSize = 20
                            }
                        }
                    }
                };

                layoutComanda.Children.Add(frame);
            }

            var lblTotal = new Label
            {
                Text = "Total " + total.ToString("F2") + "€",
                FontSize = 32,
                HorizontalOptions = LayoutOptions.Center
            };

            layoutComanda.Children.Add(lblTotal);
        }


        public async void OnOkClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Comanda", "Comanda confirmada", "OK");

            // Guardar la comanda en la base de datos
            App.Database.Insert(comanda);

            // Guardar los productos de la comanda en ProductoComanda
            foreach (var producto in comanda.Productos)
            {
                var productoComanda = new ProductoComanda
                {
                    ProductoId = producto.Id,
                    ComandaId = comanda.Id,
                    Cantidad = producto.Cantidad,
                    Producto = producto
                };

                App.Database.Insert(productoComanda); // Insertar en la tabla ProductoComanda

                Console.WriteLine("Producti:" + productoComanda.ToString());
            }


            // Obtener la mesa correspondiente a la comanda
            var mesa = App.Database.Table<Mesa>().FirstOrDefault(m => m.Numero == comanda.NumeroMesa);

            if (mesa != null)
            {
                //Marcar la mesa como ocupada y guardarla en la base de datos
                mesa.OcuparMesa(comanda);
                App.Database.Update(mesa);
            }

            await Navigation.PopToRootAsync();
        }

    }
}

