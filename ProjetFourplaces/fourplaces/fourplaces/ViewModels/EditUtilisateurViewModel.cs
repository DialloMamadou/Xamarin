using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Storm.Mvvm;
using Xamarin.Forms;

namespace fourplaces.ViewModels
{

    public class EditUtilisateurViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string firstName;
        private string lastName;
        private List<ImageItem> images;
        private ImageItem selectedImage;
        private int? imageId;
        private string imageUrl;
        private string msgErreur = "";
        public ICommand BoutonEnregister { protected set; get; }
        public ICommand BoutonChargerImage { protected set; get; }
        public ICommand BoutonPrendrePhoto { protected set; get; }
        public INavigation Navigation { get; set; }

        public ImageItem SelectedImage
        {
            get
            {
                return selectedImage;
            }

            set
            {
                selectedImage = value;
                OnPropertyChanged("SelectedImage");
                ImageId = value.Id.ToString();
                OnPropertyChanged("ImageId");
            }
        }

        public List<ImageItem> Images
        {
            get => images;
            set => SetProperty(ref images, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string MsgErreur
        {
            get => msgErreur;
            set => SetProperty(ref msgErreur, value);
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

        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                SetProperty(ref imageUrl, value);
            }
        }

        public String ImageId
        {
            get
            {
                if (imageId == null)
                    return "";
                else
                    return imageId.ToString();
            }

            set
            {
                try
                {
                    int? temp = int.Parse(value);
                    SetProperty(ref imageId, temp);
                    imageUrl = Service.URL_RACINE + Service.URL_GET_IMAGE + ImageId;
                }
                catch
                {
                    SetProperty(ref imageId, 1);
                    imageUrl = Service.URL_RACINE + Service.URL_GET_IMAGE + "1";
                }
                OnPropertyChanged("ImageUrl");
            }
        }

        public EditUtilisateurViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            firstName = App.SESSION_PROFIL.FirstName;
            lastName = App.SESSION_PROFIL.LastName;
            imageId = App.SESSION_PROFIL.ImageId;
            imageUrl = Service.URL_RACINE + Service.URL_GET_IMAGE + App.SESSION_PROFIL.ImageId;

            images = new List<ImageItem>();
            BoutonEnregister = new Command(async () => { await Enregister(); });
            BoutonChargerImage = new Command(async () => { await ChargerImage(); });
            BoutonPrendrePhoto = new Command(async () => { await PrendrePhoto(); });
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            await ChargementImage();
        }

        public async Task Enregister()
        {
            bool res = await App.SERVICE.EditUtilisateur(FirstName, LastName, int.Parse(ImageId));
            if (res)
            {
                await Navigation.PopAsync();
            }
            else
            {
                MsgErreur = "Echec de la mise à jour du profil";
            }
        }

        public async Task ChargerImage()
        {
            int? res = await App.SERVICE.ChargerImage(true);
            if (res != null)
            {
                ImageId = res.ToString();
                OnPropertyChanged("ImageId");
                await ChargementImage();
            }
            else
            {
                MsgErreur = "Problème de chargement de l'image";
            }
        }

        public async Task PrendrePhoto()
        {
            int? res = await App.SERVICE.ChargerImage(false);
            if (res != null)
            {
                ImageId = res.ToString();
                OnPropertyChanged("ImageId");
                await ChargementImage();
            }
            else
            {
                MsgErreur = "Problème de prise de photo";
            }
        }


        public async Task ChargementImage()
        {
            Images = new List<ImageItem>();
            int id = 1;
            while (await App.SERVICE.GetImage(id))
            {
                Images.Add(new ImageItem(id, "https://td-api.julienmialon.com/images/" + id));
                id++;
            }
            OnPropertyChanged("Images");
        }

        public virtual void OnPropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
    }
}
