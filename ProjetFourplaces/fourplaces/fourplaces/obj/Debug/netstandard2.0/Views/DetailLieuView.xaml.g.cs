//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("fourplaces.Views.DetailLieuView.xaml", "Views/DetailLieuView.xaml", typeof(global::fourplaces.Views.DetailLieuView))]

namespace fourplaces.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\DetailLieuView.xaml")]
    public partial class DetailLieuView : global::Xamarin.Forms.TabbedPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::ImageCircle.Forms.Plugin.Abstractions.CircleImage imageView;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private global::Xamarin.Forms.ListView ListeComments;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "0.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(DetailLieuView));
            imageView = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::ImageCircle.Forms.Plugin.Abstractions.CircleImage>(this, "imageView");
            ListeComments = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.ListView>(this, "ListeComments");
        }
    }
}
