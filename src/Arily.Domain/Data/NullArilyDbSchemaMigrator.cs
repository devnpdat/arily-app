using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Arily.Data;

/* This is used if database provider does't define
 * IArilyDbSchemaMigrator implementation.
 */
public class NullArilyDbSchemaMigrator : IArilyDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
