using HitBTC.Net.Models;
using Newtonsoft.Json;

namespace HitBTC.Net.Communication
{
    public class HitResponse
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; private set; }

        [JsonProperty("error")]
        public HitError Error { get; internal set; }

        [JsonProperty("id")]
        public string Id { get; internal set; }
    }

    public class HitResponse<T> : HitResponse
    {
        [JsonProperty("result")]
        public T Result { get; internal set; }

        public override string ToString() => $"{this.JsonRpc} | Id = {this.Id} | {this.Error?.ToString() ?? this.Result.ToString()}";
    }
}