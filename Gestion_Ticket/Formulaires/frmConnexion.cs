using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Gestion_Ticket
{
    public partial class frmConnexion : Form
    {

        private BackgroundWorker bgwLogin;
        public static Login.Login login = new Login.Login();

        public frmConnexion()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, System.EventArgs e)
        {
            if(textBoxUser.Text == "" || textBoxPass.Text == "")
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                bgwLogin = new BackgroundWorker();
                if (!bgwLogin.IsBusy)
                {
                    bgwLogin.DoWork             += BgwLogin_DoWork;
                    bgwLogin.RunWorkerCompleted += BgwLogin_RunWorkerCompleted;
                    bgwLogin.RunWorkerAsync();
                }
            }
        }

        private void BgwLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (login.estConnecteM == 1)
                {
                    Close();
                    Thread th = new Thread(AffichageFormTicket);
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                }
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }

        private void AffichageFormTicket()
        {
            Application.Run(new frmTicket());
        }

        private void BgwLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            login.idUserM = textBoxUser.Text;
            login.Motdepasse = textBoxPass.Text;
            login.Connexion();
        }

        private void linkOubliMdp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmOubliMdp frmOubliMdp = new frmOubliMdp();
            frmOubliMdp.ShowDialog();
        }
    }
}
