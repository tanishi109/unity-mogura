using System.Linq;
using CAFU.Generator.Enumerates;
using CAFU.Generator.Structure.Partial;
using JetBrains.Annotations;
using UnityEditor;

namespace CAFU.Generator.Structure.Class.Data
{
    [UsedImplicitly]
    public class Entity : ClassStructureBase
    {
        private const string StructureName = "Data/Entity";

        public override string Name { get; } = StructureName;

        protected override ParentLayerType ParentLayerType { get; } = ParentLayerType.Data;

        protected override LayerType LayerType { get; } = LayerType.Entity;

        private string ClassName { get; set; }

        private bool IsSingleton { get; set; }

        private bool HasFactory { get; set; }

        public override void OnGUI()
        {
            base.OnGUI();
            ClassName = EditorGUILayout.TextField("Class Name", ClassName);
            IsSingleton = EditorGUILayout.Toggle("Is IsSingleton?", IsSingleton);
            HasFactory = EditorGUILayout.Toggle("Use Factory?", HasFactory);
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
                IsSingleton = IsSingleton,
                Overwrite = overwrite,
            };
            parameter.Namespace = CreateNamespace(parameter);

            GeneratorWindow.GetAdditionalStructureExtensionDelegateList(LayerType)?.ToList().ForEach(x => x(parameter));

            parameter.UsingList.Add("CAFU.Core.Data.Entity");
            parameter.ImplementsInterfaceList.Add(parameter.InterfaceName);

            var generator = new ScriptGenerator(parameter, CreateTemplatePath(TemplateType.Class, StructureName));
            if (HasFactory)
            {
                generator.AddPartial(Factory.StructureName, GeneratorWindow.GetPartialStructure(Factory.StructureName).Render(parameter));
            }

            generator.Generate(CreateOutputPath(parameter));
        }
    }
}