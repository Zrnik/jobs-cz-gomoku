using gomoku.JobsAPI.Entities.Responses.Subs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.JobsAPI.Entities.Responses
{
    class GameStatus: ResponseEntity
    {
        public string playerCrossId;
        public string playerCircleId;
        public string actualPlayerId;
        public string winnerId;
        public Move[] coordinates;

        public bool isCompleted;
    }
}
