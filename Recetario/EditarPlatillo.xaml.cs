using System;
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
    /// Lógica de interacción para EditarPlatillo.xaml
    /// </summary>
    public partial class EditarPlatillo : UserControl
    {
        public EditarPlatillo()
        {
            InitializeComponent();
            foreach (var item in Enum.GetValues(typeof(Dificultad)))
            {
                Filtro.Items.Add(item);
            }
            Filtro.SelectedIndex = 0;
        }

        private void Filtro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
