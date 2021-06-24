using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presion.Models
{
    class Registro
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now.Date;
        public TimeSpan Hora { get; set; } = DateTime.Now.TimeOfDay;
        public string PSis { get; set; }
        public string PDias { get; set; }
        public string Presion { get; set; }
    }
}
