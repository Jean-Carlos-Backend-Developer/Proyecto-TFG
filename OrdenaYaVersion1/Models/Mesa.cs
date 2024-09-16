using System;
using SQLite;

namespace OrdenaYA.models
{
    public class Mesa
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Numero { get; set; }
        public bool Ocupada { get; set; }
        public int ComandaId { get; set; }

        [Ignore]
        public Comanda Comanda { get; set; }

        public Mesa(int numero)
        {
            Numero = numero;
            Ocupada = false;
            Comanda = null;
        }

        public Mesa()
        {
            Ocupada = false;
            Comanda = null;
        }

        public void OcuparMesa(Comanda comanda)
        {
            Ocupada = true;
            Comanda = comanda;
            ComandaId = comanda.Id;
        }

        public void LiberarMesa()
        {
            Ocupada = false;
            Comanda = null;
            ComandaId = 0;
        }

        public override string ToString()
        {
            return Numero.ToString();
        }
    }
}

