using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyGarden
{
    public class Plantar : INotifyPropertyChanged
    {

        public ICommand SembrarCommand { get; set; }
        public ICommand DesplantarCommand { get; set; }
        public ICommand RegarCommand { get; set; }
        public ICommand MostrarFlorCommand { get; set; }
        public ICommand CSemillasCommand { get; set; }
        public ICommand NavegarInfo { get; set; }
        ISimpleAudioPlayer plantar;
        ISimpleAudioPlayer regar;
        ISimpleAudioPlayer desplantar;
        ISimpleAudioPlayer florvista;
        ISimpleAudioPlayer recogersemilla;

        public ObservableCollection<Flor> Plantas { get; set; } = new ObservableCollection<Flor>();
        public Plantar()
        {
            Abrir();

            plantar = CrossSimpleAudioPlayer.Current;
            regar = CrossSimpleAudioPlayer.Current;
            desplantar = CrossSimpleAudioPlayer.Current;
            florvista = CrossSimpleAudioPlayer.Current;
            recogersemilla = CrossSimpleAudioPlayer.Current;

            SembrarCommand = new Command<Flor>(Sembrar);
            DesplantarCommand = new Command(Desplantar);
            RegarCommand = new Command(Regar);
            MostrarFlorCommand = new Command<Flor>(Datos);
            CSemillasCommand = new Command(ConseguirSemillas);
            NavegarInfo = new Command(Navegarinfo);

        }
        Info info;
        private void Navegarinfo(object obj)
        {
            if (info == null)
            {
                info = new Info();
            }
            Application.Current.MainPage.Navigation.PushAsync(info);

        }

        int ActualFlor;
        private int etapa;
        private string tipoflor;
        private string florDatos;
        private int floresDisp;
        private string carplant;
        private string nomcient;


        public string Etapa
        {
            get
            {
                if(etapa<=1)
                {
                    return $"Planta{etapa}";
                }
                else
                {
                    return $"Planta{tipoflor}";
                }
            }
        }



        private Flor refFlor;
        public Flor RefFlor
        {
            get { return refFlor; }
            set
            {
                refFlor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RefFlor)));
            }
        }


        private void Sembrar(Flor Sem)
        {
            if (Sem.Semilla > 0 && etapa < 1)
            {
                Sem.Semilla--;
                etapa++;
                tipoflor = Sem.Tipo;

                Flor f = new Flor()
                {
                    Tipo = Sem.Tipo,
                    NomCien = Sem.NomCien,
                    DescPlanta = Sem.DescPlanta,
                    Semilla = Sem.Semilla,
                    CantFlores = Sem.CantFlores
                };

                RefFlor = f;
                ActualFlor = Plantas.IndexOf(Sem);
                Plantas[ActualFlor] = RefFlor;

                plantar.Load("Plantar.mp3");
                plantar.Play();
            }
            Actualizar();
        }


        private void Regar(object obj)
        {
            if (etapa == 1)
            {
                etapa++;
                Guardar();
                Actualizar();

                plantar.Load("Regar.mp3");
                plantar.Play();
            }
            
        }

        private void Desplantar(object Desp)
        {
            if(etapa == 2)
            {
                RefFlor.CantFlores++;
                Plantas[ActualFlor] = RefFlor;
                etapa = 0;
            }
            else if(RefFlor.Semilla > 1) { etapa = 0; }

            Guardar();
            Actualizar();

            plantar.Load("Desplantar.mp3");
            plantar.Play();
        }

        DatosPlantas DatosP;
        private void Datos(Flor Dat)
        {
            if (DatosP == null)
            {
                DatosP = new DatosPlantas() { BindingContext = this };
            }

            Flor f = new Flor()
            {
                Tipo = Dat.Tipo,
                NomCien = Dat.NomCien,
                DescPlanta = Dat.DescPlanta,
                Semilla = Dat.Semilla,
                CantFlores = Dat.CantFlores
            };

            RefFlor = f;
            ActualFlor = Plantas.IndexOf(Dat);
            florDatos = Dat.Tipo;
            floresDisp = Dat.CantFlores;
            carplant = Dat.DescPlanta;
            nomcient = Dat.NomCien;

            Plantas[ActualFlor] = RefFlor;

            Actualizar();

            plantar.Load("CambiarCuidado.mp3");
            plantar.Play();

            Application.Current.MainPage.Navigation.PushAsync(DatosP);
        }

        public string FlorDatos
        {
            get
            {
                return florDatos;
            }
        }
        public string Color
        {
            get
            {
                if (florDatos == "Girasol")
                {
                    return "#fbb636";
                }
                else if (florDatos == "Rosa")
                {
                    return "#ae1a26";
                }
                else if (florDatos == "Cerezo")
                {
                    return "#e56f7a";
                }
                else if (florDatos == "Lirio")
                {
                    return "#449d53";
                }
                else
                {
                    return "white";
                }
            }
        }
        public int FloresDisp
        {
            get
            {
                return floresDisp;
            }
        }
        public string CaPlantacion
        {
            get
            {
                return carplant;
            }
        }

        public string NombreCient
        {
            get { return nomcient; }
        }
        private void ConseguirSemillas(object obj)
        {
            if (RefFlor.CantFlores!=0 && RefFlor.Semilla<99)
            {
                if(RefFlor.Semilla + (2 * RefFlor.CantFlores) > 99)
                {
                    RefFlor.Semilla = 99;

                    plantar.Load("RecogerSemilla.mp3");
                    plantar.Play();
                }
                else
                {
                    RefFlor.Semilla = RefFlor.Semilla + (2 * RefFlor.CantFlores);

                    plantar.Load("RecogerSemilla.mp3");
                    plantar.Play();
                }
                
                floresDisp = 0;
                RefFlor.CantFlores = 0;

                Plantas[ActualFlor] = RefFlor;

                Guardar();
                Actualizar();
            }

        }

        private void Actualizar()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private void Guardar()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var archivo = File.CreateText(ruta + "/Plantas.json");
            string json = JsonConvert.SerializeObject(Plantas);
            archivo.Write(json);
            archivo.Close();
        }


        private void Abrir()
        {
            try
            {
                var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var archivo = File.OpenText(ruta + "/Plantas.json");
                string json = archivo.ReadToEnd();
                Plantas = JsonConvert.DeserializeObject<ObservableCollection<Flor>>(json);
                archivo.Close();
            }
            catch
            {
                Plantas = new ObservableCollection<Flor>
                {
                    new Flor() { Tipo = "Girasol", NomCien = "Helianthus annuus", CantFlores = 0, Semilla = 3, DescPlanta = "Cortar 2 cm de cada tallo en diagonal. Retirar las hojas sobrantes del tallo y añadir un sobre de nutrientes para flores. Es importante que los girasoles reciban siempre luz directa del sol para crecer correctamente y recibir todo lo que necesitan para desarrollarse y lucir con todo su esplendor." },
                    new Flor() { Tipo = "Rosa", NomCien = "Chinensis Jacq", CantFlores = 0, Semilla = 3, DescPlanta = "Las rosas necesitan suficiente agua para que las raíces puedan absorber la humedad, por eso lo es recomendable rociar suficiente agua. Debe asegurarse de que su rosa tenga suficiente drenaje, si no, el agua se acumula y la ahoga." },
                    new Flor() { Tipo = "Cerezo", NomCien = "Prunus Cerasus", CantFlores = 0, Semilla = 3, DescPlanta = "Los cerezos necesitan mucho sol y no demasiado viento. Necesitan un lugar helado al menos durante 3 meses al año. Rieguelos abundantemente cuando sea necesario y asegurese de que nunca se seque." },
                    new Flor() { Tipo = "Lirio", NomCien = "Lilium", CantFlores = 0, Semilla = 3, DescPlanta = "Se obtiene mejores resultados si la planta se coloca al sol o en una zona de semisombra. Le va mejor la tierra normal de jardín que sea arcillosa, aunque hay algunas especies que necesitan un suelo pantanoso. Se riega una vez a la semana, dos si la planta se encuentra en floración. Vigilar que no se encharque nunca." }
                };

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
