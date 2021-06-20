﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recetario
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Filtro.Items.Add("Todos");
            foreach (var item in Enum.GetValues(typeof(Dificultad)))
            {
               Filtro.Items.Add(item);
            }
            Filtro.SelectedIndex = 0;
        }

        private void Filtro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Filtro.SelectedItem is string && (string)Filtro.SelectedItem == "Todos")
            {
                recetarios.FiltrarCommand.Execute(null);
            }
            else
            {
                recetarios.FiltrarCommand.Execute(Filtro.SelectedItem);
            }
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
