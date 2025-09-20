﻿using Newtonsoft.Json;
using System.Collections.Generic;

public class QDIIModel
{
    [JsonProperty("page")]
    public int Page { get; set; }
    [JsonProperty("rows")]
    public IEnumerable<QDIICellModel> Rows { get; set; }
}
public class QDIICellModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("cell")]
    public FundModel Cell { get; set; }
}
public class FundModel
{
    //{"fund_id":"160644",
    //"fund_nm":"港美互联网LOF",
    //"qtype":"E",
    //"discount_rt":"-",
    //"discount_rt2":null,
    //"apply_status":"开放申购"
    //}
    [JsonProperty("fund_id")]
    public string FundID { get; set; }
    [JsonProperty("fund_nm")]
    public string FundName { get; set; }
    [JsonProperty("qtype")]
    public string QType { get; set; }

    [JsonProperty("discount_rt")]
    public string DiscountRate { get; set; }

    [JsonProperty("discount_rt2")]
    public string DiscountRate2 { get; set; }

    [JsonProperty("apply_status")]
    public string ApplyStatus { get; set; }
}

