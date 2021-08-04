using FluentMdx.Lexer.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentMdx.Lexer
{
    internal static class RegionRulesBuilder
    {
        private readonly static Dictionary<string, RegionMdxType> RegionsNames = new Dictionary<string, RegionMdxType>
        {
            { "SELECT", RegionMdxType.Select },
            { "CROSSJOIN", RegionMdxType.CrossJoin },
            { "NON EMPTY", RegionMdxType.NonEmpty },
            { "WHERE", RegionMdxType.Where },
            { "FROM", RegionMdxType.From },
            { "WITH", RegionMdxType.With },
            { "MEASURES", RegionMdxType.Measure },
            { "[MEASURES]", RegionMdxType.Measure },
            { "AS", RegionMdxType.As },
            { "SET", RegionMdxType.SetWord },
            { "=", RegionMdxType.LogicOperator },
            { ">", RegionMdxType.LogicOperator },
            { "<", RegionMdxType.LogicOperator },
            { "<>", RegionMdxType.LogicOperator },
            { "AND", RegionMdxType.LogicOperator },
            { "OR", RegionMdxType.LogicOperator },
            { "NOT", RegionMdxType.LogicOperator },
            { "ON", RegionMdxType.On },
            { "ASC", RegionMdxType.Order },
            { "DESC", RegionMdxType.Order },
            { "BASC", RegionMdxType.Order },
            { "BDESC", RegionMdxType.Order },
            { ".", RegionMdxType.DotDelimiter },
            { ",", RegionMdxType.CommaDelimiter },
            { "*", RegionMdxType.MultiplySymbol },

            { "COLUMNS", RegionMdxType.DotDelimiter },
            { "ROWS", RegionMdxType.CommaDelimiter },
            { "PAGES", RegionMdxType.MultiplySymbol },
            { "CHAPTERS", RegionMdxType.DotDelimiter },
            { "SECTIONS", RegionMdxType.CommaDelimiter },
        };

        private static IRegionRule[] NameRegionsRules { get; } = RegionsNames
            .Where(_ => _.Key.Length > 1).Select(_ => new CommonNameRegionRule(_.Key, _.Value))
            .Cast<IRegionRule>()
            .Union(RegionsNames.Where(_ => _.Key.Length == 1).Select(_ => new SingleSymbolRegionRule(_.Key[0], _.Value)))
            .ToArray();

        private static IRegionRule[] QueryRegionsRules { get; } = new IRegionRule[]
            {
                new IdentifierRegionRule(),
                new IdentifierValueRegionRule(),
                new FunctionRegionRule(),
                new QuotedStringRegionRule(),
                new DoubleQuotedStringRegionRule(),
                new DigitRegionRule(),
                new UndefinedWord(),
                new SetRegionRule(),
                new TupleRegionRule()
            };

        private static Lazy<IRegionRule[]> AllRegionsRules { get; } = new Lazy<IRegionRule[]>(() => QueryRegionsRules.Union(NameRegionsRules).ToArray());

        public static IEnumerable<IRegionRule> GetRegionsRules()
        {
            return AllRegionsRules.Value;
        }

        internal static IEnumerable<IRegionRule> GetNameRegionsRules()
        {
            return NameRegionsRules;
        }
    }
}
