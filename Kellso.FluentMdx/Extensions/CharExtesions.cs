namespace FluentMdx.Extensions
{
    public static class CharExtesions
    {
        public static bool IsCharEqualsWithIgnoreCase(this char first, char second)
        {
            return char.ToLowerInvariant(first) == char.ToLowerInvariant(second);
        }
    }
}
