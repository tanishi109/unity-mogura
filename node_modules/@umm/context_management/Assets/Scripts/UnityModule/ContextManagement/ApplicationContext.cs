using JetBrains.Annotations;

namespace UnityModule.ContextManagement
{
    public interface IApplicationContext
    {
    }

    [PublicAPI]
    public static class ApplicationContextExtension
    {
        public static TApplicationContext As<TApplicationContext>(this IApplicationContext applicationContext) where TApplicationContext : class, IApplicationContext
        {
            return applicationContext as TApplicationContext;
        }
    }
}