using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.JobsAPI.Entities
{
    class Error : ResponseEntity
    {
        public Dictionary<string, string> errors;
    }
}
