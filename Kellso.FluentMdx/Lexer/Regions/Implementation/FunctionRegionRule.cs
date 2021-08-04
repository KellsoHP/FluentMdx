using System.Linq;

namespace FluentMdx.Lexer.Regions
{
    internal sealed class FunctionRegionRule : IRegionRule
    {
        private const char SubRegionStartSymbol = '(';

        private const char SubRegionEndSymbol = ')';

        public RegionMdxType MdxType { get; } = RegionMdxType.Function;

        public RegionPriority RegionPriority { get; } = RegionPriority.Hight;

        public bool ShouldHaveContent { get; } = false;

        public RuleCheckResult Check(char currentChar, char? nextChar, string currentRegion)
        {
            var isFunctionNameSymbol = this.IsFunctionNameSymbol(currentChar);

            if (string.IsNullOrEmpty(currentRegion))
                return isFunctionNameSymbol && !char.IsDigit(currentChar)
                    ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                    : RuleCheckResult.NotFound;

            var isRegionAlredyStarts = currentRegion.Contains(SubRegionStartSymbol);

            if (currentChar == SubRegionStartSymbol)
                return isRegionAlredyStarts 
                    ? RuleCheckResult.NotFound 
                    : RuleCheckResult.RegionPart | RuleCheckResult.SubRegionsStartPart;

            if (currentChar == SubRegionEndSymbol)
                return RuleCheckResult.Found;

            return isFunctionNameSymbol && !isRegionAlredyStarts 
                ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                : RuleCheckResult.NotFound;
        }

        private bool IsFunctionNameSymbol(char currentChar)
        {
            if (char.IsLetterOrDigit(currentChar))
                return true;

            if (RegionConstants.FunctionNameAcceptedSymbols.Contains(currentChar))
                return true;

            return false;
        }
    }
}
