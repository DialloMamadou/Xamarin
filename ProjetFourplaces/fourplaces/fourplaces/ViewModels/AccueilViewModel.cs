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
    public class AccueilViewModel : ViewModelBase
    {

        private List<PlaceItemSummary> lieux;
        private string msgErreur = "";
        public ICommand BoutonAjouterLieu { protected set; get; }
        public ICommand ButonVoirProfil { protected set; get; }
        public ICommand BoutonActualiser { protected set; get; }
        public INavigation Navigation { get; set; }



        public List<PlaceItemSummary> Lieux
        {
            get => lieux;
            set => SetProperty(ref lieux, value);
        }

        public string MsgErreur
        {
            get => msgErreur;
            set => SetProperty(ref msgErreur, value);
        }

        public AccueilViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            ButonVoirProfil = new Command(async () => { await Navigation.PushAsync(new UtilisateurView()); });
            BoutonAjouterLieu = new Command(async () => { await Navigation.PushAsync(new AjoutLieuView()); });
            BoutonActualiser = new Command(async () => { await OnResume(); });
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            MsgErreur = "";
            ListeLieux lieux = await App.SERVICE.GetListLieux();
            if (lieux != null)
            {
                Console.WriteLine("Liste not null");
                foreach (PlaceItemSummary element in lieux.Lieux)
                {
                    element.ImageUrl = Service.URL_RACINE + Service.URL_GET_IMAGE + element.ImageId;
                }

                Lieux = lieux.Lieux;
                Console.WriteLine("Lieu ok");
            }
            else
            {
                MsgErreur = "Erreur lors de la récupération des lieux";
            }
        }
    }
}