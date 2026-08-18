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
        public List<string> Names = new List<string>();
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
            else if (Names.Contains(nick))
                label3.Text = "That name already exists. Please enter another name.";
            else if (TWordFilter.IsForbidden(nick))
                label3.Text = "This name is not allowed! Please choose another name.";
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
            if (Names.Contains(nick))
            {
                label3.Text = "That name already exists. Please enter another name.";
                return;
            }
            if (TWordFilter.IsForbidden(nick))
            {
                label3.Text = "This name is not allowed! Please choose another name.";
                return;
            }
            Names.Add(nick);
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
