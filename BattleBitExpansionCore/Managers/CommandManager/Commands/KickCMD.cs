using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand("Kick Player from Gameserver", "/kick <Player> [Message]")]
[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class KickCMD : Command
{
    public KickCMD() : base("kick")
    {
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        // Gameserver = Program.Instance.L.ConnectedGameServers.ToList()[0];

        if (Args.Length == 0)
        {
            sendCommandUsage();
            return false;
        }

        var a = args.ToList();
        var subarg = a[0];
        a.RemoveAt(0);
        args = a.ToArray();
        var tp = getPlayerBySteamID(subarg);
        if (tp == null) tp = getPlayerByName(subarg);
        if (tp == null)
        {
            Tools.ConsoleLog($"Error! Could not find a Player that had a Name or SteamID that matched `{subarg}`");
            return false;
        }

        tp.GameServer.SayToAllChat($"{tp.Name} has been kicked from the server!");
        Tools.ConsoleLog($"{tp.Name} has been kicked from the server!");
        tp.Kick(args.Length > 0 ? string.Join(" ", args) : "You have been kicked from the Server!");
        return true;
    }

    public override bool RunConsoleCommand(string[] args)
    {
        if (Args.Length == 0)
        {
            sendCommandUsage();
            return false;
        }

        var a = args.ToList();
        var subarg = a[0];
        a.RemoveAt(0);
        args = a.ToArray();

        var tp = getPlayerBySteamID(subarg);
        if (tp == null) tp = getPlayerByName(subarg);
        if (tp == null)
        {
            Tools.ConsoleLog($"Error! Could not find a Player that had a Name or SteamID that matched `{subarg}`");
            return false;
        }

        tp.GameServer.SayToAllChat($"{tp.Name} has been kicked from the server!");
        Tools.ConsoleLog($"{tp.Name} has been kicked from the server!");
        tp.Kick(args.Length > 0 ? string.Join(" ", args) : "You have been kicked from the Server!");
        return true;
    }

    public override void OnSuccess()
    {
    }
}