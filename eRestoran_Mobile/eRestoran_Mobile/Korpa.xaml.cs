using eRestoran_PCL.Model;
using eRestoran_PCL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Korpa : ContentPage
	{
        //private WebAPIHelper narudzbeService = new WebAPIHelper("http://192.168.93.1", "api/Narudzbe");
        //private WebAPIHelper popustiService = new WebAPIHelper("http://192.168.93.1", "api/Popusti");
        //private WebAPIHelper stavkeMenijaService = new WebAPIHelper("http://192.168.93.1", "api/StavkeMenija");
        //private WebAPIHelper narudzbeStavkeService = new WebAPIHelper("http://192.168.93.1", "api/NarudzbeStavke");
        private WebAPIHelper narudzbeService = new WebAPIHelper("http://localhost:49327/", "api/Narudzbe");
        private WebAPIHelper popustiService = new WebAPIHelper("http://localhost:49327/", "api/Popusti");
        private WebAPIHelper stavkeMenijaService = new WebAPIHelper("http://localhost:49327/", "api/StavkeMenija");
        private WebAPIHelper narudzbeStavkeService = new WebAPIHelper("http://localhost:49327/", "api/NarudzbeStavke");

        Narudzbe narudzba;
        List<NarudzbeStavkeEdit> narudzbaStavke;
        string error;
        bool pickerSelected = false;
        bool Uredi;
        int narudzbaID;
        public Korpa (bool isUredi = false, int narudzbaId = 0)
		{
            Uredi = isUredi;
            narudzbaID = narudzbaId;
			InitializeComponent ();
            placanjeKarticom.IsVisible = false;
		}

        protected override void OnAppearing()
        {

            if (Uredi == true)//edit
            {
                HttpResponseMessage response = narudzbeService.GetActionResponse("ById", narudzbaID.ToString());
                var jsonObject = response.Content.ReadAsStringAsync();
                narudzba = JsonConvert.DeserializeObject<Narudzbe>(jsonObject.Result);

                HttpResponseMessage narudzbeStavkeResponse = narudzbeStavkeService.GetResponse(narudzbaID.ToString());
                jsonObject = narudzbeStavkeResponse.Content.ReadAsStringAsync();
                narudzbaStavke = JsonConvert.DeserializeObject<List<NarudzbeStavkeEdit>>(jsonObject.Result);

                foreach (var item in narudzbaStavke.ToList())
                {
                    narudzba.NarudzbeStavke.Add(new NarudzbeStavke
                    {
                        StavkaMenijaID = item.StavkaMenijaID,
                        NarudzbaStavkaID = item.NarudzbaStavkaID,
                        Kolicina = item.Kolicina,
                        Napomena = item.Napomena,
                        NarudzbaID = item.NarudzbaID
                    });
                }

                FillUrediPolja();
            }
            else
                narudzba = Global.narudzba;

            FillStavke();
            if (Global.prijavljeniKlijent.Adresa == "" || Global.prijavljeniKlijent.Adresa == null)
                adresaSwitch.IsEnabled = false;

            for (int i = DateTime.Now.Month; i <= 12; i++)
            {
                karticaIstekMjesec.Items.Add(Convert.ToString(i));
                if(narudzba.kartica != null)
                    if (narudzba.kartica.MjesecIsteka == i)
                        karticaIstekMjesec.SelectedIndex = i;
            }
            for (int i = DateTime.Now.Year; i <= 2028; i++)
            {
                karticaIstekGodina.Items.Add(Convert.ToString(i));
                if (narudzba.kartica != null)
                    if (narudzba.kartica.GodinaIsteka == i)
                        karticaIstekGodina.SelectedIndex = i;
            }

            base.OnAppearing();
        }

        private void FillUrediPolja()
        {
            if(narudzba.AdresaDostave == Global.prijavljeniKlijent.Adresa)
            {
                adresaSwitch.IsToggled = true;
                adresa.IsEnabled = false;
            }
            else
            {
                adresa.IsEnabled = true;
                adresaSwitch.IsToggled = false;
            }
            adresa.Text = narudzba.AdresaDostave;

            prioritetnaDostava.IsToggled = narudzba.PrioritetnaDostava;

            pickerSelected = true;

            if (narudzba.IsGotovina)
            {
                placanjeKarticom.IsVisible = false;
                nacinPlacanja.SelectedIndex = 0;
            }
            else
            {
                placanjeKarticom.IsVisible = true;
                nacinPlacanja.SelectedIndex = 1;

                karticaIme.Text = narudzba.kartica.Ime;
                karticaPrezime.Text = narudzba.kartica.Prezime;
                karticaBroj.Text = narudzba.kartica.BrojKartice;
                karticaSigurnosniKod.Text = narudzba.kartica.SigurnosniKod;
            }

            posaljiNarudzbu.Text = "SNIMI NARUDŽBU";
            posaljiNarudzbu.BackgroundColor = Color.LightGreen;
            editBrisiPart.IsVisible = true;

        }

        private void FillStavke()
        {
            if (narudzba.NarudzbeStavke.Count == 0) {
                if (Uredi == false)
                    Application.Current.MainPage = new NavigationPage(new Menu(new PregledArtikala()));
                else
                {
                    HttpResponseMessage deleteResponseMessage = narudzbeService.DeleteResponse(narudzbaID.ToString());
                    Application.Current.MainPage = new NavigationPage(new Menu(new TrenutneNarudzbe("delete")));
                }
            }

            KorpaVM model = new KorpaVM();

                foreach (var item in narudzba.NarudzbeStavke.ToList())
                {
                    HttpResponseMessage tempResponse = stavkeMenijaService.GetActionResponse("ById", item.StavkaMenijaID.ToString());
                    var jsonObject2 = tempResponse.Content.ReadAsStringAsync();
                    StavkeMenijaEditPrikaz tempStavka = JsonConvert.DeserializeObject<StavkeMenijaEditPrikaz>(jsonObject2.Result);
                    model.stavke.Add(new KorpaStavke
                    {
                        stavkaId = tempStavka.StavkaMenijaID,
                        cijena = tempStavka.cijena,
                        kolicina = item.Kolicina,
                        naziv = tempStavka.naziv
                    });
                }
            
            artikliList.ItemsSource = model.stavke;

            HttpResponseMessage response = popustiService.GetActionResponse("TrenutniPopusti", "");
            var jsonObject = response.Content.ReadAsStringAsync();
            List<Popusti> stavkePopusti = JsonConvert.DeserializeObject<List<Popusti>>(jsonObject.Result);

            decimal ukupno = 0;


                for (int i = 0; i < narudzba.NarudzbeStavke.Count; i++)
                {
                    HttpResponseMessage tempResponse = stavkeMenijaService.GetActionResponse("ById", narudzba.NarudzbeStavke[i].StavkaMenijaID.ToString());
                    var jsonObject2 = tempResponse.Content.ReadAsStringAsync();
                    StavkeMenijaEditPrikaz tempStavka = JsonConvert.DeserializeObject<StavkeMenijaEditPrikaz>(jsonObject2.Result);

                    ukupno += narudzba.NarudzbeStavke[i].Kolicina * tempStavka.cijena;
                }
            

            lblUkupno.Text = UIHelper.DoFormat(ukupno) + " KM";

            if (stavkePopusti != null)
            {
                decimal ukupanProcenat = 0;
                dioZaPopust.IsVisible = true;
                foreach (var item in stavkePopusti)
                {
                    ukupanProcenat += item.Iznos;
                    ukupno -= (ukupno * (item.Iznos / 100));
                }
                lblUkupnoPopust.Text = "(-"+ ukupanProcenat.ToString() + "%) " + UIHelper.DoFormat(ukupno) + " KM";
            }
            else
                dioZaPopust.IsVisible = false;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            pickerSelected = true;
            var picker = sender as Picker;
            if (picker.SelectedIndex == 0)
                placanjeKarticom.IsVisible = false;
            else
                placanjeKarticom.IsVisible = true;
        }

        private bool IsValid()
        {
            error = "";
            if (!pickerSelected)
                error += "Način plaćanja nije odabran." + System.Environment.NewLine;

            if(string.IsNullOrEmpty(adresa.Text) || string.IsNullOrWhiteSpace(adresa.Text))
                error += "Polje adresa ne smije biti prazno." + System.Environment.NewLine;

            if (placanjeKarticom.IsVisible)
            {
                if(string.IsNullOrEmpty(karticaIme.Text) || string.IsNullOrWhiteSpace(karticaIme.Text))
                    error += "Polje za unos imena nosioca kartice ne smije biti prazno." + System.Environment.NewLine;
                if (string.IsNullOrEmpty(karticaPrezime.Text) || string.IsNullOrWhiteSpace(karticaPrezime.Text))
                    error += "Polje za unos prezimena nosioca kartice ne smije biti prazno." + System.Environment.NewLine;
                if (string.IsNullOrEmpty(karticaBroj.Text))
                    error += "Polje za unos broja kartice ne smije biti prazno." + System.Environment.NewLine;
                if (!string.IsNullOrEmpty(karticaBroj.Text))
                    if((!Regex.Match(karticaBroj.Text, "[0-9]{16}").Success))
                    error += "Polje broj kartice mora sadržavati tačno 16 brojeva." + System.Environment.NewLine;
                if (string.IsNullOrEmpty(karticaSigurnosniKod.Text))
                    error += "Polje za unos sigurnosnog koda ne smije biti prazno." + System.Environment.NewLine;
                if (!string.IsNullOrEmpty(karticaSigurnosniKod.Text))
                   if(!Regex.Match(karticaSigurnosniKod.Text, "[0-9]{3}").Success)
                    error += "Polje sigurnosni kod mora sadržavati tačno 3 broja." + System.Environment.NewLine;
                if(karticaIstekMjesec.SelectedIndex == -1)
                    error += "Odaberite mjesec isteka kartice." + System.Environment.NewLine;
                if (karticaIstekGodina.SelectedIndex == -1)
                    error += "Odaberite godinu isteka kartice." + System.Environment.NewLine;
            }

            if (error == "")
                return true;
            return false;
        }

        private void posaljiNarudzbu_Clicked(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                DisplayAlert("Korpa", "Unos nije validan. Greške: " + System.Environment.NewLine + System.Environment.NewLine+error, "Ok");
                return;
            }
                
            narudzba.Datum = DateTime.Now;
            narudzba.PrioritetnaDostava = prioritetnaDostava.IsToggled;
            narudzba.AdresaDostave = adresa.Text;
            narudzba.KlijentID = Global.prijavljeniKlijent.KlijentID;
            if (nacinPlacanja.SelectedIndex == 1)
            {
                narudzba.IsGotovina = false;
                narudzba.kartica = new KreditneKartice()
                {
                    Ime = karticaIme.Text,
                    Prezime = karticaPrezime.Text,
                    BrojKartice = karticaBroj.Text,
                    KlijentID = Global.prijavljeniKlijent.KlijentID,
                    GodinaIsteka = Convert.ToInt32(karticaIstekGodina.SelectedItem.ToString()),
                    MjesecIsteka = Convert.ToInt32(karticaIstekMjesec.SelectedItem.ToString()),
                    SigurnosniKod = karticaSigurnosniKod.Text
                };
            }
            else
            {
                narudzba.IsGotovina = true;
            }

            if (Uredi)
            {
                
                HttpResponseMessage putResponse = null;
                HttpResponseMessage putResponseStavke = null;
                try
                {
                    NarudzbePut tempN = new NarudzbePut {
                        PrioritetnaDostava = narudzba.PrioritetnaDostava,
                        kartica = narudzba.kartica,
                        AdresaDostave = narudzba.AdresaDostave,
                        Aktivna = narudzba.Aktivna,
                        Datum = narudzba.Datum,
                        IsGotovina = narudzba.IsGotovina,
                        KlijentID = narudzba.KlijentID,
                        NarudzbaID = narudzba.NarudzbaID
                    };

                    if (tempN.IsGotovina)
                        tempN.kartica = null;

                    List<NarudzbeStavkeEdit> listaStavke = narudzba.NarudzbeStavke.Select(x => new NarudzbeStavkeEdit
                    {
                        StavkaMenijaID = x.StavkaMenijaID,
                        NarudzbaStavkaID = x.NarudzbaStavkaID,
                        Kolicina = x.Kolicina,
                        Napomena = x.Napomena,
                        NarudzbaID = x.NarudzbaID
                    }).ToList();

                    putResponseStavke = narudzbeStavkeService.PutActionResponse(narudzbaID, "Uredi", listaStavke);

                    putResponse = narudzbeService.PutActionResponse(narudzbaID, "Uredi", tempN);
                    

                }
                catch(Exception exception)
                {
                    DisplayAlert("Greska", exception.Message, "Ok");
                }
                
                if (putResponse.IsSuccessStatusCode)
                {
                    
                    if (putResponseStavke.IsSuccessStatusCode)
                    {
                        DisplayAlert("Narudžba", "Narudžba je uspješno uređena.", "Ok");
                        Application.Current.MainPage = new NavigationPage(new Menu(new TrenutneNarudzbe()));
                    }
                    else
                    {
                        DisplayAlert("Korpa", "Došlo je do greške.", "Ok");
                    }
                }
                else if(putResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    DisplayAlert("Korpa", "Nije dozvoljeno plaćanje unesenom karticom.", "Ok");
                else
                    DisplayAlert("Korpa", "Došlo je do greške.", "Ok");
            }
            else
            {
                HttpResponseMessage response = narudzbeService.PostResponse(narudzba);
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Korpa", "Narudžba je uspješno dodata.", "Ok");
                    Global.narudzba = null;
                    Application.Current.MainPage = new NavigationPage(new Menu(new PregledArtikala()));
                }
                else
                    DisplayAlert("Korpa", "Došlo je do greške.", "Ok");
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)//za -
        {
            int stavkaId = Convert.ToInt32(e.Parameter);
            foreach (var item in narudzba.NarudzbeStavke)
            {
                if(item.StavkaMenijaID == stavkaId)
                {

                    if (item.Kolicina > 1)
                    {
                        item.Kolicina--;
                        FillStavke();
                    }
                    else if (item.Kolicina == 1)
                    {
                        narudzba.NarudzbeStavke.Remove(item);
                        FillStavke();
                    }
                    return;
                }
            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)//za +
        {
            int stavkaId = Convert.ToInt32(e.Parameter);
            foreach (var item in narudzba.NarudzbeStavke)
            {
                if (item.StavkaMenijaID == stavkaId)
                {
                    if (item.Kolicina < 50)
                    {
                        item.Kolicina++;
                        FillStavke();
                    }
                    return;
                }
            }
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var s = sender as Switch;
            if (s.IsToggled)
            {
                adresa.Text = Global.prijavljeniKlijent.Adresa;
                adresa.IsEnabled = false;
            }
            else
            {
                adresa.Text = "";
                adresa.IsEnabled = true;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if(Uredi)
                Application.Current.MainPage = new NavigationPage(new Menu(new TrenutneNarudzbe()));
            else
                Application.Current.MainPage = new NavigationPage(new Menu(new PregledArtikala()));
            return base.OnBackButtonPressed();
        }

        private void BrisiNarudzbu_Clicked(object sender, EventArgs e)
        {
            HttpResponseMessage deleteResponseMessage = narudzbeService.DeleteResponse(narudzbaID.ToString());
            Application.Current.MainPage = new NavigationPage(new Menu(new TrenutneNarudzbe("delete")));
        }
    }
}