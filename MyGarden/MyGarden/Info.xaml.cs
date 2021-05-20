using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGarden
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Info : ContentPage
    {
        ISimpleAudioPlayer Start;
        public Info()
        {
            InitializeComponent();
            Start = CrossSimpleAudioPlayer.Current;
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            Start.Load("Iniciar.mp3");
            Start.Play();
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}