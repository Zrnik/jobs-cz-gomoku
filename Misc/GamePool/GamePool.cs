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
        private bool showBoard;

        List<GameThread> GameThreads = new List<GameThread>();

        public GamePool(GameType type, bool showBoard)
        {
            this.gameType = type;
            this.showBoard = showBoard;
        }

        internal void Run(int threadAmount, int totalGames)
        {
            if (gameType == GameType.Manual)
            {
                threadAmount = 1;
            }

            int activeGames = ActiveThreadAmount();

            while (activeGames > 0 || GameThreads.Count < totalGames)
            {

                activeGames = ActiveThreadAmount();

                if (activeGames < threadAmount && GameThreads.Count < totalGames)
                {

                    int index = GameThreads.Count;

                    GameThread thread = new GameThread(index, showBoard);
                    GameThreads.Add(thread);
                    thread.Start(gameType);

                }

                activeGames = ActiveThreadAmount();
            }

            Console.Write("All games finished!");
            while (true) { }
        }

        private int ActiveThreadAmount()
        {
            int amount = 0;
            foreach (GameThread Thread in GameThreads)
            {
                if (Thread.Running())
                {
                    amount++;
                }
            }
            return amount;
        }
    }
}
