using fourplaces.Models;
using fourplaces.Views;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace fourplaces.ViewModels
{
    public class InscriptionViewModel : ViewModelBase
    {
        private string email;
        private string firstName;
        private string lastName ;
        private string password ;
        private string password2;
        private string msgErreur = "";

        //public RegisterRequest User { get; set; }
        public ICommand BoutonInscription { protected set; get; }
        public INavigation Navigation { get; set; }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string Password2
        {
            get => password2;
            set => SetProperty(ref password2, value);
        }
        public string MsgErreur
        {
            get => msgErreur;
            set => SetProperty(ref msgErreur, value);
        }
       
        public InscriptionViewModel(INavigation navigation)
        {
            //User = new RegisterRequest();
            this.Navigation = navigation;
            BoutonInscription = new Command(async () => { await Inscription(); });
        }
 
        public async Task Inscription()
        {
            //if (User.Email == "" || User.FirstName == "" || User.LastName == "" || User.Password == "" || User.Password != ConfirmerMdp)
            if (Email == "" || FirstName == "" || LastName == "" || Password == "" || Password != Password2)
            {
                MsgErreur = "Un des champs est incorrect";
            }
            else
            {
                Debug.WriteLine("Champs corrects");
                bool response = await App.SERVICE.Inscription(Email, FirstName, LastName, Password);
                Debug.WriteLine("Service ok", response);
                if (response)
                {
                    await Navigation.PushModalAsync(new NavigationPage(new AccueilView()));
                }
                else
                {
                    MsgErreur = "Problème lors de l'inscription";
                }
            }

        }
    }
}