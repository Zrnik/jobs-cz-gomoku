using gomoku.Game;
using gomoku.Game.Player;
using gomoku.Game.Positioning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gomoku.GUI
{
    public partial class GomokuBoard : Form
    {
        #region GUI Settings
        public const int ButtonSizeX = 20;
        public const int ButtonSizeY = 20;

        public const int ButtonGap = 0;

        public int clickCount = 0;
        public string clickLocation = GameLoc.Create(1, 1).ToString();
        #endregion

        #region Indicators

        public List<string> WinningMove = new List<string>();
        public List<string> BlockedWinningMove = new List<string>();

        public List<string> FourRow = new List<string>();
        public List<string> BlockedFourRow = new List<string>();

        public List<string> ThreeRow = new List<string>();
        public List<string> BlockedThreeRow = new List<string>();

        public List<string> TwoRow = new List<string>();

        #endregion

        #region Initialize
        public Dictionary<string, Button> PositionButton;

        private bool Hooked = false;

        public bool CloseAfter = true;

        public State State { get; }

        public GomokuBoard(State state)
        {
            this.State = state;
            SetupBoard();
        }        

        public void ShowInSeparateThread()
        {
            Thread GomokuBoardThread = new Thread(
                delegate ()
                {
                    this.ShowDialog();
                }
            );

            GomokuBoardThread.Start();
            while (!this.Hooked) { }
        }


        private void SetupBoard()
        {
            InitializeComponent();

            closeAfterCheck.Checked = true;

            InitializeSize();

            PositionButton = new Dictionary<string, Button>();
            for (int x = 1; x <= Settings.FieldSizeX; x++)
            {
                for (int y = 1; y <= Settings.FieldSizeY; y++)
                {
                    int LocX = ((x - 1) * (ButtonSizeX + ButtonGap));
                    int LocY = ((y - 1) * (ButtonSizeY + ButtonGap));

                    GameLoc loc = new GameLoc(x, y);


                    Button b = new Button();
                    b.Location = new Point(LocX, LocY);
                    b.Size = new Size(ButtonSizeX, ButtonSizeY);

                    /*b.BackColor = Color.White;
                    if (x % 10 == 0 || y % 10 == 0)
                    {
                        b.BackColor = Color.Cyan;
                    }
                    */

                    b.Click += delegate (object sender, EventArgs e)
                    {

                        clickLocation = loc.ToString();
                        clickCount = clickCount + 1;

                        this.Text = "Clicked on " + loc.ToString() + " Move: " + loc.ToMove().x + "/" + loc.ToMove().y;
                        gameStatus.Invalidate();
                        gameStatus.Refresh();

                    };

                    buttonPanel.Controls.Add(b);
                    PositionButton.Add(loc.ToString(), b);
                }
            }
        }

        internal void InvokeClose()
        {
            if (this.Visible) {             
                Invoke(new Action(()=> { this.Close(); }));
            }
        }

        #region InitializeSize
        private void InitializeSize()
        {
            int PanelWidth =
                Settings.FieldSizeX * (ButtonSizeX + ButtonGap)
            ;

            int PanelHeight =
                Settings.FieldSizeY * (ButtonSizeY + ButtonGap)
           ;

            this.FixSize(PanelWidth, PanelHeight);

            int PanelSizeOffsetX = this.Size.Width - buttonPanel.Size.Width;
            int PanelSizeOffsetY = this.Size.Height - buttonPanel.Size.Height;

            this.FixSize(PanelWidth + PanelSizeOffsetX, PanelHeight + PanelSizeOffsetY);


        }

        private void FixSize(int panelWidth, int panelHeight)
        {
            Size Result = new Size(panelWidth, panelHeight);
            this.MinimumSize = new Size(0, 0);
            this.MaximumSize = Result;
            this.Size = Result;
            this.MinimumSize = Result;
        }
        #endregion
        private void GomokuBoard_Load(object sender, EventArgs e)
        {
            Hook();
        }

        private void Hook()
        {
            this.State.Changed += OnStateChanged;
            Hooked = true;
        }

        private void OnStateChanged(object sender, StateChangedEventArgs e)
        {
            bool finished = State.Finished;

            if (State.Status != gameStatusLabel.Text)
            {

                Action change = new Action(() =>
                {
                    gameStatusLabel.Text = State.Status;
                    gameStatus.Invalidate();
                    gameStatus.Refresh();
                });

                try
                {
                    this.Invoke(change);
                }
                catch { }
            }

            for (int x = 1; x <= Settings.FieldSizeX; x++)
            {
                for (int y = 1; y <= Settings.FieldSizeY; y++)
                {
                    GameLoc loc = new GameLoc(x, y);
                    string locKey = loc.ToString();
                    GameLocMeta meta = loc.Meta(e.state.Field);
                    BasePlayer occupant = e.state.Field.Occupant(loc);
                    string newValue = "";

                    if (occupant != null)
                    {
                        newValue = occupant.getCharacter().ToString();
                    }

                    Button btn = PositionButton[locKey];


                    if (btn.Text != newValue)
                    {

                        Action change = new Action(() =>
                        {
                            btn.Text = newValue;
                        });

                        try
                        {
                            this.Invoke(change);
                        }
                        catch
                        { }

                    }

                    if (btn.BackColor != Color.Green && meta.isWinning())
                    {
                        SetButtonColor(btn, Color.Green);
                    }



                    if (WinningMove.Contains(locKey))
                    {
                        SetButtonColor(btn, Color.DarkGreen);
                    }

                    if (BlockedWinningMove.Contains(locKey))
                    {
                        SetButtonColor(btn, Color.Orange);
                    }

                    if (FourRow.Contains(locKey))
                    {
                        SetButtonColor(btn, Color.Purple);
                    }

                    if (BlockedFourRow.Contains(locKey))
                    {
                        SetButtonColor(btn, Color.DeepPink);
                    }



                    if (ThreeRow.Contains(locKey))
                    {
                        SetButtonColor(btn, Color.Yellow);
                    }

                    if (BlockedThreeRow.Contains(locKey))
                    {
                        SetButtonColor(btn, Color.YellowGreen);
                    }

                    if (TwoRow.Contains(locKey))
                    {
                        SetButtonColor(btn, Color.Silver);
                    }


                }
            }
        }

        private void SetButtonColor(Button btn, Color color)
        {
            Action change = new Action(() =>
            {
                btn.BackColor = color;
            });

            try
            {
                this.Invoke(change);
            }
            catch
            { }
        }


        #endregion

        private void closeAfterCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.CloseAfter = closeAfterCheck.Checked;
        }
    }
}
