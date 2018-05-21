using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

namespace CAFU.Generator
{
    public class ScriptGenerator
    {
        private const string IndentSpaces = "    ";

        private const int IndentLevelProperty = 2;

        private const int IndentLevelInitializer = 4;

        private static readonly Dictionary<string, Func<Parameter, string>> ParameterAccessor = new Dictionary<string, Func<Parameter, string>>()
        {
            {"ParentLayerName", (parameter) => parameter.ParentLayerType.ToString()},
            {"LayerType", (parameter) => parameter.LayerType.ToString()},
            {"SceneName", (parameter) => parameter.SceneName},
            {"Usings", (parameter) => parameter.CreateUsings()},
            {"Namespace", (parameter) => parameter.Namespace},
            {"ClassName", (parameter) => parameter.ClassName},
            {"InterfaceName", (parameter) => parameter.InterfaceName},
            {"BaseClassName", (parameter) => parameter.BaseClassName},
            {"ImplementsInterfaces", (parameter) => parameter.CreateImplementsInterfaces()},
            {"Properties", (parameter) => parameter.CreateProperties(IndentLevelProperty)},
            {"Initializers", (parameter) => parameter.CreateInitializers(IndentLevelInitializer)},
            {"Singleton", (parameter) => parameter.IsSingleton ? "Singleton" : string.Empty},
        };

        private Dictionary<string, string> PartialMap { get; } = new Dictionary<string, string>();

        private string TemplatePath { get; }

        private Parameter Parameter { get; }

        public ScriptGenerator(Parameter parameter, string templatePath)
        {
            Parameter = parameter;
            TemplatePath = templatePath;
        }

        public void Generate(string outputPath)
        {
            if (string.IsNullOrEmpty(outputPath))
            {
                throw new NullReferenceException("Output path is null or empty.");
            }

            if (!Parameter.Overwrite && File.Exists(outputPath))
            {
                throw new InvalidOperationException($"File `{outputPath}` already exists.");
            }

            if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            }

            File.WriteAllText(outputPath, ReplacePlaceholders());

            AssetDatabase.Refresh();
        }

        public void AddPartial(string key, string value)
        {
            PartialMap[key] = value;
        }

        public string ReplacePlaceholders()
        {
            var source = File.ReadAllText(TemplatePath);
            ParameterAccessor.ToList().ForEach(pair => { source = source.Replace($"##{pair.Key}##", pair.Value(Parameter)); });
            PartialMap.ToList().ForEach(pair => { source = source.Replace($"##{pair.Key}##", pair.Value); });
            source = Regex.Replace(source, "##[^#]+##", string.Empty);
            return source;
        }

        public static string Indent(int level)
        {
            return Enumerable.Range(0, level).Aggregate(string.Empty, (a, b) => $"{a}{IndentSpaces}");
        }
    }
}