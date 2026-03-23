using Volo.Abp.Modularity;

namespace Arily;

public abstract class ArilyApplicationTestBase<TStartupModule> : ArilyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
