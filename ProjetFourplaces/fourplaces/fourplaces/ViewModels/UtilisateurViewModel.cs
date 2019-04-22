using fourplaces.Models;
using fourplaces.Views;
using MonkeyCache.SQLite;
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
    public class UtilisateurViewModel : ViewModelBase
    {
        private string firstName;
        private string lastName;
        private string email;
        private string imageUrl;
        private string msgErreur = "";
        public ICommand BoutonEditUtil { protected set; get; }
        public ICommand BoutonEditMdp { protected set; get; }
        public INavigation Navigation { get; set; }

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

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string ImageUrl
        {
            get => imageUrl;
            set => SetProperty(ref imageUrl, value);
        }

        public string MsgErreur
        {
            get => msgErreur;
            set => SetProperty(ref msgErreur, value);
        }
        public UtilisateurViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            BoutonEditUtil = new Command(async () => { await Navigation.PushAsync(new EditUtilisateurView()); });
            BoutonEditMdp = new Command(async () => { await Navigation.PushAsync(new EditMdpView()); });
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            MsgErreur = "";
            FirstName = "Prénom: " + App.SESSION_PROFIL.FirstName;
            LastName = "Nom: " + App.SESSION_PROFIL.LastName;
            Email = "Email: " + App.SESSION_PROFIL.Email;
            ImageUrl = Service.URL_RACINE + Service.URL_GET_IMAGE + App.SESSION_PROFIL.ImageId;
            Console.WriteLine("App.SESSION_PROFIL.ImageId" + App.SESSION_PROFIL.ImageId);
        }

    }
}