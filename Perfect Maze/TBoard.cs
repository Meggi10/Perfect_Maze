using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Perfect_maze
{
    public partial class TBoard : UserControl
    {
        public List<TCell> EventCell = new List<TCell>();
        public List<TCell> SpecialCell = new List<TCell>();
        private List<TCell> algorithmPath = new List<TCell>();
        public List<TCell> Path;
        public static Random Rnd = new Random();
        public TCell[,] Cells = new TCell[N, N];
        public TCell StartCell;
        public TCell EndCell;
        public event Action<int> ScoreChanged;
        public event Action AllPointsCollected;
        public event Action GameReset;
        public event Action PlayerMove;
        public static int N = 30;
        public static float chamberSize = 0.9f;
        public static int EventCount = 15;
        public static int SpecialEventCount = 3;
        private static readonly Brush ReverseBrush = new SolidBrush(Color.Gray);
        private static readonly Brush StartBrush = new SolidBrush(Color.LimeGreen);
        private static readonly Brush EndBrush = new SolidBrush(Color.Gold);
        private static readonly Brush EventBrush = new SolidBrush(Color.Red);
        private Brush ForeBrush = new SolidBrush(Color.Black);
        private const int SnakeLength = 4;
        private static readonly Brush[] FadeBrush = Enumerable.Range(0, 11).Select(i => (Brush)new SolidBrush(Color.FromArgb(i * 25, Color.DeepSkyBlue))).ToArray();
        public int PathCount;
        public int score = 0;
        public int AnimAlgoritmStep = 0;
        public float time;
        private bool IsTeleport;
        private bool FirstMove = true;
        public TMapRevealManager MapReveal;
        public bool Reverse { get; set; } = false;
        public bool AlgoAnimDone => AnimAlgoritmStep >= algorithmPath.Count + SnakeLength;
        public TBoard()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Build();
        }

        public void Build()
        {
            algorithmPath = new List<TCell>();
            AnimAlgoritmStep = 0;
            for (int y = 0; y < N; y++)
                for (int x = 0; x < N; x++)
                {
                    var cell = new TCell();
                    cell.X = x;
                    cell.Y = y;
                    Cells[x, y] = cell;
                }
            HandleCellLanding();
            Path = TAlgorithm.GenerationMazeDFS(Cells, N, StartCell, Rnd);
            PathCount = Path.Count;
            score = 0;
            ScoreChanged?.Invoke(score);

            if (TSession.Mode == TSession.Modes.MapReveal)
            {
                MapReveal = new TMapRevealManager(N, r: 3);
                MapReveal.RevealArea(StartCell.X, StartCell.Y, Cells);
            }
            else
                MapReveal = null;
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
            var x = StartCell.X;
            var y = StartCell.Y;
            switch (e.KeyCode)
            {
                case Keys.Up: y--; break;
                case Keys.Left: x--; break;
                case Keys.Right: x++; break;
                case Keys.Down: y++; break;
            }
            if (x >= 0 && y >= 0 && x < N && y < N)
            {
                var neighbour = Cells[x, y];
                if (StartCell.Connected.Contains(neighbour))
                {
                    if (FirstMove)
                    {
                        FirstMove = false;
                        PlayerMove?.Invoke();
                    }
                    StartCell = neighbour;
                    MapReveal?.RevealArea(StartCell.X, StartCell.Y, Cells);
                    if (EventCell.Contains(StartCell) && neighbour != EndCell)
                    {
                        IsTeleport = SpecialCell.Contains(StartCell);
                        EventCell.Remove(StartCell);
                        SpecialCell.Remove(StartCell);
                        score++;
                        ScoreChanged?.Invoke(score);
                        if (IsTeleport)
                        {
                            do
                            {
                                StartCell = Cells[Rnd.Next(N), Rnd.Next(N)];
                            }
                            while (EventCell.Contains(StartCell) || StartCell == EndCell);
                        }
                        if (EventCell.Count == 0)
                            AllPointsCollected?.Invoke();
                    }
                }
                if (StartCell == EndCell && EventCell.Count == 0)
                {
                    MessageBox.Show("Congratulations!\nYour time: " +
                        TimeSpan.FromSeconds(time).ToString(@"mm\:ss\.fff"), "Maze solved!");
                    score = 0;
                    ScoreChanged?.Invoke(score);
                    Reset();
                    GameReset?.Invoke();
                }
                Invalidate();
            }
        }

        public void EscapePathBFS()
        {
            algorithmPath = TAlgorithm.BFS(StartCell, EndCell) ?? new List<TCell>();
            AnimAlgoritmStep = 0;
            Invalidate();
        }

        public void EscapePathAStar()
        {
            algorithmPath = TAlgorithm.AStar(StartCell, EndCell) ?? new List<TCell>();
            AnimAlgoritmStep = 0;
            Invalidate();
        }

        private void HandleCellLanding()
        {
            StartCell = Cells[Rnd.Next(N), Rnd.Next(N)];
            do
            {
                EndCell = Cells[Rnd.Next(N), Rnd.Next(N)];
            }
            while (EndCell == StartCell);
            EventCell.Clear();
            while (EventCell.Count < EventCount)
            {
                var nEvent = Cells[Rnd.Next(N), Rnd.Next(N)];
                if (nEvent != StartCell && nEvent != EndCell && !EventCell.Contains(nEvent))
                    EventCell.Add(nEvent);
            }
            SpecialCell.Clear();
            while (SpecialCell.Count < SpecialEventCount)
            {
                var sEvent = EventCell[Rnd.Next(EventCell.Count)];
                if (!SpecialCell.Contains(sEvent))
                    SpecialCell.Add(sEvent);
            }
        }

        public void Reset()
        {
            algorithmPath = new List<TCell>();
            AnimAlgoritmStep = 0;
            for (int y = 0; y < N; y++)
                for (int x = 0; x < N; x++)
                    Cells[x, y] = new TCell() { X = x, Y = y };
            HandleCellLanding();
            Path = TAlgorithm.GenerationMazeDFS(Cells, N, StartCell, Rnd);
            PathCount = Path.Count - 1;
            FirstMove = true;
            if (TSession.Mode == TSession.Modes.MapReveal)
            {
                MapReveal = new TMapRevealManager(N, r: 3);
                MapReveal.RevealArea(StartCell.X, StartCell.Y, Cells);
            }
            else
                MapReveal = null;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            ForeBrush?.Dispose();
            ForeBrush = new SolidBrush(ForeColor);
            Invalidate();
        }

        private static RectangleF SegmentRect(TCell prevCell, TCell actCell, float chamberSize)
        {
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
            return rc;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var cellW = (float)Width / N;
            var cellH = (float)Height / N;
            var offset = (1 - chamberSize) / 2;
            e.Graphics.ScaleTransform(cellW, cellH);
            e.Graphics.TranslateTransform(offset, offset);
            bool HiddenMap = MapReveal != null;
            if (Reverse)
            {
                for (int i = 1; i < Path.Count; i++)
                {
                    if (HiddenMap && (!MapReveal.IsVisible(Path[i - 1].X, Path[i - 1].Y) || !MapReveal.IsVisible(Path[i].X, Path[i].Y)))
                        continue;

                    e.Graphics.FillRectangle(ReverseBrush, SegmentRect(Path[i - 1], Path[i], chamberSize));
                }
            }
            for (int i = 1; i < PathCount; i++)
            {
                if (HiddenMap && (!MapReveal.IsVisible(Path[i - 1].X, Path[i - 1].Y) || !MapReveal.IsVisible(Path[i].X, Path[i].Y)))
                    continue;

                e.Graphics.FillRectangle(ForeBrush, SegmentRect(Path[i - 1], Path[i], chamberSize));
            }
            if (algorithmPath.Count > 1)
            {
                int end = Math.Min(AnimAlgoritmStep, algorithmPath.Count);
                int start = Math.Max(1, AnimAlgoritmStep - SnakeLength);
                start = Math.Min(start, algorithmPath.Count);
                for (int i = Math.Max(1, start); i < end; i++)
                {
                    float t = (i - start + 1) / (float)SnakeLength;
                    int alpha = (int)(10 * t);
                    e.Graphics.FillRectangle(FadeBrush[alpha], SegmentRect(algorithmPath[i - 1], algorithmPath[i], chamberSize));
                }
            }
            if (!HiddenMap || MapReveal.IsVisible(EndCell.X, EndCell.Y))
                e.Graphics.FillRectangle(EndBrush, new RectangleF(EndCell.X, EndCell.Y, chamberSize, chamberSize));
            e.Graphics.FillRectangle(StartBrush, new RectangleF(StartCell.X, StartCell.Y, chamberSize, chamberSize));

            foreach (var events in EventCell)
            {
                if (HiddenMap && !MapReveal.IsVisible(events.X, events.Y))
                    continue;

                e.Graphics.FillRectangle(EventBrush, new RectangleF(events.X, events.Y, chamberSize, chamberSize));
            }
        }
    }
}