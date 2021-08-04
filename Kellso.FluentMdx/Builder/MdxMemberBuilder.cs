using FluentMdx.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentMdx.Builder
{
    internal class MdxMemberBuilder : MdxExpressionBuilderBase
    {
        public override Structure ExpressionStructure { get; } = new Structure(new List<StructureDefinitionBase>
        {
            new RegionStructureDefinition(new[] { RegionMdxType.Identifier }, isRequired: true),
            new GroupRegionStructureDefinition(isRequired: false, isRepeatable: true, 
                new RegionStructureDefinition(new[] { RegionMdxType.DotDelimiter }, isRequired: true),
                new RegionStructureDefinition(new[] { RegionMdxType.Identifier }, isRequired: true)),
            new GroupRegionStructureDefinition(isRequired: false, isRepeatable: false,
                new RegionStructureDefinition(new[] { RegionMdxType.DotDelimiter }, isRequired: true),
                new RegionStructureDefinition(new[] { RegionMdxType.IdentifierValue }, isRequired: true)),
            new GroupRegionStructureDefinition(isRequired: false, isRepeatable: true,
                new RegionStructureDefinition(new[] { RegionMdxType.DotDelimiter }, isRequired: true),
                new RegionStructureDefinition(new[] { RegionMdxType.Function, RegionMdxType.Word }, isRequired: true))
        });
    }
}
