using System.Reflection;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand]
[CommandHideFromHelp]
public class InfoCommand : Command
{
    public InfoCommand() : base("info")
    {
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        Tools.ConsoleLog("INFOOO >>> ");
        Tools.ConsoleLog("INFOOO >>> ");
        Tools.ConsoleLog("INFOOO >>> ");
        Tools.ConsoleLog("INFOOO >>> ");
        Tools.ConsoleLog($"INFOOO >>>  IsExposedOnMap {Player.Modifications.IsExposedOnMap}");
        Tools.ConsoleLog($"INFOOO >>> HideOnMap {Player.Modifications.HideOnMap}");
        Tools.ConsoleLog($"INFOOO >>> AirStrafe {Player.Modifications.AirStrafe}");
        Tools.ConsoleLog($"INFOOO >>> CanDeploy {Player.Modifications.CanDeploy}");
        Tools.ConsoleLog($"INFOOO >>> CanSpectate {Player.Modifications.CanSpectate}");
        Tools.ConsoleLog($"INFOOO >>> CanUseNightVision {Player.Modifications.CanUseNightVision}");
        Tools.ConsoleLog($"INFOOO >>> CanSuicide {Player.Modifications.CanSuicide}");
        // Tools.ConsoleLog($"INFOOO >>>  {Player.Modifications.HideOnMap}");
        // Tools.ConsoleLog("}");
        try
        {
            var a = Player.Modifications;
            foreach (var v in Player.Modifications.GetType().GetFields())
            {
                Tools.ConsoleLog($"OK WE GOT THIS DATA {v.Name}");
                Tools.ConsoleLog($"OK WE GOT THIS DATA {v.Name} = {v.GetValue(a).ToString()}");
            }
        }
        catch (Exception e)
        {
            Tools.ConsoleLog("EXXXXXXXXXXXXXXXXXXXXXXXXXX");
            Tools.ConsoleLog(e);
        }

        return true;
    }

    public override void OnSuccess()
    {
    }
}