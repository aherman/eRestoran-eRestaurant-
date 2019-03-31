namespace eRestoran_UI
{
    partial class ArtikalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArtikalForm));
            this.dgvStavkeMenija = new System.Windows.Forms.DataGridView();
            this.lblNaslov = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnStavkeMenijaDodaj = new System.Windows.Forms.Button();
            this.cmbKategorija = new System.Windows.Forms.ComboBox();
            this.lblKategorija = new System.Windows.Forms.Label();
            this.lblDashboard = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.akcijaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.artikliToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.narudžbeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.popustiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ocjeneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.klijentiToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.kategorijeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.zaposleniciToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.izvještavanjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oAplikacijiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStavkeMenija)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvStavkeMenija
            // 
            this.dgvStavkeMenija.AllowUserToAddRows = false;
            this.dgvStavkeMenija.AllowUserToDeleteRows = false;
            this.dgvStavkeMenija.AllowUserToResizeColumns = false;
            this.dgvStavkeMenija.AllowUserToResizeRows = false;
            this.dgvStavkeMenija.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStavkeMenija.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvStavkeMenija.Location = new System.Drawing.Point(61, 139);
            this.dgvStavkeMenija.MultiSelect = false;
            this.dgvStavkeMenija.Name = "dgvStavkeMenija";
            this.dgvStavkeMenija.ReadOnly = true;
            this.dgvStavkeMenija.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvStavkeMenija.Size = new System.Drawing.Size(644, 199);
            this.dgvStavkeMenija.TabIndex = 14;
            this.dgvStavkeMenija.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStavkeMenija_CellContentClick);
            this.dgvStavkeMenija.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStavkeMenija_CellContentDoubleClick);
            // 
            // lblNaslov
            // 
            this.lblNaslov.AutoSize = true;
            this.lblNaslov.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblNaslov.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNaslov.Location = new System.Drawing.Point(11, 34);
            this.lblNaslov.Name = "lblNaslov";
            this.lblNaslov.Size = new System.Drawing.Size(367, 26);
            this.lblNaslov.TabIndex = 12;
            this.lblNaslov.Text = "eRestoran :: Upravljanje artiklima";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pictureBox1.Location = new System.Drawing.Point(-1, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(777, 46);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // btnStavkeMenijaDodaj
            // 
            this.btnStavkeMenijaDodaj.Font = new System.Drawing.Font("Microsoft New Tai Lue", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStavkeMenijaDodaj.Location = new System.Drawing.Point(615, 87);
            this.btnStavkeMenijaDodaj.Name = "btnStavkeMenijaDodaj";
            this.btnStavkeMenijaDodaj.Size = new System.Drawing.Size(90, 28);
            this.btnStavkeMenijaDodaj.TabIndex = 2;
            this.btnStavkeMenijaDodaj.Text = "Dodaj";
            this.btnStavkeMenijaDodaj.UseVisualStyleBackColor = true;
            this.btnStavkeMenijaDodaj.Click += new System.EventHandler(this.btnStavkeMenijaDodaj_Click);
            // 
            // cmbKategorija
            // 
            this.cmbKategorija.Font = new System.Drawing.Font("Microsoft New Tai Lue", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbKategorija.FormattingEnabled = true;
            this.cmbKategorija.Location = new System.Drawing.Point(136, 90);
            this.cmbKategorija.Name = "cmbKategorija";
            this.cmbKategorija.Size = new System.Drawing.Size(144, 25);
            this.cmbKategorija.TabIndex = 1;
            this.cmbKategorija.SelectedIndexChanged += new System.EventHandler(this.cmbKategorija_SelectedIndexChanged);
            // 
            // lblKategorija
            // 
            this.lblKategorija.AutoSize = true;
            this.lblKategorija.Font = new System.Drawing.Font("Microsoft New Tai Lue", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKategorija.Location = new System.Drawing.Point(57, 93);
            this.lblKategorija.Name = "lblKategorija";
            this.lblKategorija.Size = new System.Drawing.Size(73, 19);
            this.lblKategorija.TabIndex = 15;
            this.lblKategorija.Text = "Kategorija:";
            // 
            // lblDashboard
            // 
            this.lblDashboard.AutoSize = true;
            this.lblDashboard.BackColor = System.Drawing.SystemColors.Control;
            this.lblDashboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDashboard.Location = new System.Drawing.Point(463, 2);
            this.lblDashboard.Name = "lblDashboard";
            this.lblDashboard.Size = new System.Drawing.Size(47, 20);
            this.lblDashboard.TabIndex = 20;
            this.lblDashboard.Text = "label";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.akcijaToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.oAplikacijiToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(754, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // akcijaToolStripMenuItem
            // 
            this.akcijaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.artikliToolStripMenuItem,
            this.narudžbeToolStripMenuItem,
            this.popustiToolStripMenuItem,
            this.ocjeneToolStripMenuItem});
            this.akcijaToolStripMenuItem.Name = "akcijaToolStripMenuItem";
            this.akcijaToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.akcijaToolStripMenuItem.Text = "Akcija";
            // 
            // artikliToolStripMenuItem
            // 
            this.artikliToolStripMenuItem.Name = "artikliToolStripMenuItem";
            this.artikliToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.artikliToolStripMenuItem.Text = "Artikli";
            this.artikliToolStripMenuItem.Click += new System.EventHandler(this.artikliToolStripMenuItem_Click);
            // 
            // narudžbeToolStripMenuItem
            // 
            this.narudžbeToolStripMenuItem.Name = "narudžbeToolStripMenuItem";
            this.narudžbeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.narudžbeToolStripMenuItem.Text = "Narudžbe";
            this.narudžbeToolStripMenuItem.Click += new System.EventHandler(this.narudžbeToolStripMenuItem_Click);
            // 
            // popustiToolStripMenuItem
            // 
            this.popustiToolStripMenuItem.Name = "popustiToolStripMenuItem";
            this.popustiToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.popustiToolStripMenuItem.Text = "Popusti";
            this.popustiToolStripMenuItem.Click += new System.EventHandler(this.popustiToolStripMenuItem_Click);
            // 
            // ocjeneToolStripMenuItem
            // 
            this.ocjeneToolStripMenuItem.Name = "ocjeneToolStripMenuItem";
            this.ocjeneToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.ocjeneToolStripMenuItem.Text = "Ocjene";
            this.ocjeneToolStripMenuItem.Click += new System.EventHandler(this.ocjeneToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.klijentiToolStripMenuItem1,
            this.kategorijeToolStripMenuItem1,
            this.zaposleniciToolStripMenuItem1,
            this.izvještavanjeToolStripMenuItem});
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.adminToolStripMenuItem.Text = "Admin";
            // 
            // klijentiToolStripMenuItem1
            // 
            this.klijentiToolStripMenuItem1.Name = "klijentiToolStripMenuItem1";
            this.klijentiToolStripMenuItem1.Size = new System.Drawing.Size(140, 22);
            this.klijentiToolStripMenuItem1.Text = "Klijenti";
            this.klijentiToolStripMenuItem1.Click += new System.EventHandler(this.klijentiToolStripMenuItem1_Click);
            // 
            // kategorijeToolStripMenuItem1
            // 
            this.kategorijeToolStripMenuItem1.Name = "kategorijeToolStripMenuItem1";
            this.kategorijeToolStripMenuItem1.Size = new System.Drawing.Size(140, 22);
            this.kategorijeToolStripMenuItem1.Text = "Kategorije";
            this.kategorijeToolStripMenuItem1.Click += new System.EventHandler(this.kategorijeToolStripMenuItem1_Click);
            // 
            // zaposleniciToolStripMenuItem1
            // 
            this.zaposleniciToolStripMenuItem1.Name = "zaposleniciToolStripMenuItem1";
            this.zaposleniciToolStripMenuItem1.Size = new System.Drawing.Size(140, 22);
            this.zaposleniciToolStripMenuItem1.Text = "Zaposlenici";
            this.zaposleniciToolStripMenuItem1.Click += new System.EventHandler(this.zaposleniciToolStripMenuItem1_Click);
            // 
            // izvještavanjeToolStripMenuItem
            // 
            this.izvještavanjeToolStripMenuItem.Name = "izvještavanjeToolStripMenuItem";
            this.izvještavanjeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.izvještavanjeToolStripMenuItem.Text = "Izvještavanje";
            this.izvještavanjeToolStripMenuItem.Click += new System.EventHandler(this.izvještavanjeToolStripMenuItem_Click);
            // 
            // oAplikacijiToolStripMenuItem
            // 
            this.oAplikacijiToolStripMenuItem.Name = "oAplikacijiToolStripMenuItem";
            this.oAplikacijiToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.oAplikacijiToolStripMenuItem.Text = "O aplikaciji";
            this.oAplikacijiToolStripMenuItem.Click += new System.EventHandler(this.oAplikacijiToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.logoutToolStripMenuItem.Text = "Log out";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // ArtikalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(754, 361);
            this.Controls.Add(this.lblDashboard);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnStavkeMenijaDodaj);
            this.Controls.Add(this.cmbKategorija);
            this.Controls.Add(this.lblKategorija);
            this.Controls.Add(this.dgvStavkeMenija);
            this.Controls.Add(this.lblNaslov);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ArtikalForm";
            this.Text = "eRestoran";
            this.Load += new System.EventHandler(this.ArtikalForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStavkeMenija)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStavkeMenija;
        private System.Windows.Forms.Label lblNaslov;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnStavkeMenijaDodaj;
        private System.Windows.Forms.ComboBox cmbKategorija;
        private System.Windows.Forms.Label lblKategorija;
        private System.Windows.Forms.Label lblDashboard;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem akcijaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem artikliToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem narudžbeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem popustiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ocjeneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem klijentiToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem kategorijeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem zaposleniciToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem oAplikacijiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem izvještavanjeToolStripMenuItem;
    }
}