using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;
using Terminal.Gui;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand]
[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class SaveCommand : Command
{
    public SaveCommand() : base("save")
    {
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        if (args.Length == 0)
        {
            // Program.Instance.L.mInstanceDatabase.GetPlayerInstance()
            foreach (var p in Gameserver.AllPlayers)
            {
                BattleBitExtenderMain.Instance.DSM.SavePlayerData(p);
                Tools.ConsoleLog($"Saving {p.Name} data now...");
            }

            return false;
        }

        var subarg = args[0].ToLower();
        var al = args.ToList();
        al.RemoveAt(0);
        args = al.ToArray();

        var tp = getPlayerByName(subarg);
        if (tp == null)
        {
            Tools.ConsoleLog($"Error could not find a user with the name {subarg}]");
            return false;
        }

        BattleBitExtenderMain.Instance.DSM.SavePlayerData(tp);
        Tools.ConsoleLog($"Saved Player {tp}");


        return true;
    }

    public override bool RunConsoleCommand(string[] args)
    {
        foreach (var gs in BattleBitExtenderMain.Instance.L.ConnectedGameServers)
        foreach (var p in gs.AllPlayers)
        {
            BattleBitExtenderMain.Instance.DSM.SavePlayerData(p);
            Tools.ConsoleLog($"Saving {p.Name} data now...");
        }

        BattleBitExtenderMain.Instance.DBM.onClose();

        return false;
    }

    public override void OnSuccess()
    {
    }
}