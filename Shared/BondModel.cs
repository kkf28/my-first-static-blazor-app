using Newtonsoft.Json;


    public class BondModel
    {
        [JsonProperty("bond_id")]
        public string BondID { get; set; }

        [JsonProperty("bond_nm")]
        public string BondName { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("premium_rt")]
        public decimal PremiumRate { get; set; }

        [JsonProperty("curr_iss_amt")]
        public decimal CurrentIssueAmount { get; set; }

        [JsonProperty("ytm_rt")]
        public decimal? YtmRate { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("year_left")]
        public string YearLeft { get; set; }

        [JsonProperty("pb")]
        public string PB { get; set; }

        [JsonProperty("price_tips")]
        public string PriceTips { get; set; }

        public string RedeemPrice { get; set; }
        public string AdjustCondition { get; set; }
        public int Price_Order { get; set; }
        public int Premium_Order { get; set; }
        public int Sum_Order { get; set; }
    }
