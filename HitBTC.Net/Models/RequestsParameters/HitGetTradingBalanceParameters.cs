namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetTradingBalanceParameters : HitRequestParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.GetTradingBalance;
    }
}