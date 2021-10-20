using gomoku.Misc.GamePool;
using System;
using System.Windows.Forms;

namespace gomoku
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Console.WriteLine("====================================================================");
            Console.WriteLine("== Zrny Gomoku Bot for https://piskvorky.jobs.cz/ ");
            Console.WriteLine("====================================================================");


            GamePool gp = new GamePool(GameType.JobsCZ);
            gp.Run(3,9);
        }
    }
}
