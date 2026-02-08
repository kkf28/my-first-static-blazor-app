using BlazorApp.Shared;
using Newtonsoft.Json;
using System.Net;

public class JisiluClient : IDisposable
{
    private readonly HttpClient _client;
    private readonly CookieContainer _cookieContainer = new();
    private const string CookieFile = "stock_cookie.txt";

    // public IConfiguration Configuration { get; }

    public JisiluClient()//IConfiguration configuration
    {
        // 配置 HTTP 处理器
        var handler = new HttpClientHandler
        {
            CookieContainer = _cookieContainer,
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true, // 禁用 SSL 验证
            UseCookies = true,
            AutomaticDecompression = DecompressionMethods.All
        };

        _client = new HttpClient(handler);

        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        // Configuration = configuration;
        //LoadCookies(); // 加载持久化的 Cookie
    }
    class Login
    {
        public string aes { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string auto_login { get; set; }
    }
    // 登录操作
    public async Task LoginAsync()
    {
        var loginUrl = "https://www.jisilu.cn/account/ajax/login_process/";
        loginUrl = "https://www.jisilu.cn/webapi/account/login_process/";

        //var login = new Login();
        //var testUrl = "https://www.jisilu.cn/webapi/account/login_process/";//Configuration.GetValue<string>("AuthEndpoint");
        //loginUrl = "https://www.jisilu.cn/webapi/account/login_process/";//Configuration.GetValue<string>("AuthEndpoint");
        //var login =Configuration.GetSection("Auth:Login").Get<Login>();//.Bind(login);//.GetValue<string>("user_name");
        // Console.WriteLine(JsonConvert.SerializeObject(login, Formatting.Indented));
        // 构建加密的请求参数
        var postData = new Dictionary<string, string>
        {
            //["_post_type"] = "ajax",
            ["aes"] = "1",
            ["user_name"] = "5330b38333d45af6244a7eee195161b8",//"6150adc1f2aac9f96ee68db93a406c39",
            ["password"] = "a7f66260901803ab3609075effef8cb2",//"2ec05c00f691d78fa9dc1e4c679e9861",
            ["auto_login"] = "1"
        };

        // 发送 POST 请求
        var content = new FormUrlEncodedContent(postData);
        var response = await _client.PostAsync(loginUrl, content);
        //var response = await _client.PostAsJsonAsync(loginUrl, login);
        response.EnsureSuccessStatusCode();

        //SaveCookies(); // 保存最新的 Cookie
    }

    public async Task<QDIIModel> GetFundAsync()
    {
        var url1 = $"https://www.jisilu.cn/data/qdii/qdii_list/A?___jsl=LST___t={DateTime.UtcNow.Ticks}&only_lof=y&rp=22&page=1";
        var url2 = $"https://www.jisilu.cn/data/qdii/qdii_list/E?___jsl=LST___t={DateTime.UtcNow.Ticks}&only_lof=y&rp=22&page=1";
        var url3 = $"https://www.jisilu.cn/data/qdii/qdii_list/C?___jsl=LST___t={DateTime.UtcNow.Ticks}&only_lof=y&rp=22&page=1";
        var url4 = $"https://www.jisilu.cn/data/lof/index_lof_list/?___jsl=LST___t={DateTime.UtcNow.Ticks}&rp=25";

        var response1 = await _client.GetAsync(url1);
        response1.EnsureSuccessStatusCode();
        var ret1 = await response1.Content.ReadAsStringAsync();
        QDIIModel data1 = JsonConvert.DeserializeObject<QDIIModel>(ret1) ?? new();

        //Asia
        //
        foreach (var row in data1.Rows ?? [])
        {
            // var a=row.Cell.DiscountRate2;
            var b = row.Cell.DiscountRate;
            row.Cell.DiscountRate2 = b;
            // row.Cell.DiscountRate = a;
        }

        var response2 = await _client.GetAsync(url2);
        response2.EnsureSuccessStatusCode();
        var ret2 = await response2.Content.ReadAsStringAsync();
        QDIIModel data2 = JsonConvert.DeserializeObject<QDIIModel>(ret2) ?? new();

        var response3 = await _client.GetAsync(url3);
        response3.EnsureSuccessStatusCode();
        var ret3 = await response3.Content.ReadAsStringAsync();
        QDIIModel data3 = JsonConvert.DeserializeObject<QDIIModel>(ret3) ?? new();

        var response4 = await _client.GetAsync(url4);
        response4.EnsureSuccessStatusCode();
        var ret4 = await response4.Content.ReadAsStringAsync();
        LOFModel data4 = JsonConvert.DeserializeObject<LOFModel>(ret4) ?? new();

        List<QDIICellModel> rows4 = [];
        foreach (var row in data4.Rows?.Where(x => decimal.TryParse(x.Cell.DiscountRate, out var _) && Convert.ToDecimal(x.Cell.DiscountRate ?? "0") > 1) ?? [])
        {
            rows4.Add(new QDIICellModel
            {
                Id = row.Id,
                Cell = new FundModel
                {
                    FundID = row.Cell.FundID,
                    FundName = row.Cell.FundName,
                    QType = row.Cell.QType,
                    DiscountRate = row.Cell.DiscountRate,
                    DiscountRate2 = row.Cell.DiscountRate,
                    ApplyStatus = row.Cell.ApplyStatus,
                },
            });
        }

        //_ = data1.Rows.Concat(data2.Rows).Concat(data3.Rows);

        return new QDIIModel { Page = 1, Rows = (data1.Rows ?? []).Concat(data2.Rows ?? []).Concat(data3.Rows ?? []).Concat(rows4) };
    }

