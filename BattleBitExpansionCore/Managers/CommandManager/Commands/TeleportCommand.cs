using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand]
[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class TeleportCommand : Command
{
    public TeleportCommand() : base("tp")
    {
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        Tools.ConsoleLog($"FYI YOUR ARS {args[0]}");
        //TP {Player} | You to Player
        if (args.Length == 1)
        {
            var tp = FindPlayer(args[0]);
            if (tp == null)
            {
                Tools.ConsoleLog("NO PLAYER FOUND");
                return true;
            }

            // tp.Teleport(Player.Position);
            Player.Teleport(tp.Position);
            Tools.ConsoleLog("TEEEELELELEPOORTTEDDDD!!!!!!!!!!!!!!!!!!");
        }


        var sxp = Player.ServerXP;
        // Gameserver.MessageToPlayer(Player,"Test!!!!!");
        // Gameserver.SayToChat($"You Have {sxp} Server XP and are Level {Player.Level}", Player);

        return true;
    }

    public override void OnSuccess()
    {
        Tools.ConsoleLog("TP WAS RAN!!!");
    }

    public override void onFail()
    {
        Console.WriteLine("WTFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
        Console.WriteLine("WTFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
        Console.WriteLine("WTFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
    }
}