using eRestoran_PCL.Model;
using eRestoran_PCL.Models;
using eRestoran_PCL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetaljiDostave : ContentPage
	{
        
        //private WebAPIHelper dostaveService = new WebAPIHelper("http://192.168.93.1", "api/Dostave");
        //private WebAPIHelper popustiService = new WebAPIHelper("http://192.168.93.1", "api/Popusti");
        private WebAPIHelper dostaveService = new WebAPIHelper("http://localhost:49327/", "api/Dostave");
        private WebAPIHelper popustiService = new WebAPIHelper("http://localhost:49327/", "api/Popusti");
        int dostavaId;
        eRestoran_PCL.Model.Dostave dostava;
        KreditneKarticePrikaz kreditnaKarticaPrikaz; 

        public DetaljiDostave()
        {
            InitializeComponent();
        }

        public DetaljiDostave (int id)
		{
			InitializeComponent();
            dostavaId = id;
            HttpResponseMessage response = dostaveService.GetResponse(dostavaId.ToString());
            var jsonObject = response.Content.ReadAsStringAsync();
            dostava = JsonConvert.DeserializeObject<eRestoran_PCL.Model.Dostave>(jsonObject.Result);
        }

        protected override void OnAppearing()
        {
            HttpResponseMessage response = dostaveService.GetActionResponse("DetaljiDostave", dostavaId.ToString());
            var jsonObject = response.Content.ReadAsStringAsync();
            DetaljiDostavePregled detaljiDostave = JsonConvert.DeserializeObject<DetaljiDostavePregled>(jsonObject.Result);

            if (detaljiDostave.kreditnaKartica != null)
            {
                kreditnaKarticaPrikaz = detaljiDostave.kreditnaKartica;
                btnDetaljiKartice.IsVisible = true;
            }

            adresa.Text = detaljiDostave.adresaKlijenta;
            nacinPlacanja.Text = detaljiDostave.nacinPlacanja;
            listaProizvoda.ItemsSource = detaljiDostave.stavke;

            decimal ukupno = 0;
            foreach (var item in detaljiDostave.stavke)
            {
                ukupno += item.cijena;
            }

            ukupnaCijena.Text = UIHelper.DoFormat(ukupno) + " KM";

            HttpResponseMessage popustResponse = popustiService.GetActionResponse("ImaPopust", dostava.NarudzbaID.ToString());
            var jsonObject2 = popustResponse.Content.ReadAsStringAsync();
            List<PopustiModel> popusti = JsonConvert.DeserializeObject<List<PopustiModel>>(jsonObject2.Result);
            if (popusti.Count != 0)
            {
                decimal ukupanProcenat = 0;
                dioZaPopust.IsVisible = true;
                foreach (var item in popusti)
                {
                    ukupanProcenat += item.iznos;
                    ukupno -= (ukupno * (item.iznos / 100));
                }
                lblUkupnoPopust.Text = "(-" + ukupanProcenat.ToString() + "%) " + UIHelper.DoFormat(ukupno) + " KM";
            }
            else
                    dioZaPopust.IsVisible = false;

                base.OnAppearing();
        }

        

        private void ZavrsiDostavu_Clicked(object sender, EventArgs e)
        {
            HttpResponseMessage response = dostaveService.PutActionResponse(dostavaId, "Zavrsi", dostava);

            if (response.IsSuccessStatusCode)
            {
                DisplayAlert("Dostava", "Uspješno završena dostava", "Ok");
                Navigation.PopAsync();
            }
            else
                DisplayAlert("Dostava", "Došlo je do greške.", "Ok");
        }

        private void BtnDetaljiKartice_Clicked(object sender, EventArgs e)
        {
            string msg = "Ime i prezime: "+kreditnaKarticaPrikaz.ImePrezime+Environment.NewLine+"Broj kartice: "+kreditnaKarticaPrikaz.BrojKartice;
            DisplayAlert("Detalji o kreditnoj kartici", msg, "Ok");
        }
    }
}