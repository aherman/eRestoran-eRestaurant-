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
    public partial class MenuMaster : ContentPage
    {
        bool flag = true;
        string pageTitle;
        public MenuMaster(string title)
        {
            pageTitle = title;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {

            if (pageTitle == "eRestoran :: Korpa")
                korpaLabel.IsEnabled = false;
            if (pageTitle == "eRestoran :: Moj profil")
                profileLabel.IsEnabled = false;
            if (pageTitle == "eRestoran :: Prethodne narudžbe")
                prethodneLabel.IsEnabled = false;
            if (pageTitle == "eRestoran :: Trenutne narudžbe")
                trenutneLabel.IsEnabled = false;
            if (pageTitle == "eRestoran :: Pregled artikala")
                pregledLabel.IsEnabled = false;

            var pregledTap = new TapGestureRecognizer();
            pregledTap.Tapped += async (sender, e) => {
                if (!flag)
                    return;
                flag = false;
                await pregledLabel.FadeTo(0.3, 150);
                await pregledLabel.FadeTo(1, 150);
                await Navigation.PushAsync(new Menu(new PregledArtikala()));
                flag = true;
            };
            pregledLabel.GestureRecognizers.Add(pregledTap);

            var profileTap = new TapGestureRecognizer();
            profileTap.Tapped += async (sender, e) => {
                if (!flag)
                    return;
                flag = false;
                await profileLabel.FadeTo(0.3, 150);
                await profileLabel.FadeTo(1, 150);
                await Navigation.PushAsync(new Menu(new PregledProfila()));
                flag = true;
            };
            profileLabel.GestureRecognizers.Add(profileTap);

            var korpaTap = new TapGestureRecognizer();
            korpaTap.Tapped += async (sender, e) => {
                if (Global.narudzba.NarudzbeStavke.Count == 0)
                {
                    await DisplayAlert("Korpa", "Vaša narudžba ne sadrži nijedan artikal.", "Ok");
                    return;
                }     

                if (!flag)
                    return;
                flag = false;
                await korpaLabel.FadeTo(0.3, 150);
                await korpaLabel.FadeTo(1, 150);
                await Navigation.PushAsync(new Menu(new Korpa()));
                flag = true;
            };
            korpaLabel.GestureRecognizers.Add(korpaTap);

            var trenutneTap = new TapGestureRecognizer();
            trenutneTap.Tapped += async (sender, e) => {
                if (!flag)
                    return;
                flag = false;
                await trenutneLabel.FadeTo(0.3, 150);
                await trenutneLabel.FadeTo(1, 150);
                await Navigation.PushAsync(new Menu(new TrenutneNarudzbe()));
                flag = true;

            };
            trenutneLabel.GestureRecognizers.Add(trenutneTap);

            var prethodneTap = new TapGestureRecognizer();
            prethodneTap.Tapped += async (sender, e) => {
                if (!flag)
                    return;
                flag = false;
                await prethodneLabel.FadeTo(0.3, 150);
                await prethodneLabel.FadeTo(1, 150);
                await Navigation.PushAsync(new Menu(new PrethodneNarudzbe()));
                flag = true;
            };
            prethodneLabel.GestureRecognizers.Add(prethodneTap);

            var odjavaTap = new TapGestureRecognizer();
            odjavaTap.Tapped += async (sender, e) => {
                if (!flag)
                    return;
                flag = false;
                await odjavaLabel.FadeTo(0.3, 150);
                await odjavaLabel.FadeTo(1, 150);
                Global.prijavljeniKlijent = null;
                Global.narudzba = null;
                Application.Current.MainPage = new NavigationPage(new Login());
                flag = true;
            };
            odjavaLabel.GestureRecognizers.Add(odjavaTap);

            base.OnAppearing();
        }
    }
}