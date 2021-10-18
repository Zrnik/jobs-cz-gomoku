using gomoku.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gomoku.Misc.GamePool
{
    class GameThread
    {
        private Thread Worker;
        private int index;

        public GameThread(int index)
        {
            this.index = index;
        }
             
        internal bool Running()
        {
            if (Worker == null) {
                return false;
            }

            if (Worker.ThreadState == ThreadState.Stopped) {
                return false;
            }

            return true;
        }

        internal void Start(GamePool pool)
        {
            Worker = new Thread(delegate () {

                Controller controller = pool.CreateController(pool.gameType, index);

                List<string> labels = new List<string>();


                foreach (BasePlayer player in controller.GetState().Players)
                {
                    labels.Add(player.Label());
                }

                string vs = "";

                bool first = true;
                foreach (string label in labels) {

                    if (!first) {
                        vs += " vs ";
                    }
                    vs += label;
                    first = false;
                }


                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " | Starting game [" + index + "]: " + vs);

                try
                {
                    controller.Run(); 
                    Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " | Game [" + index + "] finished, winner: " + controller.GetState().GetWinner().Label());
                }
                catch (OpponentAFKException)
                {
                    Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " | Game [" + index + "] stopped, opponent AFK!");
                }

                controller.GetState().Board.InvokeClose();



            });
            Worker.Start();
        }
    }
}
