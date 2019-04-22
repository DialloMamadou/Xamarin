using Storm.Mvvm.Forms;
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
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AjoutLieuView : BaseContentPage
    {
        public AjoutLieuView()
        {
            InitializeComponent();
            BindingContext = new AjoutLieuViewModel(Navigation);
        }
    }
}