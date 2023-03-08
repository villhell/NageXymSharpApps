using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Services
{
    public class HealthCheckTriggerService
    {
        private readonly HttpClient httpClient;

        public HealthCheckTriggerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [Function("healthCheck")]
        public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        ILogger log)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }

}
