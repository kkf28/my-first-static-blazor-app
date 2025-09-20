using Newtonsoft.Json;
using System.Collections.Generic;

//namespace BlazorApp.Shared
{
    public class REITsModel
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("rows")]
        public IEnumerable<REITsCellModel> Rows { get; set; }
    }

    public class REITsCellModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("cell")]
        public REITsFundModel Cell { get; set; }
    }

    public class REITsFundModel
    {
        /**
         {
    "total": 1,
    "page": 1,
    "rows": [
        {
            "id": "508091",
            "cell": {
                "fund_id": "508091",
                "fund_nm": "华夏凯德商业REIT",
                "full_nm": "华夏凯德商业资产封闭式基础设施证券投资基金",
                "project_type": "购物中心",
                "close_years": 27,
                "issue_amt": "4.00",
                "raise_money": null,
                "orgs_amt": "2.40",
                "issue_price": "5.718",
                "above_rt": "254.08",
                "draw_rt": "0.19",
                "start_dt": "2025-09-09",
                "end_dt": "2025-09-10",
                "list_dt": null,
                "sub_fee": "认购金额<500万元  0.4%；认购金额>500万元  每笔1000元；",
                "major_assets": "长于长沙和广州的购物中心",
                "fund_company": "华夏基金",
                "urls": "https:\/\/www.chinaamc.com\/fund\/508091\/index.shtml",
                "orgs_amt_rt": "60.11"
            }
        }
    ]
}
         */
        [JsonProperty("fund_id")]
        public string FundID { get; set; }
        [JsonProperty("fund_nm")]
        public string FundName { get; set; }
        [JsonProperty("start_dt")]
        public string StartDate { get; set; }
        [JsonProperty("list_dt")]
        public string ListDate { get; set; }
    }
}
