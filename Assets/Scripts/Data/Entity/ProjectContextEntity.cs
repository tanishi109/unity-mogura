using CAFU.Core.Data.Entity;
//
//namespace Mogura.Data.Entity
//{
//    public interface IProjectContextEntity : IEntity
//    {
//    }
//
//    public class ProjectContextEntity : IProjectContextEntity
//    {
//    }
//}
using UnityEngine;
using UnityModule.ContextManagement;

namespace Mogura.Data.Entity
{
    [CreateAssetMenu]
    public class ProjectContextEntity : ScriptableObject, IProjectContext
    {
        [SerializeField] private string projectName;
        [SerializeField] private string sceneNamePrefix;
        [SerializeField] private string namespacePrefix;
        
        public string Name => projectName;
        
        public string SceneNamePrefix => sceneNamePrefix;

        public string NamespacePrefix => namespacePrefix;
        
        public string CreateSceneName<TEnum>(TEnum sceneName) where TEnum : struct
        {
            return $"{SceneNamePrefix}{sceneName.ToString()}";
        }
    }
}

