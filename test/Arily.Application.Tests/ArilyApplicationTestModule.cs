using Volo.Abp.Modularity;

namespace Arily;

[DependsOn(
    typeof(ArilyApplicationModule),
    typeof(ArilyDomainTestModule)
)]
public class ArilyApplicationTestModule : AbpModule
{

}
