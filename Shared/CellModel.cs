using Newtonsoft.Json;


    public class CellModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("cell")]
        public BondModel Cell { get; set; }
    }

