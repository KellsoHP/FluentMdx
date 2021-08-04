using System;

namespace FluentMdx.Builder
{
    internal class GroupRegionStructureDefinition : StructureDefinitionBase
    {
        public RegionStructureDefinition[] Group { get; }

        public GroupRegionStructureDefinition(bool isRequired = false, bool isRepeatable = false, params RegionStructureDefinition[] groups)
            : base(isRequired: isRequired, isRepeatable: isRepeatable)
        {
            this.Group = groups ?? throw new ArgumentNullException(nameof(groups));
        }
    }
}
