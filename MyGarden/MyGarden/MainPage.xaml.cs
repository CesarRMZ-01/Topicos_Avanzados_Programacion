using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyGarden
{
    public partial class MainPage : ContentPage
    {
        ISimpleAudioPlayer inicio;
        ISimpleAudioPlayer Start;
        public MainPage()
        {
            InitializeComponent();

            inicio = CrossSimpleAudioPlayer.Current;
            Start = CrossSimpleAudioPlayer.Current;

            inicio.Load("Title.mp3");
            inicio.Play();
            inicio.Loop = true;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PlantarPag());
            inicio.Stop();
            Start.Load("Iniciar.mp3");
            Start.Play();
        }
    }
}
