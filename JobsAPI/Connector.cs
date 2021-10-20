using gomoku.JobsAPI.Entities;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace gomoku.JobsAPI
{
    class Connector
    {
        private string userId;
        private string userToken;
        private RestClient client;



        private static Connector _singletonInstance;
        public static Connector Instance
        {
            get
            {
                if (_singletonInstance == null)
                {

                    /*
                     * Registrační ůdaje pro Jobs.cz API
                     * 
                     *  - tohle má být tajné, ale mě je to šumák :D
                     *  
                     *  - prosím nezneužívat!
                     *  - zneužití se trestá!
                     *  - mezi tresty patří například:
                     *      - budu smutný
                     *      - integrita klání v Jobs.cz piškvorkách bude narušena!
                     *      
                     *      S tím vším budete muset žít! Vy si jednou vzpomenete 
                     *      jak jste zneužili mé registrační údaje a neusnete kvuli
                     *      tomu. BYLI JSTE VAROVÁNÍ!
                     *
                     * ======= Zrny:
                        {
                            "statusCode": 201,
                            "userId": "b73de46a-2185-444f-8a17-733b37df1a0a",
                            "userToken": "278e43fb-ec4b-4b3d-b522-9f52fd576025",
                            "headers": {}
                        } 
                     * ======= Test:
                        {
                            "statusCode": 201,
                            "userId": "729a5f97-b49b-4176-b843-d9b4f097da20",
                            "userToken": "e895a4af-19f7-4f08-9b5c-d20039f86191",
                            "headers": {}
                        }
                    	
                        Test 2
                        {
                          "statusCode": 201,
                          "userId": "2ff1f8f4-fbf6-4a83-8916-ad802a04edac",
                          "userToken": "b56e44e6-5dd1-4b34-98b3-901df184f843",
                          "headers": {}
                        }
                     *
                     */

                    _singletonInstance = new Connector(
                        "2ff1f8f4-fbf6-4a83-8916-ad802a04edac",
                        "b56e44e6-5dd1-4b34-98b3-901df184f843"
                    );
                }

                return _singletonInstance;

            }
        }

        internal string UserId()
        {
            return this.userId;
        }

        public Connector(string userId, string userToken)
        {
            this.userId = userId;
            this.userToken = userToken;

            client = new RestClient(
                "https://piskvorky.jobs.cz/"
            );
        }


        public ResponseEntity Post(RequestEntity requestEntity)
        {

            requestEntity.userToken = userToken;

            EndpointAttribute attr =
                (EndpointAttribute)Attribute.GetCustomAttribute(
                    requestEntity.GetType(), typeof(EndpointAttribute)
                );

            RestRequest request = new RestRequest(
                attr.EndpointAddress, DataFormat.Json
            );

            string json = JsonConvert.SerializeObject(requestEntity);

            /*Console.WriteLine("-----------------------------");
            Console.WriteLine("-- REQUEST [" + attr.EndpointAddress + "]");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
            Console.WriteLine(json);
            Console.WriteLine();*/

            request.AddParameter(
                "application/json",
                json,
                ParameterType.RequestBody
            );

            return requestEntity.createResponseEntity(client.Post(request));
        }
    }
}
