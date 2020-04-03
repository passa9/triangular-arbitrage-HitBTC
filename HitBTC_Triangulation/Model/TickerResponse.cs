using Newtonsoft.Json;

namespace HitBTC_Triangulation.Model
{
  public  class TickerResponse
    {
        public string A { get; set; }
        public string B { get; set; }
        public string Symbol { get; set; }
        public  decimal QuantityIncrement { get; set; }

        [JsonProperty(PropertyName = "id")] public int Id { get; set; }
        [JsonProperty(PropertyName = "last")] public decimal Last { get; set; }
        [JsonProperty(PropertyName = "lowestAsk")] public decimal LowestAsk { get; set; }
        [JsonProperty(PropertyName = "highestBid")] public decimal HighestBid { get; set; }
        [JsonProperty(PropertyName = "percentChange")] public decimal PercentChange { get; set; }
        [JsonProperty(PropertyName = "baseVolume")] public decimal BaseVolume { get; set; }
        [JsonProperty(PropertyName = "quoteVolume")] public decimal QuoteVolume { get; set; }
        [JsonProperty(PropertyName = "high24hr")] public decimal High24hr { get; set; }
        [JsonProperty(PropertyName = "low24hr")] public decimal Low24hr { get; set; }
    }
}
