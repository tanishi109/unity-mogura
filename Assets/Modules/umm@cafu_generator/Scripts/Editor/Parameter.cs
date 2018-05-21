using System.Collections.Generic;
using System.Linq;
using CAFU.Generator.Enumerates;
using ExtraLinq;

namespace CAFU.Generator
{
    public class Parameter
    {
        public ParentLayerType ParentLayerType { get; set; } = default(ParentLayerType);

        public LayerType LayerType { get; set; } = default(LayerType);

        public string SceneName { get; set; } = string.Empty;

        public IList<string> UsingList { get; } = new List<string>();

        public string Namespace { get; set; } = string.Empty;

        public string ClassName { get; set; } = string.Empty;

        public string InterfaceName { get; set; } = string.Empty;

        public string BaseClassName { get; set; } = string.Empty;

        public IList<string> ImplementsInterfaceList { get; } = new List<string>();

        public IList<Property> PropertyList { get; } = new List<Property>();

        public bool IsSingleton { get; set; }

        public bool Overwrite { get; set; }

        public struct Property
        {
            internal Accessibility Accessibility { get; set; }
            internal string Interface { get; set; }
            internal string Type { get; set; }
            internal string Name { get; set; }
        }
    }

    // XXX: ここのアロー演算子は、ちょっとやり過ぎかな…？
    public static class ParameterExtension
    {
        public static string CreateUsings(this Parameter parameter) => parameter.UsingList.IsEmpty()
            ? string.Empty
            : parameter
                .UsingList
                .Distinct()
                .OrderBy(x => x)
                .Aggregate(string.Empty, (a, b) => $"{a}using {b};\n")
              + "\n";

        public static string CreateImplementsInterfaces(this Parameter parameter) => parameter.ImplementsInterfaceList.IsEmpty()
            ? string.Empty
            : (!string.IsNullOrEmpty(parameter.BaseClassName) ? ", " : string.Empty)
              + parameter
                  .ImplementsInterfaceList
                  .Distinct()
                  .OrderBy(x => x)
                  .Aggregate((a, b) => $"{a}, {b}");

        public static string CreateProperties(this Parameter parameter, int indentLevel) => parameter.PropertyList.IsEmpty()
            ? string.Empty
            : "\n"
              + parameter
                  .PropertyList
                  .Distinct()
                  .OrderBy(x => x.Type)
                  .Aggregate(
                      string.Empty,
                      // setter を private に制限している
                      (a, b) => $"{a}{ScriptGenerator.Indent(indentLevel)}{b.Accessibility.ToString().ToLower()} {b.Interface} {b.Name} {{ get; private set; }}\n"
                  );

        public static string CreateInitializers(this Parameter parameter, int indentLevel) => parameter.PropertyList.IsEmpty()
            ? string.Empty
            : parameter
                .PropertyList
                .Distinct()
                .OrderBy(x => x.Type)
                .Aggregate(
                    string.Empty,
                    (a, b) => $"{a}{ScriptGenerator.Indent(indentLevel)}instance.{b.Name} = new {b.Type}.Factory().Create();\n"
                );
    }
}