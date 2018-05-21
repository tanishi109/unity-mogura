using System.IO;
using CAFU.Generator.Enumerates;
using UnityModule.ContextManagement;

namespace CAFU.Generator
{
    public interface IStructure
    {

    }

    public abstract class StructureBase : IStructure
    {
        protected virtual string ModuleDirectory { get; } = "Modules";

        protected virtual string ModuleName { get; } = "umm@cafu_generator";

        protected virtual string TemplateDirectory { get; } = "Templates";

        protected virtual string TemplateExtension { get; } = ".txt";

        protected virtual string CreateTemplatePath(TemplateType templateType, string templateName)
        {
            // プロジェクト側のファイルを走査
            var path = Path.Combine(
                UnityEngine.Application.dataPath,
                TemplateDirectory,
                templateType.ToString(),
                // Windows 用に念のためディレクトリセパレータを置換
                templateName.Replace("/", Path.DirectorySeparatorChar.ToString()) + TemplateExtension
            );
            if (File.Exists(path))
            {
                return path;
            }

            // umm 側のファイルを走査
            path = Path.Combine(
                UnityEngine.Application.dataPath,
                ModuleDirectory,
                ModuleName,
                TemplateDirectory,
                templateType.ToString(),
                // Windows 用に念のためディレクトリセパレータを置換
                templateName.Replace("/", Path.DirectorySeparatorChar.ToString()) + TemplateExtension
            );
            if (File.Exists(path))
            {
                return path;
            }

            throw new FileNotFoundException("Template file not found.", path);
        }
    }

    public static class StructureExtension
    {
        public static string CreateNamespacePrefix(this IStructure structure)
        {
            if (GeneratorWindow.ProjectContext == default(IProjectContext))
            {
                return string.Empty;
            }

            return $"{GeneratorWindow.ProjectContext.NamespacePrefix.Trim('.')}.";
        }
    }
}