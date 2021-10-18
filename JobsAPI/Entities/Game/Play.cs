using gomoku.JobsAPI.Entities.Responses;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.JobsAPI.Entities.Game
{
    [Endpoint("api/v1/play")]
    class Play : RequestEntity
    {
        public string gameToken;
        public int positionX;
        public int positionY;

        public override ResponseEntity createResponseEntity(IRestResponse response)
        {

            int code = (int)response.StatusCode;

            if (code == 201 || code == 226) {
                return JsonConvert.DeserializeObject<GameStatus>(response.Content);
            }


            return base.createResponseEntity(response);
        }

    }


}
