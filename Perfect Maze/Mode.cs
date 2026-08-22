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
    public partial class Mode : Form
    {
        public Mode()
        {
            InitializeComponent();
        }

        private void Mode_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TSession.Mode = TSession.Modes.Speedrun;
            Game game = new Game();
            game.Show();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TSession.Mode = TSession.Modes.FogOfWar;
            Game game = new Game();
            game.Show();
            Close();
        }
    }
}
