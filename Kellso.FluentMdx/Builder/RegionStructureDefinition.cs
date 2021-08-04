using FluentMdx.Lexer;

namespace FluentMdx.Builder
{
    internal class RegionStructureDefinition : StructureDefinitionBase
    {
        public RegionMdxType[] RegionMdxTypes { get; }

        public RegionStructureDefinition(RegionMdxType[] regionMdxTypes, bool isRequired = false, bool isRepeatable = false)
            : base(isRequired: isRequired, isRepeatable: isRepeatable)
        {
            this.RegionMdxTypes = regionMdxTypes ?? throw new System.ArgumentNullException(nameof(regionMdxTypes));
        }
    }
}
