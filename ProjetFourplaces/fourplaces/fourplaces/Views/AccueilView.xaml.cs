using fourplaces.Models;
using fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace fourplaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccueilView : BaseContentPage
	{
		public AccueilView ()
		{
			InitializeComponent ();
            BindingContext = new AccueilViewModel(Navigation);
            ListeLieux.ItemTapped += ListeLieux_ItemTapped;
        }

        private void ListeLieux_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            PlaceItemSummary lieu = (PlaceItemSummary)e.Item;
            Navigation.PushAsync(new DetailLieuView(lieu));
        }
    }
}