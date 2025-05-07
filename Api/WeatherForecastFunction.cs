using System.Net;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;
        private readonly JisiluClient client;

        public HttpTrigger(ILoggerFactory loggerFactory, JisiluClient client)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
            this.client = client;
        }

        [Function("QDII")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            await client.LoginAsync();
            //return JsonConvert.SerializeObject(await client.GetFundAsync());
            var result = await client.GetFundAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            //await response.WriteAsJsonAsync(result);
            await response.WriteStringAsync(JsonConvert.SerializeObject(result));
            return response;
        }

        private string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing";
            }

            return summary;
        }
        private static bool SecureCompare(string a, string b)
        {
            if (a.Length != b.Length) return false;
            int result = 0;
            for (int i = 0; i < a.Length; i++)
                result |= a[i] ^ b[i];
            return result == 0;
        }
    }
}
