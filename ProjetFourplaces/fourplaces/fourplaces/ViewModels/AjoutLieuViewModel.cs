using fourplaces.Models;
using MonkeyCache.SQLite;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace fourplaces.ViewModels
{
    public class AjoutLieuViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private List<ImageItem> images;
        private ImageItem selectedImage;
        private string title = "";
        private string description = "";
        private int? imageId;
        private string imageUrl;
        private double latitude;
        private double longitude;
        private Map map;
        private string msgErreur = "";
        public ICommand BoutonChargerImage { protected set; get; }
        public ICommand BoutonPrendrePhoto { protected set; get; }
        public ICommand BoutonAjouterLieu { protected set; get; }
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public string MsgErreur
        {
            get => msgErreur;
            set => SetProperty(ref msgErreur, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ImageUrl
        {
            get => imageUrl;
            set => SetProperty(ref imageUrl, value);
        }

        public double Latitude
        {
            get => latitude;
            set
            {
                SetProperty(ref latitude, value);
                UpdateMap();
            }
        }
        public double Longitude
        {
            get => longitude;
            set
            {
                SetProperty(ref longitude, value);
                UpdateMap();
            }
        }

        public string ImageId
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

        public Map Map
        {
            get => map;
            set => SetProperty(ref map, value);
        }
        public AjoutLieuViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            BoutonAjouterLieu = new Command(async () => { await AjouterLieu(); });
            BoutonPrendrePhoto = new Command(async () => { await PrendrePhoto(); });
            BoutonChargerImage = new Command(async () => { await ChargerImage(); });

            map = new Map();
            images = new List<ImageItem>();
            imageId = 1;
            imageUrl = Service.URL_RACINE + Service.URL_GET_IMAGE + 1;
            latitude = Barrel.Current.Get<Plugin.Geolocator.Abstractions.Position>(key: "Position").Latitude;
            longitude = Barrel.Current.Get<Plugin.Geolocator.Abstractions.Position>(key: "Position").Longitude;
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            await ChargementImage();
            UpdateMap();
        }


        private async Task AjouterLieu()
        {
            bool res = await App.SERVICE.AjouterLieu(Title, Description, int.Parse(ImageId), Latitude, Longitude);
            if (res)
            {
                await Navigation.PopAsync();
            }
            else
            {
                MsgErreur = "Echec de la création";
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

        private void UpdateMap()
        {
            Xamarin.Forms.Maps.Position positionpin = new Xamarin.Forms.Maps.Position(Latitude, Longitude);
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(positionpin, Distance.FromKilometers(150)));
            var pin = new Pin
            {
                Position = positionpin,
                Label = Title
            };
            Map.Pins.Clear();
            Map.Pins.Add(pin);
        }

        public virtual void OnPropertyChanged(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
    }
}