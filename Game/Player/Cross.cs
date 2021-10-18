using gomoku.Game.InputType;
using gomoku.Game.InputType.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Game.Player
{
    public class Cross : BasePlayer
    {
        public Cross(BaseInputType input) : base(input, 'X')
        {
        }
    }
}
