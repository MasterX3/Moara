using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moara
{
    public partial class ServerSelector : Form
    {
        private Form1 form;

        public ServerSelector(Form1 form)
        {
            this.form = form;
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            int port = 0;
            try
            {
                port = int.Parse(tbPort.Text);
            } catch(Exception ex)
            {
                MessageBox.Show("Port invalid!");
                return;
            }
            form.Connect(tbIp.Text, port);
            this.Close();
        }
    }
}
