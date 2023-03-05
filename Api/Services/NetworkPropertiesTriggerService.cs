using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NageXymSharpApps.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace Api.Services
{
    public class NetworkPropertiesTriggerService : ServiceBase
    {
        private readonly HttpClient httpClient;
        public NetworkPropertiesTriggerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [Function("network/properties")]
        public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        ILogger log)
        {
            HttpResponseData response = null;
            NetworkPropertiesResponse networkPropertiesResponse = null;
            try
            {
                // ノード
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var nodeUrl = query["node_url"];

                var result = await ServiceBase.httpClient.GetAsync($"{nodeUrl}/network/properties");
                var content = await result.Content.ReadAsStringAsync();
                networkPropertiesResponse = JsonConvert.DeserializeObject<NetworkPropertiesResponse>(content);

                if (networkPropertiesResponse == null)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // internal server error
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync(ex.Message);
                return response;
            }

            response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(networkPropertiesResponse);
            return response;
        }
    }
}
