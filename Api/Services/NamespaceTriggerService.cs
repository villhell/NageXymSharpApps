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
    public class NamespaceTriggerService : ServiceBase
    {
        private readonly HttpClient httpClient;

        public NamespaceTriggerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [Function("namespaces/{namespaceId}")]
        public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        ILogger log, string namespaceId)
        {
            HttpResponseData response = null;
            NamespaceResponse namespaceResponse = null;
            try
            {
                // ノード
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var nodeUrl = query["node_url"];

                var result = await ServiceBase.httpClient.GetAsync(string.Format("{0}/namespaces/{1}", nodeUrl, namespaceId));
                var content = await result.Content.ReadAsStringAsync();
                namespaceResponse = JsonConvert.DeserializeObject<NamespaceResponse>(content);

                if (namespaceResponse == null)
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
            await response.WriteAsJsonAsync(namespaceResponse);
            return response;
        }
    }
}
