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
            var nick = textBox1.Text;
            if (string.IsNullOrWhiteSpace(nick))
                label3.Text = "Please enter your name!";
            else if (TWordFilter.IsForbidden(nick))
                label3.Text = "This name is not allowed! Please choose another name.";
            else if (nick.Length < 3)
                label3.Text = "Name must be at least 3 characters long.";
            else if (nick.Length >= 16)
                label3.Text = "Name must be no more than 15 characters long.";
            else
                label3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var nick = textBox1.Text;
            if (string.IsNullOrWhiteSpace(nick))
            {
                label3.Text = "Please enter your name!";
                return;
            }
            if (nick.Length < 3)
            {
                label3.Text = "Your name is too short. Name must contain minimum 3 characters.";
                return;
            }
            if (nick.Length >= 16)
            {
                label3.Text = "Your name is too long. Name can contain maximum 15 characters.";
                return;
            }
            if (TWordFilter.IsForbidden(nick))
            {
                label3.Text = "This name is not allowed! Please choose another name.";
                return;
            }
            TSession.PlayerName = nick;
            Game form1 = new Game();
            form1.Show();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = TNickName.GetRandomNickName();
        }
    }
}
