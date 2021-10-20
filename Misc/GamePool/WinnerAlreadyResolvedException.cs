using gomoku.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Misc.GamePool
{
    class WinnerAlreadyResolvedException : Exception
    {
        public BasePlayer winner;

        public WinnerAlreadyResolvedException(BasePlayer player)
        {
            this.winner = player;
        }
    }
}
