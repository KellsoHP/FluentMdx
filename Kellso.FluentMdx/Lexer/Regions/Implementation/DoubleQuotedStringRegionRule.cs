namespace FluentMdx.Lexer.Regions
{
    internal class DoubleQuotedStringRegionRule : BaseRegionRule
    {
        public override RegionMdxType MdxType { get; } = RegionMdxType.StringValue;

        public override RegionPriority RegionPriority { get; } = RegionPriority.Low;

        public override bool ShouldHaveContent { get; } = false;
        
        protected override string RegionStartsSymbols { get; } = "\"";

        protected override string RegionEndsSymbols { get; } = "\"";

        protected override bool IsContentSymbolAccepted(char currentChar)
        {
            return currentChar != '"';
        }
    }
}
