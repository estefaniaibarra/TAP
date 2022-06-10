using Ejemplo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ejemplo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new InicioView());
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
