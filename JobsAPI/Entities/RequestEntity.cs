using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoku.JobsAPI.Entities
{
    abstract class RequestEntity
    {
        public string userToken;

        public virtual ResponseEntity createResponseEntity(
            IRestResponse response
        ) {

            try {
                JsonConvert.DeserializeObject<Error>(response.Content);        
            } catch { }

            return new Error() {
                statusCode = 0,
                errors = new Dictionary<string, string>() {
                    { "message", "Invalid response!" },
                    { "content", response.Content }
                }
            };

        }

    }
}
