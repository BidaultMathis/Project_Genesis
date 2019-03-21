using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Gestion_Ticket
{
    public partial class frmTicketDetails : Form
    {
        public frmTicketDetails()
        {
            InitializeComponent();
        }

        Liste.TicketDetails      _ticketDetails;
        Liste.TicketMessages     _ticketMessages;
        Message.sendMessage      _sendMessage;
        Message.insertTechnicien _insertTechnicien;

        private BackgroundWorker _bgwLoadDetails;
        private BackgroundWorker _bgwLoadMessages;
        private BackgroundWorker _bgwSendMessage;

        private void sendMessageM()
        {
            _sendMessage = new Message.sendMessage();
            _sendMessage.insertMessage(lblID, txtSendMsg);

            if (_sendMessage.result == 1)
            {
                btnSendMsg.Invoke((MethodInvoker)delegate
                {
                    txtSendMsg.Clear();
                    btnSendMsg.Text = "Envoyer";
                    btnSendMsg.Enabled = true;
                    panelDiscussion.Controls.Clear();
                    _ticketMessages.getMessages(lblID, panelDiscussion);
                    panelDiscussion.VerticalScroll.Value = panelDiscussion.VerticalScroll.Maximum;
                });
            }
            else
            {
                MessageBox.Show("Erreur lors de l'envoi du message.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnSendMsg.Invoke((MethodInvoker)delegate
                {
                    btnSendMsg.Text = "Envoyer";
                    btnSendMsg.Enabled = true;
                });
            }
        }

        private void frmTicketDetails_Load(object sender, EventArgs e)
        {
            _bgwLoadDetails = new BackgroundWorker();

            if (!_bgwLoadDetails.IsBusy)
            {
                _bgwLoadDetails.DoWork             += _bgwLoadDetails_DoWork;
                _bgwLoadDetails.RunWorkerCompleted += _bgwLoadDetails_RunWorkerCompleted;
                tabControl1.SelectedTab = tabPageChargement;
                _bgwLoadDetails.RunWorkerAsync();
            }
        }

        private void _bgwLoadDetails_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                lblDate.Text          = _ticketDetails.Date;
                lblIncident.Text      = _ticketDetails.Incident;
                lblPriorite.Text      = _ticketDetails.Priorite;
                lblAuteur.Text        = _ticketDetails.Auteur;
                lblTechnicien.Text    = _ticketDetails.Technicien;
                lblStatut.Text        = _ticketDetails.Statut;
                lblDateFermeture.Text = _ticketDetails.DateFermeture;
                lblTitre.Text         = _ticketDetails.Titre; 
                txtDescription.Text   = _ticketDetails.Description;
                picDownloadFile.Tag   = _ticketDetails.URL;
                lblParticipant1.Text  = _ticketDetails.Auteur;
                lblParticipant2.Text  = _ticketDetails.Technicien;

                _ticketDetails.getCouleurPriorite(lblPriorite);

                switch (lblStatut.Text)
                {
                    case "En attente":
                        lblStatut.ForeColor = Color.Orange;
                        break;
                    case "Ouvert":
                        lblStatut.ForeColor = Color.Green;
                        break;
                    case "Fermé":
                        lblStatut.ForeColor          = Color.Red;
                        lblFactDateFermeture.Visible = true;
                        lblDateFermeture.Visible     = true;
                        break;
                }

                if (picDownloadFile.Tag.ToString() != "")
                {
                    panelAttachementDownload.Visible = true;
                    lblFactFichierJoint.Text         = "Fichier joint (1) :";
                }

                tabControl1.SelectedTab = tabPageDetails;

            }
            else
            {
                MessageBox.Show(e.Error.ToString(), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _bgwLoadDetails_DoWork(object sender, DoWorkEventArgs e)
        {
            lblID.Invoke((MethodInvoker)delegate { lblID.Text = UI.ucTicketsListe.ID; });
            _ticketDetails = new Liste.TicketDetails();
            _ticketDetails.getTicket(lblID);
        }

        private void toolStripRetour_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void picDownloadFile_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(picDownloadFile.Tag.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void picDownloadFile_MouseEnter(object sender, EventArgs e)
        {
            toolTipFichierAttache.Show(picDownloadFile.Tag.ToString(), picDownloadFile);
        }

        private void toolStripRepondre_Click(object sender, EventArgs e)
        {
            _bgwLoadMessages = new BackgroundWorker();

            if (!_bgwLoadMessages.IsBusy)
            {
                _bgwLoadMessages.DoWork             += _bgwLoadMessages_DoWork;
                _bgwLoadMessages.RunWorkerCompleted += _bgwLoadMessages_RunWorkerCompleted;
                tabControl1.SelectedTab              = tabPageChargement;
                _bgwLoadMessages.RunWorkerAsync();
            }
        }

        private void _bgwLoadMessages_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                if (lblParticipant1.Text == frmConnexion.login.idUserM || lblParticipant2.Text == frmConnexion.login.idUserM || lblParticipant2.Text == "" && frmConnexion.login.StatutM == 1)
                {
                    btnSendMsg.Enabled = true;
                    txtSendMsg.Enabled = true;
                }

                panelDiscussion.VerticalScroll.Value = panelDiscussion.VerticalScroll.Maximum;
                tabControl1.SelectedTab = tabPageReponse;

            }
            else
            {
                MessageBox.Show(e.Error.ToString(), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _bgwLoadMessages_DoWork(object sender, DoWorkEventArgs e)
        {
            _ticketMessages = new Liste.TicketMessages();
            _ticketMessages.getMessages(lblID, panelDiscussion);
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            _bgwSendMessage = new BackgroundWorker();

            if (!_bgwSendMessage.IsBusy)
            {
                _bgwSendMessage.DoWork             += _bgwSendMessage_DoWork;
                _bgwSendMessage.RunWorkerCompleted += _bgwSendMessage_RunWorkerCompleted;

                btnSendMsg.Text    = "Chargement";
                btnSendMsg.Enabled = false;

                _bgwSendMessage.RunWorkerAsync();
            }
        }

        private void _bgwSendMessage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                lblParticipant2.Text = frmConnexion.login.idUserM;
            }
            else
            {
                MessageBox.Show(e.Error.ToString(), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _bgwSendMessage_DoWork(object sender, DoWorkEventArgs e)
        {

            if (lblParticipant2.Text == "" && frmConnexion.login.StatutM == 1)
            {

                _insertTechnicien = new Message.insertTechnicien();
                _insertTechnicien.addTechnicien(lblID);

                if(_insertTechnicien.resultat == 1)
                {
                    sendMessageM();
                }
                else
                {
                    MessageBox.Show("Une erreur est survenue lors de l'exécution des requêtes.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                sendMessageM();
            }
        }
    }
}
