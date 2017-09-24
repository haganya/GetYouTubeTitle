/*

23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
23.09.2017 - Hakan Akgül - haganya.com
 
*/

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Youtube_Title_Haganya
{
    partial class HakkimdaForm : Form
    {
        public HakkimdaForm()
        {
            InitializeComponent();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.haganya.com/iletisim");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HakkimdaForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.twitch.tv/haganya");
        }
    }
}
