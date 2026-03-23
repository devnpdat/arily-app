using Volo.Abp.Settings;

namespace Arily.Settings;

public class ArilySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ArilySettings.MySetting1));
    }
}
