using Presion.Models;
using Presion.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Presion.ViewModels
{
    class RegistroVM: INotifyPropertyChanged
    {
        public ICommand NuevoRegistro { get; set; }
        public ICommand Agregar { get; set; }
        public ICommand Iniciar { get; set; }
        public ICommand Cancelar { get; set; }
        public ObservableCollection<Registro> registros { get; set; } = new ObservableCollection<Registro>();


        private Registro registrosel;

        public Registro regis
        {
            get { return registrosel; }
            set
            {
                registrosel = value;
                Actualizar(nameof(regis));
            }
        }



        ManRegistro manregis = new ManRegistro();

        VNuevoRegistro nuevoRegistroV;
        VRegistros registrosV;



        public RegistroVM()
        {
            nuevoRegistroV = new VNuevoRegistro() { BindingContext = this };
            registrosV = new VRegistros() { BindingContext = this };

            Iniciar = new Command(iniciar);
            NuevoRegistro = new Command(nuevo);
            Agregar = new Command(agregar);
            Cancelar = new Command(cancelar);

            CargarDatos();
        }
        private void CargarDatos()
        {
            registros.Clear();
            foreach (var regis in manregis.GetAll())
            {
                registros.Add(regis);
            }
        }
        private void iniciar(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(registrosV, false);
        }

        private string msj;
        public string Msj
        {
            get { return msj; }
            set
            {
                msj = value;
                Actualizar(nameof(Msj));
            }
        }
        private void agregar(object obj)
        {
            Msj = "";
            if (manregis.Insert(regis))
            {
                App.Current.MainPage.Navigation.PopAsync();
                CargarDatos();
            }
            else
            {
                Msj = string.Join("\n", manregis.Errores);
            }
        }

        private void nuevo(object obj)
        {
            regis = new Registro();
            App.Current.MainPage.Navigation.PushAsync(nuevoRegistroV, false);
        }
        private void cancelar()
        {
            App.Current.MainPage.Navigation.PopAsync();
            regis = null;
            Msj = null;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        void Actualizar(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
