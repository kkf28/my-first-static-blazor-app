using System.Net;
using System.Security.Cryptography;
using System.Text;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public static class SignatureFunction
{
    [Function("Signature")]
    public static HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {     

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

var response = req.CreateResponse(HttpStatusCode.OK);
           response.WriteString(computedSignature);

            return response;
                }
}