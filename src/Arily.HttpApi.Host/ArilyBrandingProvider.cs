using Microsoft.Extensions.Localization;
using Arily.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Arily;

[Dependency(ReplaceServices = true)]
public class ArilyBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ArilyResource> _localizer;

    public ArilyBrandingProvider(IStringLocalizer<ArilyResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
