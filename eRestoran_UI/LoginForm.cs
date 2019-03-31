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
    public partial class LoginForm : Form
    {
        private WebAPIHelper zaposleniciService = new WebAPIHelper("http://localhost:49327", "api/Zaposlenici");

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnKorisnikDodaj_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            HttpResponseMessage response = zaposleniciService.GetActionResponse("ByUsername", txtUsername.Text);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                MessageBox.Show(Messages.login_usr_err, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (response.IsSuccessStatusCode)
            {
                Zaposlenici z = response.Content.ReadAsAsync<Zaposlenici>().Result;

                if (UIHelper.GenerateHash(txtLozinka.Text, z.LozinkaSalt) == z.LozinkaHash)
                {
                    Global.prijavljeniZaposlenik = z;
                    if (Global.IsDostavljac() && !Global.IsAdmin() && !Global.IsOperater())
                    {
                        MessageBox.Show(Messages.login_denied, "Prijava", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    MessageBox.Show(Messages.login_succ, "Prijava", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    DashboardForm forma = new DashboardForm();
                    forma.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show(Messages.login_pass_err, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Error Code: " + response.StatusCode + " Message: " + response.ReasonPhrase);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Exit();
            base.OnClosed(e);
        }

        private void txtLozinka_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rbAdmin.Checked)
            {
                HttpResponseMessage response = zaposleniciService.GetActionResponse("ByUsername", "administrator");
                Global.prijavljeniZaposlenik = response.Content.ReadAsAsync<Zaposlenici>().Result;
            }
            else
            {
                HttpResponseMessage response = zaposleniciService.GetActionResponse("ByUsername", "operater");
                Global.prijavljeniZaposlenik = response.Content.ReadAsAsync<Zaposlenici>().Result;
            }
            DashboardForm forma = new DashboardForm();
            forma.Show();
            Hide();
        }
    }
}
