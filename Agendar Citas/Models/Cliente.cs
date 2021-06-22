using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agendar_Citas.Models
{

    [Table("Clientes")]
    public class Cliente
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }

        private DateTime cita = DateTime.Today;
        [NotNull]
        public DateTime DiaCita { get { return cita; } set { cita = value; } }
    }
}
