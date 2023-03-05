using System.Net.Http;

namespace Api.Services
{
    public class ServiceBase
    {
        protected static HttpClient httpClient = new HttpClient();
    }
}
