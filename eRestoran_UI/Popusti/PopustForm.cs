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
    public partial class PopustForm : Form
    {
        private WebAPIHelper popustiService = new WebAPIHelper("http://localhost:49327", "api/Popusti");

        public PopustForm()
        {
            InitializeComponent();
        }

        private void PopustForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            if (!Global.IsAdmin())
                adminToolStripMenuItem.Visible = false;
            lblDashboard.Text = "Dobrodošli, " + Global.prijavljeniZaposlenik.Ime + " " + Global.prijavljeniZaposlenik.Prezime;
            BindForm();
        }

        private void BindForm()
        {
            dgvKlijentiOcjena.DataSource = null;
            dgvKlijentiOcjena.Rows.Clear();
            dgvKlijentiOcjena.Columns.Clear();
            HttpResponseMessage popustiResponse = popustiService.GetResponse();


            if (popustiResponse.IsSuccessStatusCode)
            {
                List<Popusti> popusti = popustiResponse.Content.ReadAsAsync<List<Popusti>>().Result;

                var rbrColumn = new DataGridViewTextBoxColumn();
                rbrColumn.Name = "rbr";
                rbrColumn.HeaderText = "Rbr.";
                rbrColumn.Width = 40;
                dgvKlijentiOcjena.Columns.Add(rbrColumn);

                dgvKlijentiOcjena.DataSource = popusti;
                dgvKlijentiOcjena.RowHeadersVisible = false;
                dgvKlijentiOcjena.Columns["PopustID"].Visible = false;
                dgvKlijentiOcjena.Columns["PopustiStavke"].Visible = false;
                dgvKlijentiOcjena.Columns[2].Width += 45;
                dgvKlijentiOcjena.Columns[3].Width += 60;

                int rowNum = 0;
                int colNum = 0;
                foreach (DataGridViewRow row in dgvKlijentiOcjena.Rows)
                {
                    rowNum++;
                    dgvKlijentiOcjena.Rows[colNum].Cells[0].Value = rowNum;
                    colNum++;
                }

                dgvKlijentiOcjena.Columns[2].HeaderText = "Naziv";
                dgvKlijentiOcjena.Columns[3].HeaderText = "Opis";


                var buttonCol = new DataGridViewButtonColumn();
                buttonCol.Name = "Akcija";
                buttonCol.HeaderText = "Akcija";
                buttonCol.Text = "Obriši";
                buttonCol.UseColumnTextForButtonValue = true;
                buttonCol.Width = 65;

                dgvKlijentiOcjena.Columns.Add(buttonCol);

            }
        }

        private void dgvKlijentiOcjena_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == 8 && e.RowIndex != -1)
            {
                DialogForm dialog = new DialogForm();
                var result = dialog.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvKlijentiOcjena.Rows[e.RowIndex].Cells["PopustID"].Value);
                    HttpResponseMessage response = popustiService.DeleteResponse("Obrisi", id);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.del_disc_succ);
                        BindForm();
                    }
                    else
                    {
                        MessageBox.Show("Error code: " + response.StatusCode + " Message: " + response.RequestMessage);
                    }
                }
            }
        }

        private void dgvKlijentiOcjena_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex != 8 && e.RowIndex != -1)
            {
                int id = Convert.ToInt32(dgvKlijentiOcjena.Rows[e.RowIndex].Cells["PopustID"].Value);
                PopustDodajForm dodajForm = new PopustDodajForm(id);
                if (dodajForm.ShowDialog() == DialogResult.OK)
                {
                    BindForm();
                }
            }
        }

        private void btnPopustiDodaj_Click(object sender, EventArgs e)
        {
            PopustDodajForm dodajForm = new PopustDodajForm();
            if (dodajForm.ShowDialog() == DialogResult.OK)
            {
                BindForm();
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
