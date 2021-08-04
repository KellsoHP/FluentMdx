using FluentMdx.Lexer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentMdx.Builder
{
    internal sealed class Structure : IEnumerable<StructureDefinitionBase>
    {
        private readonly IReadOnlyCollection<StructureDefinitionBase> structureDefenition;

        public IEnumerator<StructureDefinitionBase> GetEnumerator()
        {
            return this.structureDefenition.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Structure(IReadOnlyCollection<StructureDefinitionBase> structureDefenition)
        {
            this.structureDefenition = structureDefenition ?? throw new ArgumentNullException(nameof(structureDefenition));
        }

        
    }
}
