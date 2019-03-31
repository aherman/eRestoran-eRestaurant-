using eRestoran_PCL.Model;
using eRestoran_PCL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TrenutneNarudzbe : ContentPage
	{
        //private WebAPIHelper trenutneNarudzbeService = new WebAPIHelper("http://192.168.93.1", "api/Narudzbe");
        private WebAPIHelper trenutneNarudzbeService = new WebAPIHelper("http://localhost:49327/", "api/Narudzbe");
       
        public TrenutneNarudzbe(string v = "")
        {
            InitializeComponent();
            if (v == "delete")
                DisplayAlert("Akcija", "Uspješno obrisana narudžba", "Ok");
        }

        protected override void OnAppearing()
        {
            HttpResponseMessage response = trenutneNarudzbeService.GetActionResponse("TrenutneNarudzbe", Global.prijavljeniKlijent.KlijentID.ToString());
            var jsonObject = response.Content.ReadAsStringAsync();
            List<TrenutneNarudzbeJson> stavke = JsonConvert.DeserializeObject<List<TrenutneNarudzbeJson>>(jsonObject.Result);

            ObservableCollection<TrenutneNarudzbeList> listaN = new ObservableCollection<TrenutneNarudzbeList>();
            int counter = 0;
            string thisColor = "#000000";
            foreach (TrenutneNarudzbeJson item in stavke)
            {
                if (item.aktivna == true)
                    thisColor = "#ADD8E6";
                else
                {
                    counter++;
                    if (counter < 2)
                        thisColor = "#98FB98";
                    else if (counter >= 2 && counter < 5)
                        thisColor = "#FFFF99";
                    else
                        thisColor = "#FF7F7F";
                }
                
                

                var group = new TrenutneNarudzbeList()
                {
                    narudzbaID = item.narudzbaID,
                    datum = item.datum,
                    color = thisColor,
                    aktivna = item.aktivna
                };
                foreach (var stavka in item.stavke)
                {
                    group.Add(new TrenutneNarudzbeListStavke
                    {
                        narudzbaStavkaID = stavka.narudzbaStavkaID,
                        kolicina = stavka.kolicina,
                        naziv = stavka.naziv
                    });
                }
                listaN.Add(group);
            }

            TrenutneNarudzbeCount.Text = stavke.Count.ToString();

            lista.ItemsSource = listaN;

            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new NavigationPage(new Menu(new PregledArtikala()));
            return base.OnBackButtonPressed();
        }

        private void TapGestureRecognizer_Tapped(object sender, ItemTappedEventArgs e)
        {
            var stackLayout = (StackLayout)sender;
            if(stackLayout.BackgroundColor != Color.FromHex("#ADD8E6"))
            {
                var id = (int)e.Group;
                Navigation.PushAsync(new Korpa(true, id));
            }
        }
    }
}