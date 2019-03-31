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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eRestoran_UI
{
    public partial class ArtikalDodajForm : Form
    {
        private WebAPIHelper stavkeMenijaService = new WebAPIHelper("http://localhost:49327", "api/StavkeMenija");
        private WebAPIHelper tipoviStavkeService = new WebAPIHelper("http://localhost:49327", "api/TipoviStavke");

        private int stavkeMenijaID = 0;
        byte[] slika;
        byte[] slikaThumb;

        public ArtikalDodajForm(int id = 0)
        {
            InitializeComponent();
            if (id != 0)
                stavkeMenijaID = id;
            this.AutoValidate = AutoValidate.Disable;
        }

        private void btnStavkeMenijaDodaj_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                StavkeMenija sm;
                if (stavkeMenijaID == 0)
                {
                    sm = new StavkeMenija();
                }
                else
                {
                    HttpResponseMessage stavkeMenijaResponse = stavkeMenijaService.GetActionResponse("GetById", stavkeMenijaID.ToString());
                    sm = stavkeMenijaResponse.Content.ReadAsAsync<StavkeMenija>().Result;
                }

                sm.Naziv = txtNaziv.Text;
                sm.Opis = txtOpis.Text;
                sm.Cijena = Convert.ToDecimal(txtCijena.Text);
                sm.TipStavkeID = Convert.ToInt32(cmbKategorija.SelectedValue);
                sm.Sifra = txtSifra.Text;
                sm.Status = cbStatus.Checked;
                sm.Slika = slika;
                sm.SlikaThumb = slikaThumb;

                if (stavkeMenijaID == 0)
                {
                    HttpResponseMessage response = stavkeMenijaService.PostResponse(sm);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.add_item_succ);
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
                    HttpResponseMessage response = stavkeMenijaService.PutResponse(stavkeMenijaID, sm);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.edit_item_succ);
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

        private void ArtikalDodajForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            if (!Global.IsAdmin())
                btnKategorijaDodaj.Visible = false;
            FillCmbTipoviStavke();
 
            if (stavkeMenijaID != 0)
            {
                fillIzmjena();
                btnStavkeMenijaDodaj.Text = "Izmijeni";
            }
        }

        private void fillIzmjena()
        {
            HttpResponseMessage response = stavkeMenijaService.GetActionResponse("GetById", stavkeMenijaID.ToString());
            StavkeMenija stavkaMenija = response.Content.ReadAsAsync<StavkeMenija>().Result;
            txtNaziv.Text = stavkaMenija.Naziv;
            txtOpis.Text = stavkaMenija.Opis;
            txtSifra.Text = stavkaMenija.Sifra;
            txtCijena.Text = stavkaMenija.Cijena.ToString();
            cbStatus.Checked = stavkaMenija.Status;
            cmbKategorija.SelectedValue = stavkaMenija.TipStavkeID;
            if(stavkaMenija.Slika != null)
            {
                pbSlika.Image = UIHelper.byteArrayToImage(stavkaMenija.SlikaThumb);
                slika = stavkaMenija.Slika;
                slikaThumb = stavkaMenija.SlikaThumb;
            }
                

        }

        private void FillCmbTipoviStavke()
        {
            HttpResponseMessage response = tipoviStavkeService.GetResponse();

            if (response.IsSuccessStatusCode)
            {
                List<TipoviStavke> tipoviStavke = response.Content.ReadAsAsync<List<TipoviStavke>>().Result;

                cmbKategorija.DataSource = tipoviStavke;
                cmbKategorija.DisplayMember = "Naziv";
                cmbKategorija.ValueMember = "TipStavkeID";
                cmbKategorija.SelectedItem = null;

            }
        }

        private void btnKategorijaDodaj_Click(object sender, EventArgs e)
        {
            TipoviStavkeDodajForm dodajForm = new TipoviStavkeDodajForm();
            if (dodajForm.ShowDialog() == DialogResult.OK)
            {
                FillCmbTipoviStavke();
            }
        }

        private void btnSlikaDodaj_Click(object sender, EventArgs e)
        {
            try
            {

                openFileDialog.ShowDialog();
                txtSlika.Text = openFileDialog.FileName;

                Image orgImg = Image.FromFile(txtSlika.Text);
                slika = File.ReadAllBytes(txtSlika.Text);

                int resizedImgWidth = Convert.ToInt32(ConfigurationManager.AppSettings["resizedImgWidth"]);
                int resizedImgHeight = Convert.ToInt32(ConfigurationManager.AppSettings["resizedImgHeight"]);
                int croppedImgWidth = Convert.ToInt32(ConfigurationManager.AppSettings["croppedImgWidth"]);
                int croppedImgHeight = Convert.ToInt32(ConfigurationManager.AppSettings["croppedImgHeight"]);

                if (orgImg.Width > resizedImgWidth)
                {
                    Image resizedImg = UIHelper.ResizeImage(orgImg, new Size(resizedImgWidth, resizedImgHeight));

                    if (resizedImg.Width > croppedImgWidth && resizedImg.Height > croppedImgHeight)
                    {
                        int croppedXPosition = (resizedImg.Width - croppedImgWidth) / 2;
                        int croppedYPosition = (resizedImg.Height - croppedImgHeight) / 2;

                        Image croppedImg = UIHelper.CropImage(resizedImg, new Rectangle(croppedXPosition, croppedYPosition, croppedImgWidth, croppedImgHeight));

                        MemoryStream ms = new MemoryStream();
                        croppedImg.Save(ms, orgImg.RawFormat);

                        slikaThumb = ms.ToArray();

                        pbSlika.Image = croppedImg;
                    }
                    else
                    {
                        MessageBox.Show(Messages.pic_err + " " + resizedImgWidth + "x" + resizedImgHeight + ".", "Greška",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        slika = null;
                    }

                }
            }
            catch (Exception)
            {
                slika = null;
                pbSlika.Image = null;
                txtSlika.Text = null;
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

        private void txtSifra_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSifra.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtSifra, Messages.code_req);
            }
            else
                errorProvider.SetError(txtSifra, null);
        }

        private void txtCijena_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtCijena.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtCijena, Messages.price_req);
            }
            else
                errorProvider.SetError(txtCijena, null);
        }

        private void txtCijena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void cmbKategorija_Validating(object sender, CancelEventArgs e)
        {
            if (cmbKategorija.SelectedItem == null)
            {
                e.Cancel = true;
                errorProvider.SetError(cmbKategorija, Messages.cmb_req);
            }
            else
                errorProvider.SetError(cmbKategorija, null);
        }
    }
}
