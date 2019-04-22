using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Storm.Mvvm;
using Xamarin.Forms;

namespace fourplaces.ViewModels
{
    public class EditMdpViewModel : ViewModelBase
    {
        private string anciendMdp;
        private string nouveauMdp;
        private string nouveauMdp2;
        private string msgErreur = "";
        public ICommand BoutonEnregistrer { protected set; get; }
        public INavigation Navigation { get; set; }

        public string MsgErreur
        {
            get => msgErreur;
            set => SetProperty(ref msgErreur, value);
        }

        public string AncienMdp
        {
            get => anciendMdp;
            set => SetProperty(ref anciendMdp, value);
        }

        public string NouveauMdp
        {
            get => nouveauMdp;
            set => SetProperty(ref nouveauMdp, value);
        }

        public string NouveauMdp2
        {
            get => nouveauMdp2;
            set => SetProperty(ref nouveauMdp2, value);
        }

        public EditMdpViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            AncienMdp = "";
            NouveauMdp = "";
            NouveauMdp2 = "";
            BoutonEnregistrer = new Command(async () => { await Enregistrer(); });
        }

        public async Task Enregistrer()
        {
            if (NouveauMdp == NouveauMdp2 && NouveauMdp != "")
            {
                bool res = await App.SERVICE.EditMdp(AncienMdp, NouveauMdp);
                if (res)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    MsgErreur = "Echec de la mise à jour du mot de passe";
                }
            }
            else
            {
                MsgErreur = "Un des champs incorrect";
            }
        }
    }
}