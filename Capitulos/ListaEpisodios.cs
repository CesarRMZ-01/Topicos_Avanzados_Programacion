using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace Capitulos
{
    public enum Paginas { Principal, Agregar, Modificar, Eliminar }
    public class ListaEpisodios : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Episodio> Episodios { get; set; } = new ObservableCollection<Episodio>();
        private Paginas control = Paginas.Principal;

        public Paginas Control
        {
            get { return control; }
            set { control = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Control")); }
        }

        public ICommand VerAgregarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand Filtrarcom { get; set; }

        public ListaEpisodios()
        {
            Cargar();
            VerAgregarCommand = new RelayCommand(VerAgregar);
            AgregarCommand = new RelayCommand(Agregar);
            VerEditarCommand = new RelayCommand<Episodio>(VerEditar);
            EditarCommand = new RelayCommand(Modificar);
            VerEliminarCommand = new RelayCommand<Episodio>(VerEliminar);
            EliminarCommand = new RelayCommand(Eliminar);
            CambiarVistaCommand = new RelayCommand<Paginas>(CambiarView);
            CancelarCommand = new RelayCommand(Cancelar);

        }

        private Episodio episodio;

        public Episodio Episodio
        {
            get { return episodio; }
            set { episodio = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Episodio))); }
        }

        private string errores;
        public string Errores
        {
            get { return errores; }
            set { errores = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errores))); }
        }
        private void VerAgregar()
        {
            Control = Paginas.Agregar;
            Episodio = new Episodio();
        }
        public void Agregar()
        {
            if (Episodio != null)
            {
                Errores = "";
                if (string.IsNullOrWhiteSpace(Episodio.Imagen))
                {
                    Errores = "Escriba la URL de la imagen";
                    return;
                }
                if (Episodio.NumEpisodio < 1)
                {
                    Errores = "Escriba un valor valido";
                    return;
                }
                if (Episodio.Temporada < 1)
                {
                    Errores = "Escriba un valor valido";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Episodio.Titulo))
                {
                    Errores = "Escriba el titulo del episodio";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Episodio.Descripcion))
                {
                    Errores = "Escriba la descripción del episodio";
                    return;
                }
                if (Episodios.Any(x => x.Titulo.ToUpper() == Episodio.Titulo.ToUpper()))
                {
                    Errores = "El episodio ya se encuentra registrado";
                    return;
                }


                Episodios.Add(Episodio);

                Save();


                Control = Paginas.Principal;
            }
        }
        private void VerEditar(Episodio obj)
        {
            Errores = "";
            Episodio clon = new Episodio
            {
                NumEpisodio = obj.NumEpisodio,
                Temporada = obj.Temporada,
                Titulo = obj.Titulo,
                Descripcion = obj.Descripcion,
                Imagen = obj.Imagen
            };

            posicioninicial = Episodios.IndexOf(obj);
            Episodio = clon;
            CambiarView(Paginas.Modificar);
        }

        int posicioninicial;
        public void Modificar()
        {
            if (Episodio != null)
            {
                if (string.IsNullOrWhiteSpace(Episodio.Imagen))
                {
                    Errores = "Escriba la URL de la imagen";
                    return;
                }
                if (Episodio.NumEpisodio < 1)
                {
                    Errores = "Escriba un valor de episodio valido";
                    return;
                }
                if (Episodio.Temporada < 1)
                {
                    Errores = "Escriba un valor de temporada valido";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Episodio.Titulo))
                {
                    Errores = "Escriba el titulo del episodio";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Episodio.Descripcion))
                {
                    Errores = "Escriba la descripción del episodio";
                    return;
                }

                Episodios[posicioninicial] = Episodio;

                Save();

                CambiarView(Paginas.Principal);

            }
        }


        private void VerEliminar(Episodio obj)
        {
            Episodio = obj;
            CambiarView(Paginas.Eliminar);
        }
        public void Eliminar()
        {
            if (Episodios.Remove(Episodio))
            {
                Save();
            }
            CambiarView(Paginas.Principal);
        }
        private void CambiarView(Paginas obj)
        {
            Control = obj;
        }
        private void Cancelar()
        {
            CambiarView(Paginas.Principal);
        }
        private void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Episodio>));
            XmlWriter archivo = XmlWriter.Create("Episodios.xml");

            serializer.Serialize(archivo, Episodios);
            archivo.Close();
        }
        private void Cargar()
        {
            try
            {
                XmlReader archivo = XmlReader.Create("Episodios.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Episodio>));
                Episodios = (ObservableCollection<Episodio>)serializer.Deserialize(archivo);
                archivo.Close();

            }
            catch (Exception)
            {
                Episodios = new ObservableCollection<Episodio>();
            }
        }
    }
    }
