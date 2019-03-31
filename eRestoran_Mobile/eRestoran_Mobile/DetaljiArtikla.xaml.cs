using eRestoran_PCL.Model;
using eRestoran_PCL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetaljiArtikla : ContentPage
	{
        //private WebAPIHelper ocjeneService = new WebAPIHelper("http://192.168.93.1", "api/Ocjene");
        //private WebAPIHelper stavkeMenijaService = new WebAPIHelper("http://192.168.93.1", "api/StavkeMenija");
        private WebAPIHelper ocjeneService = new WebAPIHelper("http://localhost:49327/", "api/Ocjene");
        private WebAPIHelper stavkeMenijaService = new WebAPIHelper("http://localhost:49327/", "api/StavkeMenija");
        int kolicinaArtikla = 1;
        int ratingArtikla = 0;
        List<StavkeMenijaPrikaz> preporuceni;
        public StavkeMenija artikal;
        public DetaljiArtikla()
        {
            InitializeComponent();
        }

        public DetaljiArtikla(StavkeMenija obj)
        {
            InitializeComponent();
            artikal = obj;
        }

        protected override void OnAppearing()
        {
            HttpResponseMessage response = ocjeneService.GetResponse("Rating?klijentId="+ Global.prijavljeniKlijent.KlijentID.ToString() +"&stavkaId="+ artikal.StavkaMenijaID.ToString());
            var jsonObject = response.Content.ReadAsStringAsync();
            OcjeneVM ocjena = JsonConvert.DeserializeObject<OcjeneVM>(jsonObject.Result);
            if (ocjena != null)
                rating(ocjena.Ocjena.ToString());
            else
                rating("0");

            HttpResponseMessage response2 = stavkeMenijaService.GetActionResponse("Recommender",artikal.StavkaMenijaID.ToString());
            jsonObject = response2.Content.ReadAsStringAsync();
            preporuceni = JsonConvert.DeserializeObject<List<StavkeMenijaPrikaz>>(jsonObject.Result);

            if (preporuceni.Count == 0)
            {
                preporucenoTitle.Text = "   ";
                preporucenoContent.IsVisible = false;
                DodajUKorpu.Margin = new Thickness(0,20,0,0);
            }
            else
            {
                DodajUKorpu.Margin = new Thickness(0, 0, 0, 0);
                preporucenoContent.IsVisible = true;
                preporucenoTitle.Text = "Preporučeno za Vas";

                foreach (var item in preporuceni)
                {
                    if (item.Slika != null)
                    {
                        if (slikaPredlozeni1.Source == null)
                        {
                            slikaPredlozeni1.Source = ImageSource.FromStream(() => new MemoryStream(item.Slika));
                            nazivPredlozeni1.Text = item.Naziv;
                        }
                        else if (slikaPredlozeni2.Source == null)
                        {
                            slikaPredlozeni2.Source = ImageSource.FromStream(() => new MemoryStream(item.Slika));
                            nazivPredlozeni2.Text = item.Naziv;
                        }
                        else
                        {
                            slikaPredlozeni3.Source = ImageSource.FromStream(() => new MemoryStream(item.Slika));
                            nazivPredlozeni3.Text = item.Naziv;
                        }
                    }
                }
            }
            if (artikal.SlikaThumb != null)
                slikaArtikla.Source = ImageSource.FromStream(() => new MemoryStream(artikal.SlikaThumb));


            nazivText.Text = artikal.Naziv;
            opisText.Text = artikal.Opis;
            cijenaText.Text = artikal.Cijena.ToString("0.##") + " KM";
            if(artikal.SlikaThumb != null)
                slikaArtikla.Source = ImageSource.FromStream(() => new MemoryStream(artikal.SlikaThumb));

            var tapGestureRecognizerMinus = new TapGestureRecognizer();
            tapGestureRecognizerMinus.Tapped += (s, e) => {
                changeKolicina("-");
            };
            minusKolicina.GestureRecognizers.Add(tapGestureRecognizerMinus);

            var tapGestureRecognizerPlus = new TapGestureRecognizer();
            tapGestureRecognizerPlus.Tapped += (s, e) => {
                changeKolicina("+");
            };
            plusKolicina.GestureRecognizers.Add(tapGestureRecognizerPlus);

            var tapGestureRecognizerStar1 = new TapGestureRecognizer();
            tapGestureRecognizerStar1.Tapped += (s, e) => {
                updateRating("1");
            };
            star1.GestureRecognizers.Add(tapGestureRecognizerStar1);

            var tapGestureRecognizerStar2 = new TapGestureRecognizer();
            tapGestureRecognizerStar2.Tapped += (s, e) => {
                updateRating("2");
            };
            star2.GestureRecognizers.Add(tapGestureRecognizerStar2);

            var tapGestureRecognizerStar3 = new TapGestureRecognizer();
            tapGestureRecognizerStar3.Tapped += (s, e) => {
                updateRating("3");
            };
            star3.GestureRecognizers.Add(tapGestureRecognizerStar3);

            var tapGestureRecognizerStar4 = new TapGestureRecognizer();
            tapGestureRecognizerStar4.Tapped += (s, e) => {
                updateRating("4");
            };
            star4.GestureRecognizers.Add(tapGestureRecognizerStar4);

            var tapGestureRecognizerStar5 = new TapGestureRecognizer();
            tapGestureRecognizerStar5.Tapped += (s, e) => {
                updateRating("5");
            };
            star5.GestureRecognizers.Add(tapGestureRecognizerStar5);

            //var tapGestureRecognizerFacebook = new TapGestureRecognizer();
            //tapGestureRecognizerFacebook.Tapped += async (s, e) => {
            //    var action = await DisplayAlert("Share", "Share this product on Facebook?", "Yes", "No");
            //    if (action)
            //    {
            //        await DisplayAlert("Share", "You shared " + artikal.Naziv + " on your Facebook feed.", "Ok");
            //    }
            //};
            //facebook.GestureRecognizers.Add(tapGestureRecognizerFacebook);

            //var tapGestureRecognizerInstagram = new TapGestureRecognizer();
            //tapGestureRecognizerInstagram.Tapped += async (s, e) => {
            //    var action = await DisplayAlert("Share", "Share this product on Instagram?", "Yes", "No");
            //    if (action)
            //    {
            //        await DisplayAlert("Share", "You shared " + artikal.Naziv + " on your Instagram feed.", "Ok");
            //    }
            //};
            //instagram.GestureRecognizers.Add(tapGestureRecognizerInstagram);

            //var tapGestureRecognizerTwitter = new TapGestureRecognizer();
            //tapGestureRecognizerTwitter.Tapped += async (s, e) => {
            //    var action = await DisplayAlert("Share", "Share this product on Twitter?", "Yes", "No");
            //    if (action)
            //    {
            //        await DisplayAlert("Share", "You tweeted " + artikal.Naziv + " on your feed.", "Ok");
            //    }
            //};
            //twitter.GestureRecognizers.Add(tapGestureRecognizerTwitter);

            //var tapGestureRecognizerGoogleplus = new TapGestureRecognizer();
            //tapGestureRecognizerGoogleplus.Tapped += async (s, e) => {
            //    var action = await DisplayAlert("Share", "Share this product on Google+?", "Yes", "No");
            //    if (action)
            //    {
            //        await DisplayAlert("Share", "You shared " + artikal.Naziv + " on your Google+ feed.", "Ok");
            //    }
            //};
            //googleplus.GestureRecognizers.Add(tapGestureRecognizerGoogleplus);

            //var tapGestureRecognizerTumblr = new TapGestureRecognizer();
            //tapGestureRecognizerTumblr.Tapped += async (s, e) => {
            //    var action = await DisplayAlert("Share", "Share this product on Tumblr?", "Yes", "No");
            //    if (action)
            //    {
            //        await DisplayAlert("Share", "You shared " + artikal.Naziv + " on your Tumblr feed.", "Ok");
            //    }
            //};
            //tumblr.GestureRecognizers.Add(tapGestureRecognizerTumblr);

            var tapGestureRecognizerPredlozeni1 = new TapGestureRecognizer();
            tapGestureRecognizerPredlozeni1.Tapped += async (s, e) => {
                StavkeMenija stavka = getStavka(0);
                if(stavka != null)
                    await Navigation.PushAsync(new DetaljiArtikla(stavka));
            };
            predlozeni1.GestureRecognizers.Add(tapGestureRecognizerPredlozeni1);

            var tapGestureRecognizerPredlozeni2 = new TapGestureRecognizer();
            tapGestureRecognizerPredlozeni2.Tapped += async (s, e) => {
                StavkeMenija stavka = getStavka(1);
                if (stavka != null)
                    await Navigation.PushAsync(new DetaljiArtikla(stavka));
            };
            predlozeni2.GestureRecognizers.Add(tapGestureRecognizerPredlozeni2);

            var tapGestureRecognizerPredlozeni3 = new TapGestureRecognizer();
            tapGestureRecognizerPredlozeni3.Tapped += async (s, e) => {
                StavkeMenija stavka = getStavka(2);
                if (stavka != null)
                    await Navigation.PushAsync(new DetaljiArtikla(stavka));
            };
            predlozeni3.GestureRecognizers.Add(tapGestureRecognizerPredlozeni3);

            base.OnAppearing();
        }

        private StavkeMenija getStavka(int i)
        {
            HttpResponseMessage response = stavkeMenijaService.GetActionResponse("GetById", preporuceni[i].StavkaMenijaID.ToString());
            var jsonObject = response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StavkeMenija>(jsonObject.Result);

        }

        private void updateRating(string parameter)
        {
            HttpResponseMessage response;
            OcjeneVM ocj = new OcjeneVM()
            {
                KlijentID = Global.prijavljeniKlijent.KlijentID,
                StavkaMenijaID = artikal.StavkaMenijaID,
                Ocjena = Convert.ToInt32(parameter)
            };
            if (ratingArtikla <= 0)
                response = ocjeneService.PostResponse(ocj);
            else
                response = ocjeneService.PutResponse(ocj);
            rating(parameter);
        }

        private void rating(string parameter)
        {
            switch (parameter)
            {
                case "1":
                    star1.Source = "star.png";
                    star2.Source = "star_gray.png";
                    star3.Source = "star_gray.png";
                    star4.Source = "star_gray.png";
                    star5.Source = "star_gray.png";
                    break;
                case "2":
                    star1.Source = "star.png";
                    star2.Source = "star.png";
                    star3.Source = "star_gray.png";
                    star4.Source = "star_gray.png";
                    star5.Source = "star_gray.png";
                    break;
                case "3":
                    star1.Source = "star.png";
                    star2.Source = "star.png";
                    star3.Source = "star.png";
                    star4.Source = "star_gray.png";
                    star5.Source = "star_gray.png";
                    break;
                case "4":
                    star1.Source = "star.png";
                    star2.Source = "star.png";
                    star3.Source = "star.png";
                    star4.Source = "star.png";
                    star5.Source = "star_gray.png";
                    break;
                case "5":
                    star1.Source = "star.png";
                    star2.Source = "star.png";
                    star3.Source = "star.png";
                    star4.Source = "star.png";
                    star5.Source = "star.png";
                    break;
                case "0":
                    star1.Source = "star_gray.png";
                    star2.Source = "star_gray.png";
                    star3.Source = "star_gray.png";
                    star4.Source = "star_gray.png";
                    star5.Source = "star_gray.png";
                    break;
                default:
                    break;
            }

            ratingArtikla = Convert.ToInt32(parameter);
        }

        private void changeKolicina(string parameter)
        {
            if (parameter == "-")
            {
                if (kolicinaArtikla != 1)
                    kolicinaArtikla--;
            }
            else if (parameter == "+")
            {
                if (kolicinaArtikla < 50)
                    kolicinaArtikla++;
            }
            kolicina.Text = kolicinaArtikla.ToString();    
        }

        private void DodajUKorpu_Clicked(object sender, EventArgs e)
        {
            bool postojiArtikal = false;

            foreach (var item in Global.narudzba.NarudzbeStavke)
            {
                if(item.StavkaMenijaID == artikal.StavkaMenijaID)
                {
                    postojiArtikal = true;
                    item.Kolicina += Convert.ToInt32(kolicinaArtikla);
                    DisplayAlert("Uspjeh", "Uspješno ste izmijenili količinu artikla u korpi.", "Ok");

                    break;
                }
            }

            if (!postojiArtikal)
            {
                NarudzbeStavke stavka = new NarudzbeStavke()
                {
                    StavkaMenijaID = artikal.StavkaMenijaID,
                    Kolicina = kolicinaArtikla,
                    Napomena = napomenaInput.Text,
                    Artikal = artikal
                };

                Global.narudzba.NarudzbeStavke.Add(stavka);

                DisplayAlert("Uspjeh", "Uspješno ste dodali artikal u korpu.", "Ok");
            }

            Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new NavigationPage(new Menu(new PregledArtikala()));
            return base.OnBackButtonPressed();
        }
    }
}