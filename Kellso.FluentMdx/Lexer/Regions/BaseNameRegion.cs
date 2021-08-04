using FluentMdx.Extensions;
using System;

namespace FluentMdx.Lexer.Regions
{

    internal abstract class BaseNameRegion : IRegionRule
    {
        public abstract RegionMdxType MdxType { get; }

        public RegionPriority RegionPriority { get; } = RegionPriority.Named;

        public bool ShouldHaveContent { get; } = true;

        public abstract string Name { get; }

        protected abstract string RegionStartsSymbols { get; }

        protected abstract string RegionEndsSymbols { get; }

        private readonly Lazy<string> fullRegionName;

        public RuleCheckResult Check(char currentChar, char? nextChar, string oldChars)
        {
            if (string.IsNullOrEmpty(oldChars))
            {
                if (!string.IsNullOrEmpty(this.RegionStartsSymbols))
                    return this.RegionStartsSymbols[0].IsCharEqualsWithIgnoreCase(currentChar)
                        ? RuleCheckResult.RegionPart | RuleCheckResult.StartPart
                        : RuleCheckResult.NotFound;

                return this.Name[0].IsCharEqualsWithIgnoreCase(currentChar)
                    ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                    : RuleCheckResult.NotFound;
            }

            var currentChars = oldChars + currentChar;
            if (fullRegionName.Value.Equals(currentChars, StringComparison.OrdinalIgnoreCase))
            {
                if (nextChar.HasValue && char.IsLetterOrDigit(nextChar.Value))
                    return RuleCheckResult.NotFound;

                return RuleCheckResult.Found | (currentChars.EndsWith(this.Name, StringComparison.OrdinalIgnoreCase) ? RuleCheckResult.TitlePart : RuleCheckResult.EndPart);
            }

            if (fullRegionName.Value.StartsWith(currentChars, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(this.RegionStartsSymbols) && this.RegionStartsSymbols.StartsWith(currentChars, StringComparison.OrdinalIgnoreCase))
                    return RuleCheckResult.RegionPart | RuleCheckResult.StartPart;

                var onlyNameWithEndSymbols = currentChars.Remove(0, this.RegionStartsSymbols?.Length ?? 0);
                if (this.Name.StartsWith(onlyNameWithEndSymbols, StringComparison.OrdinalIgnoreCase))
                    return RuleCheckResult.RegionPart | RuleCheckResult.TitlePart;

                var onlyEndSymbols = currentChars.Remove(0, (this.RegionStartsSymbols?.Length ?? 0) + this.Name.Length);
                if (this.RegionEndsSymbols.StartsWith(onlyEndSymbols, StringComparison.OrdinalIgnoreCase))
                    return RuleCheckResult.RegionPart | RuleCheckResult.EndPart;
            }

            return RuleCheckResult.NotFound;
        }

        public override string ToString()
        {
            return $"NameRegionRule<{this.Name}>";
        }

        public BaseNameRegion()
        {
            this.fullRegionName = new Lazy<string>(() => $"{this.RegionStartsSymbols}{this.Name}{this.RegionEndsSymbols}");
        }
    }
}
