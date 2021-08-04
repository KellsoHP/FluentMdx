using FluentMdx.Lexer;

namespace FluentMdx.Lexer
{
    public interface IRegionRule
    {
        RegionMdxType MdxType { get; }

        RegionPriority RegionPriority { get; }

        bool ShouldHaveContent { get; }

        RuleCheckResult Check(char currentChar, char? nextChar, string oldChars);
    }
}
