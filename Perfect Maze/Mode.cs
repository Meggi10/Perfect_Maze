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

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true || radioButton2.Checked == true)
                Next();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TSession.Mode = TSession.Modes.Speedrun;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TSession.Mode = TSession.Modes.FogOfWar;
        }

        private void Next()
        {
            DiffLvls diflvl = new DiffLvls();
            diflvl.Show();
            Close();
        }
    }
}
