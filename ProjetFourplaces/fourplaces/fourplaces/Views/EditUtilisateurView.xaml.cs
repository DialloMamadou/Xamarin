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
	public partial class EditUtilisateurView : BaseContentPage
	{
		public EditUtilisateurView ()
		{
			InitializeComponent ();
            BindingContext = new EditUtilisateurViewModel(Navigation);
        }
	}
}