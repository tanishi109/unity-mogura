using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CAFU.Generator.Enumerates;
using UnityEditor;
using UnityEngine;
using UnityModule.ContextManagement;

namespace CAFU.Generator
{
    public class GeneratorWindow : EditorWindow
    {
        private static IDictionary<string, IClassStructure> ClassStructureMap { get; set; } = new Dictionary<string, IClassStructure>();

        private static IDictionary<string, IPartialStructure> PartialStructureMap { get; set; } = new Dictionary<string, IPartialStructure>();

        private static IDictionary<LayerType, IList<Action>> AdditionalOptionRenderDelegateListMap { get; } = new Dictionary<LayerType, IList<Action>>();

        private static IDictionary<LayerType, IList<Action<Parameter>>> AdditionalStructureExtensionDelegateListMap { get; } = new Dictionary<LayerType, IList<Action<Parameter>>>();

        private static IList<string> StructureKeyList { get; set; } = new List<string>();

        public static IList<string> SceneNameList { get; private set; } = new List<string>();

        public static IProjectContext ProjectContext { get; private set; }

        private int CurrentStructureIndex { get; set; }

        private bool Overwrite { get; set; }

        [MenuItem("Window/CAFU/Script Generator")]
        public static void Open()
        {
            GetWindow<GeneratorWindow>("Script Generator");
        }

        public static void RegisterAdditionalOptionRenderDelegate(LayerType layerType, IClassStructureExtension classStructureExtension)
        {
            if (!AdditionalOptionRenderDelegateListMap.ContainsKey(layerType))
            {
                AdditionalOptionRenderDelegateListMap[layerType] = new List<Action>();
            }

            AdditionalOptionRenderDelegateListMap[layerType].Add(classStructureExtension.OnGUI);
        }

        public static void RegisterAdditionalStructureExtensionDelegate(LayerType layerType, IClassStructureExtension classStructureExtension)
        {
            if (!AdditionalStructureExtensionDelegateListMap.ContainsKey(layerType))
            {
                AdditionalStructureExtensionDelegateListMap[layerType] = new List<Action<Parameter>>();
            }

            AdditionalStructureExtensionDelegateListMap[layerType].Add(classStructureExtension.Process);
        }

        public static IEnumerable<Action> GetAdditionalOptionRenderDelegateList(LayerType layerType)
        {
            if (!AdditionalOptionRenderDelegateListMap.ContainsKey(layerType))
            {
                AdditionalOptionRenderDelegateListMap[layerType] = new List<Action>();
            }

            return AdditionalOptionRenderDelegateListMap[layerType];
        }

        public static IEnumerable<Action<Parameter>> GetAdditionalStructureExtensionDelegateList(LayerType layerType)
        {
            if (!AdditionalStructureExtensionDelegateListMap.ContainsKey(layerType))
            {
                AdditionalStructureExtensionDelegateListMap[layerType] = new List<Action<Parameter>>();
            }

            return AdditionalStructureExtensionDelegateListMap[layerType];
        }

        public static IPartialStructure GetPartialStructure(string name) => PartialStructureMap[name];

        private void OnGUI()
        {
            GUILayout.Label("Generate");
            EditorGUI.indentLevel++;
            CurrentStructureIndex = EditorGUILayout.Popup("Structure Type", CurrentStructureIndex, StructureKeyList.ToArray());
            var classStructure = ClassStructureMap[StructureKeyList[CurrentStructureIndex]];

            GUI.enabled = classStructure != default(IClassStructure);
            classStructure?.OnGUI();
            EditorGUI.indentLevel--;
            Overwrite = EditorGUILayout.Toggle("Overwrite?", Overwrite);
            EditorGUILayout.Space();
            if (ProjectContext == default(IProjectContext))
            {
                EditorGUILayout.HelpBox("ProjectContext does not found.\nGenerator will generate without namespace prefix.\nTo generate ProjectContextEntity if you need prepend namespace prefix to scripts.", MessageType.Info);
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Generate"))
            {
                classStructure?.Generate(Overwrite);
            }

            GUI.enabled = true;
        }

        private void OnEnable()
        {
            // クラス構成設定を収集
            ClassStructureMap = Assembly
                .GetAssembly(typeof(GeneratorWindow))
                .GetTypes()
                // IClassStructure の実装型を返す
                .Where(x => typeof(IClassStructure).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as IClassStructure)
                .Where(x => x != null)
                .ToDictionary(x => x.Name, x => x);
            // 部分構成設定を収集
            PartialStructureMap = Assembly
                .GetAssembly(typeof(GeneratorWindow))
                .GetTypes()
                // IClassStructure の実装型を返す
                .Where(x => typeof(IPartialStructure).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                .Select(x => Activator.CreateInstance(x) as IPartialStructure)
                .Where(x => x != null)
                .ToDictionary(x => x.Name, x => x);
            // 未選択状態を作っておく
            ClassStructureMap.Add(" ", null);
            StructureKeyList = ClassStructureMap.Keys.OrderBy(x => x).ToList();

            // プロジェクト情報を取得
            // ReSharper disable once SuspiciousTypeConversion.Global
            ProjectContext = AssetDatabase
                .FindAssets("t:ScriptableObject", new[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
                .OfType<IProjectContext>()
                .FirstOrDefault();

            // シーン名を収集
            SceneNameList = AssetDatabase
                .FindAssets("t:Scene", new[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(Path.GetFileNameWithoutExtension)
                .Where(x => Regex.IsMatch(x, $"^{ProjectContext?.SceneNamePrefix}"))
                .Select(x => Regex.Replace(x, $"^{ProjectContext?.SceneNamePrefix}", string.Empty))
                .Prepend(" ")
                .ToList();
        }
    }
}