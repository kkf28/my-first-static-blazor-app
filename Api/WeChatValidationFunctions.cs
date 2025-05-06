using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public static class WeChatValidationFunctions
    {
        // [Function("WeChatValidation")]
        // public static HttpResponseData Run(
        //     [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        //     ILogger log)
        // {
        //     log.LogInformation("Processing WeChat validation request.");

        //     string? signature = req.Query["signature"];
        //     string? timestamp = req.Query["timestamp"];
        //     string? nonce = req.Query["nonce"];
        //     string? echostr = req.Query["echostr"];

        //     if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(echostr))
        //     {
        //         log.LogWarning("Missing required query parameters.");
        //         return req.CreateResponse(HttpStatusCode.BadRequest);
        //     }

        //     // Add your signature validation logic here if needed.

        //     log.LogInformation("WeChat validation successful.");
        //     return new OkObjectResult(echostr);
        // }

        [Function("WeChatValidation")]
        public static HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // logger.LogInformation("WeChatValidation function processed a request.");
            var query = req.Url.Query;
            var signature = req.Query["signature"]?? string.Empty;
            var timestamp = req.Query["timestamp"];
            var nonce = req.Query["nonce"];
            var echostr = req.Query["echostr"]?? string.Empty;

            var token = "abcdefg"; // 从环境变量中获取 token //Environment.GetEnvironmentVariable("WECHAT_TOKEN")??
            
            // 排序与哈希计算（与 PHP 逻辑一致）
            var sortedParams = new[] { token, timestamp, nonce }.OrderBy(s => s);
            var combinedStr = string.Concat(sortedParams);
            using var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(combinedStr));
            var computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            // 安全比较
            if (SecureCompare(computedSignature, signature))
            {
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.WriteString(echostr);
                return response;
            }
            return req.CreateResponse(HttpStatusCode.Forbidden);
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