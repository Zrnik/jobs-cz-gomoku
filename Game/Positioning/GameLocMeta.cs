using gomoku.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.Game.Positioning
{
    public enum LineType
    {
        Horizontal,
        Vertical,
        DiagonalInc, // Increasing (/)
        DiagonalDec, // Decreasing (\)
    }

    public class GameLocMeta
    {
        private GameLoc _location;
        private Field _field;
        private BasePlayer _occupant;

        public Field Field { get { return _field; } }
        public GameLoc Location { get { return _location; } }
        public BasePlayer Owner { get { return _occupant; } }

        public GameLocMeta(Field field, GameLoc loc)
        {
            this._location = loc;
            this._field = field;
            this._occupant = _field.Occupant(loc);
        }

        #region Lines

        public Dictionary<LineType, GameLoc[]> GetLines()
        {
            Dictionary<LineType, GameLoc[]> dict = new Dictionary<LineType, GameLoc[]>();
            foreach (LineType type in Enum.GetValues(typeof(LineType)))
            {
                dict.Add(type, GetLine(type).ToArray());
            }
            return dict;
        }

        public GameLoc[] GetLine(LineType type)
        {

            switch (type)
            {
                case LineType.Horizontal:
                    return TrimLineAround(
                        Location, Owner,
                        ExpandLine(new GameLoc[] { Location }, 1, 0, 2, 2)
                    );

                case LineType.Vertical:
                    return TrimLineAround(
                        Location, Owner,
                        ExpandLine(new GameLoc[] { Location }, 0, 1, 2, 2)
                    );

                case LineType.DiagonalInc:
                    return TrimLineAround(
                        Location, Owner,
                        ExpandLine(new GameLoc[] { Location }, 1, -1, 2, 2)
                    );

                case LineType.DiagonalDec:
                    return TrimLineAround(
                        Location, Owner,
                        ExpandLine(new GameLoc[] { Location }, 1, 1, 2, 2)
                    );

                default:
                    return new GameLoc[] { Location };
            }
        }

        private GameLoc[] TrimLineAround(GameLoc center, BasePlayer owner, GameLoc[] line)
        {
            int index = Array.IndexOf(line, center);

            List<GameLoc> locations = new List<GameLoc>();

            // Forward
            for (int i = index; i < line.Length; i++)
            {
                BasePlayer occupant = Field.Occupant(line[i]);
                if (BasePlayer.IsSame(owner, occupant))
                {
                    locations.Add(line[i]);
                }
                else
                {
                    break;
                }
            }

            // Backwards
            for (int i = index - 1; i >= 0; i--)
            {
                BasePlayer occupant = Field.Occupant(line[i]);
                if (BasePlayer.IsSame(owner, occupant))
                {
                    locations.Insert(0, line[i]);
                }
                else
                {
                    break;
                }
            }

            return locations.ToArray();
        }

        internal bool isWinning()
        {
            if (Owner == null)
            {
                return false;
            }

            foreach (GameLoc[] line in this.GetLines().Values)
            {
                //Console.WriteLine("Line Size: " + line.Length);
                if (line.Length >= 5)
                {
                    return true;
                }
            }
            return false;
        }


        public static GameLoc[] ExpandLine(LineType type, GameLoc[] line, int backAmount, int frontAmount)
        {

            switch (type)
            {
                case LineType.Horizontal:
                    return ExpandLine(line, 1, 0, backAmount, frontAmount);

                case LineType.Vertical:
                    return ExpandLine(line, 0, 1, backAmount, frontAmount);

                case LineType.DiagonalInc:
                    return ExpandLine(line, 1, -1, backAmount, frontAmount);

                case LineType.DiagonalDec:
                    return ExpandLine(line, 1, 1, backAmount, frontAmount);

                default:
                    return new GameLoc[] { };
            }
        }


        private static GameLoc[] ExpandLine(GameLoc[] line, int koefX, int koefY, int backAmount, int frontAmount)
        {
            List<GameLoc> locs = new List<GameLoc>();

            if (line.Length == 0)
            {
                throw new ArgumentException("Line is empty!");
            }

            GameLoc firstLoc = line[0];
            GameLoc lastLoc = line[line.Length - 1];

            for (int i = 1; i <= backAmount; i++)
            {
                GameLoc newLoc = new GameLoc(
                   firstLoc.X - (i * koefX),
                   firstLoc.Y - (i * koefY)
                );
                               
                locs.Add(newLoc);
            }

            locs.Reverse();

            foreach (GameLoc original in line)
            {
                locs.Add(original);
            }

            for (int i = 1; i <= backAmount; i++)
            {
                GameLoc newLoc = new GameLoc(
                   lastLoc.X + (i * koefX),
                   lastLoc.Y + (i * koefY)
                );

                locs.Add(newLoc);
            }

            return locs.ToArray();
        }

        internal static GameLoc[] RemoveInvalid(GameLoc[] line, Field field)
        {
            List<GameLoc> Valid = new List<GameLoc>();

            foreach (GameLoc loc in line) {
                if (field.IsValid(loc)) {
                    Valid.Add(loc);
                }
            }

            return Valid.ToArray();
        }


        #endregion





    }
}
