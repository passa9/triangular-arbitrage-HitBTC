namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitSubscribeTickerParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.SubscribeTicker;
    }
}
