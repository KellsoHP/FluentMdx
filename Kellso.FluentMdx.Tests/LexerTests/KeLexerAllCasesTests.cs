using FluentAssertions;
using FluentMdx.Lexer;
using NUnit.Framework;
using System.Linq;

namespace FluentMdx.Tests.LexerTests
{
    [TestFixture]
    public class KeLexerAllCasesTests
    {
        [Test]
        public void ShouldTokenizeSubRegionsWithSubRegions()
        {
            const string query = @"WITH SET [Metrics] AS { 
   (
     [Metric].[Id].&[1],
     [Measures].[Bank QLY]
   ), 
   (
     [Metric].[Id].&[1],
     [Measures].[PG AVG QLY]
   ), 
   (
     [Metric].[Id].&[1],
     [Measures].[QoQ GR QLY]
   )
}";
            var lexer = new KeLexer();

            var tokens = lexer.Tokenize(query);
            tokens.Should().HaveCount(5);
            tokens.Select(_ => _.RegionMdxType).Should().BeEquivalentTo(new[] 
            { 
                RegionMdxType.With,
                RegionMdxType.SetWord,
                RegionMdxType.Identifier,
                RegionMdxType.As,
                RegionMdxType.Tuple
            });
        }
    }
}
