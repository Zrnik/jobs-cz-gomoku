using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.JobsAPI.Entities.Responses
{
    public class GameInfo : ResponseEntity
    {
        public string gameToken;
        public string gameId;
    }
}
