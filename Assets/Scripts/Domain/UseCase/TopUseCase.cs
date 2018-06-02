using CAFU.Core.Domain.UseCase;

namespace Mogura.Domain.UseCase
{
    public interface ITopUseCase : IUseCase
    {
    }

    public class TopUseCase : ITopUseCase
    {
        public class Factory : DefaultUseCaseFactory<TopUseCase>
        {
            protected override void Initialize(TopUseCase instance)
            {
                base.Initialize(instance);
            }
        }
    }
}