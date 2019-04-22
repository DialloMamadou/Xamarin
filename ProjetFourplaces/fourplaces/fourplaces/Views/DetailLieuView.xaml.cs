using fourplaces.ViewModels;
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
	public partial class DetailLieuView : TabbedPage
	{
		public DetailLieuView (Models.PlaceItemSummary lieu)
        {
            InitializeComponent();
            BindingContext = new DetailLieuViewModel(lieu);

        }
	}
}