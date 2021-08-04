namespace FluentMdx.Lexer.Regions
{
    internal static class RegionConstants
    {
        // TODO: Gorodilov_RA: Check this symbols.

        public static char[] ForbiddenSymbols { get; } = new[] { '.', ',', ';', '\'', '`', ':', '/', '\\', '*', '|', '?', '"', '&', '%', '$', '!', '+', '=', '[', ']', '{', '}', '<', '>', '\r', '\n', '\t' };

        public static char[] StrongForbiddenSymbols { get; } = new[] { ',', ';', '\'', '`', '/', '\\', '*', '|', '?', '"', '$', '[', ']', '{', '}', '<', '>', '\r', '\n', '\t' };

        public static char[] FunctionNameAcceptedSymbols { get; } = new[] { '_' };
    }
}
