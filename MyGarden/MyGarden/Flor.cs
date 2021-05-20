using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MyGarden
{
     public class Flor: INotifyPropertyChanged
     { 
        public string Tipo { get; set; }
        public string NomCien { get; set; }
        public int Semilla { get; set; }
        public int CantFlores { get; set; }
        public string DescPlanta { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
