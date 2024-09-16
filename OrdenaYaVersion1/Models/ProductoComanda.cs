using System;
using OrdenaYA.models;
using SQLite;

namespace OrdenaYaVersion1.Models
{
    public class ProductoComanda
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public int ComandaId { get; set; }
        public int Cantidad { get; set; }

        // Referencia al producto asociado (opcional, pero útil)
        [Ignore]
        public Producto Producto { get; set; }

        public override string ToString()
        {
            return $"{Producto?.Nombre} x {Cantidad} de la comanda {ComandaId}"; // Usar el nombre del producto si está disponible
        }
    }
}

