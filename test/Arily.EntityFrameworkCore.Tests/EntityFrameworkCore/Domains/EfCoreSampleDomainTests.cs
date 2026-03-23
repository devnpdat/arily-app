using Arily.Samples;
using Xunit;

namespace Arily.EntityFrameworkCore.Domains;

[Collection(ArilyTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ArilyEntityFrameworkCoreTestModule>
{

}
