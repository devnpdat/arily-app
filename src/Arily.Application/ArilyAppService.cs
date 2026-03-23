using System;
using System.Collections.Generic;
using System.Text;
using Arily.Localization;
using Volo.Abp.Application.Services;

namespace Arily;

/* Inherit your application services from this class.
 */
public abstract class ArilyAppService : ApplicationService
{
    protected ArilyAppService()
    {
        LocalizationResource = typeof(ArilyResource);
    }
}
