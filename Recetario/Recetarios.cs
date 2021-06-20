using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace Recetario
{
    public enum Vistas { Datos, Agregar, Editar, Eliminar }
    public class Recetarios : INotifyPropertyChanged
    {


        public ObservableCollection<Receta> Recetas { get; set; }
        public ObservableCollection<Receta> PlatillosFiltrados { get; set; } = new ObservableCollection<Receta>();

        private Vistas vistas = Vistas.Datos;
        public Vistas Vista
        {
            get { return vistas; }
            set
            {
                vistas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
            }
        }
        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand AgregarNuevoCommand { get; set; }
        public ICommand EditarActualCommand { get; set; }
        public ICommand FiltrarCommand { get; set; }
        public ICommand MostrarDatos { get; set; }

        public Recetarios()
        {
            Cargar();
            Filtrar(null);

            AgregarCommand = new RelayCommand(Agregarvista);
            AgregarNuevoCommand = new RelayCommand(Agrega);

            EditarCommand = new RelayCommand<Receta>(EditarVista);
            EditarActualCommand = new RelayCommand(Editar);

            VerEliminarCommand = new RelayCommand<Receta>(VerEliminar);
            EliminarCommand = new RelayCommand(Eliminar);

            CambiarVistaCommand = new RelayCommand<Vistas>(CambiarVista);
            MostrarDatos = new RelayCommand<Receta>(MostrarInfo);
            
            FiltrarCommand = new RelayCommand<Dificultad?>(Filtrar);
            

        }

        private void MostrarInfo(Receta Dat)
        {
            Receta r = new Receta()
            {
                Ingredientes = Dat.Ingredientes,
                Procedimiento = Dat.Procedimiento,
                Imagen = Dat.Imagen
            };

            Recetass = r;
            posicionOriginal = Recetas.IndexOf(Dat);

            Recetas[posicionOriginal] = Recetass;
        }

        private void Filtrar(Dificultad? Dif)
        {
            PlatillosFiltrados.Clear();
            var datos = Recetas.Where(x => Dif == null || x.Dificultad == Dif).OrderBy(x => x.Nombre);

            foreach (var pais in datos)
            {
                PlatillosFiltrados.Add(pais);
            }
        }


        private void Agregarvista()
        {
            Vista = Vistas.Agregar;
            Recetass = new Receta();
        }
        private void Agrega()
        {
            if (Recetass != null)
            {
                Error = "";

                if (string.IsNullOrWhiteSpace(Recetass.Nombre))
                {
                    Error = "Escriba el nombre del platillo.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Recetass.Ingredientes))
                {
                    Error = "Escriba los ingredientes.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Recetass.Procedimiento))
                {
                    Error = "Escriba el procedimiento.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Recetass.Imagen))
                {
                    Error = "Introduzca un URL que corresponda al platillo.";
                    return;
                }


                Recetas.Add(Recetass);
                Filtrar(null);
                Guardar();

                Vista = Vistas.Datos;
            }
        }


        int posicionOriginal;
        private void EditarVista(Receta obj)
        {
            Error = "";
            Receta clon = new Receta
            { 
                Nombre = obj.Nombre, 
                Ingredientes = obj.Ingredientes, 
                Procedimiento = obj.Procedimiento, 
                Imagen = obj.Imagen, 
                Dificultad = obj.Dificultad 
            };

            posicionOriginal = Recetas.IndexOf(obj);
            Recetass = clon;
            CambiarVista(Vistas.Editar);
        }
        private void Editar()
        {
            if (Recetass != null)
            {
                Error = "";

                if (string.IsNullOrWhiteSpace(Recetass.Nombre))
                {
                    Error = "Escriba el nombre del platillo.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Recetass.Ingredientes))
                {
                    Error = "Escriba los ingredientes.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Recetass.Procedimiento))
                {
                    Error = "Escriba el procedimiento.";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Recetass.Imagen))
                {
                    Error = "Introduzca un URL que corresponda al platillo.";
                    return;
                }

                Recetas[posicionOriginal] = Recetass;

                Filtrar(null);
                Guardar();

                CambiarVista(Vistas.Datos);
            }
        }


        private void VerEliminar(Receta obj)
        {
            Recetass = obj;
            CambiarVista(Vistas.Eliminar);
        }
        private void Eliminar()
        {
            if (Recetas.Remove(Recetass))
            {
                Guardar();
            }
            Filtrar(null);
            CambiarVista(Vistas.Datos);
        }





        private Receta recetass;
        public Receta Recetass
        {
            get { return recetass; }
            set
            {
                recetass = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Recetass)));
            }
        }
        private string error;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));

            }
        }

        private void CambiarVista(Vistas obj)
        {
            Vista = obj;

            if (Vista == Vistas.Agregar)
            {
                Recetass = new Receta();
            }
        }

        private void Guardar()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Receta>));
            XmlWriter archivo = XmlWriter.Create("Recetario.xml");

            serializer.Serialize(archivo, Recetas);
            archivo.Close();
        }


        private void Cargar()
        {
            try
            {

                XmlReader archivo = XmlReader.Create("Recetario.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Receta>));
                Recetas = (ObservableCollection<Receta>)serializer.Deserialize(archivo);
                archivo.Close();

            }
            catch (Exception)
            {
                Recetas = new ObservableCollection<Receta>()
                {
                    new Receta() {
                        Nombre = "Pollo al horno", 
                        Procedimiento= "Precalienta el horno a 425 grados F.Limpie el pollo enjuagándolo por dentro y por fuera; si tiene exceso de grasa, retírela y séquela con toallas de papel.Pliega las puntas del ala debajo del cuerpo y sazona bien el interior del pollo con sal y pimienta.Mezcla el romero, el jugo de limón, la cáscara de limón, la mantequilla y el ajo triturado para formar una pasta.", 
                        Ingredientes = "Pechuga de pollo, sasonador, mantequilla, aceite",
                        Dificultad = Dificultad.Medio,
                    Imagen = "Pollo.jpg"},

                        new Receta() {
                        Nombre = "Carne Asada",
                        Procedimiento= "Precalienta el horno a 425 grados F.Limpie el pollo enjuagándolo por dentro y por fuera; si tiene exceso de grasa, retírela y séquela con toallas de papel.Pliega las puntas del ala debajo del cuerpo y sazona bien el interior del pollo con sal y pimienta.Mezcla el romero, el jugo de limón, la cáscara de limón, la mantequilla y el ajo triturado para formar una pasta.",
                        Ingredientes = "Filete, sasonador, mantequilla, aceite",
                        Dificultad = Dificultad.Dificil,
                    Imagen = "https://d37k6lxrz24y4c.cloudfront.net/v2/es-mx/e9dc924f238fa6cc29465942875fe8f0/537b85de-8565-4a2e-91f3-b1e4d7f221bc-600.jpg"},
                        new Receta() {
                        Nombre = "Ensalada sencilla",
                        Procedimiento= "Simplemente agrega verduras en un bowl",
                        Ingredientes = "Lechuga, tomate, cebolla, garbanzos...",
                        Dificultad = Dificultad.Facil,
                    Imagen = "https://t1.rg.ltmcdn.com/es/images/1/7/7/ensalada_de_apio_tomate_y_aguacate_60771_orig.jpg"}

                };
            }
        }

    }
}
