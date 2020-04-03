namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitSubscribeTradesParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.SubscribeTrades;
    }
}