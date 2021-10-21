using gomoku.Game.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Game.Positioning
{
    public class Field
    {

        private Dictionary<string, BasePlayer> Positions;
        public Field(int Width, int Height)
        {
            this.Positions = new Dictionary<string, BasePlayer>();
            SizeX = Width;
            SizeY = Height;
        }

        internal Field Clone()
        {
            Field clone = new Field(this.SizeX, this.SizeY);

            clone.Positions = new Dictionary<string, BasePlayer>(this.Positions);

            clone._lastMove = this._lastMove;

            return clone;
        }

        public void AddMove(GameLoc point, BasePlayer player)
        {

            if (!IsValid(point))
            {
                throw new ArgumentException("Point " + point.ToString() + " is not valid!");
            }

            if (Positions.ContainsKey(point.ToString()))
            {
                Positions[point.ToString()] = player;
            }
            else
            {
                Positions.Add(point.ToString(), player);
            }
            _lastMove = point;
        }

        private GameLoc _lastMove;

        public readonly int SizeX;
        public readonly int SizeY;

        internal GameLoc GetLastMove()
        {
            return _lastMove;
        }

        public GameLoc[] OccupiedPlaces()
        {

            List<GameLoc> places = new List<GameLoc>();

            foreach (string key in this.Positions.Keys)
            {
                GameLoc loc = GameLoc.Parse(key);
                places.Add(loc);
            }

            return places.ToArray();
        }

        internal bool IsValid(GameLoc point)
        {
            if (point.X < 1)
            {
                return false;
            }

            if (point.Y < 1)
            {
                return false;
            }

            if (point.X > SizeX)
            {
                return false;
            }

            if (point.Y > SizeY)
            {
                return false;
            }

            return true;
        }

        internal int OccupiedCount()
        {
            return OccupiedPlaces().Count();
        }

        public bool IsOccuppied(GameLoc point)
        {
            return this.Occupant(point) != null;
        }

        public BasePlayer Occupant(GameLoc point)
        {
            if (Positions.ContainsKey(point.ToString()))
            {
                return Positions[point.ToString()];
            }

            return null;
        }

        internal bool IsResolved()
        {
            for (int x = 1; x <= SizeX; x++)
            {
                for (int y = 1; y <= SizeY; y++)
                {
                    if (GameLoc.Create(x, y).Meta(this).isWinning())
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /**
         * Aby AI vs. AI  hry byly trocku rozmanite.
         * Nyni je nahodny jen ten prvni tah kolecka a pak 
         * uz je vse deterministicke. Takze existuje jen 8 
         * ruznych vysledku.
         */
        public XY[] getShuffledLocations() {

            List<XY> locations = new List<XY>();

            for (int x = 1; x <= SizeX; x++)
            {
                for (int y = 1; y <= SizeY; y++)
                {
                    locations.Add(new XY()
                    {
                        x = x,
                        y = y
                    });
                }
            }

            locations.Shuffle();
            return locations.ToArray();        
        }

    }
}
