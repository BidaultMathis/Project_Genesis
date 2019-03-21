using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Gestion_Ticket
{
    public partial class frmTicket : Form
    {
        public frmTicket()
        {
            InitializeComponent();
        }

        public static Liste.Tickets _tickets;
        BackgroundWorker            _bgwTickets;

        private void Ticket_Load(object sender, EventArgs e)
        {
            lblIdUser.Text = frmConnexion.login.idUserM;
            lblStatutUser.Text = frmConnexion.login.StatutConvertiM;

            _bgwTickets = new BackgroundWorker();
            if (!_bgwTickets.IsBusy)
            {
                tabControl.SelectedTab          = tabPageChargement;
                _bgwTickets.DoWork             += _bgwTickets_DoWork;
                _bgwTickets.RunWorkerCompleted += _bgwTickets_RunWorkerCompleted;
                _bgwTickets.RunWorkerAsync();
            }
        }

        private void _bgwTickets_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                tabControl.SelectedTab = tabPagePrincipale;
            }
            else
            {
                MessageBox.Show(e.Error.ToString(), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _bgwTickets_DoWork(object sender, DoWorkEventArgs e)
        {
            _tickets = new Liste.Tickets();
            _tickets.getAllTickets(panelTickets);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
