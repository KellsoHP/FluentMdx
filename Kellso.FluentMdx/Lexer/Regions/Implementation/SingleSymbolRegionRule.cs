using System;
using System.Diagnostics;

namespace FluentMdx.Lexer.Regions
{
    [DebuggerDisplay("'{MdxType}' - ' {Symbol} '")]
    internal sealed class SingleSymbolRegionRule : IRegionRule
    {
        public RegionMdxType MdxType { get; }

        public char Symbol { get; }

        public RegionPriority RegionPriority { get; } = RegionPriority.Named;

        public bool ShouldHaveContent { get; }

        public RuleCheckResult Check(char currentChar, char? nextChar, string oldChars)
        {
            return currentChar == Symbol ? RuleCheckResult.Found | RuleCheckResult.TitlePart : RuleCheckResult.NotFound;
        }

        public SingleSymbolRegionRule(char symbol, RegionMdxType mdxType)
        {
            this.Symbol = symbol;
            this.MdxType = mdxType;
        }
    }
}
