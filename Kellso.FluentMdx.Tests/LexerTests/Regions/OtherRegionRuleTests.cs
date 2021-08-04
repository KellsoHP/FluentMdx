using FluentAssertions;
using FluentMdx.Lexer;
using FluentMdx.Lexer.Regions;
using NUnit.Framework;

namespace FluentMdx.Tests.LexerTests.Regions
{
    [TestFixture]
    public class OtherRegionRuleTests
    {
        [TestCaseSource(typeof(FunctionRegionRuleCaseSourceData), nameof(FunctionRegionRuleCaseSourceData.CheckCaseData))]
        public void FunctionRegionRuleShouldReturnExpectedResult(int currentChar, int? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new FunctionRegionRule();
            var ruleCheckStatus = regionRule.Check((char)currentChar, (char?)nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }

        [TestCaseSource(typeof(DigitRegionRuleCaseSourceData), nameof(DigitRegionRuleCaseSourceData.CheckCaseData))]
        public void DigitRegionRuleShouldReturnExpectedResult(char currentChar, char? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new DigitRegionRule();
            var ruleCheckStatus = regionRule.Check(currentChar, nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }

        [TestCaseSource(typeof(UndefinedWordRegionRuleCaseSourceData), nameof(UndefinedWordRegionRuleCaseSourceData.CheckCaseData))]
        public void UndefinedWordRegionRuleShouldReturnExpectedResult(char currentChar, char? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new UndefinedWord();
            var ruleCheckStatus = regionRule.Check(currentChar, nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }
    }

    public class FunctionRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            new object[] { (int)'F', (int?)'u', "", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { (int)'u', (int?)'n', "F", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { (int)'n', (int?)'c', "Fu", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { (int)'c', (int?)'(', "Fun", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { (int)'(', (int?)')', "Func", RuleCheckResult.RegionPart | RuleCheckResult.SubRegionsStartPart},
            new object[] { (int)')', (int?)null, "Func(", RuleCheckResult.Found },

            new object[] { (int)'(', (int?)null, "f(", RuleCheckResult.NotFound },

            new object[] { (int)'(', (int?)null, "", RuleCheckResult.NotFound },

            new object[] { (int)'a', (int?)')', "f(", RuleCheckResult.NotFound }
        };
    }

    public class DigitRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            new object[] { '1', '0', "", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '0', ' ', "1", RuleCheckResult.Found | RuleCheckResult.TitlePart },

            new object[] { '.', '1', "10", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '1', '-', "10.", RuleCheckResult.Found | RuleCheckResult.TitlePart },

            new object[] { '-', '.', "", RuleCheckResult.NotFound },

            new object[] { '-', '1', "", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '1', ' ', "-", RuleCheckResult.Found | RuleCheckResult.TitlePart },

            new object[] { '1', '.', "-", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '.', '0', "-1", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '.', ' ', "-1", RuleCheckResult.NotFound },

            new object[] { '0', ' ', "-1.", RuleCheckResult.Found | RuleCheckResult.TitlePart },
        };
    }

    public class UndefinedWordRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            new object[] { 'F', 'u', "", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 'u', 'n', "F", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 'n', 'c', "Fu", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 'c', '1', "Fun", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '1', '_', "Func", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '_', 'a', "Func1", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 'a', null, "Func1_", RuleCheckResult.Found | RuleCheckResult.TitlePart },
            new object[] { 'a', '!', "Func1_", RuleCheckResult.Found | RuleCheckResult.TitlePart },

            new object[] { '?', null, "", RuleCheckResult.NotFound },

            new object[] { 't', 't', "selec", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 't', null, "select", RuleCheckResult.Found | RuleCheckResult.TitlePart }
        };
    }
}
