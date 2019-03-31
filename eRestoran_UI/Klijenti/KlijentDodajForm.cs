using eRestoran_API.Models;
using eRestoran_UI.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eRestoran_UI
{
    public partial class KlijentDodajForm : Form
    {
        private WebAPIHelper klijentiService = new WebAPIHelper("http://localhost:49327", "api/Klijenti");
        int klijentID = 0;
        string myHash, mySalt = "";
        public KlijentDodajForm(int id = 0)
        {
            InitializeComponent();
            if(id != 0)
            {
                klijentID = id;
            }
            AutoValidate = AutoValidate.Disable;
        }

        private void KlijentDodajForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            fillCmbSpolovi();
            txtBrojTelefona.AutoSize = false;
            txtBrojTelefona.Height = 25;
            if (klijentID != 0)
            {
                fillIzmjena();
                btnKlijentDodaj.Text = "Izmijeni";
            }
        }

        private void fillIzmjena()
        {
            HttpResponseMessage response = klijentiService.GetActionResponse("GetById", klijentID.ToString());
            Klijenti klijent = response.Content.ReadAsAsync<Klijenti>().Result;
            txtIme.Text = klijent.Ime;
            txtPrezime.Text = klijent.Prezime;
            cmbSpol.Text = klijent.Spol;
            txtBrojTelefona.Text = klijent.Telefon;
            txtEmail.Text = klijent.Email;
            txtAdresa.Text = klijent.Adresa;
            txtUsername.Text = klijent.KorisnickoIme;
            myHash = klijent.LozinkaHash;
            mySalt = klijent.LozinkaSalt;
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

        private void btnKlijentDodaj_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                Klijenti k;
                if (klijentID == 0)
                {
                    k = new Klijenti();
                }
                else
                {
                    HttpResponseMessage kResponse = klijentiService.GetResponse(klijentID.ToString());
                    k = kResponse.Content.ReadAsAsync<Klijenti>().Result;
                }

                k.Ime = txtIme.Text;
                k.Prezime = txtPrezime.Text;
                k.Spol = cmbSpol.Text;
                k.Telefon = txtBrojTelefona.Text;
                k.Email = txtEmail.Text;
                k.Adresa = txtAdresa.Text;
                k.KorisnickoIme = txtUsername.Text;

                if(klijentID != 0)
                {
                    if (!string.IsNullOrEmpty(txtLozinka.Text))
                    {
                        k.LozinkaSalt = UIHelper.GenerateSalt();
                        k.LozinkaHash = UIHelper.GenerateHash(txtLozinka.Text, k.LozinkaSalt);
                    }
                    else
                    {
                        k.LozinkaSalt = mySalt;
                        k.LozinkaHash = myHash;
                    }
                }
                else
                {
                    k.LozinkaSalt = UIHelper.GenerateSalt();
                    k.LozinkaHash = UIHelper.GenerateHash(txtLozinka.Text, k.LozinkaSalt);
                }
                

                if (klijentID == 0)
                {
                    HttpResponseMessage response = klijentiService.PostResponse(k);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.add_client_succ);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Error code: " + response.StatusCode + " Message: " + response.RequestMessage);
                    }
                }
                else
                {
                    HttpResponseMessage response = klijentiService.PutResponse(klijentID, k);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.edit_client_succ);
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
            else if (txtUsername.Text.Count() < 3)
            {
                e.Cancel = true;
                errorProvider.SetError(txtUsername, Messages.user_err_length);
            }
            else if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtUsername, Messages.user_err_space);
            }
            else if(UsernameVecPostoji(txtUsername.Text, txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtUsername, Messages.user_con);
            }
            else
                errorProvider.SetError(txtUsername, null);
        }

        private bool UsernameVecPostoji(string username, string email)
        {
            HttpResponseMessage response = klijentiService.GetActionResponse("GetByUsername", username);
            Klijenti k = response.Content.ReadAsAsync<Klijenti>().Result;
            if (k != null)
            {
                if (k.KorisnickoIme == username && k.Email == email)
                    return false;
                return true;
            }
            else
                return false;
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBrojTelefona.Text)){
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
                if (ispravno)
                    errorProvider.SetError(txtEmail, null);
            }
        }

        private void txtLozinka_Validating(object sender, CancelEventArgs e)
        {
            if (klijentID != 0 && String.IsNullOrEmpty(txtLozinka.Text))
                return;

            if (String.IsNullOrEmpty(txtLozinka.Text))
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
            if (klijentID != 0 && String.IsNullOrEmpty(txtLozinka.Text))
                return;

            if (String.IsNullOrEmpty(txtPotvrdiLozinku.Text))
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

        private void txtBrojTelefona_Validating_1(object sender, CancelEventArgs e)
        {
            if (!Regex.Match(txtBrojTelefona.Text, "[0-9]{9}").Success && !String.IsNullOrEmpty(txtBrojTelefona.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtBrojTelefona, Messages.phnum_reg);
            }
            else
                errorProvider.SetError(txtBrojTelefona, null);
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
