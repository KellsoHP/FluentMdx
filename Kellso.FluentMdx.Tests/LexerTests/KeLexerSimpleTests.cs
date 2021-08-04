using FluentAssertions;
using FluentMdx.Lexer;
using NUnit.Framework;
using System.Linq;

namespace FluentMdx.Tests.LexerTests
{
    [TestFixture]
    public class KeLexerSimpleTests
    {
        [TestCase("   select ", new[] { RegionMdxType.Select }, new[] { "select" })]
        [TestCase("select \"a\"", new[] { RegionMdxType.Select, RegionMdxType.StringValue }, new[] { "select", "a" })]
        [TestCase("select \"a\" FROM   ", new[] { RegionMdxType.Select, RegionMdxType.StringValue, RegionMdxType.From }, new[] { "select", "a", "FROM" })]
        [TestCase("SELECT [Dim].[Attr].Members FROM CubeName", 
            new[] 
            { 
                RegionMdxType.Select, 
                RegionMdxType.Identifier,
                RegionMdxType.DotDelimiter,
                RegionMdxType.Identifier,
                RegionMdxType.DotDelimiter,
                RegionMdxType.Word,
                RegionMdxType.From,
                RegionMdxType.Word
            }, 
            new[] { "SELECT", "Dim", ".", "Attr", ".", "Members", "FROM", "CubeName" })]
        public void ShouldTokenizeSimpleSelect(string query, RegionMdxType[] regionMdxTypes, string[] values)
        {
            var lexer = new KeLexer();

            var tokens = lexer.Tokenize(query);

            tokens.Select(t => t.RegionMdxType).Should().BeEquivalentTo(regionMdxTypes);
            tokens.Select(t => t.Value).Should().BeEquivalentTo(values);
        }

        [TestCase("{[Dim].[Attr]}", RegionMdxType.Tuple, new[] { RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier }, "", new[] { "Dim", ".", "Attr" })]
        [TestCase("       { [Dim]. [Attr] }", RegionMdxType.Tuple, new[] { RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier }, "", new[] { "Dim", ".", "Attr" })]
        [TestCase("([Dim].[Attr])", RegionMdxType.Set, new[] { RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier }, "", new[] { "Dim", ".", "Attr" })]
        [TestCase("       ( [Dim]. [Attr] )", RegionMdxType.Set, new[] { RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier }, "", new[] { "Dim", ".", "Attr" })]
        [TestCase("Function([Dim].[Attr])", RegionMdxType.Function, new[] { RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier }, "Function", new[] { "Dim", ".", "Attr" })]
        [TestCase("    Function(     [Dim] . [Attr]    )    ", RegionMdxType.Function, new[] { RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier }, "Function", new[] { "Dim", ".", "Attr" })]
        public void ShouldTokenizeSubRegions(string query, RegionMdxType regionMdxType, RegionMdxType[] subRegionsTypes, string value, string[] subValues)
        {
            var lexer = new KeLexer();

            var tokens = lexer.Tokenize(query);

            tokens.Should().HaveCount(1);

            var mainToken = tokens.First();

            mainToken.RegionMdxType.Should().Be(regionMdxType);
            mainToken.SubRegionsTokens.Select(_ => _.RegionMdxType).Should().BeEquivalentTo(subRegionsTypes);

            mainToken.Value.Should().Be(value);
            mainToken.SubRegionsTokens.Select(_ => _.Value).Should().BeEquivalentTo(subValues);
        }

        [Test]
        public void ShouldTokenizeSubRegionsWithSubRegions()
        {
            const string query = "({[Dim].[Attr].&[1], [Dim].[Attr].&[1]}, {[Dim].[Attr2].&[1], [Dim].[Attr2].&[1]})";
            var lexer = new KeLexer();

            var tokens = lexer.Tokenize(query);
            tokens.Should().HaveCount(1);

            var token = tokens.First();
            token.RegionMdxType.Should().Be(RegionMdxType.Set);
            token.Value.Should().BeEmpty();

            var subTokens = token.SubRegionsTokens;
            subTokens.Should().HaveCount(3);

            var secondSubToken = subTokens[0];
            secondSubToken.RegionMdxType.Should().Be(RegionMdxType.Tuple);
            secondSubToken.SubRegionsTokens.Should().HaveCount(11);
            secondSubToken.SubRegionsTokens.Select(_ => _.RegionMdxType).Should().BeEquivalentTo(new[] 
            {
                RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType .IdentifierValue,
                RegionMdxType.CommaDelimiter,
                RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType .IdentifierValue,
            });

            subTokens[1].RegionMdxType.Should().Be(RegionMdxType.CommaDelimiter);
            subTokens[1].Value.Should().Be(",");

            var lastSubToken = subTokens[2];
            lastSubToken.RegionMdxType.Should().Be(RegionMdxType.Tuple);
            lastSubToken.SubRegionsTokens.Should().HaveCount(11);
            lastSubToken.SubRegionsTokens.Select(_ => _.RegionMdxType).Should().BeEquivalentTo(new[]
            {
                RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType .IdentifierValue,
                RegionMdxType.CommaDelimiter,
                RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType.Identifier, RegionMdxType.DotDelimiter, RegionMdxType .IdentifierValue,
            });
        }

        [Test]
        public void SharedNamesShouldHaveHighterPriorityThanIdentifire()
        {
            const string query = " [MeasuRes] ";
            var lexer = new KeLexer();

            var tokens = lexer.Tokenize(query);

            tokens.Should().ContainSingle();

            var token = tokens.First();
            token.RegionMdxType.Should().Be(RegionMdxType.Measure);
            token.Value.Should().Be("[MeasuRes]");
        }
    }
}
