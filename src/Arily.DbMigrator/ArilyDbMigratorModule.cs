using Arily.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Arily.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ArilyEntityFrameworkCoreModule),
    typeof(ArilyApplicationContractsModule)
    )]
public class ArilyDbMigratorModule : AbpModule
{
}
