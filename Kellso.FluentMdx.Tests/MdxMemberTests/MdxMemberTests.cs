using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace FluentMdx.Tests.MdxMemberTests
{
    [TestFixture]
    public class MdxMemberTests
    {
        [Test]
        public void ShouldCreateWithTitled()
        {
            const string expectedString = "[Dimension].[Attribute]";

            var member = new MdxMember().Titled("Dimension", "Attribute");

            member.ToString().Should().Be(expectedString);
        }

        [Test]
        public void ShouldCreateWithTitledAndValue()
        {
            const string expectedString = "[Dimension].[Attribute].&[1]";

            var member = new MdxMember().Titled("Dimension", "Attribute").WithValue("1");

            member.ToString().Should().Be(expectedString);
        }

        [TestCase("[Dimension].[Attribute].&[1].Properties(\"Key\")", "Properties", new[] { "Key" })]
        public void ShouldCreateWithTitledAndValueAndNavigationFunction(string expectedString, string functionName, object[] functionParams)
        {
            var memberFunction = new MdxNavigationFunction().Titled(functionName).WithParameters(functionParams.Select(Mdx.ConstantValue).ToArray());
            var member = new MdxMember().Titled("Dimension", "Attribute").WithValue("1").WithFunction(memberFunction);

            member.ToString().Should().Be(expectedString);
        }
    }
}
