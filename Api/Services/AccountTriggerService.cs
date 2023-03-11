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
    public class AccountTriggerService : ServiceBase
    {
        private readonly HttpClient httpClient;
        public AccountTriggerService(HttpClient httpClient)
        {
            ServiceBase.httpClient = httpClient;
        }

        [Function("accounts/{address}")]
        public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        ILogger log, string address)
        {
            HttpResponseData response = null;
            AccountResponse ret = null;
            try
            {
                // ノード
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var nodeUrl = query["node_url"];


                // アドレスの文字列かどうか
                if (Api.Server.Utils.CheckWordCount(address, 39))
                {
                    var accountResult = await ServiceBase.httpClient.GetAsync(string.Format("{0}/accounts/{1}", nodeUrl, address));
                    var content = await accountResult.Content.ReadAsStringAsync();
                    ret = JsonConvert.DeserializeObject<AccountResponse>(content);

                    if (ret!.Account == null)
                    {
                        return null;
                    }
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
            await response.WriteAsJsonAsync<AccountResponse>(ret);
            return response;
        }
    }
}
