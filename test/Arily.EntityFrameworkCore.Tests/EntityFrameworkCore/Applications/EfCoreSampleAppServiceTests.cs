using Arily.Samples;
using Xunit;

namespace Arily.EntityFrameworkCore.Applications;

[Collection(ArilyTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ArilyEntityFrameworkCoreTestModule>
{

}
