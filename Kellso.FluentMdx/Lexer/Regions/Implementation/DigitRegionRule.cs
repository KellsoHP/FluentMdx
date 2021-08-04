using System.Linq;

namespace FluentMdx.Lexer.Regions
{
    internal sealed class DigitRegionRule : IRegionRule
    {
        public RegionMdxType MdxType { get; } = RegionMdxType.DigitValue;

        public RegionPriority RegionPriority { get; } = RegionPriority.Low;

        public bool ShouldHaveContent { get; } = true;

        public RuleCheckResult Check(char currentChar, char? nextChar, string currentRegion)
        {
            var isNumberSeparator = IsNumberSeparator(currentChar);
            if (string.IsNullOrEmpty(currentRegion))
            {
                if (IsNegativeNumberSymbol(currentChar) && nextChar.HasValue && char.IsDigit(nextChar.Value))
                    return RuleCheckResult.RegionPart | RuleCheckResult.TitlePart;

                return isNumberSeparator || !char.IsDigit(currentChar)
                    ? RuleCheckResult.NotFound
                    : RuleCheckResult.RegionPart | RuleCheckResult.TitlePart;
            }

            if (isNumberSeparator)
            {
                if (currentRegion.Any(IsNumberSeparator))
                    return RuleCheckResult.NotFound;

                return nextChar.HasValue && char.IsDigit(nextChar.Value)
                    ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                    : RuleCheckResult.NotFound;
            }

            if (!nextChar.HasValue)
                return char.IsDigit(currentChar) ? RuleCheckResult.Found | RuleCheckResult.TitlePart : RuleCheckResult.NotFound;

            if (char.IsDigit(currentChar))
                return IsNumberSeparator(nextChar.Value)
                    ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                    : RuleCheckResult.Found | RuleCheckResult.TitlePart;

            return RuleCheckResult.NotFound;
        }

        private static bool IsNumberSeparator(char nextChar)
        {
            return nextChar == '.' || nextChar == ',';
        }

        private static bool IsNegativeNumberSymbol(char nextChar)
        {
            return nextChar == '-';
        }
    }
}
