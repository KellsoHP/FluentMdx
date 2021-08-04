using System;
using System.Diagnostics;

namespace FluentMdx.Lexer.Regions
{
    [DebuggerDisplay("'{MdxType}' - ' {Name} '")]
    internal sealed class CommonNameRegionRule : BaseNameRegion
    {
        public override RegionMdxType MdxType { get; }

        public override string Name { get; }

        protected override string RegionStartsSymbols { get; } = null;

        protected override string RegionEndsSymbols { get; } = null;

        public CommonNameRegionRule(string name, RegionMdxType mdxType)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.MdxType = mdxType;
        }
    }
}
