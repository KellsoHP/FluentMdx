//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using FluentAssertions;
//using FluentMdx;
//using FluentMdx.EnumerableExtensions;
//using NUnit.Framework;

//namespace FluentMdx.Tests
//{
//    [TestFixture, ExcludeFromCodeCoverage, Ignore("OLD")]
//    public class MdxParserPartsTests
//    {
//        private Lexer.Lexer _lexer;

//        [OneTimeSetUp]
//        public void TestFixtureSetUp()
//        {
//            _lexer = new Lexer.Lexer();
//        }

//        [Test]
//        public void ParseAxisParameter_WithSubsequentIdentifiers_SucceedsReturnsAxisParameter()
//        {
//            const string queryString = "[Aaa].[Bbb].[Ccc].FUNCTION(1, 2).FUNCTION";
//            const string expectedString = "[Aaa].[Bbb].[Ccc].FUNCTION(1, 2).FUNCTION";

//            var tokens = _lexer.Tokenize(queryString).GetStatedTwoWayEnumerator();
//            bool isSucceeded = MdxParser.TryParseMember(tokens, out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxMember>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseNavigationFunction_WithSubsequentFunctions_SuccedsAndReturnsFunction()
//        {
//            const string queryString = "FUNCTION(1, 2)";
//            const string expectedString = "FUNCTION(1, 2)";

//            bool isSucceeded = MdxParser.TryParseNavigationFunction(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxNavigationFunction>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseAxis_WithParameters_SecceedsAndReturnsAxis()
//        {
//            const string queryString = "NON EMPTY { [Aaa].[Bbb].[Ccc].FUNCTION(1, 2).FUNCTION, [Aaa] } ON Columns";
//            const string expectedString = "NON EMPTY { [Aaa].[Bbb].[Ccc].FUNCTION(1, 2).FUNCTION, [Aaa] } ON Columns";

//            bool isSucceeded = MdxParser.TryParseAxis(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxAxis>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseAxis_WithParameterAndDimensionProperties_SecceedsAndReturnsAxis()
//        {
//            const string queryString = "NON EMPTY { [Aaa] } DIMENSION PROPERTIES CATALOG_NAME, CUSTOM_ROLLUP ON Columns";
//            const string expectedString = "NON EMPTY { [Aaa] } DIMENSION PROPERTIES CATALOG_NAME, CUSTOM_ROLLUP ON Columns";

//            bool isSucceeded = MdxParser.TryParseAxis(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxAxis>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseCube_WithParameters_SecceedsAndReturnsCube()
//        {
//            const string queryString = "[Aaa].[Bbb].[Ccc]";
//            const string expectedString = "[Aaa].[Bbb].[Ccc]";

//            bool isSucceeded = MdxParser.TryParseCube(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxCube>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseMember_WithSubsequentIdentifiersAndValue_SuceeedsAndReturnsValueMember()
//        {
//            const string queryString = "[Aaa].[Bbb].[Ccc].&[1]";
//            const string expectedString = "[Aaa].[Bbb].[Ccc].&[1]";

//            bool isSucceeded = MdxParser.TryParseMember(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxMember>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseMember_WithFunctionAfterValueMember_SuceeedsAndReturnsValueMemberWithFunction()
//        {
//            const string queryString = "[Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1].AllMembers";
//            const string expectedString = "[Dim1 Hierarchy].[Dim1].[Dim1 Key].&[1].AllMembers";

            
//            bool isSucceeded = MdxParser.TryParseMember(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxMember>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseFunction_WithNoParameters_SuceeedsAndReturnsFunction()
//        {
//            const string queryString = "MYFUNCTION()";
//            const string expectedString = "MYFUNCTION()";

//            bool isSucceeded = MdxParser.TryParseFunction(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxFunction>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseFunction_WithSingleFunctionParameter_SuceeedsAndReturnsFunction()
//        {
//            const string queryString = "MYFUNCTION(MYOTHERFUNCTION())";
//            const string expectedString = "MYFUNCTION(MYOTHERFUNCTION())";

//            bool isSucceeded = MdxParser.TryParseFunction(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxFunction>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseFunction_WithSingleSetParameter_SuceeedsAndReturnsFunction()
//        {
//            const string queryString = "MYFUNCTION(())";
//            const string expectedString = "MYFUNCTION((  ))";

//            bool isSucceeded = MdxParser.TryParseFunction(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxFunction>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseFunction_WithSingleTupleParameter_SuceeedsAndReturnsFunction()
//        {
//            const string queryString = "MYFUNCTION({ })";
//            const string expectedString = "MYFUNCTION({  })";
            
//            bool isSucceeded = MdxParser.TryParseFunction(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxFunction>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseFunction_WithSingleMemberParameter_SuceeedsAndReturnsFunction()
//        {
//            const string queryString = "MYFUNCTION([Id])";
//            const string expectedString = "MYFUNCTION([Id])";

//            bool isSucceeded = MdxParser.TryParseFunction(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxFunction>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseFunction_WithSingleExpressionParameter_SuceeedsAndReturnsFunction()
//        {
//            const string queryString = "MYFUNCTION([Id] > 1)";
//            const string expectedString = "MYFUNCTION([Id] > 1)";

//            bool isSucceeded = MdxParser.TryParseFunction(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxFunction>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseFunction_WithSingleNumberAndTextExpressionParameter_SuceeedsAndReturnsFunction()
//        {
//            const string queryString = "MYFUNCTION.Func(1 + 'asd')";
//            const string expectedString = "MYFUNCTION.Func(1 + 'asd')";

//            bool isSucceeded = MdxParser.TryParseFunction(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxFunction>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseExpression_WithNegatedExpression_SuceeedsAndReturnsExpression()
//        {
//            const string queryString = "NOT 1=1+2-1-1";
//            const string expectedString = "(NOT (1 = 1 + 2 - 1 - 1))";

//            bool isSucceeded = MdxParser.TryParseExpression(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxExpression>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseExpression_WithNegatedEqualityExpression_SuceeedsAndReturnsExpression()
//        {
//            const string queryString = "TRUE AND NOT 1=2";
//            const string expectedString = "TRUE AND (NOT (1 = 2))";

//            bool isSucceeded = MdxParser.TryParseExpression(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxExpression>();
//            expression.ToString().Should().Be(expectedString);
//        }
        
//        [Test]
//        public void ParseExpression_WithSimpleExpression_SuceeedsAndReturnsExpression()
//        {
//            const string queryString = "TRUE AND NOT FALSE";
//            const string expectedString = "TRUE AND (NOT (FALSE))";

//            bool isSucceeded = MdxParser.TryParseExpression(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxExpression>();
//            expression.ToString().Should().Be(expectedString);
//        }

//        [Test]
//        public void ParseRange_WithTwoMembers_SuceeedsAndReturnsRange()
//        {
//            const string queryString = "[A].&[1]:[A].&[2]";
//            const string expectedString = "[A].&[1]:[A].&[2]";

//            bool isSucceeded = MdxParser.TryParseRange(_lexer.Tokenize(queryString).GetStatedTwoWayEnumerator(), out var expression);

//            isSucceeded.Should().BeTrue();
//            expression.Should().BeAssignableTo<MdxRange>();
//            expression.ToString().Should().Be(expectedString);
//        }
//    }
//}
