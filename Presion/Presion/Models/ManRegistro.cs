using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Presion.Models
{
    class ManRegistro
    {
        public SQLiteConnection conexion { get; set; }
        string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Registro.db";

        public ManRegistro()
        {
            VerificarBD();
            conexion = new SQLiteConnection(ruta);
        }

        private void VerificarBD()
        {
            if (!File.Exists(ruta))
            {
                Assembly a = Assembly.GetExecutingAssembly();
                var stream = a.GetManifestResourceStream("Presion.Data.Registro.db");
                var archivo = File.Create(ruta);
                stream.CopyTo(archivo);
                stream.Close();
                archivo.Close();
            }
        }

        public IEnumerable<Registro> GetPokemonByGeneracion(int Id)
        {
            return conexion.Table<Registro>().Where(x => x.Id == Id);
        }

        public List<string> Errores { get; set; }

        public bool Insert(Registro regist)
        {
            Errores = new List<string>();

            if (string.IsNullOrWhiteSpace(regist.PSis))
            {
                Errores.Add("Indique la presion sistólica.");
            }
            if (string.IsNullOrWhiteSpace(regist.PDias))
            {
                Errores.Add("Indique la presion diastólica.");
            }
            if (string.IsNullOrWhiteSpace(regist.Presion))
            {
                Errores.Add("Indique el Pulso.");
            }


            if (Errores.Count > 0) return false;

            conexion.Insert(regist);

            return true;
        }

        internal IEnumerable<Registro> GetAll()
        {
            return conexion.Table<Registro>().OrderBy(x => x.Id);
        }
    }
}
