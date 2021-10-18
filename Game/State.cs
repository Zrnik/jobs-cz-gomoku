using gomoku.Game.InputType.Types;
using gomoku.Game.Player;
using gomoku.Game.Positioning;
using gomoku.GUI;
using gomoku.JobsAPI;
using System;
using System.Collections.Generic;

namespace gomoku.Game
{
    public class State
    {
        private string _status = "";
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private Field _field;
        private GomokuBoard _board;

        public Field Field
        {
            get { return _field; }
        }

        public GomokuBoard Board
        {
            get { return _board; }
        }

        private int CurrentPlayerIndex = 0;

        public List<BasePlayer> Players;

        public event EventHandler<StateChangedEventArgs> Changed;

        internal void AddMove(BasePlayer player, GameLoc loc)
        {
            this.Field.AddMove(loc, player);
        }

        public void InvokeChange()
        {
            string PlayerListString = "Players["+Players.Count+"]:   ";

            //int index = 0;
            foreach (BasePlayer p in Players)
            {
                string name = p.getInput().getName().ToLower();

                if (p.Equals(CurrentPlayer))
                {
                    name = name.ToUpper();
                }

                PlayerListString += name + "[" + p.getCharacter() + "]";
                PlayerListString += "   ";
            }

            Status = PlayerListString;
           

            EventHandler<StateChangedEventArgs> handler = Changed;
            StateChangedEventArgs e = new StateChangedEventArgs(this);
            handler?.Invoke(this, e);
        }

        public State()
        {
            this.Players = new List<BasePlayer>();
            this._field = new Field(Settings.FieldSizeX, Settings.FieldSizeY);
            this._board = new GomokuBoard(this);
        }

        internal BasePlayer GetWinner()
        {
            if (this.Finished) {
                return this.CurrentPlayer;
            }


            return null;
        }

        internal void AddPlayer(BasePlayer player)
        {
            this.Players.Add(player);
        }


        public BasePlayer CurrentPlayer {
            get { return Players[CurrentPlayerIndex];  }
        }

        public bool Finished {
            get
            {
                return this.Field.IsResolved();
            }
        }
  

        internal void Initialize()
        {
            InvokeChange();
        }

        internal void RequestMove()
        {
            GameLoc position = this.CurrentPlayer.GetNextMove(this, this.CurrentPlayer);
            this.Field.AddMove(position, this.CurrentPlayer);
            this.NextTurn();

            // Musime odeslat vitezny tah, jelikoz je hra dohrana a nikdo neni na tahu,
            // JobsCzAPI nedostane sanci jej odeslat, tak to duelame tady!
            if (Finished && this.CurrentPlayer.getInput() is JobsCzAPI) {
                JobsCzAPI player = (JobsCzAPI) this.CurrentPlayer.getInput();
                Jobs.Play(position, player.getGameInfo());
            }

            this.InvokeChange();
        }

        private void NextTurn()
        {
            CurrentPlayerIndex++;
            if (CurrentPlayerIndex >= Players.Count) {
                CurrentPlayerIndex = 0;
            }
        }
    }
}
