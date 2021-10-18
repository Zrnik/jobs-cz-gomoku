using gomoku.Game;
using gomoku.Game.Player;
using gomoku.Game.Positioning;
using System;
using System.Diagnostics;

namespace gomoku
{
    class Controller
    {
        private State State;

        public Controller(bool showBoard = false)
        {
            this.State = new State();
            if (showBoard) {
                this.State.Board.ShowInSeparateThread();
            }
            this.State.Initialize();
        }

        public BasePlayer AddPlayer(BasePlayer player)
        {
            this.State.AddPlayer(player);
            return player;
        }

        internal void AddMove(BasePlayer player, GameLoc loc)
        {
            this.State.AddMove(player, loc);
        }

        internal void Run()
        {
            State.InvokeChange();
            while (!State.Finished)
            {
                State.RequestMove();
            }

            State.InvokeChange();

        }

        internal void InvokeChange()
        {
            State.InvokeChange();
        }

        internal State GetState()
        {
            return this.State;
        }
    }
}
