using FluentAssertions;
using FluentMdx.Lexer;
using FluentMdx.Lexer.Regions;
using NUnit.Framework;

namespace FluentMdx.Tests.LexerTests.Regions
{
    [TestFixture]
    public class BaseRegionRuleTests
    {
        [TestCaseSource(typeof(IdentifierRegionRuleCaseSourceData), nameof(IdentifierRegionRuleCaseSourceData.CheckCaseData))]
        public void IdentifierRegionRuleShouldReturnExpectedResult(char currentChar, char? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new IdentifierRegionRule();
            var ruleCheckStatus = regionRule.Check(currentChar, nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }

        [TestCaseSource(typeof(IdentifierValueRegionRuleCaseSourceData), nameof(IdentifierValueRegionRuleCaseSourceData.CheckCaseData))]
        public void IdentifierValueRegionRuleShouldReturnExpectedResult(char currentChar, char? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new IdentifierValueRegionRule();
            var ruleCheckStatus = regionRule.Check(currentChar, nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }

        [TestCaseSource(typeof(DoubleQuotedStringRegionRuleCaseSourceData), nameof(DoubleQuotedStringRegionRuleCaseSourceData.CheckCaseData))]
        public void DoubleQuotedStringRegionRuleShouldReturnExpectedResult(char currentChar, char? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new DoubleQuotedStringRegionRule();
            var ruleCheckStatus = regionRule.Check(currentChar, nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }

        [TestCaseSource(typeof(QuotedStringRegionRuleCaseSourceData), nameof(QuotedStringRegionRuleCaseSourceData.CheckCaseData))]
        public void QuotedStringRegionRuleShouldReturnExpectedResult(char currentChar, char? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new QuotedStringRegionRule();
            var ruleCheckStatus = regionRule.Check(currentChar, nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }

        [TestCaseSource(typeof(SetRegionRuleCaseSourceData), nameof(SetRegionRuleCaseSourceData.CheckCaseData))]
        public void MdxSetRegionRuleShouldReturnExpectedResult(int currentChar, int? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new SetRegionRule();
            var ruleCheckStatus = regionRule.Check((char)currentChar, (char?)nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }

        [TestCaseSource(typeof(TupleRegionRuleCaseSourceData), nameof(TupleRegionRuleCaseSourceData.CheckCaseData))]
        public void MdxTupleRegionRuleShouldReturnExpectedResult(char currentChar, char? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var regionRule = new TupleRegionRule();
            var ruleCheckStatus = regionRule.Check(currentChar, nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }
    }

    public class IdentifierRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            // [a_1]
            new object[] { '[', 'a', "", RuleCheckResult.RegionPart | RuleCheckResult.StartPart },
            new object[] { 'a', '_', "[", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '_', '1', "[a", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '1', ']', "[a_", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { ']', null, "[a_1", RuleCheckResult.Found | RuleCheckResult.EndPart },

            // Incorrect start symbols
            new object[] { 'a', null, "", RuleCheckResult.NotFound },
            new object[] { ' ', null, "", RuleCheckResult.NotFound },
            new object[] { '_', null, "", RuleCheckResult.NotFound },
            new object[] { '1', null, "", RuleCheckResult.NotFound },
            new object[] { ']', null, "", RuleCheckResult.NotFound },

            // [[
            new object[] { '[', 'a', "[", RuleCheckResult.NotFound },

            // []
            new object[] { ']', 'a', "[", RuleCheckResult.NotFound }
        };
    }

    public class IdentifierValueRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            new object[] { '&', '[', "", RuleCheckResult.RegionPart | RuleCheckResult.StartPart },
            new object[] { '[', 'a', "&", RuleCheckResult.RegionPart | RuleCheckResult.StartPart },
            new object[] { 'a', '_', "&[", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '_', '1', "&[a", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '1', ']', "&[a_", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { ']', null, "&[a_1", RuleCheckResult.Found | RuleCheckResult.EndPart },

            new object[] { '[', 'a', "", RuleCheckResult.NotFound },
            new object[] { 'a', 'a', "", RuleCheckResult.NotFound },
            new object[] { ' ', 'a', "", RuleCheckResult.NotFound },
            new object[] { '_', 'a', "", RuleCheckResult.NotFound },
            new object[] { '1', 'a', "", RuleCheckResult.NotFound },
            new object[] { ']', 'a', "", RuleCheckResult.NotFound },

            new object[] { '&', '[', "&", RuleCheckResult.NotFound },
            new object[] { '[', 'a', "&[", RuleCheckResult.NotFound },

            new object[] { ']', 'a', "&[", RuleCheckResult.NotFound },
        };
    }

    public class DoubleQuotedStringRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            new object[] { '"', 'a', "", RuleCheckResult.RegionPart | RuleCheckResult.StartPart },
            new object[] { 'a', '_', "\"", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '_', '1', "\"a", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '1', '-', "\"a_", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '-', ' ', "\"a_1", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { ' ', 'a', "\"a_1-", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 'a', 'a', "\"a_1- ", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 'a', '[', "\"a_1- a", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '[', '"', "\"a_1- a[", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '"', null, "\"a_1- a[]", RuleCheckResult.Found | RuleCheckResult.EndPart },

            new object[] { '"', 'a', "\"", RuleCheckResult.Found | RuleCheckResult.EndPart },

            new object[] { 'a', '"', "", RuleCheckResult.NotFound },
        };
    }

    public class QuotedStringRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            new object[] { '\'','a', "", RuleCheckResult.RegionPart | RuleCheckResult.StartPart },
            new object[] { 'a', '_', "'", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '_', '1', "'a", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '1', '-', "'a_", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '-', ' ', "'a_1", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { ' ', 'a', "'a_1-", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 'a', 'a', "'a_1- ", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { 'a', '[', "'a_1- a", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '[', '\'', "'a_1- a[", RuleCheckResult.RegionPart | RuleCheckResult.TitlePart },
            new object[] { '\'', null, "'a_1- a[]", RuleCheckResult.Found | RuleCheckResult.EndPart },

            new object[] { '\'', 'a', "'", RuleCheckResult.Found | RuleCheckResult.EndPart },

            new object[] { 'a', '\'', "", RuleCheckResult.NotFound },
        };
    }

    public class TupleRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            new object[] { '{', '}', "", RuleCheckResult.SubRegionsStartPart },
            new object[] { '}', null, "{", RuleCheckResult.Found },
            new object[] { 'a', null, "{", RuleCheckResult.NotFound },
            new object[] { '}', null, "{some", RuleCheckResult.Found }
        };
    }

    public class SetRegionRuleCaseSourceData
    {
        public static object[] CheckCaseData =
        {
            new object[] { (int)'(', (int?)')', "", RuleCheckResult.SubRegionsStartPart },
            new object[] { (int)')', null, "(", RuleCheckResult.Found },
            new object[] { (int)'a', null, "(", RuleCheckResult.NotFound },
            new object[] { (int)')', null, "(some", RuleCheckResult.Found }
        };
    }
}
