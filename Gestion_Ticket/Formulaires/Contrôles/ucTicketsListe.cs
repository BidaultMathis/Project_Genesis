using System.Windows.Forms;

namespace Gestion_Ticket.UI
{
    public partial class ucTicketsListe : UserControl
    {
        public ucTicketsListe()
        {
            InitializeComponent();
        }
        public static string ID { get; set; }

        public Label lblIdTicketM
        {
            get { return lblIdTicket; }
            set { lblIdTicket = value; }
        }
        public Label lblTypeIncidentM
        {
            get { return lblTypeIncident; }
            set { lblTypeIncident = value; }
        }
        public LinkLabel lblTitreM
        {
            get { return lblTitre; }
            set { lblTitre = value; }
        }
        public Label lblPrioriteM
        {
            get { return lblPriorite; }
            set { lblPriorite = value; }
        }
        public Label lblAuteurM
        {
            get { return lblAuteur; }
            set { lblAuteur = value; }
        }
        public Label lblTechnicienM
        {
            get { return lblTechnicien; }
            set { lblTechnicien = value; }
        }
        public Label lblDateOuvertureM
        {
            get { return lblDateOuverture; }
            set { lblDateOuverture = value; }
        }

        private void lblTitre_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ID = lblTitre.Tag.ToString();
            frmTicketDetails frmTicketDetails = new frmTicketDetails();
            frmTicketDetails.ShowDialog();
        }
    }
}
