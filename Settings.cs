using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku
{
    static class Settings
    {
        // Field size from https://piskvorky.jobs.cz/api/doc

        public static int FieldMinX = -28;
        public static int FieldMaxX = 28;

        public static int FieldMinY = -20;
        public static int FieldMaxY = 20;
        



       /*
        *  Below are generated!
        */

        public static int FieldSizeX
        {
            get { return FieldMaxX - FieldMinX + 1; }
        }
        public static int FieldSizeY
        {
            get { return FieldMaxY - FieldMinY + 1; }
        }
    }
}
