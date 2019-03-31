using eRestoran_PCL.Model;
using eRestoran_PCL.Util;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PregledProfila : ContentPage
	{
        //private WebAPIHelper klijentiService = new WebAPIHelper("http://192.168.93.1", "api/Klijenti");
        private WebAPIHelper klijentiService = new WebAPIHelper("http://localhost:49327/", "api/Klijenti");
        bool izmjena = false;
        string errors;
        Klijenti klijent;
        public PregledProfila ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            LoadData();

            base.OnAppearing();
        }

        private void LoadData()
        { 
            HttpResponseMessage response = klijentiService.GetActionResponse("GetById", Global.prijavljeniKlijent.KlijentID.ToString());
            var jsonObject = response.Content.ReadAsStringAsync();
            klijent = JsonConvert.DeserializeObject<Klijenti>(jsonObject.Result);

            imeInput.IsEnabled = false;
            prezimeInput.IsEnabled = false;
            emailInput.IsEnabled = false;
            lozinkaInput.IsEnabled = false;
            adresaInput.IsEnabled = false;
            telefonInput.IsEnabled = false;

            imeInput.Text = klijent.Ime;
            prezimeInput.Text = klijent.Prezime;
            emailInput.Text = klijent.Email;
            lozinkaInput.Text = "********";
            adresaInput.Text = klijent.Adresa;
            telefonInput.Text = klijent.Telefon;
        }

        private void izmjenaButton_Clicked(object sender, EventArgs e)
        {
            if (!izmjena)
            {
                izmjenaButton.Text = "Potvrdi";
                izmjenaButton.BackgroundColor = Color.FromHex("#f77979");

                imeInput.IsEnabled = true;
                prezimeInput.IsEnabled = true;
                emailInput.IsEnabled = true;
                lozinkaInput.IsEnabled = true;
                adresaInput.IsEnabled = true;
                telefonInput.IsEnabled = true;

                lozinkaInput.Text = "";
                lozinkaInput.Placeholder = "Unesi lozinku ili ostavi prazno";
                izmjena = true;
            }
            else
            {
                if(IsValidated())
                {
                    Klijenti k = new Klijenti
                    {
                        Adresa = adresaInput.Text,
                        Ime = imeInput.Text,
                        Prezime = prezimeInput.Text,
                        Email = emailInput.Text,
                        Telefon = telefonInput.Text,
                        KorisnickoIme = klijent.KorisnickoIme,
                        Spol = klijent.Spol,
                        KlijentID = klijent.KlijentID
                    };

                    if(lozinkaInput.Text != "")
                    {
                        k.LozinkaSalt = UIHelper.GenerateSalt();
                        k.LozinkaHash = UIHelper.GenerateHash(lozinkaInput.Text, k.LozinkaSalt);
                    }
                    else
                    {
                        k.LozinkaSalt = klijent.LozinkaSalt;
                        k.LozinkaHash = klijent.LozinkaHash;
                    }

                    HttpResponseMessage response = klijentiService.PutResponse(k.KlijentID, k);
                    if (response.IsSuccessStatusCode)
                    {
                        izmjena = false;
                        DisplayAlert("Izmjena podataka", "Podaci su uspješno izmijenjeni.", "Ok");
                        LoadData();
                        izmjenaButton.Text = "Izmijeni";
                        izmjenaButton.BackgroundColor = Color.FromHex("#13818d");
                    }
                    else
                        DisplayAlert("Izmjena podataka", "Došlo je do greške.", "Ok");
                }
                else
                    DisplayAlert("Izmjena podataka", "Izmjena nije moguća. Greške:"
                        +System.Environment.NewLine + System.Environment.NewLine+errors, "Ok");
            }
        }



        private bool IsValidated()
        {
            errors = "";

            if (String.IsNullOrWhiteSpace(imeInput.Text))
                errors += "Polje ime ne smije biti prazno" + System.Environment.NewLine;

            if (String.IsNullOrWhiteSpace(prezimeInput.Text))
                errors += "Polje prezime ne smije biti prazno" + System.Environment.NewLine;

            if(!String.IsNullOrWhiteSpace(emailInput.Text) && !EmailValid(emailInput.Text))
                errors += "Email nije u ispravnom formatu" + System.Environment.NewLine;

            if(!String.IsNullOrWhiteSpace(lozinkaInput.Text) && lozinkaInput.Text.Length < 8)
                errors += "Unesena lozinka treba imati minimalno 8 znakova" + System.Environment.NewLine;

            if(!String.IsNullOrWhiteSpace(telefonInput.Text) && !Regex.Match(telefonInput.Text, "[0-9]{9}").Success)
                errors += "Broj telefona mora sadržavati 9 cifara od 0 do 9" + System.Environment.NewLine;

            if (errors == "")
                return true;
            return false;
        }

        public bool EmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new NavigationPage(new Menu(new PregledArtikala()));
            return base.OnBackButtonPressed();
        }
    }
}