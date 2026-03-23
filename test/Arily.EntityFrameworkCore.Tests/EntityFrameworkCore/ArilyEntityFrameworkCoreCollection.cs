using Xunit;

namespace Arily.EntityFrameworkCore;

[CollectionDefinition(ArilyTestConsts.CollectionDefinitionName)]
public class ArilyEntityFrameworkCoreCollection : ICollectionFixture<ArilyEntityFrameworkCoreFixture>
{

}
