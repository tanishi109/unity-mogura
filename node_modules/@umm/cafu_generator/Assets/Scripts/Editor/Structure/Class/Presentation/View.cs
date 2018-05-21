using System.IO;
using System.Linq;
using CAFU.Generator.Enumerates;
using CAFU.Generator.Structure.Partial;
using ExtraLinq;
using JetBrains.Annotations;
using UnityEditor;

namespace CAFU.Generator.Structure.Class.Presentation
{
    [UsedImplicitly]
    public class View : ClassStructureBase
    {
        private const string StructureName = "Presentation/View";

        public override string Name { get; } = StructureName;

        protected override ParentLayerType ParentLayerType { get; } = ParentLayerType.Presentation;

        protected override LayerType LayerType { get; } = LayerType.View;

        private string ClassName { get; set; }

        private int CurrentSceneNameIndex { get; set; }

        public override void OnGUI()
        {
            base.OnGUI();
            CurrentSceneNameIndex = EditorGUILayout.Popup("Scene Name", CurrentSceneNameIndex, GeneratorWindow.SceneNameList.ToArray());
            ClassName = EditorGUILayout.TextField("Class Name", ClassName);
            GeneratorWindow.GetAdditionalOptionRenderDelegateList(LayerType)?.ToList().ForEach(x => x());
        }

        public override void Generate(bool overwrite)
        {
            var parameter = new Parameter()
            {
                ParentLayerType = ParentLayerType,
                LayerType = LayerType,
                ClassName = ClassName,
                SceneName = GeneratorWindow.SceneNameList[CurrentSceneNameIndex],
                Overwrite = overwrite,
            };
            parameter.Namespace = CreateNamespace(parameter);

            GeneratorWindow.GetAdditionalStructureExtensionDelegateList(LayerType)?.ToList().ForEach(x => x(parameter));

            if (parameter.UsingList.IsEmpty())
            {
                parameter.UsingList.Add("UnityEngine");
                parameter.UsingList.Add("CAFU.Core.Presentation.View");
                parameter.ImplementsInterfaceList.Add("IView");
                parameter.BaseClassName = "MonoBehaviour";
            }

            var generator = new ScriptGenerator(parameter, CreateTemplatePath(TemplateType.Class, StructureName));
            generator.AddPartial(StartMethod.StructureName, GeneratorWindow.GetPartialStructure(StartMethod.StructureName).Render(parameter));
            generator.Generate(CreateOutputPath(parameter));
        }

        protected override string CreateNamespace(Parameter parameter) => $"{this.CreateNamespacePrefix()}{ParentLayerType.ToString()}.{LayerType.View.ToString()}.{parameter.SceneName}";

        protected override string CreateOutputPath(Parameter parameter) => Path.Combine(UnityEngine.Application.dataPath, OutputDirectory, parameter.ParentLayerType.ToString(), parameter.LayerType.ToString(), parameter.SceneName, $"{parameter.ClassName}{ScriptExtension}");
    }
}