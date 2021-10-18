using gomoku.Game.Player;
using gomoku.Game.Positioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Game.InputType
{
    public abstract class BaseInputType
    {
        private readonly Guid hash;

        public BaseInputType() {
            this.hash = Guid.NewGuid();
        }

        internal virtual string getName()
        {
            return this.GetType().Name;
        }

        public abstract GameLoc GetNextMove(State state, BasePlayer player);

        protected GameLoc MoveRandom(State state)
        {
            // If any occupied, random around it
            GameLoc previous = state.Field.GetLastMove();
            if (previous != null) {

                List<GameLoc> possible = new List<GameLoc>();

                possible.Add(previous.Clone().AddX(1));
                possible.Add(previous.Clone().AddX(-1));

                possible.Add(previous.Clone().AddY(1));
                possible.Add(previous.Clone().AddY(-1));

                possible.Add(previous.Clone().AddX(1).AddY(1));
                possible.Add(previous.Clone().AddX(1).AddY(-1));
                possible.Add(previous.Clone().AddX(-1).AddY(1));
                possible.Add(previous.Clone().AddX(-1).AddY(-1));

                possible.Shuffle();

                foreach (GameLoc loc in possible) {
                    if (state.Field.IsValid(loc) && !state.Field.IsOccuppied(loc)) {
                        return loc;
                    }
                }
            }



            // If none ocupied, use center
            if (state.Field.OccupiedCount() == 0) {
                return new GameLoc(
                    (int) Math.Round(Settings.FieldSizeX / 2d),
                    (int) Math.Round(Settings.FieldSizeY / 2d)
                );
            }

            // If nothing found, Just go for 1/1...
            return new GameLoc(1,1);
        }

        public override bool Equals(object obj)
        {
            if (obj is BaseInputType) {
                BaseInputType that = (BaseInputType)obj;
                return this.hash == that.hash;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.hash.GetHashCode();
        }
    }
}
