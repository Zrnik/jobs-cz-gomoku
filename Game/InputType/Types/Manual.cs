using gomoku.Game.Player;
using gomoku.Game.Positioning;
using gomoku.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Game.InputType.Types
{
    public class Manual : BaseInputType
    {
        public override GameLoc GetNextMove(State state, BasePlayer player)
        {
            GameLoc move = null;

            int click = state.Board.clickCount;
            while (move == null) {

                // Check for every click
                if (click != state.Board.clickCount) {
                    click = state.Board.clickCount;


                    // Check if move is OK
                    GameLoc newMove = GameLoc.Parse(state.Board.clickLocation);

                    if (newMove != null && state.Field.IsValid(newMove) && !state.Field.IsOccuppied(newMove)) {
                        move = newMove;
                        break;
                    }
                }
            }

            return move;
        }
    }
}
