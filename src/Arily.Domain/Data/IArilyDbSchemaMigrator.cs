using System.Threading.Tasks;

namespace Arily.Data;

public interface IArilyDbSchemaMigrator
{
    Task MigrateAsync();
}
