using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text;

namespace Gestion_Ticket.NewPass
{
    public class NewPass
    {

        public string Motdepasse { get; set; }

        private string randomPass()
        {
            int length = 13;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res  = new StringBuilder();
            Random rnd         = new Random();

            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }

        public void setNewPassword(TextBox txtTel, TextBox txtPass)
        {

            Motdepasse = randomPass();

            try
            {
                using (MySqlConnection connection = new MySqlConnection())
                {
                    connection.ConnectionString = Login.Login.connectionString;
                    connection.Open();

                    const string query = "UPDATE Utilisateurs SET Motdepasse=@mdp WHERE telUtilisateur=@tel";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new MySqlParameter("@mdp", MySqlDbType.VarChar));
                        cmd.Parameters.Add(new MySqlParameter("@tel", MySqlDbType.Int16));
                        cmd.Parameters["@mdp"].Value = Motdepasse;
                        cmd.Parameters["@tel"].Value = txtTel.Text;
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            txtPass.Text = Motdepasse;
                        }
                        else
                        {
                            MessageBox.Show("Impossible de mettre à jour votre mot de passe", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible de se connecter au serveur distant. Veuillez contacter un administrateur.", "Erreur", MessageBoxButtons.OK);
                Application.Restart();
            }
        }
    }
}
