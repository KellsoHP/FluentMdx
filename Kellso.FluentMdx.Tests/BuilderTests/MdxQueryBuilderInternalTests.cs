using FluentMdx.Lexer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMdx.Tests.BuilderTests
{
    [TestFixture]
    public class MdxQueryBuilderInternalTests
    {
        [Test]
        public void InternalBuild_SimpleQuery_ShouldReturnExpectedResult()
        {
            var tokens = new[]
            {
                new RegionToken { RegionMdxType = RegionMdxType.Select, Value = "sElEcT" },
                new RegionToken
                {
                    RegionMdxType = RegionMdxType.Tuple,
                    SubRegionsTokens = new[]
                    {
                        new RegionToken { RegionMdxType = RegionMdxType.Identifier, Value = "Airline" },
                        new RegionToken { RegionMdxType = RegionMdxType.DotDelimiter },
                        new RegionToken { RegionMdxType = RegionMdxType.Identifier, Value = "Orel" }
                    }
                },
                new RegionToken { RegionMdxType = RegionMdxType.On, Value = "on" },
                new RegionToken { RegionMdxType = RegionMdxType.AxisNameIdentifier, Value = "Columns" },
                new RegionToken { RegionMdxType = RegionMdxType.CommaDelimiter },
                new RegionToken
                {
                    RegionMdxType = RegionMdxType.Tuple,
                    SubRegionsTokens = new[]
                    {
                        new RegionToken { RegionMdxType = RegionMdxType.Identifier, Value = "2000" },
                        new RegionToken { RegionMdxType = RegionMdxType.DotDelimiter },
                        new RegionToken { RegionMdxType = RegionMdxType.Identifier, Value = "Jan" },
                        new RegionToken { RegionMdxType = RegionMdxType.CommaDelimiter },
                        new RegionToken { RegionMdxType = RegionMdxType.Identifier, Value = "2000" },
                        new RegionToken { RegionMdxType = RegionMdxType.DotDelimiter },
                        new RegionToken { RegionMdxType = RegionMdxType.Identifier, Value = "Feb" },
                    }
                },
                new RegionToken { RegionMdxType = RegionMdxType.On, Value = "ON" },
                new RegionToken { RegionMdxType = RegionMdxType.AxisNameIdentifier, Value = "ROWS" },
                new RegionToken { RegionMdxType = RegionMdxType.From },
                new RegionToken { RegionMdxType = RegionMdxType.Word, Value = "Cube" },
                new RegionToken { RegionMdxType = RegionMdxType.Where, Value = "where"},
                new RegionToken { RegionMdxType = RegionMdxType.Measure, Value = "[MEASURES]" },
                new RegionToken { RegionMdxType = RegionMdxType.DotDelimiter },
                new RegionToken { RegionMdxType = RegionMdxType.Identifier, Value =  "Passengers count" },
            };
        }
    }
}
