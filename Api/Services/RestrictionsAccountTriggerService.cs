using CatSdk.Symbol;
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
    public class RestrictionsAccountTriggerService : ServiceBase
    {
        private readonly HttpClient httpClient;
        public RestrictionsAccountTriggerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [Function("restrictionsaccount")]
        public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        ILogger log)
        {
            HttpResponseData response = null;
            RestrictionsAccountResponse restrictionsAccountResponse = null;
            try
            {
                // ノード
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var nodeUrl = query["node_url"];
                var address = query["address"];

                var result = await ServiceBase.httpClient.GetAsync(string.Format("{0}/restrictions/account?address={1}", nodeUrl, address));

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    restrictionsAccountResponse = JsonConvert.DeserializeObject<RestrictionsAccountResponse>(content);

                }
                else
                {
                    // internal server error
                    response = req.CreateResponse(result.StatusCode);
                    //await response.WriteStringAsync();
                    return response;
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
            await response.WriteAsJsonAsync(restrictionsAccountResponse);
            return response;
        }
    }
}
