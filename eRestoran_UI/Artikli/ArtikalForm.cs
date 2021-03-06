﻿using eRestoran_API.Models;
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
    public partial class ArtikalForm : Form
    {
        private WebAPIHelper stavkeMenijaService = new WebAPIHelper("http://localhost:49327", "api/StavkeMenija");
        private WebAPIHelper tipoviStavkeService = new WebAPIHelper("http://localhost:49327", "api/TipoviStavke");
        

        public ArtikalForm()
        {
            InitializeComponent();
        }

        private void ArtikalForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            if (!Global.IsAdmin())
                adminToolStripMenuItem.Visible = false;
            lblDashboard.Text = "Dobrodošli, " + Global.prijavljeniZaposlenik.Ime + " " + Global.prijavljeniZaposlenik.Prezime;
            FillCmbKategorije();
            BindForm();
        }

        private void FillCmbKategorije()
        {
            HttpResponseMessage tipoviStavkeResponse = tipoviStavkeService.GetResponse();
            List<TipoviStavke> tipoviStavke = tipoviStavkeResponse.Content.ReadAsAsync<List<TipoviStavke>>().Result;

            tipoviStavke.Add(new TipoviStavke { Naziv = "Sve", TipStavkeID = 0 });
            tipoviStavke = tipoviStavke.OrderByDescending(i => i.Naziv).ToList();
            cmbKategorija.ValueMember = "TipStavkeID";
            cmbKategorija.DisplayMember = "Naziv";
            cmbKategorija.DataSource = tipoviStavke;
            
        }

        private void BindForm(string kategorija = "Sve")
        {
            dgvStavkeMenija.DataSource = null;
            dgvStavkeMenija.Rows.Clear();
            dgvStavkeMenija.Columns.Clear();
            HttpResponseMessage stavkeMenijaResponse = stavkeMenijaService.GetResponse(kategorija);


            if (stavkeMenijaResponse.IsSuccessStatusCode)
            {
                List<StavkeMenijaPrikaz> nalozi = stavkeMenijaResponse.Content.ReadAsAsync<List<StavkeMenijaPrikaz>>().Result;

                var rbrColumn = new DataGridViewTextBoxColumn();
                rbrColumn.Name = "rbr";
                rbrColumn.HeaderText = "Rbr.";
                rbrColumn.Width = 40;
                dgvStavkeMenija.Columns.Add(rbrColumn);
                
                dgvStavkeMenija.DataSource = nalozi;
                dgvStavkeMenija.RowHeadersVisible = false;
                dgvStavkeMenija.Columns["StavkaMenijaID"].Visible = false;

                int rowNum = 0;
                int colNum = 0;
                foreach (DataGridViewRow row in dgvStavkeMenija.Rows)
                {
                    rowNum++;
                    dgvStavkeMenija.Rows[colNum].Cells[0].Value = rowNum;
                    colNum++;
                }

                dgvStavkeMenija.Columns[2].HeaderText = "Naziv";
                dgvStavkeMenija.Columns[3].HeaderText = "Cijena";
                dgvStavkeMenija.Columns[4].HeaderText = "Kategorija";
                dgvStavkeMenija.Columns[4].Width = 150;
                dgvStavkeMenija.Columns[2].Width = 150;

                var buttonCol = new DataGridViewButtonColumn();
                buttonCol.Name = "Akcija";
                buttonCol.HeaderText = "Akcija";
                buttonCol.Text = "Obriši";
                buttonCol.Width = 83;
                buttonCol.UseColumnTextForButtonValue = true;

                dgvStavkeMenija.Columns.Add(buttonCol);


            }
        }

        private void cmbKategorija_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindForm(cmbKategorija.Text);
        }

        private void dgvStavkeMenija_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == 6 && e.RowIndex != -1)
            {
                DialogForm dialog = new DialogForm();
                var result = dialog.ShowDialog();
                if(result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvStavkeMenija.Rows[e.RowIndex].Cells["StavkaMenijaID"].Value);
                    HttpResponseMessage response = stavkeMenijaService.DeleteResponse("Obrisi", id);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(Messages.del_item_succ);
                        BindForm(cmbKategorija.Text);
                    }
                    else
                    {
                        MessageBox.Show("Error code: " + response.StatusCode + " Message: " + response.RequestMessage);
                    }
                }
            }
        }

        private void btnStavkeMenijaDodaj_Click(object sender, EventArgs e)
        {
            ArtikalDodajForm dodajForm = new ArtikalDodajForm();
            if (dodajForm.ShowDialog() == DialogResult.OK)
            {
                BindForm(cmbKategorija.Text);
                FillCmbKategorije();
            }
            else
                FillCmbKategorije();
        }

        private void dgvStavkeMenija_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex != 6 && e.RowIndex != -1)
            {
                int id = Convert.ToInt32(dgvStavkeMenija.Rows[e.RowIndex].Cells["StavkaMenijaID"].Value);
                ArtikalDodajForm dodajForm = new ArtikalDodajForm(id);
                if (dodajForm.ShowDialog() == DialogResult.OK)
                {
                    BindForm(cmbKategorija.Text);
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
