using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace OrdenaYA.models
{
    public class Comanda
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int NumeroMesa { get; set; }

        [Ignore]
        public List<Producto> Productos { get; set; }

        public Comanda(int numeroMesa, List<Producto> productos)
        {
            NumeroMesa = numeroMesa;
            Productos = productos;
        }

        public Comanda() { }

        public override string ToString()
        {
            var productosStr = string.Join(", ", Productos.Select(p => $"{p.Nombre} (Cantidad: {p.Cantidad})"));
            return $"ID: {Id}, Número de Mesa: {NumeroMesa}, Productos: {productosStr}";
        }

        public double GetTotal()
        {
            double total = 0;
            foreach (var producto in Productos)
            {
                total += producto.Precio * producto.Cantidad;
            }
            return total;
        }
    }
}

