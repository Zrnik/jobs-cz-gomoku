using gomoku.JobsAPI.Entities.Responses.Subs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Game.Positioning
{
    public class GameLoc
    {        
        public int X { get { return _x; } }    
        public int Y { get { return _y; } }

        private int _x;
        private int _y;

        public GameLoc(int xCoord, int yCoord)
        {
            _x = xCoord;
            _y = yCoord;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + this._x + this._y;
        }

        public static GameLoc Create(int x, int y)
        {
            return new GameLoc(x,y);
        }

        public override bool Equals(object o)
        {
            if (o is GameLoc)
            {
                GameLoc that = (GameLoc)o;
                return this._x == that._x && this._y == that._y;
            }
            return false;
        }

        public override string ToString()
        {
            return this._x.ToString() + "-" + this._y.ToString();
        }

        public GameLoc AddY(int val)
        {
            _y = _y + val;
            return this;
        }

        public GameLoc AddX(int val)
        {
            _x = _x + val;
            return this;
        }

        public Move ToMove()
        {
            return new Move()
            {
                x = this.X + Settings.FieldMinX - 1,
                y = this.Y + Settings.FieldMinY - 1,
            };
        }

        public static GameLoc FromMove(Move m)
        {

            int offsetX = Math.Abs(Settings.FieldMinX);
            int offsetY = Math.Abs(Settings.FieldMinY);

            int locX = m.x + offsetX + 1;
            int locY = m.y + offsetY + 1;

            return GameLoc.Create(locX, locY);
        }

        public GameLoc Clone()
        {
            return new GameLoc(_x, _y);
        }

        public static GameLoc Parse(string key)
        {
            string[] parts = key.Split('-');
            if (parts.Length == 2) {
                int locX = 0;
                int locY = 0;
                int.TryParse(parts[0], out locX);
                int.TryParse(parts[1], out locY);
                return new GameLoc(locX, locY);
            }
            return null;
        }

        public GameLocMeta Meta(Field field)
        {
            return new GameLocMeta(field, this);
        }
    }
}
