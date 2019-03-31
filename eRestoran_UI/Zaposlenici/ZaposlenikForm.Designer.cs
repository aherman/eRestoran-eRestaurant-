namespace eRestoran_UI
{
    partial class ZaposlenikForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZaposlenikForm));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dgvNalozi = new System.Windows.Forms.DataGridView();
            this.lblPretraga = new System.Windows.Forms.Label();
            this.txtPretraga = new System.Windows.Forms.TextBox();
            this.lblUloga = new System.Windows.Forms.Label();
            this.cmbUloga = new System.Windows.Forms.ComboBox();
            this.btnZaposlenikDodaj = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNalozi)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(428, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "eRestoran :: Upravljanje zaposlenicima";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pictureBox1.Location = new System.Drawing.Point(0, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(786, 46);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // dgvNalozi
            // 
            this.dgvNalozi.AllowUserToAddRows = false;
            this.dgvNalozi.AllowUserToDeleteRows = false;
            this.dgvNalozi.AllowUserToResizeRows = false;
            this.dgvNalozi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNalozi.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvNalozi.Location = new System.Drawing.Point(43, 123);
            this.dgvNalozi.MultiSelect = false;
            this.dgvNalozi.Name = "dgvNalozi";
            this.dgvNalozi.ReadOnly = true;
            this.dgvNalozi.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvNalozi.Size = new System.Drawing.Size(664, 226);
            this.dgvNalozi.TabIndex = 4;
            this.dgvNalozi.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNalozi_CellContentClick);
            this.dgvNalozi.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNalozi_CellContentDoubleClick);
            // 
            // lblPretraga
            // 
            this.lblPretraga.AutoSize = true;
            this.lblPretraga.Font = new System.Drawing.Font("Microsoft New Tai Lue", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPretraga.Location = new System.Drawing.Point(39, 92);
            this.lblPretraga.Name = "lblPretraga";
            this.lblPretraga.Size = new System.Drawing.Size(64, 19);
            this.lblPretraga.TabIndex = 7;
            this.lblPretraga.Text = "Pretraga:";
            // 
            // txtPretraga
            // 
            this.txtPretraga.Font = new System.Drawing.Font("Microsoft New Tai Lue", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPretraga.Location = new System.Drawing.Point(109, 89);
            this.txtPretraga.Name = "txtPretraga";
            this.txtPretraga.Size = new System.Drawing.Size(145, 25);
            this.txtPretraga.TabIndex = 1;
            this.txtPretraga.TextChanged += new System.EventHandler(this.txtPretraga_TextChanged);
            // 
            // lblUloga
            // 
            this.lblUloga.AutoSize = true;
            this.lblUloga.Font = new System.Drawing.Font("Microsoft New Tai Lue", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUloga.Location = new System.Drawing.Point(279, 92);
            this.lblUloga.Name = "lblUloga";
            this.lblUloga.Size = new System.Drawing.Size(48, 19);
            this.lblUloga.TabIndex = 9;
            this.lblUloga.Text = "Uloga:";
            // 
            // cmbUloga
            // 
            this.cmbUloga.Font = new System.Drawing.Font("Microsoft New Tai Lue", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbUloga.FormattingEnabled = true;
            this.cmbUloga.Location = new System.Drawing.Point(333, 89);
            this.cmbUloga.Name = "cmbUloga";
            this.cmbUloga.Size = new System.Drawing.Size(144, 25);
            this.cmbUloga.TabIndex = 2;
            this.cmbUloga.SelectedIndexChanged += new System.EventHandler(this.cmbUloga_SelectedIndexChanged);
            // 
            // btnZaposlenikDodaj
            // 
            this.btnZaposlenikDodaj.Font = new System.Drawing.Font("Microsoft New Tai Lue", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZaposlenikDodaj.Location = new System.Drawing.Point(616, 86);
            this.btnZaposlenikDodaj.Name = "btnZaposlenikDodaj";
            this.btnZaposlenikDodaj.Size = new System.Drawing.Size(91, 28);
            this.btnZaposlenikDodaj.TabIndex = 3;
            this.btnZaposlenikDodaj.Text = "Dodaj";
            this.btnZaposlenikDodaj.UseVisualStyleBackColor = true;
            this.btnZaposlenikDodaj.Click += new System.EventHandler(this.btnZaposlenikDodaj_Click);
            // 
            // lblDashboard
            // 
            this.lblDashboard.AutoSize = true;
            this.lblDashboard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDashboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDashboard.Location = new System.Drawing.Point(463, 2);
            this.lblDashboard.Name = "lblDashboard";
            this.lblDashboard.Size = new System.Drawing.Size(47, 20);
            this.lblDashboard.TabIndex = 20;
            this.lblDashboard.Text = "label";
            this.lblDashboard.Click += new System.EventHandler(this.lblDashboard_Click);
            // 
            // menuStrip1
            // 
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
            this.oAplikacijiToolStripMenuItem.Click += new System.EventHandler(this.oAplikacijiToolStripMenuItem_Click_1);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.logoutToolStripMenuItem.Text = "Log out";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // ZaposlenikForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(754, 361);
            this.Controls.Add(this.lblDashboard);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnZaposlenikDodaj);
            this.Controls.Add(this.cmbUloga);
            this.Controls.Add(this.lblUloga);
            this.Controls.Add(this.txtPretraga);
            this.Controls.Add(this.lblPretraga);
            this.Controls.Add(this.dgvNalozi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ZaposlenikForm";
            this.Text = "eRestoran";
            this.Load += new System.EventHandler(this.ZaposlenikForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNalozi)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dgvNalozi;
        private System.Windows.Forms.Label lblPretraga;
        private System.Windows.Forms.TextBox txtPretraga;
        private System.Windows.Forms.Label lblUloga;
        private System.Windows.Forms.ComboBox cmbUloga;
        private System.Windows.Forms.Button btnZaposlenikDodaj;
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