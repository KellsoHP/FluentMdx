using FluentMdx.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMdx.Builder
{
    internal abstract class MdxExpressionBuilderBase
    {
        public abstract Structure ExpressionStructure { get; }

        public bool TryGetMdxExpression(IEnumerable<RegionToken> tokens)
        {
            return true;
        }
    }
}
