using CAFU.Generator.Enumerates;
using JetBrains.Annotations;

namespace CAFU.Generator.Structure.Partial
{
    [UsedImplicitly]
    public class Factory : PartialStructureBase
    {
        public const string StructureName = "Factory";

        public override string Name { get; } = StructureName;

        public override string Render(Parameter parameter)
        {
            return new ScriptGenerator(parameter, CreateTemplatePath(TemplateType.Partial, StructureName))
                .ReplacePlaceholders();
        }
    }
}