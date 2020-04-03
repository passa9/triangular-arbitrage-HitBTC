namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetCurrenciesParameters : HitGetCurrencyParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.GetCurrencies;
    }
}