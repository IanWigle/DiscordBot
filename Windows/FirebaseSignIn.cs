using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SynovianEmpireDiscordBot.Windows
{
    public partial class FirebaseSignIn : Form
    {
        public string username = "", password = "";
        public FirebaseSignIn()
        {
            InitializeComponent();
        }

        private void signInButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            password = textBox1.Text;
        }

        private void userNameBox_TextChanged(object sender, EventArgs e)
        {
            username = userNameBox.Text;
        }
    }
}
