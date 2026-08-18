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
    public partial class Scoreboard : Form
    {
        //public List<> Easy = new List<>();
        //public List<> Medium = new List<>();
        //public List<> Hard = new List<>();
        int selectedIdx;
        public Scoreboard()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Scoreboard_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main_Menu form2 = new Main_Menu();
            form2.Show();
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIdx = comboBox1.SelectedIndex;
            switch(selectedIdx)
            {
                case 0:
                    //...
                    break;
                case 1:
                    //...
                    break;
                case 2:
                    //...
                    break;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
