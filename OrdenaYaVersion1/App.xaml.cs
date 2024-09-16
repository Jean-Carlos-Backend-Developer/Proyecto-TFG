using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OrdenaYA.models;
using OrdenaYaVersion1.Models;
using OrdenaYaVersion1.Views.AccesoApp;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrdenaYaVersion1
{
    public partial class App : Application
    {
        public static SQLite.SQLiteConnection Database;

        public App()
        {
            InitializeComponent();

            //Inicializar la base de datos
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OrdenaYa.db3");
            Database = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Database.CreateTable<Mesa>();
            Database.CreateTable<Comanda>();
            Database.CreateTable<Producto>();
            Database.CreateTable<ProductoComanda>();

            //Insertar datos a mis tablas
            InsertarDatosIniciales();

            MainPage = new NavigationPage(new LoginPage());
        }

        private void InsertarDatosIniciales()
        {
            // Insertar mesas
            if (!Database.Table<Mesa>().Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    var mesa = new Mesa(i);
                    Database.Insert(mesa);
                }
            }

            // Insertar productos
            if (!Database.Table<Producto>().Any())
            {
                var productos = new List<Producto>
                {
                    new Producto { Nombre = "Coca-Cola", Precio = 2.80, Tipo = "Bebida" },
                    new Producto { Nombre = "Fanta naranja", Precio = 1.90, Tipo = "Bebida" },
                    new Producto { Nombre = "Agua", Precio = 1.10, Tipo = "Bebida" },
                    new Producto { Nombre = "Aquarius", Precio = 2.10, Tipo = "Bebida"},
                    new Producto { Nombre = "Croquetas", Precio = 1.99, Tipo = "Entrante" },
                    new Producto { Nombre = "Nachos", Precio = 2.99, Tipo = "Entrante" },
                    new Producto { Nombre = "Ensalada", Precio = 3.99, Tipo = "Entrante" },
                    new Producto { Nombre = "Fajitas", Precio = 2.99, Tipo = "Entrante" },
                    new Producto { Nombre = "Chuletas de cerdo", Precio = 8.99, Tipo = "Carne" },
                    new Producto { Nombre = "Pollo empanado", Precio = 6.99, Tipo = "Carne" },
                    new Producto { Nombre = "Cordero asado", Precio = 7.55, Tipo = "Carne" },
                    new Producto { Nombre = "Pollo frito", Precio = 2.99, Tipo = "Carne" },
                    new Producto { Nombre = "Dorada frita", Precio = 8.99, Tipo = "Pescado" },
                    new Producto { Nombre = "Pescadilla", Precio = 4.99, Tipo = "Pescado" },
                    new Producto { Nombre = "Pulpo gallega", Precio = 5.58, Tipo = "Pescado" },
                    new Producto { Nombre = "Calamares", Precio = 3.99, Tipo = "Pescado" },
                    new Producto { Nombre = "Tarta de fresa", Precio = 3.99, Tipo = "Postre" },
                    new Producto { Nombre = "Bola de helado" , Precio = 2.65, Tipo = "Postre" },
                    new Producto { Nombre = "Cafe", Precio = 1.92, Tipo = "Postre" },
                    new Producto { Nombre = "Tortitas", Precio = 3.99, Tipo = "Postre" },
                };
                foreach (var producto in productos)
                {
                    Database.Insert(producto);
                }
            }
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

