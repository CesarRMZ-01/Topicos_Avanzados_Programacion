using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetario
{
    public enum Dificultad { Facil, Medio, Dificil }
    [Serializable]
    public class Receta
    {
        public string Nombre { get; set; }
        public string Ingredientes { get; set; }
        public string Procedimiento { get; set; }
        public Dificultad Dificultad { get; set; }
        public string Imagen { get; set; }
    }
}
