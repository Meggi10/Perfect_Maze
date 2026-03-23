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
        private bool bfsFlag = false;
        public Form1()
        {
            InitializeComponent();
            tBoard1.ScoreChanged += (score) => label1.Text = score.ToString();
            tBoard1.AllPointsCollected += () => label4.Visible = true;
            label4.Text = "Points collected! Go to the exit!";
            tBoard1.GameReset += () => label4.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bfsFlag)
            {
                if (!tBoard1.BfsAnimDone)
                    tBoard1.BfsAnimStep++;
                else
                {
                    bfsFlag = true;
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
            bfsFlag = false;
            reverseFlag = false;
            tBoard1.Reverse = false;
            tBoard1.PathCount = 0;
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bfsFlag = false;
            reverseFlag = true;
            tBoard1.Reverse = true;
            tBoard1.PathCount = tBoard1.Path.Count - 1;
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tBoard1.EscapePath();
            bfsFlag = true;
            timer1.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
