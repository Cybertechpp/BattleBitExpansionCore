using BattleBitAPI.Common;
using CyberTechBattleBit2.Commands;
using CyberTechBattleBit2.Managers;
using YamlDotNet.Serialization.ObjectGraphVisitors;

namespace CyberTechBattleBit2;

public class CommandManager : LogTools.ModuleLogHelper<CommandManager>
{
    public Dictionary<string, Command> CommandList { get; private set; } = new();

    // private readonly CustomGameServer GameServer;
    internal readonly BattleBitExtenderMain P;

    public PermissionManager PM;

    public CommandManager(BattleBitExtenderMain gs)
    {
        P = gs;
        PM = P.PermissionManager;
        LoadCommands();
    }

    public bool AddCommand(Command c)
    {
        //TODO check to see if Command Class has PluginCOmmand Attribute
        c.onPreAddToServer();
        CommandList.Add(c.GetCommand().ToLower(), c);
        Log.Info($"Adding command <{c.GetCommand()}> To command list");
        return true;
    }

    private void LoadBaseCommands()
    {
        var cmds = new List<Command>
        {
            new SXP_Command(),
            new TeleportHereCommand(),
            new ReviveCommand(),
            new SayCommand(),
            new SaveCommand(),
            new HelpCMD(),
            new KickCMD(),
            new NextMapCMD(),
            new InfoCommand(),
            new TeleportCommand(),
            new GameserverCMD(),
            new BanCMD()
        };
        foreach (var c in cmds)
        {
            c.onPreAddToServer();
            CommandList.Add(c.GetCommand().ToLower(), c);
            Log.Info($"Adding Command {c.GetCommand()} To List {c.Perms.Count}!");
        }
    }

    private void LoadCommands()
    {
        LoadBaseCommands();
    }

    public bool HandleCommands(CustomGameServer? gs, string cmd, string[] ags, CustomPlayer? player, ChatChannel channel)
    {
        if (CommandList.ContainsKey(cmd.ToLower()))
        {
            Log.Info("Command found :" + cmd.ToLower());
            var e = CommandList[cmd.ToLower()];
            try
            {
                //TODO Add event to call... Redundant but IG
                var r = e.TryRunCommand(ags, player, channel, gs);
            }
            catch (Exception ee)
            {
                Log.Info("WFAsssadsd d sadas ghw egsdsgertsddsfrer");
                Log.Info(ee);
                if(player != null)player.SayToChat(BBColors.Red+"Error Running Command!");
            }
        }
        else
        {
            Log.Info("No Command found :" + cmd);
            if(player != null)player.SayToChat(BBColors.Red+"No Command found :" + cmd);
        }

        return false;
    }
}