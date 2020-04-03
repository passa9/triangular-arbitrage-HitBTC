using Newtonsoft.Json;

namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitLoginParameters : HitRequestParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.Login;
        
        [JsonProperty("algo")]
        public HitLoginAlgo Algo { get; set; }
        
        [JsonProperty("pKey")]
        public string PublicKey { get; set; }
        
        [JsonProperty("sKey")]
        public string SecretKey { get; set; }
        
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
        
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}