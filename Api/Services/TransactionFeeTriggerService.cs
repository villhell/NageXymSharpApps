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
    public class TransactionFeeTriggerService : ServiceBase
    {
        private readonly HttpClient httpClient;
        public TransactionFeeTriggerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [Function("network/fees/transaction")]
        public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        ILogger log)
        {
            HttpResponseData response = null;
            TransactionFeeResponse transactionFeeResponse = null;
            try
            {
                // ノード
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var nodeUrl = query["node_url"];

                var accountResult = await ServiceBase.httpClient.GetAsync(string.Format("{0}/network/fees/transaction", nodeUrl));
                var content = await accountResult.Content.ReadAsStringAsync();
                transactionFeeResponse = JsonConvert.DeserializeObject<TransactionFeeResponse>(content);

                if (transactionFeeResponse == null)
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
            await response.WriteAsJsonAsync(transactionFeeResponse);
            return response;
        }
    }
}
