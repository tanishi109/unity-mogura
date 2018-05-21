using System.Linq;
using CAFU.Generator.Enumerates;
using JetBrains.Annotations;
using UnityEditor;

namespace CAFU.Generator.Structure.Class.Domain
{
    [UsedImplicitly]
    public class Translator : ClassStructureBase
    {
        private const string StructureName = "Domain/Translator";

        public override string Name { get; } = StructureName;

        protected override ParentLayerType ParentLayerType { get; } = ParentLayerType.Domain;

        protected override LayerType LayerType { get; } = LayerType.Translator;

        private string ClassName { get; set; }

        public override void OnGUI()
        {
            base.OnGUI();
            ClassName = EditorGUILayout.TextField("Class Name", ClassName);
            GeneratorWindow.GetAdditionalOptionRenderDelegateList(LayerType)?.ToList().ForEach(x => x());
        }

        public override void Generate(bool overwrite)
        {
            var parameter = new Parameter()
            {
                ParentLayerType = ParentLayerType,
                LayerType = LayerType,
                InterfaceName = $"I{ClassName}",
                ClassName = ClassName,
                Overwrite = overwrite,
            };
            parameter.Namespace = CreateNamespace(parameter);

            GeneratorWindow.GetAdditionalStructureExtensionDelegateList(LayerType)?.ToList().ForEach(x => x(parameter));

            var generator = new ScriptGenerator(parameter, CreateTemplatePath(TemplateType.Class, StructureName));

            generator.Generate(CreateOutputPath(parameter));
        }
    }
}