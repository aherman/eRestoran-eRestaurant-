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
    public partial class KlijentForm : Form
    {
        private WebAPIHelper klijentiService = new WebAPIHelper("http://localhost:49327", "api/Klijenti");

        public KlijentForm()
        {
            InitializeComponent();
            
        }

        private void KlijentForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            if (!Global.IsAdmin())
                adminToolStripMenuItem.Visible = false;
            lblDashboard.Text = "Dobrodošli, " + Global.prijavljeniZaposlenik.Ime + " " + Global.prijavljeniZaposlenik.Prezime;
            BindForm();
        }

        private void BindForm(string parameter = "")
        {

            dgvNalozi.DataSource = null;
            dgvNalozi.Rows.Clear();
            dgvNalozi.Columns.Clear();

            HttpResponseMessage klijentiResponse;

            if (parameter == "")
                klijentiResponse = klijentiService.GetResponse();
            else
                klijentiResponse = klijentiService.GetActionResponse("GetByParameter", parameter);


            if (klijentiResponse.IsSuccessStatusCode)
            {
                List<NaloziKlijenti> nalozi = klijentiResponse.Content.ReadAsAsync<List<NaloziKlijenti>>().Result;

                var rbrColumn = new DataGridViewTextBoxColumn();
                rbrColumn.Name = "rbr";
                rbrColumn.HeaderText = "Rbr.";
                rbrColumn.Width = 40;
                dgvNalozi.Columns.Add(rbrColumn);

                dgvNalozi.DataSource = nalozi;
                dgvNalozi.RowHeadersVisible = false;
                dgvNalozi.Columns["klijentID"].Visible = false;

                int rowNum = 0;
                int colNum = 0;
                foreach (DataGridViewRow row in dgvNalozi.Rows)
                {
                    rowNum++;
                    dgvNalozi.Rows[colNum].Cells[0].Value = rowNum;
                    colNum++;
                }

                dgvNalozi.Columns[2].HeaderText = "Ime i prezime";
                dgvNalozi.Columns[3].HeaderText = "Username";
                dgvNalozi.Columns[4].HeaderText = "E-mail";
                dgvNalozi.Columns[5].HeaderText = "Adresa";
                dgvNalozi.Columns[5].Width = 175;
                dgvNalozi.Columns[4].Width = 145;

                var buttonCol = new DataGridViewButtonColumn();
                buttonCol.Name = "Akcija";
                buttonCol.HeaderText = "Akcija";
                buttonCol.Text = "Obriši";
                buttonCol.UseColumnTextForButtonValue = true;

                dgvNalozi.Columns.Add(buttonCol);
            }
        }

        private void txtPretraga_TextChanged(object sender, EventArgs e)
        {
            BindForm(txtPretraga.Text);
        }

        private void dgvNalozi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == 6 && e.RowIndex != -1)
            {
                DialogForm dialog = new DialogForm();
                var result = dialog.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvNalozi.Rows[e.RowIndex].Cells["klijentID"].Value);
                    HttpResponseMessage response = klijentiService.DeleteResponse("Obrisi", id);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.del_client_succ);
                        BindForm(txtPretraga.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error code: " + response.StatusCode + " Message: " + response.RequestMessage + " Reason Phrase:" + response.ReasonPhrase);
                    }
                }
            }
        }

        private void dgvNalozi_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex != 6 && e.RowIndex != -1)
            {
                int id = Convert.ToInt32(dgvNalozi.Rows[e.RowIndex].Cells["klijentID"].Value);
                KlijentDodajForm dodajForm = new KlijentDodajForm(id);
                if (dodajForm.ShowDialog() == DialogResult.OK)
                {
                    BindForm(txtPretraga.Text);
                }
            }
        }

        private void btnKlijentDodaj_Click(object sender, EventArgs e)
        {
            KlijentDodajForm dodajForm = new KlijentDodajForm();
            if (dodajForm.ShowDialog() == DialogResult.OK)
            {
                BindForm(txtPretraga.Text);
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
