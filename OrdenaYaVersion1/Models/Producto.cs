using System;
using SQLite;

namespace OrdenaYA.models
{
    public class Producto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public string Tipo { get; set; }

        public Producto(int id, string nombre, double precio, int cantidad, string tipo)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            Cantidad = cantidad;
            Tipo = tipo;
        }

        public Producto() { }

        public double GetTotal()
        {
            return Cantidad * Precio;
        }

        public override string ToString()
        {
            return Nombre + " " + Precio + "€ " + Cantidad;
        }
    }
}

