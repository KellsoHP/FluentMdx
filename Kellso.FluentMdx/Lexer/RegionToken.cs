using System.Diagnostics;

namespace FluentMdx.Lexer
{
    [DebuggerDisplay("'{Value}':[{RegionMdxType}]")]
    public class RegionToken
    {
        public RegionMdxType RegionMdxType { get; set; }

        public string Value { get; set; }

        public RegionToken[] SubRegionsTokens { get; set; }
    }
}
