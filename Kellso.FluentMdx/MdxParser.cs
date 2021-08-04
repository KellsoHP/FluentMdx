using System;
using System.Linq;
using FluentMdx.Lexer;

namespace FluentMdx
{
    /// <summary>
    /// Represents a machine that performs syntactical analysis.
    /// </summary>
    public sealed class MdxParser : IMdxParser
    {
        private readonly ILexer lexer;

        public MdxMember ParseMember(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            var tokens = this.lexer.Tokenize(source).ToArray();
            if (tokens.Length == 0)
                return null;

            throw new NotImplementedException();
        }

        public MdxQuery ParseQuery(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentException($"'{nameof(source)}' cant be empty or whitespace.", nameof(source));

            var tokens = this.lexer.Tokenize(source);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MdxParser"/> with default lexical analysis machine.
        /// </summary>
        public MdxParser()
        {
            lexer = new KeLexer();
        }
    }
}