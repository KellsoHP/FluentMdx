﻿namespace FluentMdx
{
    /// <summary>
    /// Performs syntactic analysis on text and provides strongly-typed structures as a result.
    /// </summary>
    public interface IMdxParser
    {
        /// <summary>
        /// Performs syntactic analysis on text and if suceeds, returns constructed <see cref="MdxQuery"/> as a result.
        /// </summary>
        /// <param name="source">String containing query text representation.</param>
        /// <returns>Instance of parsed <see cref="MdxQuery"/>.</returns>
        MdxQuery ParseQuery(string source);

        /// <summary>
        /// Performs syntactic analysis on text and if suceeds, returns constructed <see cref="MdxMember"/> as a result.
        /// </summary>
        /// <param name="source">String containing Mdx Member text representation.</param>
        /// <returns>Instance of parsed <see cref="MdxMember"/>.</returns>
        MdxMember ParseMember(string source);
    }
}