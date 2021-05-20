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
    public partial class PlantarPag : ContentPage
    {
        ISimpleAudioPlayer juego;
        public PlantarPag()
        {
            InitializeComponent();

            juego = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();

            juego.Load("Monkeys.mp3");
            juego.Play();
            juego.Loop=true;
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DatosPlantas());
        }
    }
}