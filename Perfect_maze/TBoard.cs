using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Perfect_maze
{
    public partial class TBoard : UserControl
    {
        public static int N = 20;
        public static float chamberSize = 0.5f;
        public TCell[,] Cells = new TCell[N, N];
        public TCell StartCell;
        public TCell EndCell;
        public List<TCell> EventCell = new List<TCell>();
        public int EventCount = 5;
        public static Random Rnd = new Random();
        public List<TCell> Path;
        public int PathCount;
        public int score = 0;
        public event Action<int> ScoreChanged;
        public bool Reverse { get; set; } = false;
        public TBoard()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Build();
            //this.TabStop = true;
        }
        public void Build()
        {
            for (int y = 0; y < N; y++)
                for (int x = 0; x < N; x++)
                {
                    var cell = new TCell();
                    cell.X = x;
                    cell.Y = y;
                    Cells[x, y] = cell;
                }
            StartCell = Cells[Rnd.Next(N), Rnd.Next(N)];
            EndCell = Cells[0, 0];
            EventCell.Clear();
            while (EventCell.Count < EventCount)
            {
                var nEvents = Cells[Rnd.Next(N), Rnd.Next(N)];
                if (nEvents != StartCell &&  nEvents != EndCell && !EventCell.Contains(nEvents))
                    EventCell.Add(nEvents);
            }
            var depthCells = new List<TCell>();
            var actCell = StartCell;
            depthCells.Add(actCell);
            Path = new List<TCell>();
            while (depthCells.Count > 0)
            {
                Path.Add(actCell);
                var freeCells = new List<TCell>();
                if (actCell.X > 0)
                {
                    var neighbour = Cells[actCell.X - 1, actCell.Y];
                    if (neighbour.Connected.Count == 0)
                        freeCells.Add(neighbour);
                }
                if (actCell.X < N - 1)
                {
                    var neighbour = Cells[actCell.X + 1, actCell.Y];
                    if (neighbour.Connected.Count == 0)
                        freeCells.Add(neighbour);
                }
                if (actCell.Y > 0)
                {
                    var neighbour = Cells[actCell.X, actCell.Y - 1];
                    if (neighbour.Connected.Count == 0)
                        freeCells.Add(neighbour);
                }
                if (actCell.Y < N - 1)
                {
                    var neighbour = Cells[actCell.X, actCell.Y + 1];
                    if (neighbour.Connected.Count == 0)
                        freeCells.Add(neighbour);
                }
                if (freeCells.Count > 0)
                {
                    var neighbour = freeCells[Rnd.Next(freeCells.Count)];
                    actCell.Connected.Add(neighbour);
                    neighbour.Connected.Add(actCell);
                    depthCells.Add(actCell);
                    actCell = neighbour;
                }
                else
                {
                    actCell = depthCells.Last();
                    depthCells.RemoveAt(depthCells.Count - 1);
                }
            }
            PathCount = Path.Count;
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Left:
                case Keys.Down:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            TCell neighbour = null;

            if (e.KeyCode == Keys.Escape)
                PathCount = Path.Count - 1;
            if (e.KeyCode == Keys.Up && StartCell.Y > 0)
                neighbour = Cells[StartCell.X, StartCell.Y - 1];
            else if (e.KeyCode == Keys.Down && StartCell.Y < N - 1)
                neighbour = Cells[StartCell.X, StartCell.Y + 1];
            else if (e.KeyCode == Keys.Left && StartCell.X > 0)
                neighbour = Cells[StartCell.X - 1, StartCell.Y];
            else if (e.KeyCode == Keys.Right && StartCell.X < N - 1)
                neighbour = Cells[StartCell.X + 1, StartCell.Y];
            if (neighbour != null && StartCell.Connected.Contains(neighbour))
            {
                StartCell = neighbour;

                if (EventCell.Contains(StartCell))
                {
                    score++;
                    EventCell.Remove(StartCell);
                    ScoreChanged?.Invoke(score);
                }

                if (StartCell == EndCell)
                {
                    MessageBox.Show("Congratulations!", "Maze solved!");
                    score = 0;
                    ScoreChanged?.Invoke(score);
                    Reset();
                }
            }
            Invalidate();
        }
        public void Reset()
        {
            for (int y = 0; y < N; y++)
                for (int x = 0; x < N; x++)
                    Cells[x, y] = new TCell() { X = x, Y = y };
            EndCell = Cells[0, 0];
            StartCell = Cells[Rnd.Next(N), Rnd.Next(N)];
            EventCell.Clear();
            while (EventCell.Count < EventCount)
            {
                var nEvent = Cells[Rnd.Next(N), Rnd.Next(N)];
                if (nEvent != StartCell && nEvent != EndCell && !EventCell.Contains(nEvent))
                    EventCell.Add(nEvent);
            }
            var cell = StartCell;
            List<TCell> depthCells = new List<TCell>();
            Path = new List<TCell>();
            do
            {
                Path.Add(cell);
                List<TCell> freeCells = new List<TCell>();
                var offset = new int[] { -1, 0, 0, 1, 1, 0, 0, -1 };
                for (int i = 0; i < 8; i += 2)
                {
                    var y = cell.Y + offset[i];
                    var x = cell.X + offset[i + 1];
                    if (y >= 0 && x >= 0 && y < N && x < N)
                    {
                        var neighbor = Cells[x, y];
                        if (neighbor.Connected.Count == 0)
                            freeCells.Add(neighbor);
                    }
                }
                if (freeCells.Count > 0)
                {
                    var neighbor = freeCells[Rnd.Next(freeCells.Count)];
                    cell.Connected.Add(neighbor);
                    neighbor.Connected.Add(cell);
                    depthCells.Add(cell);
                    cell = neighbor;
                }
                else
                {
                    cell = depthCells[depthCells.Count - 1];
                    depthCells.RemoveAt(depthCells.Count - 1);
                }
            } while (depthCells.Count > 0);
            Path.Add(StartCell);
            PathCount = Path.Count - 1;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var cellW = (float)Width / N;
            var cellH = (float)Height / N;
            var offset = (1 - chamberSize) / 2;
            e.Graphics.ScaleTransform(cellW, cellH);
            e.Graphics.TranslateTransform(offset, offset);

            if (Reverse)
            {
                var reverseBrush = new SolidBrush(Color.OrangeRed);
                for (int i = 1; i < Path.Count; i++)
                {
                    var actCell = Path[i];
                    var prevCell = Path[i - 1];
                    var rc = new RectangleF();
                    if (actCell.Y == prevCell.Y)
                    {
                        rc.X = Math.Min(prevCell.X, actCell.X);
                        rc.Y = actCell.Y;
                        rc.Width = 1 + chamberSize;
                        rc.Height = chamberSize;
                    }
                    else
                    {
                        rc.Y = Math.Min(prevCell.Y, actCell.Y);
                        rc.X = actCell.X;
                        rc.Height = 1 + chamberSize;
                        rc.Width = chamberSize;
                    }
                    e.Graphics.FillRectangle(reverseBrush, rc);
                }
            }
            var brush = new SolidBrush(ForeColor);
            for (int i = 1; i < PathCount; i++)
            {
                var actCell = Path[i];
                var prevCell = Path[i - 1];
                var rc = new RectangleF();
                if (actCell.Y == prevCell.Y)
                {
                    rc.X = Math.Min(prevCell.X, actCell.X);
                    rc.Y = actCell.Y;
                    rc.Width = 1 + chamberSize;
                    rc.Height = chamberSize;
                }
                else
                {
                    rc.Y = Math.Min(prevCell.Y, actCell.Y);
                    rc.X = actCell.X;
                    rc.Height = 1 + chamberSize;
                    rc.Width = chamberSize;
                }
                e.Graphics.FillRectangle(brush, rc);
            }
            var startBrush = new SolidBrush(Color.LimeGreen);
            var endBrush = new SolidBrush(Color.Gold);
            var eventsBrush = new SolidBrush(Color.Red);
            var rc_S = new RectangleF(StartCell.X, StartCell.Y, chamberSize, chamberSize);
            var rc_E = new RectangleF(EndCell.X, EndCell.Y, chamberSize, chamberSize);
            e.Graphics.FillRectangle(startBrush, rc_S);
            e.Graphics.FillRectangle(endBrush, rc_E);
            foreach (var events in EventCell)
            {
                var rc_Events = new RectangleF(events.X, events.Y, chamberSize, chamberSize);
                e.Graphics.FillRectangle(eventsBrush, rc_Events);
            }
        }
    }
}