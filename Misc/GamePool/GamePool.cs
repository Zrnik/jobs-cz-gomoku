using gomoku.Game.InputType.Types;
using gomoku.Game.Player;
using gomoku.JobsAPI;
using gomoku.JobsAPI.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Threading;

namespace gomoku.Misc.GamePool
{
    public enum GameType
    {
        JobsCZ, // Plays AI vs Jobs.cz
        Manual, // Plays Manual vs AI
        AI,     // Plays AI vs AI
    }

    class GamePool
    {
        public GameType gameType;

        List<GameThread> GameThreads = new List<GameThread>();

        public GamePool(GameType type)
        {
            this.gameType = type;
        }

        internal void Run(int threadAmount, int totalGames)
        {
            if (gameType == GameType.Manual) {
                threadAmount = 1;
            }

            int activeGames = ActiveThreadAmount();

            while (activeGames > 0 || GameThreads.Count < totalGames) {

                activeGames = ActiveThreadAmount();

                if (activeGames < threadAmount && GameThreads.Count < totalGames) {
                                                        
                    int index = GameThreads.Count;

                    GameThread thread = new GameThread(index);
                    GameThreads.Add(thread);
                    thread.Start(this);

                }

                activeGames = ActiveThreadAmount();
            }

            Console.Write("All games finished!");
            while (true) { }
        }

        public Controller CreateController(GameType type, int index)
        {

            switch (type)
            {
                case GameType.AI:
                    Controller aiGame = new Controller();
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

     

        private int ActiveThreadAmount()
        {
            int amount = 0;
            foreach (GameThread Thread in GameThreads) {
                if (Thread.Running())
                {
                    amount++;
                }
            }
            return amount;
        }




        private static Controller CreateJobsController(int index, bool DisplayMessages = false)
        {
            if (DisplayMessages)
            {
                Console.WriteLine("");
                Console.WriteLine("== Creating Jobs.cz Game as " + Jobs.UserId());
            }
            GameInfo game = Jobs.CreateGame();

            if (game == null) {
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
            else {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " | Game #" + index+" is waiting for opponent! ("+ game.gameId+")");
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

            if (DisplayMessages) {
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
