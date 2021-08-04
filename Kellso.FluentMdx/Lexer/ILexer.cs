using System.Collections.Generic;

namespace FluentMdx.Lexer
{
    /// <summary>
    /// Transforms characters sequence into list of <see cref="RegionToken"/> objects.
    /// </summary>
    public interface ILexer
    {
        /// <summary>
        /// Transforms text into list of <see cref="RegionToken"/> objects.
        /// </summary>
        /// <param name="source">Text to tokenize.</param>
        /// <returns>Returns list of tokenized items.</returns>
        IEnumerable<RegionToken> Tokenize(string source);
    }
}