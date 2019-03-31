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
	public partial class PrethodneNarudzbe : ContentPage
	{
        //private WebAPIHelper prethodneNarudzbeService = new WebAPIHelper("http://192.168.93.1", "api/Narudzbe");
        private WebAPIHelper prethodneNarudzbeService = new WebAPIHelper("http://localhost:49327/", "api/Narudzbe");

        public PrethodneNarudzbe ()
		{
			InitializeComponent ();
		}


        protected override void OnAppearing()
        {
            HttpResponseMessage response = prethodneNarudzbeService.GetActionResponse("PrethodneNarudzbe", Global.prijavljeniKlijent.KlijentID.ToString());
            var jsonObject = response.Content.ReadAsStringAsync();
            List<TrenutneNarudzbeJson> stavke = JsonConvert.DeserializeObject<List<TrenutneNarudzbeJson>>(jsonObject.Result);

            ObservableCollection<TrenutneNarudzbeList> listaN = new ObservableCollection<TrenutneNarudzbeList>();

            foreach (TrenutneNarudzbeJson item in stavke)
            {
                var group = new TrenutneNarudzbeList()
                {
                    narudzbaID = item.narudzbaID,
                    datum = item.datum
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

            PrethodneNarudzbeCount.Text = stavke.Count.ToString();

            lista.ItemsSource = listaN;

            base.OnAppearing();
        }


        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new NavigationPage(new Menu(new PregledArtikala()));
            return base.OnBackButtonPressed();
        }
    }
}