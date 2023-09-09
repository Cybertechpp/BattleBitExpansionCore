using CyberTechBattleBit2;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace BattleBitExpansionCore_TestPlugin;

[PluginAttributes.PluginCommand()]
public class RevengeCMD : Command
{
    public RevengeCMD() : base("revenge")
    {
    }

    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        // Tools.ConsoleLog("TESTING THIS MESSAGE ONLY!!!!");
        var a = (TestPlugin)TestPlugin.getInstance();
        // Tools.ConsoleLog($"TESTING THIS MESSAGE ONLY!!!! {a} {a.GetType()}");
        var xx = String.Join(" , ", a.Rdata[Player.SteamID]);
        Player.SayToChat($"You need to Kill {xx}");
        Tools.ConsoleLog($"{Player.Name} needs to kill {xx}");
        return true;
    }
}