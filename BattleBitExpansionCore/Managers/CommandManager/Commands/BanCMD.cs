using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand("Ban Player from Gameserver", "/ban <Player> <mins> <hours> <days> [Message]")]
[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class BanCMD : Command
{
    public BanCMD() : base("ban")
    {
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        if (Args.Length < 4)
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

        tp.GameServer.SayToAllChat($"{tp.Name} has been banned from the server!");
        Tools.ConsoleLog($"{tp.Name} has been banned from the server!");
        var aaa = args.ToList();
        int tm;
        int th;
        int td;
        try
        {
            tm = int.Parse(aaa[0]);
            th = int.Parse(aaa[1]);
            td = int.Parse(aaa[2]);
        }
        catch (Exception e)
        {
            sender.SayToChat("Error! Please make sure command matches:");
            sender.SayToChat("/ban <player> 1 1 1 [reason]");
            sendCommandUsage();
            return false;
        }

        var aaaa = aaa.Skip(3).ToList();
        tp.BanPlayer(aaaa.Count > 0 ? string.Join(" ", aaaa) : $"You have been banned from the Server for {td} Days {th} Hours {tm} Mins!", sender.Name + "|" + sender.SteamID, new TimeSpan(td, th, tm, 0));
        return true;
    }

    public override bool RunConsoleCommand(string[] args)
    {
        if (Args.Length < 4)
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

        tp.GameServer.SayToAllChat($"{tp.Name} has been banned from the server!");
        Tools.ConsoleLog($"{tp.Name} has been banned from the server!");
        var aaa = args.ToList();
        int tm;
        int th;
        int td;
        try
        {
            tm = int.Parse(aaa[0]);
            th = int.Parse(aaa[1]);
            td = int.Parse(aaa[2]);
        }
        catch (Exception e)
        {
            Tools.ConsoleLog("Error! Please make sure command matches:");
            Tools.ConsoleLog("/ban <player> 1 1 1 [reason]");
            sendCommandUsage();
            return false;
        }

        var aaaa = aaa.Skip(3).ToList();
        tp.BanPlayer(aaaa.Count > 0 ? string.Join(" ", aaaa) : $"You have been banned from the Server for {td} Days {th} Hours {tm} Mins!", "CONSOLE", new TimeSpan(td, th, tm, 0));
        return true;
    }

    public override void OnSuccess()
    {
    }
}