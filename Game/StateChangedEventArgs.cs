using System;

namespace gomoku.Game
{
    public class StateChangedEventArgs : EventArgs
    {
        public State state;

        public StateChangedEventArgs(State state)
        {
            this.state = state;
        }
    }
}