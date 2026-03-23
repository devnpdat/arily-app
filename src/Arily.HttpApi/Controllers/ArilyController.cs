using Arily.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arily.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ArilyController : AbpControllerBase
{
    protected ArilyController()
    {
        LocalizationResource = typeof(ArilyResource);
    }
}
