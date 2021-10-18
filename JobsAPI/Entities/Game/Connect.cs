using gomoku.JobsAPI.Entities.Responses;
using Newtonsoft.Json;
using RestSharp;

namespace gomoku.JobsAPI.Entities.Game
{
    [Endpoint("api/v1/connect")]
    class Connect : RequestEntity
    {

        public override ResponseEntity createResponseEntity(IRestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Created) {
                return JsonConvert.DeserializeObject<GameInfo>(response.Content);
            }

            return base.createResponseEntity(response);
        }

    }
}
