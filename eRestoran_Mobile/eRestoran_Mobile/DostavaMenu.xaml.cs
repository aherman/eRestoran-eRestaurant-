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
	public partial class DostavaMenu : MasterDetailPage
	{
		public DostavaMenu ()
		{
			InitializeComponent ();
            Master = new DostaveMenuMaster();
            #if __ANDROID__
                Detail = new NavigationPage(new Dostave());
            #endif
            #if WINDOWS_UWP
                Detail = new Dostave();
            #endif
            IsPresented = false;
            MasterBehavior = MasterBehavior.Popover;
        }
	}
}