using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal abstract class HitRequestParameters
    {
        [JsonIgnore]
        public abstract HitRequestMethod HitRequestMethod { get; }
    }
}