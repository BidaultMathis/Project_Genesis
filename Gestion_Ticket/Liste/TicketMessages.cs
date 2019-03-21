using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Gestion_Ticket.Liste
{
    public class TicketMessages
    {
        public string Messages { get; set; }
        public string idUser   { get; set; }

        public void getMessages(Label lblIdTicket, Panel panelMessages)
        {
            using (MySqlConnection connection = new MySqlConnection())
            {
                connection.ConnectionString = Login.Login.connectionString;
                connection.Open();

                const string query = "SELECT refIdUtilisateur, message, DateH FROM Reponses WHERE refIdTicket=@idTicket ORDER BY DateH ASC";

                using(MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new MySqlParameter("@idTicket", MySqlDbType.Int16));
                    cmd.Parameters["@idTicket"].Value = lblIdTicket.Text;

                    int space = 0;

                    using (MySqlDataReader _reader = cmd.ExecuteReader())
                    {
                        if (_reader.HasRows)
                        {
                            while (_reader.Read())
                            {
                                Messages = _reader.GetString("message");
                                idUser = _reader.GetString("refIdUtilisateur");

                                UI.ucMessage _ucMessage = new UI.ucMessage();
                                _ucMessage.lblIdUserM.Text  = idUser;
                                _ucMessage.txtMessageM.Text = Messages;

                                _ucMessage.Location = new System.Drawing.Point(4, 10 + (space++ * 93));
                                panelMessages.Invoke((MethodInvoker)delegate { panelMessages.Controls.Add(_ucMessage); });
                               
                            }
                        }
                    }
                }
            }
        }
    }
}
