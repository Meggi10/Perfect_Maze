using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Perfect_maze
{
    public partial class Game : Form
    {
        private readonly TGameSoundtrack Track = new TGameSoundtrack();
        //private readonly TSyntezator Syntezator = new TSyntezator();
        private Stopwatch watch = new Stopwatch();
        private bool reverseFlag = false;
        private bool algorithmFlag = false;
        private bool IsGameRunning = true;
        public int score = 0;
        public Game()
        {
            InitializeComponent();
            tBoard1.ScoreChanged += (score) => label1.Text = score.ToString();
            tBoard1.AllPointsCollected += () => label4.Visible = true;
            label4.Text = "Points collected! Go to the exit!";
            tBoard1.GameReset += () =>
            {
                label4.Visible = false;
                watch.Restart();
                timer2.Stop();
                label8.Text = "00:00.000";
            };
            Track.Play("Tracks/Project_73.mp3", volume: 0.1f);
            tBoard1.PlayerMove += StartGameTimer;
            label10.Text += TSession.PlayerName;
            label13.Text += THelpers.GetDisplayName(TSession.Mode);
            label14.Text += THelpers.GetLvlName(TSession.DifficultyLvl);
        }
        private void StartGameTimer()
        {
            watch.Restart();
            timer2.Start();
            IsGameRunning = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (algorithmFlag)
            {
                if (tBoard1.AlgoAnimDone) { timer1.Stop(); return; }

                tBoard1.AnimAlgoritmStep++;
                tBoard1.Invalidate();
                return;
            }
            if (reverseFlag)
            {
                if (tBoard1.PathCount <= 0) { timer1.Stop(); return; }

                tBoard1.PathCount--;
                tBoard1.Invalidate();
                return;
            }
            if (tBoard1.PathCount >= tBoard1.Path.Count) { timer1.Stop(); return; }
            tBoard1.PathCount++;
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
                TSession.Clear();
                Main_Menu form2 = new Main_Menu();
                form2.Show();
                Close();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (IsGameRunning)
            {
                label8.Text = watch.Elapsed.ToString(@"mm\:ss\.fff");
                tBoard1.time = (float)watch.Elapsed.TotalSeconds;
                if (tBoard1.StartCell == tBoard1.EndCell && tBoard1.EventCell.Count == 0)
                {
                    watch.Stop();
                    timer2.Stop();
                    IsGameRunning = false;
                    label8.Text = watch.Elapsed.ToString(@"mm\:ss\.fff");
                }
                tBoard1.Invalidate();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
