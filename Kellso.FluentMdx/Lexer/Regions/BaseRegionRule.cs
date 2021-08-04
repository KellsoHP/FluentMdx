using System.Diagnostics;
using System.Linq;

namespace FluentMdx.Lexer.Regions
{
    [DebuggerDisplay("Region - '{MdxType}'")]
    internal abstract class BaseRegionRule : IRegionRule
    {
        public abstract RegionMdxType MdxType { get; }

        public abstract RegionPriority RegionPriority { get; }

        public abstract bool ShouldHaveContent { get; }

        protected abstract string RegionStartsSymbols { get; }

        protected abstract string RegionEndsSymbols { get; }

        public virtual RuleCheckResult Check(char currentChar, char? nextChar, string currentRegion)
        {
            var isRegionStartSymbol = this.IsRegionStartSymbol(currentChar);
            var isNullOrEmpty = string.IsNullOrEmpty(currentRegion);

            // "" + &
            // "" + ]
            // "" + a
            if (isNullOrEmpty)
                return isRegionStartSymbol && this.RegionStartsSymbols[0] == currentChar 
                    ? RuleCheckResult.RegionPart | RuleCheckResult.StartPart
                    : RuleCheckResult.NotFound;

            var isSymbolAccepted = this.IsContentSymbolAccepted(currentChar);
            var nextRegion = (currentRegion ?? string.Empty) + currentChar;

            // &  + [
            // &[ + [
            if (isRegionStartSymbol && this.RegionStartsSymbols != this.RegionEndsSymbols)
            {
                // &[ start with & or &[
                if (this.RegionStartsSymbols.StartsWith(nextRegion))
                    return RuleCheckResult.RegionPart | RuleCheckResult.StartPart;

                // (&[ + a) start with &[ and a is accepted (or not)
                if (nextRegion.StartsWith(this.RegionStartsSymbols))
                    return isSymbolAccepted 
                        ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                        : RuleCheckResult.NotFound;
                else
                    return RuleCheckResult.NotFound;
            }

            var isRegionEndSymbol = this.IsRegionEndSymbol(currentChar);
            var contentAndEndSymbols = nextRegion.Remove(0, this.RegionStartsSymbols.Length);

            // &[a  + ]
            // &[   + ]
            // &[a] + &
            // &[]  + &
            if (isRegionEndSymbol)
            {
                var contentCountEmpty = contentAndEndSymbols.TrimEnd(this.RegionEndsSymbols.ToCharArray());
                if (string.IsNullOrEmpty(contentCountEmpty))
                {
                    if (this.ShouldHaveContent)
                        return RuleCheckResult.NotFound;

                    if (this.RegionEndsSymbols[0] == currentChar)
                        return this.RegionEndsSymbols.Length == 1 
                            ? RuleCheckResult.Found | RuleCheckResult.EndPart
                            : RuleCheckResult.RegionPart | RuleCheckResult.EndPart;
                    
                    return RuleCheckResult.NotFound;
                }

                var firstEndIndex = contentAndEndSymbols.LastIndexOf(this.RegionEndsSymbols[0]);
                var endSymbols = firstEndIndex != -1 ? contentAndEndSymbols.Substring(firstEndIndex) : string.Empty;

                if (this.RegionEndsSymbols.StartsWith(endSymbols))
                    return this.RegionEndsSymbols.Length == endSymbols.Length 
                        ? RuleCheckResult.Found | RuleCheckResult.EndPart
                        : RuleCheckResult.RegionPart | RuleCheckResult.EndPart;
            }

            return isSymbolAccepted 
                ? RuleCheckResult.RegionPart | RuleCheckResult.TitlePart
                : RuleCheckResult.NotFound;
        }

        protected virtual bool IsRegionStartSymbol(char currentChar)
        {
            return this.RegionStartsSymbols.Contains(currentChar);
        }

        protected virtual bool IsRegionEndSymbol(char currentChar)
        {
            return this.RegionEndsSymbols.Contains(currentChar);
        }

        protected virtual bool IsContentSymbolAccepted(char currentChar)
        {
            return true;
        }
    }
}
