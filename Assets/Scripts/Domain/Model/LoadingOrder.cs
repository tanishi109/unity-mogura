using CAFU.Core.Domain.Model;

namespace Mogura.Domain.Model
{
    public interface ILoadingOrder : IModel
    {
    }

    public class LoadingOrder : ILoadingOrder
    {
        public static string From;
        public static string To;
        public static bool Overlay;

        private LoadingOrder()
        {
        }
    }
}