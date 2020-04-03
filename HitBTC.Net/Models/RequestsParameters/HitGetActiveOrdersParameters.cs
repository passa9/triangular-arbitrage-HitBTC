namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetActiveOrdersParameters : HitRequestParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.GetOrders;
    }
}