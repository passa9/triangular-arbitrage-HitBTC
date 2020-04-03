namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitUnsubscribeTradesParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.UnsubscribeTrades;
    }
}