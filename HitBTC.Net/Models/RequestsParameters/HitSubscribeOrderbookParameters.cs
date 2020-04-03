namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitSubscribeOrderbookParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.SubscribeOrderbook;
    }
}
