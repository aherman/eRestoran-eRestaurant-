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
    public partial class OcjeneForm : Form
    {
        private WebAPIHelper ocjeneService = new WebAPIHelper("http://localhost:49327", "api/Ocjene");

        public OcjeneForm()
        {
            InitializeComponent();
        }

        private void OcjeneForm_Load(object sender, EventArgs e)
        {
            lblDashboard.Text = "Dobrodošli, " + Global.prijavljeniZaposlenik.Ime + " " + Global.prijavljeniZaposlenik.Prezime;
            CenterToScreen();
            if (!Global.IsAdmin())
                adminToolStripMenuItem.Visible = false;
            BindForm();
        }

        private void BindForm()
        {
            dgvOcjene.DataSource = null;
            dgvOcjene.Rows.Clear();
            dgvOcjene.Columns.Clear();
            HttpResponseMessage ocjeneResponse = ocjeneService.GetResponse();


            if (ocjeneResponse.IsSuccessStatusCode)
            {
                List<esp_OcjenePrikaz_Result> ocjene = ocjeneResponse.Content.ReadAsAsync<List<esp_OcjenePrikaz_Result>>().Result;

                var rbrColumn = new DataGridViewTextBoxColumn();
                rbrColumn.Name = "rbr";
                rbrColumn.HeaderText = "Rbr.";
                rbrColumn.Width = 40;
                dgvOcjene.Columns.Add(rbrColumn);

                dgvOcjene.DataSource = ocjene;
                dgvOcjene.RowHeadersVisible = false;
                dgvOcjene.Columns["StavkaMenijaID"].Visible = false;
                dgvOcjene.Columns["Srednja_ocjena"].HeaderText = "Srednja ocjena";
                dgvOcjene.Columns[2].Width = 210;
                dgvOcjene.Columns[3].Width = 95;

                int rowNum = 0;
                int colNum = 0;
                foreach (DataGridViewRow row in dgvOcjene.Rows)
                {
                    rowNum++;
                    dgvOcjene.Rows[colNum].Cells[0].Value = rowNum;
                    colNum++;
                }
            }
        }

        private void dgvOcjene_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.RowIndex != -1)
            {
                int id = Convert.ToInt32(dgvOcjene.Rows[e.RowIndex].Cells["StavkaMenijaID"].Value);
                OcjeneDetaljiForm detaljiForm = new OcjeneDetaljiForm(id);
                if (detaljiForm.ShowDialog() == DialogResult.OK)
                {
                    BindForm();
                }
            }
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
    }
}
