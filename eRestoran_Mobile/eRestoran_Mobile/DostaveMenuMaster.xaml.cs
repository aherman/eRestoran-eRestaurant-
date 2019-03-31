using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DostaveMenuMaster : ContentPage
    {
        bool flag = true;
        public DostaveMenuMaster()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var odjavaTap = new TapGestureRecognizer();
            odjavaTap.Tapped += async (sender, e) => {
                if (!flag)
                    return;
                flag = false;
                await odjavaLabel.FadeTo(0.3, 150);
                await odjavaLabel.FadeTo(1, 150);
                Global.prijavljeniZaposlenik = null;
                Application.Current.MainPage = new NavigationPage(new Login());
                flag = true;
            };
            odjavaLabel.GestureRecognizers.Add(odjavaTap);

            base.OnAppearing();
        }
    }
}