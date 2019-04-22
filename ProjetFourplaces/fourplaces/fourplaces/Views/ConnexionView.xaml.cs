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
	public partial class ConnexionView :BaseContentPage
	{
		public ConnexionView ()
		{
			InitializeComponent ();
            BindingContext = new ConnexionViewModel(Navigation);
        }
	}
}