namespace FluentMdx.Builder
{
    internal abstract class StructureDefinitionBase
    {
        public bool IsRequired { get; }

        public bool IsRepeatable { get; }

        public StructureDefinitionBase(bool isRequired = false, bool isRepeatable = false)
        {
            this.IsRequired = isRequired;
            this.IsRepeatable = isRepeatable;
        }
    }
}
