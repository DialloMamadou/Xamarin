using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using fourplaces.Models;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace fourplaces.ViewModels
{
    public class DetailLieuViewModel : ViewModelBase
    {
        private int id;
        private string commentaire;
        private string title;
        private string description;
        private string imageUrl;
        private double latitude;
        private double longitude;
        private double distanceP;
        private Map map;
        private List<CommentItem> commentaires;
        private string msgErreur = "";
        public ICommand BoutonCommenter { protected set; get; }


        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Commentaire
        {
            get => commentaire;
            set => SetProperty(ref commentaire, value);
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
            set => SetProperty(ref latitude, value);
        }
        public double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        public double DistanceP
        {
            get => distanceP;
            set => SetProperty(ref distanceP, value);
        }

        public Map Map
        {
            get => map;
            set => SetProperty(ref map, value);
        }

        public List<CommentItem> Commentaires
        {
            get => commentaires;
            set => SetProperty(ref commentaires, value);
        }

        public String MsgErreur
        {
            get => msgErreur;
            set => SetProperty(ref msgErreur, value);
        }

        public DetailLieuViewModel(PlaceItemSummary lieu)
        {
            id = lieu.Id;
            title = lieu.Title;
            description = lieu.Description;
            imageUrl = lieu.ImageUrl;
            latitude = lieu.Latitude;
            longitude = lieu.Longitude;
            distanceP = lieu.Distance;
            map = new Map();
            commentaires = new List<CommentItem>();
            BoutonCommenter = new Command(async () => { await Commenter(); });
            Actualiser();
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            await Actualiser();
        }

        public async Task Actualiser()
        {
            MsgErreur = "";
            PlaceItem place = await App.SERVICE.GetLieu(Id);
            if (place != null)
            {
                Commentaires = place.Comments;
                Position position_pin = new Position(Latitude, Longitude);
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(position_pin, Distance.FromKilometers(DistanceP * 1.5)));
                var pin = new Pin
                {
                    Position = position_pin,
                    Label = Title
                };
                Map.Pins.Add(pin);
            }
            else
            {
                MsgErreur = "Erreur lors du chargement des détails du lieu";
            }
        }

        public async Task Commenter()
        {

            if (Commentaire != "" && Commentaire != null)
            {
                if (await App.SERVICE.Commenter(Commentaire, Id))
                {
                    Commentaire = "";
                    await this.OnResume();
                }
                else
                {
                    MsgErreur = "Erreur lors de l'enregistrement du commentaire";
                }
            }
            else
            {
                MsgErreur = "Commentaire vide";
            }
        }
    }
}
