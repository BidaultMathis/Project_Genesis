using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Gestion_Ticket.Liste
{
    public class TicketDetails
    {

        public string Date              { get; set; }
        public string Incident          { get; set; }
        public string Priorite          { get; set; }
        public string Auteur            { get; set; }
        public string Technicien        { get; set; }
        public string Statut            { get; set; }
        public string DateFermeture     { get; set; }
        public string Description       { get; set; }
        public string Titre             { get; set; }
        public string URL               { get; set; }


        public void getCouleurPriorite(Label lblPriorite)
        {
            switch (lblPriorite.Text)
            {
                case "Faible":
                    lblPriorite.ForeColor = Color.Green;
                    break;
                case "Normal":
                    lblPriorite.ForeColor = Color.SteelBlue;
                    break;
                case "Grave":
                    lblPriorite.ForeColor = Color.Red;
                    break;
                case "Très grave":
                    lblPriorite.ForeColor = Color.Red;
                    break;
            }
        }

        public void getTicket(Label lblID)
        {
            using (MySqlConnection connection = new MySqlConnection())
            {
                connection.ConnectionString = Login.Login.connectionString;
                connection.Open();

                const string query = "SELECT tc.dateCreation, tc.dateFermeture, tc.refTypeIncident, tc.titre, tc.description, tc.refTypePriorite, tc.refIdUtilisateur," +
                                     "tc.refIdTechnicien, fl.refIdTicket, fl.Fichier FROM Tickets tc INNER JOIN FichiersJoints fl ON tc.idTicket = fl.refIdTicket WHERE" +
                                     " tc.idTicket=@idTicket";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new MySqlParameter("@idTicket", MySqlDbType.Int16));
                    cmd.Parameters["@idTicket"].Value = lblID.Text;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Date          = reader.GetDateTime("dateCreation").ToString("dd/MM/yyyy HH:mm");
                                Incident      = reader.GetString("refTypeIncident");
                                Priorite      = reader.GetString("refTypePriorite");
                                Auteur        = reader.GetString("refIdUtilisateur");
                                Description   = reader.GetString("description");
                                Titre         = reader.GetString("titre");

                                if (reader["refIdTechnicien"] != DBNull.Value)
                                    Technicien = reader.GetString("refIdTechnicien");
                                else
                                    Technicien = "";
                                

                                if (Technicien == "")
                                    Statut = "En attente";
                                else
                                    Statut = "Ouvert";


                                if (reader["Fichier"] != DBNull.Value)
                                    URL = reader.GetString("Fichier");
                                else
                                    URL = "";
                                

                                if (reader["dateFermeture"] != DBNull.Value)
                                {
                                    DateFermeture = reader.GetDateTime("dateFermeture").ToString("dd/MM/yyyy HH:mm");
                                    Statut = "Fermé";
                                }
                                else
                                {
                                    DateFermeture = "";
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Aucune donnée trouvée.");
                        }
                    }
                }
            }
        }

    }
}
