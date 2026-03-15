using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class LoFTLCModel
{
    /**
     * {
  "code": 0,
  "data": {
    "date": "2026-03-13",
    "funds": [
      {
        "arbitrage_types": [1],
        "code": "501225",
        "name": "全球芯片LOF",
        "premium_rate": 10.83,
        "purchase_up_limit": 0,
        "status": 2
      }
    ] },
  "message": "",
  "success": true
}
     */
    [JsonProperty("code")]
    public int Code { get; set; }
    [JsonProperty("data")]
    public LoFTLCDataModel Data { get; set; }
}

public class LoFTLCDataModel
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }
    [JsonProperty("funds")]
    public List<LoFTLCFundModel> Funds { get; set; } = [];
}

public class LoFTLCFundModel
{
    [JsonProperty("arbitrage_types")]
    public int[] ArbitrageType { get; set; } = [];
    [JsonProperty("code")]
    public int Code { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("premium_rate")]
    public decimal PremiumRate { get; set; }
    [JsonProperty("purchase_up_limit")]
    public decimal PurchaseUpLimit { get; set; }
    [JsonProperty("status")]
    public int Status { get; set; }
}
