using Newtonsoft.Json;


    public class CBModel
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("rows")]
        public IEnumerable<CellModel> Rows { get; set; }
    }

