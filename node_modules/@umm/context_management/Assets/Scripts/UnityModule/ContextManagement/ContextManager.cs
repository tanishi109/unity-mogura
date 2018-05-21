using System.Collections.Generic;
using JetBrains.Annotations;

namespace UnityModule.ContextManagement
{
    [PublicAPI]
    public static class ContextManager
    {
        private static Dictionary<string, IProjectContext> projectContextMap;

        private static Dictionary<string, IProjectContext> ProjectContextMap
        {
            get
            {
                if (projectContextMap == default(Dictionary<string, IProjectContext>))
                {
                    projectContextMap = new Dictionary<string, IProjectContext>();
                }

                return projectContextMap;
            }
            set { projectContextMap = value; }
        }

        public static IProjectContext CurrentProject { get; set; }

        // ReSharper disable once ParameterHidesMember
        public static void RegisterProjectContextMap(Dictionary<string, IProjectContext> projectContextMap)
        {
            ProjectContextMap = projectContextMap;
        }

        public static void RegisterProjectContext(IProjectContext projectContext)
        {
            ProjectContextMap[projectContext.Name] = projectContext;
        }
    }
}