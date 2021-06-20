using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capitulos
{
    [Serializable]
    public class Episodio
    {
        public string Imagen { get; set; }
        public int NumEpisodio { get; set; }
        public int Temporada { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
