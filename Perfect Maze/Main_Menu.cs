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
    public partial class Main_Menu : Form
    {
        public Main_Menu()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayerNameForm playerForm = new PlayerNameForm();
            playerForm.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Game_Rules game_rules = new Game_Rules();
            game_rules.Show();
            Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure, you want to exit?",
                "Exit",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Credits credits = new Credits();
            credits.Show();
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Scoreboard scoreboard = new Scoreboard();
            scoreboard.Show();
            Hide();
        }
    }
}
