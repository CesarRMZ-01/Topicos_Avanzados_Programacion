using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendar_Citas.Models
{
    public class Citas
    {
        SQLiteConnection conex;

        public Citas()
        {
            conex = new SQLiteConnection("RegistroCitas.db3");
            conex.CreateTable<Cliente>();
        }
        public List<string> Errores { get; set; }

        public bool Insert(Cliente client)
        {
            Errores = new List<string>();

            if (string.IsNullOrWhiteSpace(client.Nombre))
            {
                Errores.Add("Indique el nombre del cliente.");
            }

            if (Errores.Count > 0) return false;

            conex.Insert(client);

            return true;
        }

        public IEnumerable<Cliente> GetAll()
        {
            return conex.Table<Cliente>().OrderBy(x => x.DiaCita);
        }
        public Cliente Get(int id)
        {
            return conex.Table<Cliente>().FirstOrDefault(x => x.Id == id);
        }

        public bool Update(Cliente client)
        {
            Errores = new List<string>();

            if (string.IsNullOrWhiteSpace(client.Nombre))
            {
                Errores.Add("No se ha indicado el titulo del cuento.");
            }

            if (Errores.Count > 0) return false;

            var clientbd = Get(client.Id);

            clientbd.Nombre = client.Nombre;
            clientbd.Telefono = client.Telefono;
            clientbd.Mail = client.Mail;
            clientbd.DiaCita = client.DiaCita;

            conex.Update(clientbd);
            return true;
        }
        public void Delete(Cliente c)
        {
            conex.Delete(c);
        }
    }
}
