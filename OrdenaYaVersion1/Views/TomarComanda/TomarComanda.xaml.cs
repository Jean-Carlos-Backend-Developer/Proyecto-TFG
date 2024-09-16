using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdenaYA.models;
using OrdenaYaVersion1.Conexion;
using OrdenaYaVersion1.Views.PreviaComanda;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using OrdenaYaVersion1.Models;
using Xamarin.Forms.PlatformConfiguration;

namespace OrdenaYaVersion1.Views.TomarComanda
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TomarComanda : TabbedPage
    {
        private Comanda comanda;
        List<List<Producto>> todasLasListas = new List<List<Producto>>();

        // Nueva lista para almacenar los productos de la comanda
        List<ProductoComanda> productosComanda = new List<ProductoComanda>();
        int numMesa;

        public TomarComanda(Comanda comanda)
        {
            InitializeComponent();

            this.comanda = comanda;

            //Reiniciar la lista de productos de la comanda
            productosComanda = new List<ProductoComanda>();

            //Poner el numero de mesa en el título de la página
            numMesa = comanda.NumeroMesa;
            this.Title = "Mesa " + numMesa;

            //Obtener productos de la base de datos
            var bebidas = App.Database.Table<Producto>().Where(p => p.Tipo == "Bebida").ToList();
            var entrantes = App.Database.Table<Producto>().Where(p => p.Tipo == "Entrante").ToList();
            var carnes = App.Database.Table<Producto>().Where(p => p.Tipo == "Carne").ToList();
            var pescados = App.Database.Table<Producto>().Where(p => p.Tipo == "Pescado").ToList();
            var cafesPostres = App.Database.Table<Producto>().Where(p => p.Tipo == "Postre").ToList();

            //Crear layouts para los productos
            CrearLayout(layoutBebidas, bebidas);
            CrearLayout(layoutEntrantes, entrantes);
            CrearLayout(layoutCarnes, carnes);
            CrearLayout(layoutPescados, pescados);
            CrearLayout(layoutCafesPostres, cafesPostres);

            //Añadir todas las listas a una nueva lista que engloba todas
            todasLasListas.Add(bebidas);
            todasLasListas.Add(entrantes);
            todasLasListas.Add(carnes);
            todasLasListas.Add(pescados);
            todasLasListas.Add(cafesPostres);
        }

        public void CrearLayout(StackLayout layoutPadre, List<Producto> productos)
        {
            foreach (var producto in productos)
            {
                // Si la mesa está ocupada, buscar la cantidad de cada producto en la comanda
                if (comanda != null)
                {
                    var productoComanda = productosComanda.FirstOrDefault(pc => pc.ProductoId == producto.Id);
                    if (productoComanda != null)
                    {
                        producto.Cantidad = productoComanda.Cantidad;
                    }
                }

                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Spacing = 10
                };
                var nombreLabel = new Label
                {
                    Text = producto.Nombre,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    WidthRequest = 100,
                    Margin = new Thickness(5, 0, 0, 0),
                    FontSize = 16,
                    TextColor = Color.FromHex("#333333")
                };
                var precioLabel = new Label
                {
                    Text = producto.Precio.ToString("F2") + "€",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                var btnMenos = new Button
                {
                    Text = "-",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    WidthRequest = 60,
                    HeightRequest = 40,
                    BackgroundColor = Color.FromHex("#333333"), // Cambiamos el color de fondo a un gris oscuro
                    TextColor = Color.White // Cambiamos el color del texto a blanco
                };
                var cantidadProducto = new Label
                {
                    Text = producto.Cantidad.ToString(),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                var btnMas = new Button
                {
                    Text = "+",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    WidthRequest = 60,
                    HeightRequest = 40,
                    BackgroundColor = Color.FromHex("#333333"), // Cambiamos el color de fondo a un gris oscuro
                    TextColor = Color.White // Cambiamos el color del texto a blanco
                };

                // Eventos de clic
                btnMenos.Clicked += (s, e) =>
                {
                    var productoComanda = productosComanda.FirstOrDefault(pc => pc.ProductoId == producto.Id);
                    if (productoComanda != null && productoComanda.Cantidad > 0)
                    {
                        productoComanda.Cantidad--;
                        cantidadProducto.Text = productoComanda.Cantidad.ToString();
                    }
                };

                btnMas.Clicked += (s, e) =>
                {
                    var productoComanda = productosComanda.FirstOrDefault(pc => pc.ProductoId == producto.Id);
                    if (productoComanda == null)
                    {
                        productoComanda = new ProductoComanda { ProductoId = producto.Id, Cantidad = 0 };
                        productosComanda.Add(productoComanda);
                    }
                    productoComanda.Cantidad++;
                    cantidadProducto.Text = productoComanda.Cantidad.ToString();
                };

                stackLayout.Children.Add(nombreLabel);
                stackLayout.Children.Add(precioLabel);
                stackLayout.Children.Add(btnMenos);
                stackLayout.Children.Add(cantidadProducto);
                stackLayout.Children.Add(btnMas);

                //Añade el StackLayout a la interfaz de usuario
                layoutPadre.Children.Add(stackLayout);
            }
        }


        public async void OnConfirmarClicked(object sender, EventArgs e)
        {
            var productos = new List<Producto>();
            bool hayProductos = false;
            foreach (var productoComanda in productosComanda)
            {
                if (productoComanda.Cantidad > 0)
                {
                    var producto = App.Database.Table<Producto>().FirstOrDefault(p => p.Id == productoComanda.ProductoId);
                    if (producto != null)
                    {
                        producto.Cantidad = productoComanda.Cantidad;
                        productos.Add(producto);
                        hayProductos = true;
                    }
                }
            }

            if (!hayProductos)
            {
                await DisplayAlert("Error", "No puedes mandar una comanda vacía.", "OK");
                return;
            }

            //Guardar los productos seleccionado en la comanda
            this.comanda.Productos = productos;

            Console.WriteLine("Comandi" + comanda);

            await Navigation.PushAsync(new PreviaComanda.PreviaComanda(productos, numMesa));
        }

    }
}
