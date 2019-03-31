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
    public partial class OcjeneDetaljiForm : Form
    {
        private WebAPIHelper ocjeneService = new WebAPIHelper("http://localhost:49327", "api/Ocjene");

        int stavkaID;
        public OcjeneDetaljiForm(int id)
        {
            InitializeComponent();
            stavkaID = id;
        }

        private void OcjeneDetaljiForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            fillData();
        }

        private void fillData()
        {
            HttpResponseMessage response = ocjeneService.GetActionResponse("GetById", stavkaID.ToString());
            esp_OcjeneStavkaPrikaz_Result stavkaOcjena = response.Content.ReadAsAsync<esp_OcjeneStavkaPrikaz_Result>().Result;
            txtStavkaMenija.Text = stavkaOcjena.Naziv;
            txtOcjena.Text = stavkaOcjena.Srednja_ocjena.ToString();

            HttpResponseMessage response2 = ocjeneService.GetActionResponse("GetOcjeneByStavka", stavkaID.ToString());
            if (response2.IsSuccessStatusCode)
            {
                List<esp_OcjeneByStavka_Result> ocjene = response2.Content.ReadAsAsync<List<esp_OcjeneByStavka_Result>>().Result;
                BindForm(ocjene);
            }
            
        }

        private void BindForm(List<esp_OcjeneByStavka_Result> ocjene)
        {
            dgvKlijentiOcjena.DataSource = null;
            dgvKlijentiOcjena.Rows.Clear();
            dgvKlijentiOcjena.Columns.Clear();

            var rbrColumn = new DataGridViewTextBoxColumn();
            rbrColumn.Name = "rbr";
            rbrColumn.HeaderText = "Rbr.";
            rbrColumn.Width = 40;
            dgvKlijentiOcjena.Columns.Add(rbrColumn);

            dgvKlijentiOcjena.DataSource = ocjene;
            dgvKlijentiOcjena.RowHeadersVisible = false;
            dgvKlijentiOcjena.Columns["ImePrezime"].HeaderText = "Ime i prezime";
            dgvKlijentiOcjena.Columns["Ocjena"].Width = 70;
            int rowNum = 0;
            int colNum = 0;
            foreach (DataGridViewRow row in dgvKlijentiOcjena.Rows)
            {
                rowNum++;
                dgvKlijentiOcjena.Rows[colNum].Cells[0].Value = rowNum;
                colNum++;
            }
        }

        private void btnOcjeneNazad_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
