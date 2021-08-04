using System;
using System.Collections.Generic;

namespace FluentMdx.Lexer
{
    internal class KeLexerState
    {
        public AdvancedCharEnumerator CharEnumerator { get; set; }

        public List<RegionToken> Tokens { get; set; }

        public string CurrentParsedString  { get; set; }
    }
}
