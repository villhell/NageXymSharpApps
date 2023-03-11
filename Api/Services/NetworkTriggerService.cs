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
    public class NetworkTriggerService : ServiceBase
    {
        private readonly HttpClient httpClient;
        public NetworkTriggerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [Function("network")]
        public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        ILogger log)
        {
            HttpResponseData response = null;
            NetworkResponse networkResponse = null;
            try
            {
                // ノード
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var nodeUrl = query["node_url"];

                var networkResult = await ServiceBase.httpClient.GetAsync(string.Format("{0}/network", nodeUrl));
                var content = await networkResult.Content.ReadAsStringAsync();
                networkResponse = JsonConvert.DeserializeObject<NetworkResponse>(content);

                if (networkResponse == null)
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
            await response.WriteAsJsonAsync(networkResponse);
            return response;
        }
    }
}
