using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Presion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VInicio : ContentPage
    {
        public VInicio()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
        }
    }
}