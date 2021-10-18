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
    [Endpoint("api/v1/checkStatus")]
    class CheckStatus : RequestEntity
    {
        public string gameToken;

        public override ResponseEntity createResponseEntity(IRestResponse response)
        {

            if (response.StatusCode == System.Net.HttpStatusCode.OK || (int)response.StatusCode == 226) {

                GameStatus status = JsonConvert.DeserializeObject<GameStatus>(response.Content);
                status.isCompleted = (int)response.StatusCode == 226;
                return status;
            }

            return base.createResponseEntity(response);
        }
    }
}
