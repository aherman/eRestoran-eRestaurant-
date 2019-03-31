using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : MasterDetailPage
    {
        public Menu(ContentPage page)
        {
            InitializeComponent();
            Master = new MenuMaster(page.Title);
            #if __ANDROID__
                Detail = new NavigationPage(page);
            #endif
            #if WINDOWS_UWP
                Detail = page;
            #endif
            IsPresented = false;
            MasterBehavior = MasterBehavior.Popover;
            
        }

    }
}