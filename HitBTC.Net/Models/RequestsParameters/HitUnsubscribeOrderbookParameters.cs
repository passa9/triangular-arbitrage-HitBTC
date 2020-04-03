namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitUnsubscribeOrderbookParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.UnsubscribeOrderbook;
    }
}
