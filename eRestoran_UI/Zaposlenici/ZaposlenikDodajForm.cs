using eRestoran_API.Models;
using eRestoran_UI.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eRestoran_UI
{
    public partial class ZaposlenikDodajForm : Form
    {
        private WebAPIHelper zaposleniciService = new WebAPIHelper("http://localhost:49327", "api/Zaposlenici");
        private WebAPIHelper ulogeService = new WebAPIHelper("http://localhost:49327", "api/Uloge");
        int zaposlenikID = 0;
        public ZaposlenikDodajForm(int id = 0)
        {
            InitializeComponent();
            if (id != 0)
                zaposlenikID = id;
            AutoValidate = AutoValidate.Disable;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            txtBrojTelefona.AutoSize = false;
            txtBrojTelefona.Height = 25;
            fillCmbUloge();
            fillCmbSpolovi();

            if (zaposlenikID != 0)
            {
                fillIzmjena();
                btnKorisnikDodaj.Text = "Izmijeni";
            }
        }

        private void fillIzmjena()
        {
            HttpResponseMessage response = zaposleniciService.GetActionResponse("GetById", zaposlenikID.ToString());
            Zaposlenici zaposlenik = response.Content.ReadAsAsync<Zaposlenici>().Result;
            txtIme.Text = zaposlenik.Ime;
            txtPrezime.Text = zaposlenik.Prezime;
            cmbSpol.Text = zaposlenik.Spol;
            txtBrojTelefona.Text = zaposlenik.Telefon;
            txtEmail.Text = zaposlenik.Email;
            txtStaz.Text = zaposlenik.Staz.ToString();
            txtUsername.Text = zaposlenik.KorisnickoIme;
            for (int i = 0; i < clbUloge.Items.Count; i++)
            {
                foreach (var item in zaposlenik.ZaposleniciUloge)
                {
                    if (item.Uloge.UlogaID == ((Uloge)clbUloge.Items[i]).UlogaID)
                        clbUloge.SetItemChecked(i, true);
                }
            }
        }

        private void fillCmbUloge()
        {
            HttpResponseMessage response = ulogeService.GetResponse();

            if (response.IsSuccessStatusCode)
            {
                List<Uloge> uloge = response.Content.ReadAsAsync<List<Uloge>>().Result;

                clbUloge.DataSource = uloge;
                clbUloge.DisplayMember = "Naziv";
                clbUloge.ValueMember = "UlogaID";
                clbUloge.ClearSelected();

            }
        }

        private void fillCmbSpolovi()
        {
            var spolovi = new List<String>();
            spolovi.Add("Muško");
            spolovi.Add("Žensko");
            spolovi.Add("Neodređeno");
            cmbSpol.DataSource = spolovi;
            cmbSpol.SelectedItem = null;
        }

        private void btnKorisnikDodaj_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                Zaposlenici z;
                if (zaposlenikID == 0)
                {
                    z = new Zaposlenici();
                }
                else
                {
                    HttpResponseMessage zResponse = zaposleniciService.GetActionResponse("GetById", zaposlenikID.ToString());
                    z = zResponse.Content.ReadAsAsync<Zaposlenici>().Result;
                }

                z.Ime = txtIme.Text;
                z.Prezime = txtPrezime.Text;
                z.Spol = cmbSpol.Text;
                z.Telefon = txtBrojTelefona.Text;
                z.Email = txtEmail.Text;
                z.Staz = Convert.ToInt32(txtStaz.Text);
                z.KorisnickoIme = txtUsername.Text;

                if (!string.IsNullOrEmpty(txtLozinka.Text)){
                    z.LozinkaSalt = UIHelper.GenerateSalt();
                    z.LozinkaHash = UIHelper.GenerateHash(txtLozinka.Text, z.LozinkaSalt);
                }
               
                z.uloge = clbUloge.CheckedItems.Cast<Uloge>().ToList();

                if (zaposlenikID == 0)
                {
                    HttpResponseMessage response = zaposleniciService.PostResponse(z);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.add_usr_succ);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        string err_msg = response.ReasonPhrase;

                        if (!string.IsNullOrEmpty(Messages.ResourceManager.GetString(response.ReasonPhrase)))
                            err_msg = Messages.ResourceManager.GetString(response.ReasonPhrase);

                        MessageBox.Show("Error code: " + response.StatusCode + " Message: " + err_msg);
                    }
                }
                else
                {
                    Zaposlenici putZaposlenik = z;
                    HttpResponseMessage response = zaposleniciService.PutResponse(zaposlenikID, putZaposlenik);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.edit_usr_succ);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Error code: " + response.StatusCode + " Message: " + response.RequestMessage);
                    }
                }
            }
        }

        private void txtIme_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtIme.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtIme, Messages.name_req);
            }
            else
                errorProvider.SetError(txtIme, null);
        }

        private void txtPrezime_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPrezime.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtPrezime, Messages.sname_req);
            }
            else
                errorProvider.SetError(txtPrezime, null);
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtUsername, Messages.user_req);
            }
            else if(txtUsername.Text.Count() < 3)
            {
                e.Cancel = true;
                errorProvider.SetError(txtUsername, Messages.user_err_length);
            }
            else if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtUsername, Messages.user_err_space);
            }
            else if (UsernameVecPostoji(txtUsername.Text, txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtUsername, Messages.user_con);
            }
            else
                errorProvider.SetError(txtUsername, null);
        }

        private bool UsernameVecPostoji(string username, string email)
        {
            HttpResponseMessage response = zaposleniciService.GetActionResponse("ByUsername", username);
            Zaposlenici z = response.Content.ReadAsAsync<Zaposlenici>().Result;
            if (z != null)
            {
                if (z.KorisnickoIme == username && z.Email == email)
                    return false;
                return true;
            }
            else
                return false;
        }

        private void clbUloge_Validating(object sender, CancelEventArgs e)
        {
            if (clbUloge.CheckedItems.Count == 0)
            {
                e.Cancel = true;
                errorProvider.SetError(clbUloge, Messages.user_role_err);
            }
            else
                errorProvider.SetError(clbUloge, null);
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtEmail, Messages.email_req);
            }
            else
            {
                bool ispravno = true;
                try
                {
                    MailAddress mail = new MailAddress(txtEmail.Text);
                }
                catch (Exception)
                {
                    e.Cancel = true;
                    errorProvider.SetError(txtEmail, Messages.email_format_err);
                    ispravno = false;
                }
                if(ispravno)
                    errorProvider.SetError(txtEmail, null);
            }
                
        }

        private void txtLozinka_Validating(object sender, CancelEventArgs e)
        {
            if (zaposlenikID != 0 && string.IsNullOrEmpty(txtLozinka.Text))
                return;

            if (string.IsNullOrEmpty(txtLozinka.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtLozinka, Messages.user_pass_req);
            }
            else if (txtLozinka.Text.Length < 8)
            {
                e.Cancel = true;
                errorProvider.SetError(txtLozinka, Messages.user_pass_format_err);
            }
            else
                errorProvider.SetError(txtLozinka, null);
        }

        private void txtPotvrdiLozinku_Validating(object sender, CancelEventArgs e)
        {
            if (zaposlenikID != 0 && string.IsNullOrEmpty(txtLozinka.Text))
                return;

            if (string.IsNullOrEmpty(txtPotvrdiLozinku.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtPotvrdiLozinku, Messages.user_passconf_req);
            }
            else if (txtLozinka.Text != txtPotvrdiLozinku.Text)
            {
                e.Cancel = true;
                errorProvider.SetError(txtPotvrdiLozinku, Messages.user_passconf_err);
            }
            else
                errorProvider.SetError(txtPotvrdiLozinku, null);
        }

        private void txtBrojTelefona_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.Match(txtBrojTelefona.Text, "[0-9]{9}").Success && !string.IsNullOrEmpty(txtBrojTelefona.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtBrojTelefona, Messages.phnum_reg);
            }
            else
                errorProvider.SetError(txtBrojTelefona, null);
        }

        private void txtStaz_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStaz.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtStaz, Messages.exp_req);
            }
            else if (Convert.ToInt32(txtStaz.Text) < 0 || Convert.ToInt32(txtStaz.Text) > 50)
            {
                e.Cancel = true;
                errorProvider.SetError(txtStaz, Messages.exp_err);
            }
            else
                errorProvider.SetError(txtStaz, null);
        }

        private void txtStaz_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cmbSpol_Validating(object sender, CancelEventArgs e)
        {
            if (cmbSpol.SelectedItem == null)
            {
                e.Cancel = true;
                errorProvider.SetError(cmbSpol, Messages.cmb_req);
            }
            else
                errorProvider.SetError(cmbSpol, null);
        }
    }
}
