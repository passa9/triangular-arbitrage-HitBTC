namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitUnsubscribeTickerParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.UnsubscribeTicker;
    }
}