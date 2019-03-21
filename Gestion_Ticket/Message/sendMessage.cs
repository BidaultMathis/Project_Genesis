using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Gestion_Ticket.Message
{
    public class sendMessage
    {

        public int result { get; set; }

        public void insertMessage(Label idTicket, TextBox message)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection())
                {
                    connection.ConnectionString = Login.Login.connectionString;
                    connection.Open();

                    const string query = "INSERT INTO Reponses (refIdTicket, refIdUtilisateur, Message, DateH) VALUES (@idTicket, @idUser, @message, @date)";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new MySqlParameter("@idTicket", MySqlDbType.Int16));
                        cmd.Parameters.Add(new MySqlParameter("@idUser", MySqlDbType.VarChar));
                        cmd.Parameters.Add(new MySqlParameter("@message", MySqlDbType.VarChar));
                        cmd.Parameters.Add(new MySqlParameter("@date", MySqlDbType.DateTime));
                        cmd.Parameters["@idTicket"].Value = idTicket.Text;
                        cmd.Parameters["@idUser"].Value = frmConnexion.login.idUserM;
                        cmd.Parameters["@message"].Value = message.Text;
                        cmd.Parameters["@date"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        int reader = cmd.ExecuteNonQuery();

                        if (reader > 0)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
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
