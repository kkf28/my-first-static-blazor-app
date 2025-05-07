using Newtonsoft.Json;
using System.Collections.Generic;


public class CBModel
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("rows")]
        public IEnumerable<CellModel> Rows { get; set; }
    }

