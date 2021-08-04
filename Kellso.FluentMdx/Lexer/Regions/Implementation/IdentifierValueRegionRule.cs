using System.Linq;

namespace FluentMdx.Lexer.Regions
{
    internal sealed class IdentifierValueRegionRule : BaseRegionRule
    {
        public override RegionMdxType MdxType { get; } = RegionMdxType.IdentifierValue;

        public override RegionPriority RegionPriority { get; } = RegionPriority.Normal;

        public override bool ShouldHaveContent { get; } = true;

        protected override string RegionStartsSymbols { get; } = "&[";

        protected override string RegionEndsSymbols { get; } = "]";

        protected override bool IsContentSymbolAccepted(char currentChar)
        {
            return !RegionConstants.StrongForbiddenSymbols.Contains(currentChar);
        }
    }
}
