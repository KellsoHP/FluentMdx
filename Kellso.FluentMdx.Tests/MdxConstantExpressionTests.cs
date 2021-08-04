using FluentAssertions;
using FluentMdx;
using NUnit.Framework;

namespace FluentMdx.Tests
{
    [TestFixture]
    public class MdxConstantExpressionTests
    {
        [TestCase("1", "\"1\"")]
        [TestCase("SomeProperty", "\"SomeProperty\"")]
        [TestCase("   ", "\"   \"")]
        [TestCase("\r\n", "\"\"")]
        [TestCase("null", null)]
        [TestCase(MdxConstants.NullConstant, null)]
        public void ShouldGetCorrectStringConstant(string value, string expectedExpr)
        {
            var constExpr = new MdxConstantExpression().WithValue(value);
            var expr = constExpr.ToString();
            expr.Should().Be(expectedExpr);
        }

        [TestCase(0, "0")]
        [TestCase(-1, "-1")]
        [TestCase(1, "1")]
        [TestCase(4379213, "4379213")]
        public void ShouldGetCorrectIntConstant(int value, string expectedExpr)
        {
            var constExpr = new MdxConstantExpression().WithValue(value);
            var expr = constExpr.ToString();
            expr.Should().Be(expectedExpr);
        }

        [Test]
        public void ShouldGetCorrectNullConstant()
        {
            var constExpr = new MdxConstantExpression().WithValue(null);
            var expr = constExpr.ToString();
            expr.Should().Be(MdxConstants.NullConstant);
        }
    }
}
