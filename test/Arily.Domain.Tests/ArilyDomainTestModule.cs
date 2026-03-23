using Volo.Abp.Modularity;

namespace Arily;

[DependsOn(
    typeof(ArilyDomainModule),
    typeof(ArilyTestBaseModule)
)]
public class ArilyDomainTestModule : AbpModule
{

}
