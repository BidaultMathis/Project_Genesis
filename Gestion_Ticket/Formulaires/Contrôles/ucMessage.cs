using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_Ticket.UI
{
    public partial class ucMessage : UserControl
    {
        public ucMessage()
        {
            InitializeComponent();
        }

        public Label lblIdUserM
        {
            get { return lblIdUser; }
            set { lblIdUser = value; }
        }

        public TextBox txtMessageM
        {
            get { return txtMessage; }
            set { txtMessage = value; }
        }
    }
}
