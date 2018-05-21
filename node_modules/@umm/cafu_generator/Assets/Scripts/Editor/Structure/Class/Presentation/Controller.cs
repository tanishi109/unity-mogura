using System.IO;
using System.Linq;
using CAFU.Generator.Enumerates;
using JetBrains.Annotations;
using UnityEditor;

namespace CAFU.Generator.Structure.Class.Presentation
{
    [UsedImplicitly]
    public class Controller : ClassStructureBase
    {
        private const string StructureName = "Presentation/Controller";

        public override string Name { get; } = StructureName;

        protected override ParentLayerType ParentLayerType { get; } = ParentLayerType.Presentation;

        protected override LayerType LayerType { get; } = LayerType.Controller;

        public bool HasPresenterFactory { get; private set; } = true;

        public int CurrentSceneNameIndex { get; private set; }

        public override void OnGUI()
        {
            base.OnGUI();
            CurrentSceneNameIndex = EditorGUILayout.Popup("Scene Name", CurrentSceneNameIndex, GeneratorWindow.SceneNameList.ToArray());
            HasPresenterFactory = EditorGUILayout.Toggle("Use Presenter Factory?", HasPresenterFactory);
            GeneratorWindow.GetAdditionalOptionRenderDelegateList(LayerType)?.ToList().ForEach(x => x());
        }

        public override void Generate(bool overwrite)
        {
            var parameter = new Parameter()
            {
                ParentLayerType = ParentLayerType.Presentation,
                LayerType = LayerType.View,
                ClassName = "Controller",
                SceneName = GeneratorWindow.SceneNameList[CurrentSceneNameIndex],
                Overwrite = overwrite,
            };
            parameter.Namespace = CreateNamespace(parameter);

            GeneratorWindow.GetAdditionalStructureExtensionDelegateList(LayerType)?.ToList().ForEach(x => x(parameter));

            parameter.UsingList.Add("CAFU.Core.Presentation.View");
            parameter.UsingList.Add($"{this.CreateNamespacePrefix()}{ParentLayerType.ToString()}.{LayerType.Presenter.ToString()}");

            var generator = new ScriptGenerator(parameter, CreateTemplatePath(TemplateType.Class, StructureName));
            generator.Generate(CreateOutputPath(parameter));

            new Presenter(CurrentSceneNameIndex, HasPresenterFactory).Generate(overwrite);
        }

        protected override string CreateNamespace(Parameter parameter) => $"{this.CreateNamespacePrefix()}{ParentLayerType.ToString()}.{LayerType.View.ToString()}.{parameter.SceneName}";

        protected override string CreateOutputPath(Parameter parameter) => Path.Combine(UnityEngine.Application.dataPath, OutputDirectory, parameter.ParentLayerType.ToString(), parameter.LayerType.ToString(), parameter.SceneName, $"{parameter.ClassName}{ScriptExtension}");
    }
}