    public async Task<REITsModel> GetREITsAsync()
    {
        var let_url = $"""https://www.jisilu.cn/data/cnreits/pre_list/?___jsl=LST___t={DateTime.UtcNow.Ticks}""";

        var response1 = await _client.GetAsync(let_url);
        response1.EnsureSuccessStatusCode();
        var ret1 = await response1.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<REITsModel>(ret1) ?? new();
    }

    // 获取数据
    public async Task<CBModel> GetCbDataAsync()
    {
        //var randomValue = new Random().NextDouble().ToString("F16");
        var dataUrl = $"https://www.jisilu.cn/data/cbnew/cb_list_new/?___jsl=LST___t={DateTime.UtcNow.Ticks}";

        var response = await _client.GetAsync(dataUrl);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        CBModel data = JsonConvert.DeserializeObject<CBModel>(result) ?? new();
        HtmlParser htmlParser = new HtmlParser();
        int index = 1;
        foreach (var row in (data.Rows?.Where(x => x.Cell.YtmRate is not null && x.Cell.YtmRate >= 0 && x.Cell.BondName.Contains("EB") == false && x.Cell.PriceTips != "待上市") ?? []).OrderBy(x => x.Cell.Price))
        {
            var bdUrl = $"https://www.jisilu.cn/data/convert_bond_detail/{row.Cell.BondID}";

            var responseBD = await _client.GetAsync(bdUrl);
            responseBD.EnsureSuccessStatusCode();
            var abc = await responseBD.Content.ReadAsStringAsync();
            var obj = htmlParser.ParseHtml(abc);
            row.Cell.RedeemPrice = obj.redeemPrice;
            row.Cell.AdjustCondition = obj.adjustCondition;
            row.Cell.Price_Order = index;
            index++;
        }
        index = 1;
        foreach (var row in (data.Rows?.Where(x => x.Cell.YtmRate is not null && x.Cell.YtmRate >= 0 && x.Cell.BondName.Contains("EB") == false && x.Cell.PriceTips != "待上市") ?? []).OrderBy(x => x.Cell.PremiumRate))
        {
            row.Cell.Premium_Order = index * 2;
            row.Cell.Sum_Order = row.Cell.Price_Order + row.Cell.Premium_Order;
            index++;
        }
        return data;
        //return new ContentResult
        //{
        //    Content = await response.Content.ReadAsStringAsync(),
        //    ContentType = response.Content.Headers.ContentType?.ToString(),
        //    StatusCode = (int)response.StatusCode
        //}; 
    }

    // Cookie 持久化（Mozilla 格式兼容）
    private void SaveCookies()
    {
        //var cookieHeader = _cookieContainer.GetCookieHeader(new Uri("https://www.jisilu.cn"));
        //System.IO.File.WriteAllText(CookieFile, cookieHeader);
    }

    private void LoadCookies()
    {
        if (!System.IO.File.Exists(CookieFile)) return;

        var cookies = System.IO.File.ReadAllText(CookieFile);
        _cookieContainer.SetCookies(new Uri("https://www.jisilu.cn"), cookies);
    }

    public void Dispose()
    {
        _client?.Dispose();
        GC.SuppressFinalize(this);
    }
}
