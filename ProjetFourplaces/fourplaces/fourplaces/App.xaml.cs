using System;
using fourplaces;
using fourplaces.Models;
using fourplaces.Views;
using MonkeyCache.SQLite;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace fourplaces
{
    public partial class App : Application
    {
        
        public static Service SERVICE { get; set; }
        public static UserItem SESSION_PROFIL { get; set; }
        public static LoginResult SESSION_LOGIN { get; set; }


        public App()
        {
           
            SERVICE = new Service();


            Barrel.ApplicationId = "fourPlace";
            Barrel.Current.Add(key: "Position", data: new Position(0.0, 0.0), expireIn: TimeSpan.FromDays(1));


            InitializeComponent();
            MainPage = new NavigationPage(new ConnexionView());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
