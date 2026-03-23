using Volo.Abp.Modularity;

namespace Arily;

/* Inherit from this class for your domain layer tests. */
public abstract class ArilyDomainTestBase<TStartupModule> : ArilyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
