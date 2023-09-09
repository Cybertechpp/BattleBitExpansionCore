using System.Drawing;
using ANSIConsole;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand("Manage Gameservers", "/Gameserver")]
[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class GameserverCMD : Command
{
    public GameserverCMD() : base("gameserver")
    {
        ServerOnlyCommand = true;
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        return true;
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

    public override bool RunCommandConsole(string[] args)
    {
            BattleBitExtenderMain Main = BattleBitExtenderMain.Instance;
        if (Args.Length == 0)
        {
            var gsl = Main.L.ConnectedGameServers.ToList();
            if (gsl.Count == 0)
            {
                Tools.ConsoleLog("Error no Gameservers connected!".Color(ConsoleColor.White).Background(ConsoleColor.Red));
                return true;
            }
            var rs = "Available Gameservers\n";
            var k = 0;
            foreach (var gs in gsl)
            {
                rs += $"[{k}] => {gs.ServerName.Substring(0,15)}\n";
                k++;
            }
            
            Tools.ConsoleLog(rs);
            
            return true;
        }


        var a = args.ToList();
        var subarg = a[0];
        a.RemoveAt(0);
        args = a.ToArray();

        var gsl2 = Main.L.ConnectedGameServers.ToList();
        if (gsl2.Count == 0)
        {
            Tools.ConsoleLog("Error no Gameservers connected!".Color(ConsoleColor.White).Background(ConsoleColor.Red));
            return true;
        }

        int i;
        try
        {
            i = int.Parse(subarg);
        }
        catch (Exception e)
        {
            
            Tools.ConsoleLog($"Error Getting Gameservers `{subarg}` as Int!".Color(ConsoleColor.White).Background(ConsoleColor.Red));
            return true;
        }

        CustomGameServer? z = gsl2[i];
        Main.TargetGameServer = z;
        Tools.ConsoleLog($"Successfully set Gameserver to {z.ServerName.Color(Color.Aqua)}".Background(ConsoleColor.DarkGreen).Color(ConsoleColor.White));
        
        return true;
    }

    public override void OnSuccess()
    {
    }
}