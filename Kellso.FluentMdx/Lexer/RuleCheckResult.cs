using System;

namespace FluentMdx.Lexer
{
    [Flags]
    public enum RuleCheckResult
    {
        Unknown = 1,
        Found = 2,
        NotFound = 4,
        RegionPart = 8,
        StartPart = 16,
        EndPart = 32,
        TitlePart = 64,
        SubRegionsStartPart = 128
    }
}
