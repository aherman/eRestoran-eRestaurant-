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
    public partial class TipoviStavkeDodajForm : Form
    {
        private WebAPIHelper tipoviStavkeService = new WebAPIHelper("http://localhost:49327", "api/TipoviStavke");
        int tipStavkeID;
        public TipoviStavkeDodajForm(int id = 0)
        {
            if (id != 0)
                tipStavkeID = id;
            AutoValidate = AutoValidate.Disable;
            InitializeComponent();
        }

        private void BtnTipoviStavkeDodaj_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                TipoviStavke obj;
                if (tipStavkeID == 0)
                {
                    obj = new TipoviStavke();
                }
                else
                {
                    HttpResponseMessage tipoviStavkeResponse = tipoviStavkeService.GetActionResponse("GetById", tipStavkeID.ToString());
                    obj = tipoviStavkeResponse.Content.ReadAsAsync<TipoviStavke>().Result;
                }
                obj.Naziv = txtNaziv.Text;
                obj.Opis = txtOpis.Text;

                if (tipStavkeID == 0)
                {
                    HttpResponseMessage response = tipoviStavkeService.PostResponse(obj);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.add_itemtype_succ);
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
                    HttpResponseMessage response = tipoviStavkeService.PutResponse(tipStavkeID, obj);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.edit_itemtype_succ);
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

        private void TipoviStavkeDodajForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            if (tipStavkeID != 0)
            {
                fillIzmjena();
                btnTipoviStavkeDodaj.Text = "Izmijeni";
            }
        }

        private void fillIzmjena()
        {
            HttpResponseMessage response = tipoviStavkeService.GetActionResponse("GetById", tipStavkeID.ToString());
            TipoviStavke tipoviStavke = response.Content.ReadAsAsync<TipoviStavke>().Result;
            txtNaziv.Text = tipoviStavke.Naziv;
            txtOpis.Text = tipoviStavke.Opis;
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
    }
}
