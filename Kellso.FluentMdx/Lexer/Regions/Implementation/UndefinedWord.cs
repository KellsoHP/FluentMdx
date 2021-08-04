using System.Linq;

namespace FluentMdx.Lexer.Regions
{
    internal class UndefinedWord : IRegionRule
    {
        public RegionMdxType MdxType { get; } = RegionMdxType.Word;

        public RegionPriority RegionPriority { get; } = RegionPriority.Normal;

        public bool ShouldHaveContent { get; } = false;

        public RuleCheckResult Check(char currentChar, char? nextChar, string currentRegion)
        {
            var isNameSymbol = this.IsNameSymbol(currentChar);

            if (string.IsNullOrEmpty(currentRegion))
                return isNameSymbol && !char.IsDigit(currentChar)
                    ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                    : RuleCheckResult.NotFound;

            if (isNameSymbol)
            {
                if (nextChar.HasValue)
                    return this.IsNameSymbol(nextChar.Value)
                        ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                        : RuleCheckResult.Found | RuleCheckResult.TitlePart;

                return RuleCheckResult.Found | RuleCheckResult.TitlePart;

            }

            return RuleCheckResult.NotFound;
        }

        private bool IsNameSymbol(char currentChar)
        {
            if (char.IsLetterOrDigit(currentChar))
                return true;

            if (RegionConstants.FunctionNameAcceptedSymbols.Contains(currentChar))
                return true;

            return false;
        }
    }
}
