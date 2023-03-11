using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NageXymSharpApps.Shard.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Services
{
    public class TransactionStatusTriggerService : ServiceBase
    {
        private readonly HttpClient httpClient;
        public TransactionStatusTriggerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [Function("transactionStatus/{hash}")]
        public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        ILogger log, string hash)
        {
            HttpResponseData response = null;
            TransactionStatusResponse transactionStatusResponse = null;
            try
            {
                // ノード
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var nodeUrl = query["node_url"];

                var transactionStatusResult = await ServiceBase.httpClient.GetAsync(string.Format("{0}/transactionStatus/{1}", nodeUrl, hash));
                var content = await transactionStatusResult.Content.ReadAsStringAsync();
                transactionStatusResponse = JsonConvert.DeserializeObject<TransactionStatusResponse>(content);

                if (transactionStatusResponse == null)
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
            await response.WriteAsJsonAsync(transactionStatusResponse);
            return response;
        }
    }
}
