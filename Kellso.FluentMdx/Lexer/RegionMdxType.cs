using System;

namespace FluentMdx.Lexer
{
    public enum RegionMdxType
    {
        Identifier,
        IdentifierValue,
        Measure,
        DigitValue,
        StringValue,
        Function,
        NonEmpty,
        CrossJoin,
        Select,
        Where,
        From,
        With,
        SetWord,
        MemberWord,
        Tuple,
        Set,
        DotDelimiter,
        CommaDelimiter,
        Word,
        As,
        LogicOperator,
        On,
        Order,
        MultiplySymbol,
        AxisNameIdentifier
    }
}
