using System;
using System.Drawing;
using System.Windows.Forms;
using Gestion_Ticket.UI;
using MySql.Data.MySqlClient;

namespace Gestion_Ticket.Liste
{
    public class Tickets
    {

        private int idTicket;
        public void setIdTicket(int pIdTicket)
        {
            idTicket = pIdTicket;
        }
        public int getidTicket()
        {
            return idTicket;
        }


        private string typeIncident;
        public void setTypeIncident(string pTypeIncedent)
        {
            typeIncident = pTypeIncedent;
        }
        public string getTypeIncident()
        {
            return typeIncident;
        }


        private string titreTicket;
        public void setTitreTicket(string pTitreTicket)
        {
            titreTicket = pTitreTicket;
        }
        public string getTitreTicket()
        {
            return titreTicket;
        }


        private string priorite;
        public void setPriorite(string pPriorite)
        {
            priorite = pPriorite;
        }
        public string getPriorite()
        {
            return priorite;
        }


        private string auteur;
        public void setAuteur(string pAuteur)
        {
            auteur = pAuteur;
        }
        public string getAuteur()
        {
            return auteur;
        }


        private string technicien;
        public void setTechnicien(string pTechnicien)
        {
            technicien = pTechnicien;
        }
        public string getTechnicien()
        {
            return technicien;
        }


        private DateTime dateOuverture;
        public void setDateOuverture(DateTime pDateOuverture)
        {
            dateOuverture = pDateOuverture;
        }
        public DateTime getDateOuverture()
        {
            return dateOuverture;
        }

        private void getCouleurPriorite(ucTicketsListe _ticketsListe)
        {
            switch (frmTicket._tickets.getPriorite())
            {
                case "Faible":
                    _ticketsListe.lblPrioriteM.ForeColor = Color.Green;
                    break;
                case "Normal":
                    _ticketsListe.lblPrioriteM.ForeColor = Color.SteelBlue;
                    break;
                case "Grave":
                    _ticketsListe.lblPrioriteM.ForeColor = Color.Red;
                    break;
                case "Très grave":
                    _ticketsListe.lblPrioriteM.ForeColor = Color.Red;
                    break;
            }
        }
        
        public void getAllTickets(Panel panel)
        {
            try
            {
                using (MySqlConnection _connection = new MySqlConnection())
                {
                    _connection.ConnectionString = Login.Login.connectionString;
                    _connection.Open();

                    const string query = "SELECT idTicket, refTypeIncident, titre, refTypePriorite, refIdUtilisateur, refIdTechnicien, dateCreation FROM Tickets";
                    using (MySqlCommand _cmd = new MySqlCommand(query, _connection))
                    {
                        using (MySqlDataReader _reader = _cmd.ExecuteReader())
                        {
                            int space = 0;

                            if (_reader.HasRows)
                            {
                                while (_reader.Read())
                                {
                                    setAuteur       (_reader.GetString("refIdUtilisateur"));
                                    setDateOuverture(_reader.GetDateTime("dateCreation"));
                                    setIdTicket     (_reader.GetInt16("idTicket"));
                                    setPriorite     (_reader.GetString("refTypePriorite"));
                                    setTitreTicket  (_reader.GetString("titre"));
                                    setTypeIncident (_reader.GetString("refTypeIncident"));

                                    if (_reader["refIdTechnicien"] != DBNull.Value)
                                    {
                                        setTechnicien(_reader.GetString("refIdTechnicien"));
                                    }
                                    else
                                    {
                                        setTechnicien("");
                                    }

                                    ucTicketsListe _ticketsListe = new ucTicketsListe();

                                    _ticketsListe.lblAuteurM.Text        = getAuteur();
                                    _ticketsListe.lblDateOuvertureM.Text = getDateOuverture().ToString("dd/MM/yyyy");
                                    _ticketsListe.lblIdTicketM.Text      = getidTicket().ToString();
                                    _ticketsListe.lblPrioriteM.Text      = getPriorite();
                                    _ticketsListe.lblTechnicienM.Text    = getTechnicien();
                                    _ticketsListe.lblTitreM.Text         = getTitreTicket();
                                    _ticketsListe.lblTypeIncidentM.Text  = getTypeIncident();
                                    _ticketsListe.lblTitreM.Tag          = getidTicket();

                                    getCouleurPriorite(_ticketsListe);

                                    _ticketsListe.Location = new Point(82, 3 + (space++ * 150));
                                    panel.Invoke((MethodInvoker)delegate { panel.Controls.Add(_ticketsListe); });
                                }
                            }
                        }
                    }
                }
            }
            catch(MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Impossible de se connecter au serveur distant. Veuillez contacter un administrateur.", "Erreur", MessageBoxButtons.OK);
                        break;
                    case 1045:
                        MessageBox.Show("", "Erreur", MessageBoxButtons.OK);
                        break;
                }
            }
        }
    }
}