using FluentAssertions;
using FluentMdx.Lexer;
using FluentMdx.Lexer.Regions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FluentMdx.Tests.LexerTests.Regions
{
    [TestFixture]
    public class NamedRegionsRulesTests
    {
        [TestCaseSource(typeof(SharedNameRegionsRulesCaseSourceData), nameof(SharedNameRegionsRulesCaseSourceData.CheckCaseData))]
        public void SharedKeyWordRegionRuleShouldReturnExpectedResult(IRegionRule regionRule, char currentChar, char? nextChar, string currentRegion, RuleCheckResult expectedResult)
        {
            var ruleCheckStatus = regionRule.Check(currentChar, nextChar, currentRegion);
            ruleCheckStatus.Should().Be(expectedResult);
        }
    }

    public class SharedNameRegionsRulesCaseSourceData
    {
        public static object[] CheckCaseData = GetCaseData().ToArray();

        private static IEnumerable<object> GetCaseData()
        {
            var regionsRules = RegionRulesBuilder.GetNameRegionsRules().OfType<CommonNameRegionRule>().ToArray();
            foreach (var rule in regionsRules)
            {
                var currentString = string.Empty;
                for (int i = 0; i < rule.Name.Length; i++)
                {
                    var currentChar = rule.Name[i];
                    if (i == rule.Name.Length - 1)
                        yield return new object[] { rule, currentChar, null, currentString, RuleCheckResult.Found | RuleCheckResult.TitlePart };
                    else
                        yield return new object[] { rule, i % 2 == 0 ? char.ToLowerInvariant(currentChar) : char.ToUpperInvariant(currentChar), rule.Name[i + 1], currentString, RuleCheckResult.RegionPart | RuleCheckResult.TitlePart };

                    currentString += currentChar;
                }

                yield return new object[] { rule, 'P', null, rule.Name.Remove(rule.Name.Length - 2, 1), RuleCheckResult.NotFound };
                yield return new object[] { rule, '.', rule.Name[0], string.Empty, RuleCheckResult.NotFound };
                yield return new object[] { rule, rule.Name.Last(), 'Z', rule.Name.Substring(0, rule.Name.Length - 1), RuleCheckResult.NotFound };

            }

            var singleSymbolsRegionsRules = RegionRulesBuilder.GetNameRegionsRules().OfType<SingleSymbolRegionRule>().ToArray();
            foreach (var rule in singleSymbolsRegionsRules)
            {
                yield return new object[] { rule, rule.Symbol, null, string.Empty, RuleCheckResult.Found | RuleCheckResult.TitlePart };
                yield return new object[] { rule, 'ё', null, string.Empty, RuleCheckResult.NotFound };
            }
        }
    }
}
