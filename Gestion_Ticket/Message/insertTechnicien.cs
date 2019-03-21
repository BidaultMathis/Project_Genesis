using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Gestion_Ticket.Message
{
    public class insertTechnicien
    {

        public int resultat { get; set; }

        public void addTechnicien(Label idTicket)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection())
                {
                    connection.ConnectionString = Login.Login.connectionString;
                    connection.Open();

                    const string query = "UPDATE Tickets SET refIdTechnicien=@idTechnicien WHERE idTicket=@idTicket";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new MySqlParameter("@idTechnicien", MySqlDbType.VarChar));
                        cmd.Parameters.Add(new MySqlParameter("@idTicket", MySqlDbType.Int16));
                        cmd.Parameters["@idTechnicien"].Value = frmConnexion.login.idUserM;
                        cmd.Parameters["@idTicket"].Value = idTicket.Text;

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            resultat = 1;
                        }
                        else
                        {
                            resultat = 0;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
