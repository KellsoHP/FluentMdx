namespace FluentMdx.Lexer.Regions
{
    internal class TupleRegionRule : IRegionRule
    {
        public RegionMdxType MdxType { get; } = RegionMdxType.Tuple;

        public RegionPriority RegionPriority { get; } = RegionPriority.Normal;

        public bool ShouldHaveContent { get; } = true;

        protected char RegionStartsSymbols { get; } = '{';

        protected char RegionEndsSymbols { get; } = '}';

        public RuleCheckResult Check(char currentChar, char? nextChar, string oldChars)
        {
            if (string.IsNullOrEmpty(oldChars))
                return currentChar == RegionStartsSymbols ? RuleCheckResult.SubRegionsStartPart : RuleCheckResult.NotFound;

            if (currentChar == RegionEndsSymbols)
                return RuleCheckResult.Found;

            return RuleCheckResult.NotFound;
        }
    }
}
