using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdenaYA.models;
using OrdenaYaVersion1.Views.Mesas;
using OrdenaYaVersion1.Views.TraspasarMesa;
using OrdenaYaVersion1.Views.UnirMesa;
using Xamarin.Forms;
using OrdenaYaVersion1.Views.MesasComanda;
using OrdenaYaVersion1.Models;
using Firebase.Auth;
using OrdenaYaVersion1.Conexion;
using OrdenaYaVersion1.Views.AccesoApp;

namespace OrdenaYaVersion1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            txtMensaje.Text = "Bienvenido";
        }

        public async void TomarComanda_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Mesas());
        }

        public async void TraspasarMesa_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TraspasarMesa());
        }

        public async void UnirMesa_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UnirMesa());
        }

        public async void PagarMesa_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MesasComanda());

        }

        public async void Salir_Click(object sender, EventArgs e)
        {
            try
            {
                // "Cerrar sesión" eliminando el token de Application.Current.Properties
                if (Application.Current.Properties.ContainsKey("FirebaseToken"))
                {
                    Application.Current.Properties.Remove("FirebaseToken");
                }

                await Application.Current.MainPage.DisplayAlert("Éxito", "Has cerrado sesión correctamente.", "Aceptar");

                //Redirigir al usuario a la página de inicio de sesión
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Hubo un problema al cerrar sesión.", "Aceptar");
            }
        }


        public void RestablecerBaseDeDatos()
        {
            App.Database.DeleteAll<Comanda>();
            App.Database.DeleteAll<Mesa>();
            App.Database.DeleteAll<Producto>();
            App.Database.DeleteAll<ProductoComanda>();

        }

    }
}

