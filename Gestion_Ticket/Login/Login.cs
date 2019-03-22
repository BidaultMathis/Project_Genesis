using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Gestion_Ticket.Login
{
    public class Login
    {
        public string idUserM         { get; set; }
        public string NomM            { get; set; }
        public string PrenomM         { get; set; }
        public int StatutM            { get; set; }
        public string StatutConvertiM { get; set; }
        public int estConnecteM       { get; set; }
        public string Motdepasse      { get; set; }

        public static string connectionString = "server=sl-eu-gb-p00.dblayer.com;Port=17327; userid=admin; password=7cf2409d326db0c197844dce15f3aee5c9dc8fa265b7a60d38de; database=Tickets";

        private void CheckGrade()
        {
            switch (StatutM)
            {
                case 0:
                    estConnecteM = 1;
                    StatutConvertiM = "Utilisateur";
                    break;
                case 1:
                    estConnecteM = 1;
                    StatutConvertiM = "Technicien";
                    break;
                case 2:
                    estConnecteM = 0;
                    MessageBox.Show("Votre accès a été refusé, veuillez prendre contact avec un Administrateur.", "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                default:
                    estConnecteM = 0;
                    MessageBox.Show("Identification de votre statut impossible, veuillez prendre contact avec un Administrateur.", "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }


        public void Connexion()
        {
            try
            {
                using (MySqlConnection _connection = new MySqlConnection())
                {
                    _connection.ConnectionString = connectionString;
                    _connection.Open();

                    const string query = "SELECT idUtilisateur, Motdepasse, Nom, Prenom, refGradeUtilisateur FROM Utilisateurs WHERE idUtilisateur=@user AND Motdepasse=@pass";
                    using (MySqlCommand _cmd = new MySqlCommand(query, _connection))
                    {
                        _cmd.Parameters.Add(new MySqlParameter("@user", MySqlDbType.VarChar));
                        _cmd.Parameters.Add(new MySqlParameter("@pass", MySqlDbType.VarChar));
                        _cmd.Parameters["@user"].Value = idUserM;
                        _cmd.Parameters["@pass"].Value = Motdepasse;

                        using (MySqlDataReader _result = _cmd.ExecuteReader())
                        {
                            if (_result.HasRows)
                            {
                                while (_result.Read())
                                {
                                    idUserM    = _result.GetString("idUtilisateur");
                                    NomM       = _result.GetString("Nom");
                                    PrenomM    = _result.GetString("Prenom");
                                    StatutM    = _result.GetInt16("refGradeUtilisateur");
                                    Motdepasse = _result.GetString("Motdepasse");
                                }

                                CheckGrade();
                            }
                            else
                            {
                                estConnecteM = 0;
                                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
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
