using eRestoran_API.Models;
using eRestoran_UI.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eRestoran_UI
{
    public partial class PopustDodajForm : Form
    {
        private WebAPIHelper popustiService = new WebAPIHelper("http://localhost:49327", "api/Popusti");

        int popustID;
        public PopustDodajForm(int id = 0)
        {
            InitializeComponent();
            if (id != 0)
                popustID = id;
            AutoValidate = AutoValidate.Disable;
        }

        private void PopustDodajForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            if (popustID != 0)
            {
                fillIzmjena();
                btnPopustiDodaj.Text = "Izmijeni";
                dtpDatumPocetka.MinDate = DateTime.Now;
                dtpDatumZavrsetka.MinDate = DateTime.Now;
            }
        }

        private void fillIzmjena()
        {
            HttpResponseMessage response = popustiService.GetActionResponse("GetById", popustID.ToString());
            Popusti popusti = response.Content.ReadAsAsync<Popusti>().Result;
            txtNaziv.Text = popusti.Naziv;
            txtOpis.Text = popusti.Opis;
            txtIznos.Text = popusti.Iznos.ToString();
            dtpDatumPocetka.Value = popusti.DatumPocetka;
            dtpDatumZavrsetka.Value = popusti.DatumZavrsetka;
        }

        private void btnPopustiDodaj_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                Popusti obj;
                if (popustID == 0)
                {
                    obj = new Popusti();
                }
                else
                {
                    HttpResponseMessage popustiResponse = popustiService.GetActionResponse("GetById", popustID.ToString());
                    obj = popustiResponse.Content.ReadAsAsync<Popusti>().Result;
                }
                obj.Naziv = txtNaziv.Text;
                obj.Opis = txtOpis.Text;
                obj.DatumPocetka = dtpDatumPocetka.Value;
                obj.DatumZavrsetka = dtpDatumZavrsetka.Value;
                obj.Iznos = Convert.ToDecimal(txtIznos.Text);

                if (popustID == 0)
                {
                    HttpResponseMessage response = popustiService.PostResponse(obj);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.add_disc_succ);
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
                    HttpResponseMessage response = popustiService.PutResponse(popustID, obj);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.edit_disc_succ);
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

        private void txtNaziv_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtNaziv.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtNaziv, Messages.title_req);
            }
            else
                errorProvider.SetError(txtNaziv, null);
        }

        private void dtpDatumPocetka_Validating(object sender, CancelEventArgs e)
        {
            if (dtpDatumPocetka.Value > dtpDatumZavrsetka.Value)
            {
                e.Cancel = true;
                errorProvider.SetError(dtpDatumPocetka, Messages.date_err);
            }
            else
                errorProvider.SetError(dtpDatumPocetka, null);
        }

        private void dtpDatumZavrsetka_Validating(object sender, CancelEventArgs e)
        {
        }

        private void txtIznos_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtIznos.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtIznos, Messages.val_req);
            }
            else if (Convert.ToDecimal(txtIznos.Text) <= 0 || Convert.ToDecimal(txtIznos.Text) >= 100)
            {
                e.Cancel = true;
                errorProvider.SetError(txtIznos, Messages.val_err);
            }
            else
                errorProvider.SetError(txtIznos, null);
        }
    }
}
