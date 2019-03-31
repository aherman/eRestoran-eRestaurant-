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
    public partial class NarudzbaAktivacijaForm : Form
    {
        private WebAPIHelper narudzbeStavkeService = new WebAPIHelper("http://localhost:49327", "api/NarudzbeStavke");
        private WebAPIHelper narudzbeService = new WebAPIHelper("http://localhost:49327", "api/Narudzbe");
        private WebAPIHelper popustiService = new WebAPIHelper("http://localhost:49327", "api/Popusti");

        private Narudzbe n;
        private int narudzbaID;
        private decimal cijena;
        List<string> napomene;
        public NarudzbaAktivacijaForm(int narudzbaid)
        {
            InitializeComponent();
            narudzbaID = narudzbaid;
            HttpResponseMessage response = narudzbeService.GetResponse(narudzbaID.ToString());
            n = response.Content.ReadAsAsync<Narudzbe>().Result;
        }

        private void NarudzbaAktivacijaForm_Load(object sender, EventArgs e)
        {
            napomene = new List<string>();
            CenterToScreen();
            BindForm();
        }

        private void BindForm()
        {
            HttpResponseMessage narudzbeStavkeResponse = narudzbeStavkeService.GetActionResponse("GetByNarudzba", narudzbaID.ToString());


            if (narudzbeStavkeResponse.IsSuccessStatusCode)
            {
                List<NarudzbeStavkePrikaz> narudzbeStavke = narudzbeStavkeResponse.Content.ReadAsAsync<List<NarudzbeStavkePrikaz>>().Result;

                foreach (var item in narudzbeStavke.ToList())
                {
                    napomene.Add(item.napomena);
                }

                var rbrColumn = new DataGridViewTextBoxColumn();
                rbrColumn.Name = "rbr";
                rbrColumn.HeaderText = "Rbr.";
                rbrColumn.Width = 40;
                dgvNarudzbe.Columns.Add(rbrColumn);

                var buttonCol = new DataGridViewTextBoxColumn();
                buttonCol.Name = "Napomena";
                buttonCol.HeaderText = "Napomena";
                buttonCol.ReadOnly = true;
                //buttonCol.Text = "Prikaži";
                //buttonCol.UseColumnTextForButtonValue = true;

                dgvNarudzbe.DataSource = narudzbeStavke;
                dgvNarudzbe.RowHeadersVisible = false;
                dgvNarudzbe.Columns["narudzbaStavkaID"].Visible = false;
                dgvNarudzbe.Columns["napomena"].Visible = false;

                int rowNum = 0;
                int colNum = 0;
                foreach (DataGridViewRow row in dgvNarudzbe.Rows)
                {
                    rowNum++;
                    dgvNarudzbe.Rows[colNum].Cells[0].Value = rowNum;
                    colNum++;
                }

                dgvNarudzbe.Columns[2].HeaderText = "Naziv artikla";
                dgvNarudzbe.Columns[3].HeaderText = "Cijena";
                dgvNarudzbe.Columns[4].HeaderText = "Količina";
                dgvNarudzbe.Columns[5].HeaderText = "Količina";
                dgvNarudzbe.Columns[6].HeaderText = "Ukupna cijena";
                dgvNarudzbe.Columns[2].Width = 150;
                dgvNarudzbe.Columns[3].Width = 60;
                dgvNarudzbe.Columns[4].Width = 60;
                dgvNarudzbe.Columns[5].Width = 105;

                dgvNarudzbe.Columns.Add(buttonCol);

                for (int i = 0; i < dgvNarudzbe.Rows.Count; i++)
                {
                        if (string.IsNullOrEmpty(napomene[i]))
                    {
                        dgvNarudzbe.Rows[i].Cells[7].Value = "N/A";
                        dgvNarudzbe.Rows[i].Cells[7].Style.BackColor = Color.PaleVioletRed;
                    }
                    else
                    {
                        dgvNarudzbe.Rows[i].Cells[7].Value = "Prikaži";
                        dgvNarudzbe.Rows[i].Cells[7].Style.BackColor = Color.LightGreen;
                    }
                        
                }

                dgvNarudzbe.Columns[7].Width = 70;

                HttpResponseMessage narudzbeResponse = narudzbeService.GetResponse(narudzbaID.ToString());


                if (narudzbeResponse.IsSuccessStatusCode)
                {
                    NarudzbePrikaz narudzbe = narudzbeResponse.Content.ReadAsAsync<NarudzbePrikaz>().Result;

                    txtImePrezime.Text = narudzbe.imePrezime;
                    txtLokacija.Text = narudzbe.lokacija;
                    txtEmail.Text = narudzbe.email;
                    txtDatumVrijemeNarudzbe.Text = narudzbe.datumVrijeme;
                    txtTelefon.Text = narudzbe.telefon;


                    HttpResponseMessage ukupnaCijenaResponse = narudzbeService
                        .GetActionResponse("CijenaByNarudzbaId", narudzbaID.ToString());
                    string ukupnaCijena = ukupnaCijenaResponse
                        .Content.ReadAsAsync<string>().Result;
                    cijena = Convert.ToDecimal(ukupnaCijena);
                    txtUkupnaCijena.Text = Math.Round(Convert.ToDecimal(ukupnaCijena), 2).ToString() + " KM";

                    Popusti();
                }
            }
        }

        private void Popusti()
        {
            HttpResponseMessage response = popustiService.GetActionResponse("ImaPopust", narudzbaID.ToString());
            List<PopustiModel> popusti = response.Content.ReadAsAsync<List<PopustiModel>>().Result;
            if (popusti.Count != 0)
            {
                lblUkupnaCijenaPopust.Visible = true;
                txtUkupnaCijenaPopust.Visible = true;
                lblPostotak.Visible = true;

                decimal ukupno = 0;
                foreach (var item in popusti)
                {
                    ukupno += item.iznos;
                }

                txtUkupnaCijenaPopust.Text = 
                    Math.Round(cijena - (cijena * (ukupno/100)),2).ToString() + " KM";
                lblPostotak.Text = "(" + ukupno.ToString() + "%)";
            }
            else
            {
                lblUkupnaCijenaPopust.Visible = false;
                txtUkupnaCijenaPopust.Visible = false;
                lblPostotak.Visible = false;
            }
        }

        private void btnAktiviraj_Click(object sender, EventArgs e)
        {
            HttpResponseMessage response = narudzbeService.PutResponse(narudzbaID,n);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show(Messages.act_order_succ, "Aktivacija", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void dgvNarudzbe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == 7 && e.RowIndex != -1)
            {
                if(!string.IsNullOrEmpty(napomene[e.RowIndex]))
                    MessageBox.Show(napomene[e.RowIndex]);
            }
        }
    }
}
