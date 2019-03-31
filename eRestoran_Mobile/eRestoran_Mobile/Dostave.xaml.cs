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
	public partial class Dostave : ContentPage
	{
        
        //private WebAPIHelper dostaveService = new WebAPIHelper("http://192.168.93.1", "api/Dostave");
        private WebAPIHelper dostaveService = new WebAPIHelper("http://localhost:49327/", "api/Dostave");

        public Dostave ()
		{
			InitializeComponent ();
            
		}

        protected override void OnAppearing()
        {
            HttpResponseMessage response = dostaveService.GetResponse("TrenutneDostave");
            var jsonObject = response.Content.ReadAsStringAsync();
            List<TrenutneDostaveJson> stavke = JsonConvert.DeserializeObject<List<TrenutneDostaveJson>>(jsonObject.Result);

            ObservableCollection<TrenutneDostave> listaN = new ObservableCollection<TrenutneDostave>();

            foreach (var item in stavke)
            {
                var group = new TrenutneDostave()
                {
                    dostavaId = item.dostavaId,
                    imePrezime = item.imePrezime
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

            TrenutneDostaveCount.Text = stavke.Count.ToString();

            lista.ItemsSource = listaN;

            

            base.OnAppearing();
        }

        private void TapGestureRecognizer_Tapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new DetaljiDostave((int)e.Group));
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.Quit();

            return base.OnBackButtonPressed();
        }
    }
}