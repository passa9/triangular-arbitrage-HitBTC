namespace HitBTC.Net.Models.RequestsParameters
{
    internal class HitGetSymbolsParameters : HitGetSymbolParameters
    {
        public override HitRequestMethod HitRequestMethod => HitRequestMethod.GetSymbols;

        public HitGetSymbolsParameters() => this.Symbol = string.Empty;
    }
}
