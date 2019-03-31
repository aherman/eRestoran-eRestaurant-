using eRestoran_API.Models;
using eRestoran_UI.Util;
using Microsoft.Reporting.WinForms;
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
    public partial class IzvjestavanjeForm : Form
    {
        private WebAPIHelper izvjestajiService = new WebAPIHelper("http://localhost:49327", "api/Izvjestaji");
        private WebAPIHelper zaposleniciService = new WebAPIHelper("http://localhost:49327", "api/Zaposlenici");
        private WebAPIHelper klijentiService = new WebAPIHelper("http://localhost:49327", "api/Klijenti");
        private WebAPIHelper tipoviStavkeService = new WebAPIHelper("http://localhost:49327", "api/TipoviStavke");
        private WebAPIHelper narudzbeService = new WebAPIHelper("http://localhost:49327", "api/Narudzbe");

        public IzvjestavanjeForm()
        {
            InitializeComponent();
        }

        private void IzvjestavanjeForm_Load(object sender, EventArgs e)
        {
            lblDashboard.Text = "Dobrodošli, " + Global.prijavljeniZaposlenik.Ime + " " + Global.prijavljeniZaposlenik.Prezime;
            CenterToScreen();
            fillCmbTip();
        }

        private void fillCmbTip()
        {
            Dictionary<int, string> obj = new Dictionary<int, string>();
            obj.Add(1, "Prodani artikli po klijentima");
            obj.Add(2, "Top 5 najprodavanijih artikala");
            obj.Add(3, "Top 5 najskupljih narudžbi");

            cmbTip.ValueMember = "Key";
            cmbTip.DisplayMember = "Value";
            cmbTip.DataSource = new BindingSource(obj, null);

        }

        private void cmbTip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTip.SelectedValue.ToString() == "1")
            {
                fillCmbKlijentiStavke();
            }
            else
            {
                lblOsoba.Visible = false;
                cmbParametar.Visible = false;
            }
        }


            private void BindFormStavkeKlijenti(string klijentID)
            {
                HttpResponseMessage izvjestajiResponse = izvjestajiService.GetActionResponse("ProdaniArtikliKlijent", klijentID);

                if (izvjestajiResponse.IsSuccessStatusCode)
                {
                    List<esp_IzvjestajProdatiArtikliKupac_Result> lista =
                             izvjestajiResponse.Content.ReadAsAsync<List<esp_IzvjestajProdatiArtikliKupac_Result>>().Result;

                    HttpResponseMessage response = narudzbeService.GetActionResponse("CijenaByKlijent", klijentID);
                    string cijena = Math.Round(response.Content.ReadAsAsync<decimal>().Result, 2).ToString() + " KM";

                    ReportDataSource rds = new ReportDataSource("dsProdaniArtikliKlijent", lista);
                    reportViewer1.LocalReport.DataSources.Add(rds);
                    reportViewer1.LocalReport.SetParameters(new ReportParameter("Klijent", cmbParametar.Text.ToString()));
                    reportViewer1.LocalReport.SetParameters(new ReportParameter("UkupnoPopust", cijena));
                    reportViewer1.RefreshReport();

                }
            }

        private void BindFormTop5Artikala()
        {
            HttpResponseMessage izvjestajiResponse = izvjestajiService.GetActionResponse("Top5Prodanih", "");

            if (izvjestajiResponse.IsSuccessStatusCode)
            {
                List<esp_IzvjestajTop5ProdavanihArtikala_Result> lista =
                         izvjestajiResponse.Content.ReadAsAsync<List<esp_IzvjestajTop5ProdavanihArtikala_Result>>().Result;


                ReportDataSource rds = new ReportDataSource("dsTop5Prodanih", lista);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();

            }
        }

        private void BindFormTop5Narudzbi()
        {
            HttpResponseMessage izvjestajiResponse = izvjestajiService.GetActionResponse("Top5Najskupljih", "");

            if (izvjestajiResponse.IsSuccessStatusCode)
            {
                List<esp_IzvjestajTop5NajskupljihNarudzbi_Result> lista =
                         izvjestajiResponse.Content.ReadAsAsync<List<esp_IzvjestajTop5NajskupljihNarudzbi_Result>>().Result;


                ReportDataSource rds = new ReportDataSource("dsTop5Narudzbi", lista);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();

            }
        }

        private void fillCmbKlijentiStavke()
            {
                cmbParametar.DataSource = null;
                lblOsoba.Text = "Klijent:";

                HttpResponseMessage klijentiResponse = klijentiService.GetResponse();
                List<NaloziKlijenti> klijenti = klijentiResponse.Content.ReadAsAsync<List<NaloziKlijenti>>().Result;

                klijenti = klijenti.OrderByDescending(i => i.imePrezime).ToList();
                cmbParametar.ValueMember = "klijentID";
                cmbParametar.DisplayMember = "imePrezime";
                cmbParametar.DataSource = klijenti;

            }


            private void oAplikacijiToolStripMenuItem_Click(object sender, EventArgs e)
            {
                MessageBox.Show(Messages.info, "O aplikaciji", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            private void klijentiToolStripMenuItem1_Click(object sender, EventArgs e)
            {
                KlijentForm forma = new KlijentForm();
                forma.Show();
                Hide();
            }

            private void kategorijeToolStripMenuItem1_Click(object sender, EventArgs e)
            {
                TipoviStavkeForm forma = new TipoviStavkeForm();
                forma.Show();
                Hide();
            }

            private void zaposleniciToolStripMenuItem1_Click(object sender, EventArgs e)
            {
                ZaposlenikForm forma = new ZaposlenikForm();
                forma.Show();
                Hide();
            }

            private void artikliToolStripMenuItem_Click(object sender, EventArgs e)
            {
                ArtikalForm forma = new ArtikalForm();
                forma.Show();
                Hide();
            }

            private void narudžbeToolStripMenuItem_Click(object sender, EventArgs e)
            {
                NarudzbeForm forma = new NarudzbeForm();
                forma.Show();
                Hide();
            }

            private void popustiToolStripMenuItem_Click(object sender, EventArgs e)
            {
                PopustForm forma = new PopustForm();
                forma.Show();
                Hide();
            }

            private void ocjeneToolStripMenuItem_Click(object sender, EventArgs e)
            {
                OcjeneForm forma = new OcjeneForm();
                forma.Show();
                Hide();
            }

            private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
            {
                LoginForm forma = new LoginForm();
                forma.Show();
                Hide();
                Global.prijavljeniZaposlenik = null;
            }

            private void izvještavanjeToolStripMenuItem_Click(object sender, EventArgs e)
            {
                IzvjestavanjeForm forma = new IzvjestavanjeForm();
                forma.Show();
                Hide();
            }

            private void button1_Click(object sender, EventArgs e)
            {
                reportViewer1.Reset();
                //foreach (var item in reportViewer1.LocalReport.DataSources)
                //{
                //    reportViewer1.LocalReport.DataSources.Remove(item);
                //}

                if (cmbTip.SelectedValue.ToString() == "1")
                {
                    try
                    {
                        reportViewer1.LocalReport.ReportEmbeddedResource = "eRestoran_UI.Izvjestavanje.ProdaniArtikliKlijent.rdlc";
                        BindFormStavkeKlijenti(cmbParametar.SelectedValue.ToString());
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (cmbTip.SelectedValue.ToString() == "2")
                {
                    try
                    {
                        reportViewer1.LocalReport.ReportEmbeddedResource = "eRestoran_UI.Izvjestavanje.Top5ProdanihArtikala.rdlc";
                        BindFormTop5Artikala();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    try
                    {
                    reportViewer1.LocalReport.ReportEmbeddedResource = "eRestoran_UI.Izvjestavanje.Top5Narudzbi.rdlc";
                    BindFormTop5Narudzbi();
                }
                catch (Exception)
                    {
                    }
                }
            }
        }
    }

