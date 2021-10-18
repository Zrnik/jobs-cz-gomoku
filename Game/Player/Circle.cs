using gomoku.Game.InputType;
using gomoku.Game.InputType.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Game.Player
{
    public class Circle : BasePlayer
    {
        public Circle(BaseInputType input) : base(input, 'O')
        {
        }
    }
}
