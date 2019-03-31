using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using eRestoran_PCL.Model;
using eRestoran_PCL.Util;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eRestoran_Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Registracija : ContentPage
	{
        //private WebAPIHelper klijentiService = new WebAPIHelper("http://192.168.93.1", "api/Klijenti");
        private WebAPIHelper klijentiService = new WebAPIHelper("http://localhost:49327/", "api/Klijenti");
        
        string errors = "";
        public Registracija ()
		{
			InitializeComponent ();
        }

        private void registracijaButton_Clicked(object sender, EventArgs e)
        {
            

            if (!IsValidated())
            {
                DisplayAlert("Registracija", "Uneseni podaci nisu validni. Greške:"
                        + System.Environment.NewLine + System.Environment.NewLine + errors, "Ok");
                return;
            }


            HttpResponseMessage response;

            try
            {
                Klijenti k = new Klijenti();
                k.Ime = imeInput.Text;
                k.Prezime = prezimeInput.Text;
                k.KorisnickoIme = korisnickoImeInput.Text;
                k.Spol = spolInput.SelectedItem.ToString();
                k.Telefon = telefonInput.Text;
                k.Adresa = adresaInput.Text;
                k.Email = emailInput.Text;
                k.LozinkaSalt = UIHelper.GenerateSalt();
                k.LozinkaHash = UIHelper.GenerateHash(lozinkaInput.Text, k.LozinkaSalt);
                response = klijentiService.PostResponse(k);
            }
            catch (Exception)
            {
                throw;
            }

            if (response.IsSuccessStatusCode)
            {
                DisplayAlert("Registracija", "Uspješna registracija", "Ok");
                Navigation.PopAsync();
            }
            else
                DisplayAlert("Registracija", "Došlo je do greške.", "Ok");

        }

        private bool IsValidated()
        {
            errors = "";

            if (String.IsNullOrWhiteSpace(imeInput.Text))
                errors += "Polje ime ne smije biti prazno" + System.Environment.NewLine;

            if (String.IsNullOrWhiteSpace(prezimeInput.Text))
                errors += "Polje prezime ne smije biti prazno" + System.Environment.NewLine;

            if (!String.IsNullOrWhiteSpace(emailInput.Text) && !EmailValid(emailInput.Text))
                errors += "Email nije u ispravnom formatu" + System.Environment.NewLine;

            if (String.IsNullOrWhiteSpace(lozinkaInput.Text))
                errors += "Polje lozinka ne smije biti prazno" + System.Environment.NewLine;

            if (!String.IsNullOrWhiteSpace(lozinkaInput.Text) && lozinkaInput.Text.Length < 8)
                errors += "Unesena lozinka treba imati minimalno 8 znakova" + System.Environment.NewLine;

            if (!String.IsNullOrWhiteSpace(telefonInput.Text) && !Regex.Match(telefonInput.Text, "[0-9]{9}").Success)
                errors += "Broj telefona mora sadržavati 9 cifara od 0 do 9" + System.Environment.NewLine;

            HttpResponseMessage getResponse = klijentiService.GetActionResponse("GetByUsername", korisnickoImeInput.Text);
            var jsonObject = getResponse.Content.ReadAsStringAsync();
            Klijenti temp = JsonConvert.DeserializeObject<Klijenti>(jsonObject.Result);

            if (temp != null && !String.IsNullOrWhiteSpace(korisnickoImeInput.Text))
            {
                errors += "Korisničko ime već postoji"+System.Environment.NewLine;
            }

            if (String.IsNullOrWhiteSpace(korisnickoImeInput.Text))
                errors += "Polje korisničko ime ne smije biti prazno" + System.Environment.NewLine;

            if(spolInput.SelectedIndex == -1)
                errors += "Spol nije odabran" + System.Environment.NewLine;

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
    }

}