using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Perfect_maze
{
    public partial class Form1 : Form
    {
        private bool reverseFlag = false;
        public int score = 0;
        private bool algorithmFlag = false;
        int selectedIdx;
        private readonly TGameSoundtrack Track = new TGameSoundtrack();
        //private readonly TSyntezator Syntezator = new TSyntezator();
        public Form1()
        {
            InitializeComponent();
            tBoard1.ScoreChanged += (score) => label1.Text = score.ToString();
            tBoard1.AllPointsCollected += () => label4.Visible = true;
            label4.Text = "Points collected! Go to the exit!";
            tBoard1.GameReset += () => label4.Visible = false;
            Track.Play("Tracks/Project_73.mp3", volume: 0.5f);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (algorithmFlag)
            {
                if (!tBoard1.BfsAnimDone)
                    tBoard1.AnimAlgoritmStep++;
                else
                {
                    algorithmFlag = true;
                    timer1.Stop();
                }
            }
            else if (reverseFlag)
            {
                if (tBoard1.PathCount > 0)
                    tBoard1.PathCount--;
                else
                    timer1.Stop();
            }
            else
            {
                if (tBoard1.PathCount < tBoard1.Path.Count)
                    tBoard1.PathCount++;
                else
                    timer1.Stop();
            }
            tBoard1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            algorithmFlag = false;
            reverseFlag = false;
            tBoard1.Reverse = false;
            tBoard1.PathCount = 0;
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            algorithmFlag = false;
            reverseFlag = true;
            tBoard1.Reverse = true;
            tBoard1.PathCount = tBoard1.Path.Count - 1;
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tBoard1.EscapePathBFS();
            algorithmFlag = true;
            timer1.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tBoard1.EscapePathAStar();
            algorithmFlag = true;
            timer1.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure, you want to exit the game?",
                "Exit",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Track.Stop();
                Form2 form2 = new Form2();
                form2.Show();
                Hide();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIdx = comboBox1.SelectedIndex;
            if (selectedIdx == 0)
                selectedIdx = TBoard.Rnd.Next(1, 4);
            DialogResult result = MessageBox.Show(
                "Are you sure, you want change the difficulty level? All points will be lost!",
                "Change",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                switch (selectedIdx)
                {
                    case 1:
                        TBoard.N = 10;
                        TBoard.EventCount = 5;
                        break;
                    case 2:
                        TBoard.N = 20;
                        TBoard.EventCount = 10;
                        break;
                    case 3:
                        TBoard.N = 30;
                        TBoard.EventCount = 15;
                        break;
                }
                tBoard1.Build();
                tBoard1.Invalidate();
            }
        }
    }
}
