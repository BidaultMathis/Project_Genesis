using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Gestion_Ticket
{
    public partial class frmOubliMdp : Form
    {
        public frmOubliMdp()
        {
            InitializeComponent();
        }

        API.SMS _sms              = new API.SMS();
        NewPass.NewPass  _newPass = new NewPass.NewPass();
        BackgroundWorker _bgwGetPass;

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            if (txtCounty.Text == "" || txtTel.Text == "")
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (txtTel.Text.Length < 9 || txtTel.Text.Length > 9)
            {
                MessageBox.Show("Veuillez indiquer un numéro de téléphone valide.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    _sms.sendSMS(txtCounty, txtTel);
                    MessageBox.Show("Un SMS contenant le code de récupération vient d'être envoyé au numéro indiqué.");
                    btnSendSMS.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur est survenue lors de l'envoi du SMS." + Environment.NewLine + "Code : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    btnSendSMS.Enabled = true;
                }
            }
        }

        private void btnConfirmer_Click(object sender, EventArgs e)
        {
            if (txtCodeRecup.Text == _sms.code.ToString())
            {
                _bgwGetPass = new BackgroundWorker();

                if (!_bgwGetPass.IsBusy)
                {
                    _bgwGetPass.DoWork             += _bgwGetPass_DoWork;
                    _bgwGetPass.RunWorkerCompleted += _bgwGetPass_RunWorkerCompleted;
                    _bgwGetPass.RunWorkerAsync();
                }
            }
            else
            {
                MessageBox.Show("Le code confidentiel entré est incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _bgwGetPass_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                tabControl.SelectedTab = tabGetPass;
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
                tabControl.SelectedTab = tabSendConfirmSMS;
            }
        }

        private void _bgwGetPass_DoWork(object sender, DoWorkEventArgs e)
        {
            _newPass.setNewPassword(txtTel, txtPass);
        }

        private void btnCopyPass_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtPass.Text);
        }
    }
}
