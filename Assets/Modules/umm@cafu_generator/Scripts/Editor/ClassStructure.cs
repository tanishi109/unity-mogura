using System.IO;
using CAFU.Generator.Enumerates;

namespace CAFU.Generator
{
    public interface IClassStructure : IStructure
    {
        string Name { get; }

        void OnGUI();

        void Generate(bool overwrite);
    }

    public interface IClassStructureExtension : IStructure
    {
        void OnGUI();

        void Process(Parameter parameter);
    }

    public abstract class ClassStructureBase : StructureBase, IClassStructure
    {
        protected const string OutputDirectory = "Scripts";

        protected const string ScriptExtension = ".cs";

        public abstract string Name { get; }

        protected abstract ParentLayerType ParentLayerType { get; }

        protected abstract LayerType LayerType { get; }

        public virtual void OnGUI()
        {
        }

        public abstract void Generate(bool overwrite);

        protected virtual string CreateNamespace(Parameter parameter) => $"{this.CreateNamespacePrefix()}{ParentLayerType.ToString()}.{LayerType.ToString()}";

        protected virtual string CreateOutputPath(Parameter parameter) => Path.Combine(UnityEngine.Application.dataPath, OutputDirectory, parameter.ParentLayerType.ToString(), parameter.LayerType.ToString(), $"{parameter.ClassName}{ScriptExtension}");
    }
}