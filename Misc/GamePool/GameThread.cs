using gomoku.Game.InputType.Types;
using gomoku.Game.Player;
using gomoku.JobsAPI;
using gomoku.JobsAPI.Entities.Responses;
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
        private bool showBoard;

        public GameThread(int index, bool showBoard)
        {
            this.index = index;
            this.showBoard = showBoard;
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

        internal void Start(GameType type)
        {
            Worker = new Thread(delegate () {

                Controller controller = this.CreateController(type, index);

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
                    Console.WriteLine(
                        DateTime.Now.ToString("HH:mm:ss") 
                        + " | Game [" + index + "] finished, winner: " 
                        + controller.GetState().GetWinner().Label()
                    );
                }
                catch (WinnerAlreadyResolvedException ex) 
                {
                    Console.WriteLine(
                        DateTime.Now.ToString("HH:mm:ss")
                        + " | Game [" + index + "] finished, winner: "
                        + ex.winner.Label()
                    );
                }
                catch (OpponentAFKException)
                {
                    Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " | Game [" + index + "] stopped, opponent AFK!");
                }

                if (controller.GetState().Board.CloseAfter)
                {
                    controller.GetState().Board.InvokeClose();
                }



            });
            Worker.Start();
            Thread.Sleep(1000);
        }


        public Controller CreateController(GameType type, int index)
        {

            switch (type)
            {
                case GameType.AI:
                    Controller aiGame = new Controller(showBoard);
                    aiGame.AddPlayer(new Cross(new AI()));
                    aiGame.AddPlayer(new Circle(new AI()));
                    return aiGame;

                case GameType.Manual:
                    Controller manualGame = new Controller(true);
                    manualGame.AddPlayer(new Cross(new Manual()));
                    manualGame.AddPlayer(new Circle(new AI()));
                    return manualGame;

                case GameType.JobsCZ:
                    return CreateJobsController(index);

                default:
                    throw new Exception("Unsupported type!");

            }


        }


        private static Controller CreateJobsController(int index, bool DisplayMessages = false)
        {
            if (DisplayMessages)
            {
                Console.WriteLine("");
                Console.WriteLine("== Creating Jobs.cz Game as " + Jobs.UserId());
            }
            GameInfo game = Jobs.CreateGame();

            if (game == null)
            {
                if (DisplayMessages)
                {
                    JobsGameError("Unable to create game!");
                }
                return null;
            }

            if (DisplayMessages)
            {
                Console.WriteLine("== == == == == == == == == == == == ==");
                // GAME FOUND START
                Console.Write("== ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("GAME FOUND!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("");
                // GAME FOUND END
                Console.WriteLine("== gameId: " + game.gameId);
                Console.WriteLine("== == == == == == == == == == == == ==");
                Console.WriteLine("== Waiting for oponent! ");
            }
            else
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " | Game #" + index + " is waiting for opponent! (" + game.gameId + ")");

                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " | Token: " + game.gameToken);
            }

            GameStatus gs = Jobs.GameStatus(game);

            while (gs != null && gs.playerCircleId == null || gs.playerCrossId == null)
            {
                Thread.Sleep(1000);
                gs = Jobs.GameStatus(game);
            }


            if (gs == null)
            {
                if (DisplayMessages)
                {
                    JobsGameError("Unable to get game status!");
                }
                return null;
            }

            bool AmICross = gs.playerCrossId == Jobs.UserId();
            string oponentId = (AmICross ? gs.playerCircleId : gs.playerCrossId);

            if (DisplayMessages)
            {
                Console.WriteLine("== oponentId: " + oponentId);
                Console.WriteLine("== == == == == == == == == == == == ==");
            }

            string playerLabel = "";

            Controller controller = new Controller();

            if (AmICross)
            {
                BasePlayer cross = controller.AddPlayer(new Cross(new AI()));
                BasePlayer circle = controller.AddPlayer(new Circle(new JobsCzAPI(game, oponentId)));


                playerLabel = cross.Label();
            }
            else
            {
                controller.AddPlayer(new Cross(new JobsCzAPI(game, oponentId)));
                BasePlayer circle = controller.AddPlayer(new Circle(new AI()));
                playerLabel = circle.Label();
            }


            if (DisplayMessages)
            {
                Console.WriteLine("== Playing as: " + playerLabel);
                Console.WriteLine("== == == == == == == == == == == == ==");
                Console.WriteLine("== Game started!");
            }

            return controller;
        }
        
        private static void JobsGameError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Error: " + text);
        }
    }
}
