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
    public partial class TipoviStavkeForm : Form
    {
        private WebAPIHelper tipoviStavkeService = new WebAPIHelper("http://localhost:49327", "api/TipoviStavke");

        public TipoviStavkeForm()
        {
            InitializeComponent();
        }

        private void TipoviStavkeForm_Load(object sender, EventArgs e)
        {
            lblDashboard.Text = "Dobrodošli, " + Global.prijavljeniZaposlenik.Ime + " " + Global.prijavljeniZaposlenik.Prezime;
            CenterToScreen();
            if (!Global.IsAdmin())
                adminToolStripMenuItem.Visible = false;
            BindForm();
        }

        private void BindForm()
        {
            dgvKategorije.DataSource = null;
            dgvKategorije.Rows.Clear();
            dgvKategorije.Columns.Clear();
            HttpResponseMessage tipoviStavkeResponse = tipoviStavkeService.GetResponse();


            if (tipoviStavkeResponse.IsSuccessStatusCode)
            {
                List<TipoviStavke> nalozi = tipoviStavkeResponse.Content.ReadAsAsync<List<TipoviStavke>>().Result;

                var rbrColumn = new DataGridViewTextBoxColumn();
                rbrColumn.Name = "rbr";
                rbrColumn.HeaderText = "Rbr.";
                rbrColumn.Width = 40;
                dgvKategorije.Columns.Add(rbrColumn);

                dgvKategorije.DataSource = nalozi;
                dgvKategorije.RowHeadersVisible = false;
                dgvKategorije.Columns["TipStavkeID"].Visible = false;
                dgvKategorije.Columns["StavkeMenija"].Visible = false;

                int rowNum = 0;
                int colNum = 0;
                foreach (DataGridViewRow row in dgvKategorije.Rows)
                {
                    rowNum++;
                    dgvKategorije.Rows[colNum].Cells[0].Value = rowNum;
                    colNum++;
                }

                dgvKategorije.Columns[2].HeaderText = "Naziv";
                dgvKategorije.Columns[3].HeaderText = "Opis";
                

                var buttonCol = new DataGridViewButtonColumn();
                buttonCol.Name = "Akcija";
                buttonCol.HeaderText = "Akcija";
                buttonCol.Text = "Obriši";
                buttonCol.UseColumnTextForButtonValue = true;
                buttonCol.Width = 67;

                dgvKategorije.Columns.Add(buttonCol);

            }
        }

        private void dgvKategorije_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == 5 && e.RowIndex != -1)
            {
                DialogForm dialog = new DialogForm(true);
                var result = dialog.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvKategorije.Rows[e.RowIndex].Cells["TipStavkeID"].Value);
                    HttpResponseMessage response = tipoviStavkeService.DeleteResponse("Obrisi", id);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.del_itemtype_succ);
                        BindForm();
                    }
                    else
                    {
                        MessageBox.Show("Error code: " + response.StatusCode + " Message: " + response.RequestMessage);
                    }
                }
                
            }
        }

        private void btnKategorijaDodaj_Click(object sender, EventArgs e)
        {
            TipoviStavkeDodajForm dodajForm = new TipoviStavkeDodajForm();
            if (dodajForm.ShowDialog() == DialogResult.OK)
            {
                BindForm();
            }
        }

        private void dgvKategorije_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex != 5 && e.RowIndex != -1)
            {
                int id = Convert.ToInt32(dgvKategorije.Rows[e.RowIndex].Cells["TipStavkeID"].Value);
                TipoviStavkeDodajForm dodajForm = new TipoviStavkeDodajForm(id);
                if (dodajForm.ShowDialog() == DialogResult.OK)
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
