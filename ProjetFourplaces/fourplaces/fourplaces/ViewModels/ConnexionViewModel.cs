using fourplaces.Models;
using fourplaces.Views;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace fourplaces.ViewModels
{
    public class ConnexionViewModel : ViewModelBase
    {
        //private LoginRequest User;
        private string email;
        private string password;
        private string msgErreur = "";

        /*public LoginRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }*/

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string MsgErreur
        {
            get => msgErreur;
            set => SetProperty(ref msgErreur, value);
        }
        public ICommand BoutonConnexion { protected set; get; }
        public ICommand BoutonInscription { protected set; get; }
        public INavigation Navigation { get; set; }

        public ConnexionViewModel(INavigation navigation)
        {
            //User = new LoginRequest();
            this.Navigation = navigation;
            BoutonConnexion = new Command(async () => { await ConnexionAsync(); });
            BoutonInscription = new Command(async () => { await InscriptionAsync(); });

        }
        public async Task ConnexionAsync()
        {
            if (Email == "" || Password == "")
            {
                MsgErreur = "Un des champs non renseigné";
               
            }
            else
            {
                bool res = await App.SERVICE.Connexion(email, password);
                if (res)
                {
                    Console.WriteLine("connexion ok");
                    await Navigation.PushModalAsync(new NavigationPage(new AccueilView()));
                }
                else
                {
                    Password = "";
                    MsgErreur = "Problème de connection";
                }
            }
        }

        public async Task InscriptionAsync()
        {
            await Navigation.PushAsync(new InscriptionView());
        }
    }


}
    
	
