using Agendar_Citas.Models;
using Agendar_Citas.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Agendar_Citas.ViewModels
{
    class CitasCom : INotifyPropertyChanged
    {
        public ObservableCollection<Cliente> Lista { get; set; } = new ObservableCollection<Cliente>();
        private Cliente cliente;

        public ICommand RegistrarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand VerCommand { get; set; }
        public ICommand EliminarCommand { get; set; }

        Citas citas = new Citas();

        VInicio InicioV;
        VRegistrar RegistrarV;
        VEditar EditarV;
        VCitas CitasV;
        public CitasCom()
        {
            InicioV = new VInicio() { DataContext = this };
            RegistrarV = new VRegistrar() { DataContext = this };
            EditarV = new VEditar() { DataContext = this };
            CitasV = new VCitas() { DataContext = this };

            Control = InicioV;

            RegistrarCommand = new RelayCommand(Nuevo);
            CancelarCommand = new RelayCommand(Cancelar);
            AgregarCommand = new RelayCommand(Agregar);
            VerCommand = new RelayCommand<Cliente>(Ver);
            EditarCommand = new RelayCommand<Cliente>(Editar);
            GuardarCommand = new RelayCommand(Guardar);
            EliminarCommand = new RelayCommand<Cliente>(Eliminar);

            CargarDatos();
        }

        public Cliente Cliente
        {
            get { return cliente; }
            set
            {
                cliente = value;
                Actualizar(nameof(Cliente));
            }
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

        private Control control;
        public Control Control
        {
            get { return control; }
            set
            {
                control = value;
                Actualizar(nameof(Control));
            }
        }

        private void Ver(Cliente obj)
        {
            Cliente = obj;
            Control = CitasV;
        }

        private void CargarDatos()
        {
            Lista.Clear();
            foreach (var cuentos in citas.GetAll())
            {
                Lista.Add(cuentos);
            }
        }
        private void Nuevo()
        {
            Cliente = new Cliente();
            Control = RegistrarV;
        }
        private void Agregar()
        {
            Msj = "";
            if (citas.Insert(Cliente))
            {
                Control = CitasV;
                CargarDatos();
            }
            else
            {
                Msj = string.Join("\n",citas.Errores);
            }
        }

        private void Editar(Cliente obj)
        {
            Cliente = new Cliente { Id = obj.Id, Nombre = obj.Nombre, Telefono = obj.Telefono, Mail = obj.Mail, DiaCita = obj.DiaCita };
            Control = EditarV;
            Msj = "";
        }
        private void Guardar()
        {
            Msj = "";
            try
            {
                if (citas.Update(Cliente))
                {
                    Control = CitasV;
                    CargarDatos();
                }
                else
                {
                    Msj = string.Join("\n", citas.Errores);
                }
            }
            catch (Exception ex)
            {
                Msj = ex.Message;
            }
        }

        private void Eliminar(Cliente obj)
        {
            citas.Delete(obj);
            CargarDatos();
        }
        private void Cancelar()
        {
            Control = CitasV;
            Cliente = null;
            Msj = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Actualizar(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
