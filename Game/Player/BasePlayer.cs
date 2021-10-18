using gomoku.Game.InputType;
using gomoku.Game.Positioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Game.Player
{
    public class BasePlayer
    {
        private BaseInputType input;
        private char character;

        public BasePlayer(BaseInputType input, char character)
        {
            this.input = input;
            this.character = character;
        }

        public char getCharacter()
        {
            return this.character;
        }
        public BaseInputType getInput()
        {
            return this.input;
        }

        internal GameLoc GetNextMove(State state, BasePlayer player)
        {
            return this.input.GetNextMove(state, player);
        }

        public static bool IsSame(BasePlayer currentOccupant, BasePlayer locOccupant)
        {
            if (currentOccupant == null && locOccupant == null)
            {
                return true;
            }

            if (currentOccupant == null)
            {
                return false;
            }

            if (locOccupant == null)
            {
                return false;
            }

            return currentOccupant.Equals(locOccupant);
        }

        internal void Write(string text)
        {
            Console.WriteLine(this.Label() +": " + text);
        }

        public string Label()
        {
            return getInput().getName() + "(" + this.GetType().Name + ")["+this.getCharacter().ToString()+"]";
        }
    }
}
