using gomoku.Game.Player;
using gomoku.Game.Positioning;
using gomoku.GUI;
using gomoku.JobsAPI;
using gomoku.JobsAPI.Entities;
using gomoku.JobsAPI.Entities.Game;
using gomoku.JobsAPI.Entities.Responses;
using gomoku.JobsAPI.Entities.Responses.Subs;
using gomoku.Misc.GamePool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gomoku.Game.InputType.Types
{
    public class JobsCzAPI : BaseInputType
    {
        private readonly GameInfo gameInfo;
        private string opponentId;

        public JobsCzAPI(GameInfo gameInfo, string opponentId) : base() {
            this.gameInfo = gameInfo;
            this.opponentId = opponentId;
        }

        internal GameStatus GameStatus { get; private set; }

        public override GameLoc GetNextMove(State state, BasePlayer player)
        {
            GameStatus status = Jobs.GameStatus(gameInfo);

            bool isAIMove = status.actualPlayerId == Jobs.UserId();

            if (isAIMove) {
                status = Jobs.Play(state.Field.GetLastMove(), gameInfo);
            }

            Stopwatch s = new Stopwatch();
            while (true)
            {
                isAIMove = status.actualPlayerId == Jobs.UserId();

                if (isAIMove)
                {      
                    return GameLoc.FromMove(status.coordinates[0]);
                }

                if (s.ElapsedMilliseconds > 2 *60 * 1000) {
                    throw new OpponentAFKException();
                }

                Thread.Sleep(1000);
                status = Jobs.GameStatus(gameInfo);
            }
        }

        internal GameInfo getGameInfo()
        {
            return gameInfo;
        }

        internal override string getName()
        {
            return "Jobs.cz " + opponentId;
        }

    }
}
