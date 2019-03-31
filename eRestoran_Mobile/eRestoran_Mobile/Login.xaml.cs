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
	public partial class Login : ContentPage
	{

        //private WebAPIHelper klijentiService = new WebAPIHelper("http://192.168.93.1/", "api/Klijenti");
        //private WebAPIHelper zaposleniciService = new WebAPIHelper("http://192.168.93.1/", "api/Zaposlenici");
        private WebAPIHelper klijentiService = new WebAPIHelper("http://localhost:49327/", "api/Klijenti");
        private WebAPIHelper zaposleniciService = new WebAPIHelper("http://localhost:49327/", "api/Zaposlenici");

        public Login ()
		{
			InitializeComponent ();
            testPicker.Items.Add("klijent1");
            testPicker.Items.Add("klijent2");
            testPicker.Items.Add("dostavljac");
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registracija());
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            try
            {
                HttpResponseMessage response = klijentiService.GetActionResponse("GetByUsername", usernameInput.Text);
                HttpResponseMessage response2 = zaposleniciService.GetActionResponse("ByUsername", usernameInput.Text);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = response.Content.ReadAsStringAsync();
                    Klijenti k = JsonConvert.DeserializeObject<Klijenti>(jsonResult.Result);
                    try
                    {
                        if (k.LozinkaHash == UIHelper.GenerateHash(passwordInput.Text, k.LozinkaSalt))
                        {
                            Global.prijavljeniKlijent = k;
                            Navigation.PushAsync(new Menu(new PregledArtikala()));
                        }
                        else
                        {
                            DisplayAlert("Login", "Unijeli ste pogrešnu lozinku!", "OK");
                        }
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Login", "Unijeli ste pogrešnu lozinku!", "OK");
                    }
                }
                else if (response2.IsSuccessStatusCode)
                {
                    var jsonResult = response2.Content.ReadAsStringAsync();
                    Zaposlenici z = JsonConvert.DeserializeObject<Zaposlenici>(jsonResult.Result);
                    try
                    {
                        if (z.LozinkaHash == UIHelper.GenerateHash(passwordInput.Text, z.LozinkaSalt))
                        {
                            HttpResponseMessage response3 = zaposleniciService.GetActionResponse("IsDostavljac", z.ZaposlenikID.ToString());
                            if (!response3.IsSuccessStatusCode)
                            {
                                    DisplayAlert("Login", "Nemate pravo pristupa.", "OK");
                                    return;
                            }
                            Global.prijavljeniZaposlenik = z;
                            Application.Current.MainPage = new NavigationPage(new DostavaMenu());
                        }
                        else
                        {
                            DisplayAlert("Login", "Unijeli ste pogrešnu lozinku!", "OK");
                        }
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Login", "Unijeli ste pogrešnu lozinku!", "OK");
                    }
                }
                else
                    DisplayAlert("Login", "Niste unijeli ispravne podatke za prijavu!", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Login", ex.Message, "OK");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            HttpResponseMessage response;
            string selected = "";
            if (testPicker.SelectedIndex != -1)
                switch (testPicker.Items[testPicker.SelectedIndex])
                {
                    case "klijent1":
                        response = klijentiService.GetActionResponse("GetByUsername", "klijent");
                        var jsonResult1 = response.Content.ReadAsStringAsync();
                        Global.prijavljeniKlijent = JsonConvert.DeserializeObject<Klijenti>(jsonResult1.Result);
                        selected = "k";
                        break;
                    case "klijent2":
                        response = klijentiService.GetActionResponse("GetByUsername", "magi");
                        var jsonResult2 = response.Content.ReadAsStringAsync();
                        Global.prijavljeniKlijent = JsonConvert.DeserializeObject<Klijenti>(jsonResult2.Result);
                        selected = "k";
                        break;
                    case "dostavljac":
                        response = zaposleniciService.GetActionResponse("ByUsername", "dostavljac");
                        var jsonResult3 = response.Content.ReadAsStringAsync();
                        Global.prijavljeniZaposlenik = JsonConvert.DeserializeObject<Zaposlenici>(jsonResult3.Result);
                        selected = "z";
                        break;
                    default:
                        break;
                }
            if (selected == "z")
                Application.Current.MainPage = new NavigationPage(new DostavaMenu());
            else if (selected == "k")
                Navigation.PushAsync(new Menu(new PregledArtikala()));
            else
                DisplayAlert("Upozorenje", "Korisnik nije selektovan", "Ok");
        }
    }
}