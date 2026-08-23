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
    public partial class DiffLvls : Form
    {
        public DiffLvls()
        {
            InitializeComponent();
        }

        private void DiffLvls_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TBoard.N = 10;
            TBoard.EventCount = 5;
            TBoard.SpecialEventCount = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TBoard.N = 20;
            TBoard.EventCount = 10;
            TBoard.SpecialEventCount = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            TBoard.N = 30;
            TBoard.EventCount = 15;
            TBoard.SpecialEventCount = 3;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true || radioButton2.Checked == true || radioButton3.Checked == true)
                GoToGame();
        }

        private void GoToGame()
        {
            Game game = new Game();
            game.Show();
            Close();
        }
    }
}
