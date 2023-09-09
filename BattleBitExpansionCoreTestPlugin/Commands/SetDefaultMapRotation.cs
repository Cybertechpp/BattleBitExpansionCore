using BattleBitExpansionCore.DataSaver.Managers;
using CyberTechBattleBit2;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace BattleBitExpansionCore_TestPlugin;


[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
[PluginAttributes.PluginCommand()]
public class SetDefaultMapRotation : Command
{
    public SetDefaultMapRotation() : base("setdefaultmaprotation")
    {
    }

    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        foreach (GameServerSettingHolder gameServerSettingHolder in BattleBitExtenderMain.Instance.GameServerSettings.GameServerSettings.Values)
        {
            Tools.ConsoleLog($"ON THIS PLUGIN {gameServerSettingHolder.ServerSettings.Name}");
            gameServerSettingHolder.ExtenderGamemodeMapData.AddDefaultMaps();
        }

        return true;
    }

    public override bool RunConsoleCommand(string[] args)
    {
        foreach (GameServerSettingHolder gameServerSettingHolder in BattleBitExtenderMain.Instance.GameServerSettings.GameServerSettings.Values)
        {
            Tools.ConsoleLog($"ON THIS PLUGIN {gameServerSettingHolder.ServerSettings.Name}");
            gameServerSettingHolder.ExtenderGamemodeMapData.AddDefaultMaps();
        }

        return true;
    }
}