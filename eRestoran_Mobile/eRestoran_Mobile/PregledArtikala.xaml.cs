using eRestoran_PCL.Model;
using eRestoran_PCL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PregledArtikala : ContentPage
	{                                                         
        //private WebAPIHelper stavkeMenijaService = new WebAPIHelper("http://192.168.93.1", "api/StavkeMenija");
        //private WebAPIHelper tipoviStavkeService = new WebAPIHelper("http://192.168.93.1", "api/TipoviStavke");
        private WebAPIHelper stavkeMenijaService = new WebAPIHelper("http://localhost:49327/", "api/StavkeMenija");
        private WebAPIHelper tipoviStavkeService = new WebAPIHelper("http://localhost:49327/", "api/TipoviStavke");

        public List<ArtikliGroup> ListaArtikala { get; set; }

        public PregledArtikala ()
		{
			InitializeComponent ();
            if (Global.narudzba == null)
            {
                Global.narudzba = new Narudzbe();
            }
		}

        protected override void OnAppearing()
        {
            FillList("Sve");

            base.OnAppearing();
        }

        private void FillList(string parameter)
        {
            HttpResponseMessage response = stavkeMenijaService.GetActionResponse("GetByNaziv",parameter);
            var jsonObject = response.Content.ReadAsStringAsync();
            List<StavkeMenija> stavke = JsonConvert.DeserializeObject<List<StavkeMenija>>(jsonObject.Result);

            HttpResponseMessage response2 = tipoviStavkeService.GetResponse();
            var jsonObject2 = response2.Content.ReadAsStringAsync();
            List<TipoviStavke> kategorije = JsonConvert.DeserializeObject<List<TipoviStavke>>(jsonObject2.Result);


            var lista = new List<ArtikliGroup>();

            foreach (var kategorija in kategorije)
            {
                var listaArtikala = new ArtikliGroup();
                foreach (var stavka in stavke)
                {
                    if (kategorija.TipStavkeID == stavka.TipStavkeID)
                        listaArtikala.Add(stavka);
                }
                listaArtikala.Heading = kategorija.Naziv;

                lista.Add(listaArtikala);
            }


            artikliList.ItemsSource = lista;
            artikliList.IsGroupingEnabled = true;
        }

        private void pretragaInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pretragaInput.Text == "")
                FillList("Sve");
            else
                FillList(pretragaInput.Text);
        }

        private void artikliList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
                Navigation.PushAsync(new DetaljiArtikla(e.Item as StavkeMenija));
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.Quit();
            return base.OnBackButtonPressed();
        }
    }
}