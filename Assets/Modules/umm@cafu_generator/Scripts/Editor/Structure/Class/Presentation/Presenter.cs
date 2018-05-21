using System.Linq;
using CAFU.Generator.Enumerates;
using CAFU.Generator.Structure.Partial;
using ExtraLinq;
using JetBrains.Annotations;
using UnityEditor;

namespace CAFU.Generator.Structure.Class.Presentation
{
    [UsedImplicitly]
    public class Presenter : ClassStructureBase
    {
        private const string StructureName = "Presentation/Presenter";

        public override string Name { get; } = StructureName;

        protected override ParentLayerType ParentLayerType { get; } = ParentLayerType.Presentation;

        protected override LayerType LayerType { get; } = LayerType.Presenter;

        private bool HasFactory { get; set; }

        private int CurrentSceneNameIndex { get; set; }

        public Presenter()
        {
            HasFactory = true;
        }

        public Presenter(int currentSceneNameIndex, bool hasFactory)
        {
            CurrentSceneNameIndex = currentSceneNameIndex;
            HasFactory = hasFactory;
        }

        public override void OnGUI()
        {
            base.OnGUI();
            CurrentSceneNameIndex = EditorGUILayout.Popup("Scene Name", CurrentSceneNameIndex, GeneratorWindow.SceneNameList.ToArray());
            HasFactory = EditorGUILayout.Toggle("Use Factory?", HasFactory);
            GeneratorWindow.GetAdditionalOptionRenderDelegateList(LayerType)?.ToList().ForEach(x => x());
        }

        public override void Generate(bool overwrite)
        {
            var parameter = new Parameter()
            {
                ParentLayerType = ParentLayerType,
                LayerType = LayerType,
                ClassName = $"{GeneratorWindow.SceneNameList[CurrentSceneNameIndex]}{LayerType.ToString()}",
                SceneName = GeneratorWindow.SceneNameList[CurrentSceneNameIndex],
                Overwrite = overwrite,
            };
            parameter.Namespace = CreateNamespace(parameter);

            GeneratorWindow.GetAdditionalStructureExtensionDelegateList(LayerType)?.ToList().ForEach(x => x(parameter));

            if (parameter.UsingList.IsEmpty())
            {
                parameter.UsingList.Add("CAFU.Core.Presentation.Presenter");
                parameter.ImplementsInterfaceList.Add("IPresenter");
            }

            if (HasFactory)
            {
                parameter.UsingList.Add("CAFU.Core.Presentation.Presenter");
            }

            var generator = new ScriptGenerator(parameter, CreateTemplatePath(TemplateType.Class, StructureName));
            if (HasFactory)
            {
                generator.AddPartial(Factory.StructureName, GeneratorWindow.GetPartialStructure(Factory.StructureName).Render(parameter));
            }

            generator.Generate(CreateOutputPath(parameter));
        }
    }
}