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
    public partial class NarudzbeForm : Form
    {
        private WebAPIHelper narudzbeService = new WebAPIHelper("http://localhost:49327", "api/Narudzbe");

        public NarudzbeForm()
        {
            InitializeComponent();
        }

        private void NarudzbeForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            if (!Global.IsAdmin())
                adminToolStripMenuItem.Visible = false;
            lblDashboard.Text = "Dobrodošli, " + Global.prijavljeniZaposlenik.Ime + " " + Global.prijavljeniZaposlenik.Prezime;
            BindForm();
        }

        private void BindForm()
        {
            HttpResponseMessage narudzbeResponse = narudzbeService.GetActionResponse("NarudzbePrikaz");


            if (narudzbeResponse.IsSuccessStatusCode)
            {
                dgvNarudzbe.DataSource = null;
                dgvNarudzbe.Rows.Clear();
                dgvNarudzbe.Columns.Clear();

                List<NarudzbePrikaz> narudzbe = narudzbeResponse.Content.ReadAsAsync<List<NarudzbePrikaz>>().Result;

                var rbrColumn = new DataGridViewTextBoxColumn();
                rbrColumn.Name = "rbr";
                rbrColumn.HeaderText = "Rbr.";
                rbrColumn.Width = 40;
                dgvNarudzbe.Columns.Add(rbrColumn);

                dgvNarudzbe.DataSource = narudzbe;
                dgvNarudzbe.RowHeadersVisible = false;
                dgvNarudzbe.Columns["narudzbaID"].Visible = false;
                dgvNarudzbe.Columns["lokacija"].Visible = false;
               
                int rowNum = 0;
                int colNum = 0;
                foreach (DataGridViewRow row in dgvNarudzbe.Rows)
                {
                    rowNum++;
                    dgvNarudzbe.Rows[colNum].Cells[0].Value = rowNum;
                    colNum++;
                }

                dgvNarudzbe.Columns[2].HeaderText = "Ime i prezime klijenta";
                dgvNarudzbe.Columns[3].HeaderText = "Telefon";
                dgvNarudzbe.Columns[4].HeaderText = "E-mail";
                dgvNarudzbe.Columns[6].HeaderText = "Datum i vrijeme";
                dgvNarudzbe.Columns[7].HeaderText = "Ukupna cijena";
                dgvNarudzbe.Columns[2].Width = 140;
                dgvNarudzbe.Columns[3].Width = 115;
                dgvNarudzbe.Columns[4].Width = 140;

                dgvNarudzbe.Columns["datumVrijeme"].HeaderText = "Datum";


                var buttonCol = new DataGridViewButtonColumn();
                buttonCol.Name = "Akcija";
                buttonCol.HeaderText = "Akcija";
                buttonCol.Text = "Aktiviraj";
                buttonCol.UseColumnTextForButtonValue = true;
                buttonCol.Width = 80;
                dgvNarudzbe.Columns.Add(buttonCol);

            }
        }

        private void dgvNarudzbe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == 8 && e.RowIndex != -1)
            {
                int id = Convert.ToInt32(dgvNarudzbe.Rows[e.RowIndex].Cells["narudzbaID"].Value);
                NarudzbaAktivacijaForm forma = new NarudzbaAktivacijaForm(id);
                if (forma.ShowDialog() == DialogResult.OK)
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
