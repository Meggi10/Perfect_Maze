using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Perfect_maze
{
    public partial class PlayerNameForm : Form
    {
        public PlayerNameForm()
        {
            InitializeComponent();
        }

        private void PlayerNameForm_Load(object sender, EventArgs e)
        {
            TWordFilter.Load();
            TNickName.Load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main_Menu form2 = new Main_Menu();
            form2.Show();
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string error = ValidateNick(textBox1.Text);
            label3.Text = error ?? "";
        }

        private string ValidateNick(string nick)
        {
            if (string.IsNullOrWhiteSpace(nick))
                return "Please enter your name!";
            if (TWordFilter.IsForbidden(nick))
                return "This name is not allowed! Please choose another name.";
            if (nick.Length < 3)
                return "Name must be at least 3 characters long.";
            if (nick.Length >= 16)
                return "Name must be no more than 15 characters long.";
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nick = textBox1.Text;
            string error = ValidateNick(nick);
            if (error != null)
            {
                label3.Text = error;
                return;
            }
            TSession.PlayerName = nick;
            Mode mode = new Mode();
            mode.Show();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = TNickName.GetRandomNickName();
        }
    }
}
