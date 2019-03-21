using System;
using System.Collections.Specialized;
using System.Net;
using System.Windows.Forms;

namespace Gestion_Ticket.API
{
    public class SMS
    {

        public string resultat      { get; set; }
        public int code             { get; set; } 
        public string numero        { get; set; }
        public string message       { get; set; }
        public string messageEncode { get; set; }

        Random _random;
        WebClient _webClient;


        public void sendSMS(TextBox txtCounty, TextBox txtNumero)
        {
            _random = new Random();

            code          = _random.Next(10001, 99999);
            numero        = txtCounty + txtNumero.Text;
            message       = "Voici votre code de vérification : " + code;
            messageEncode = WebUtility.UrlEncode(message);

            using (_webClient  = new WebClient())
            {
                byte[] reponse = _webClient.UploadValues("https://api.txtlocal.com/send/", new NameValueCollection()
                {
                {"apikey" , "hIobzpijaX8-miGqEiWtNLKVW1T74G1gFERmVxJhmT"},
                {"numbers" , numero},
                {"message" , messageEncode},
                {"sender" , "Genesis"}
                });

                resultat = System.Text.Encoding.UTF8.GetString(reponse);
            }
        }
    }
}
