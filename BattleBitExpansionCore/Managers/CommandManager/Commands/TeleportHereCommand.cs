using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand]
[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class TeleportHereCommand : Command
{
    public TeleportHereCommand() : base("tphere")
    {
    }


    public CustomPlayer? findPlayer(string name)
    {
        CustomPlayer? tp = null;
        var rs = int.MaxValue;
        // Console.WriteLine($"TYYYYYYYYYYYYYYYY : {Gameserver == null}");
        // Console.WriteLine($"TYYYYYYYYYYYYYYYY : {Gameserver.AllPlayers}");
        foreach (var pp in Gameserver.AllPlayers)
        {
            var pn = pp.Name;
            Console.WriteLine($"ON PLAYER {pn} {name} {pn.Contains(name)}");
            if (pn.Contains(name))
            {
                // Console.WriteLine($"ON CONATIANS PLAYER {pn}");
                var trs = pn.Length - name.Length;
                Console.WriteLine($"ON CONATIANS PLAYER {pn} {rs} > {trs}");
                if (trs < rs)
                {
                    tp = pp;
                    rs = trs;
                }
            }
        }

        return tp;
    }

    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        Tools.ConsoleLog($"FYI YOUR ARS {args[0]}");
        //TP {Player} | You to Player
        if (args.Length == 1)
        {
            var tp = findPlayer(args[0]);
            if (tp == null)
            {
                Tools.ConsoleLog("NO PLAYER FOUND");
                return true;
            }

            tp.Teleport(Player.Position);
            Tools.ConsoleLog("TEEEELELELEPOORTTEDDDD!!!!!!!!!!!!!!!!!!");
        }


        var sxp = Player.ServerXP;
        // Gameserver.MessageToPlayer(Player,"Test!!!!!");
        Gameserver.SayToChat($"You Have {sxp} Server XP and are Level {Player.Level}", Player);

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