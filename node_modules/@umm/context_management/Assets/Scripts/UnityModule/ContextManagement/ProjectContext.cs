using JetBrains.Annotations;
using UnityEngine;
#pragma warning disable 649

namespace UnityModule.ContextManagement
{
    [PublicAPI]
    public interface IProjectContext
    {
        string Name { get; }

        string SceneNamePrefix { get; }

        string NamespacePrefix { get; }

        // 本来であれば extension method にしたいところだが、 il2cpp の AOT 制約により enum な extension method は NG なのでこちらに定義する
        string CreateSceneName<TEnum>(TEnum sceneName) where TEnum : struct;
    }

    public class ProjectContext : IProjectContext
    {
        [SerializeField] private string name;

        public string Name
        {
            get { return name; }
        }

        [SerializeField] private string sceneNamePrefix;

        public string SceneNamePrefix
        {
            get { return sceneNamePrefix; }
        }

        [SerializeField] private string namespacePrefix;

        public string NamespacePrefix
        {
            get { return namespacePrefix; }
        }

        public string CreateSceneName<TEnum>(TEnum sceneName) where TEnum : struct
        {
            return string.Format("{0}{1}", SceneNamePrefix, sceneName.ToString());
        }
    }

    [PublicAPI]
    public static class ProjectContextExtension
    {
        public static TProjectContext As<TProjectContext>(this IProjectContext projectContext) where TProjectContext : class, IProjectContext
        {
            return projectContext as TProjectContext;
        }
    }
}