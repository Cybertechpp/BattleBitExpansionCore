using System.Collections;
using System.Drawing;
using System.Reflection;
using ANSIConsole;
using BattleBitAPI.Common;
using BattleBitExpansionCore.Managers.PluginManager;
using CyberTechBattleBit2;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2;

public abstract class Command : CommandBase
{
    public Command(string command, List<PluginAttributes.CommandPermissionAttribute>? perms = null) : base(command, perms)
    {
    }

    // public Object getSubCommandFromArgs(string[] args)
    // {
    //     Object l = new Object();
    //     var subarg = args[0].ToLower();
    //     var al = args.ToList();
    //     al.RemoveAt(0);
    //     args = al.ToArray();
    //     l["a"] = subarg;
    //     l.Add(args);
    //     return l;
    // }

    public CustomPlayer? FindPlayer(string name)
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

    public virtual void onPreAddToServer()
    {
        List<CommandHideFromHelp> hideFromHelp = GetType().GetCustomAttributes<CommandHideFromHelp>().ToList();
        List<PluginAttributes.PluginCommandAttribute> commandInfo = GetType().GetCustomAttributes<PluginAttributes.PluginCommandAttribute>().ToList();
        if (hideFromHelp.Count > 0) HideFromHelp = true;
        if (commandInfo.Count > 0)
        {
            var v = commandInfo[0];
            Description = v.CommandDescription;
            Usage = v.CommandUsage;
            // this.Description = v;
        }
        //TODO Check to see if it has CommandInfo Attribute and Check if it has 

        ReregisterCommandPermissions();
    }

    public void ReregisterCommandPermissions()
    {
        if (Perms.Count == 0)
        {
            List<PluginAttributes.CommandPermissionAttribute> rawPermList = GetType().GetCustomAttributes<PluginAttributes.CommandPermissionAttribute>().ToList();
            if (rawPermList.Count == 0)
                Perms.Add(new PluginAttributes.CommandPermissionAttribute(ServerBasicPermissionLevel.Public));
            else
                Perms.AddRange(rawPermList);
        }
    }

    public CustomPlayer? getPlayerByName(string name)
    {
        if (Gameserver == null)
        {
            Tools.ConsoleLog("Unable to get player while Gameserver is null!");
            return null;
        }

        CustomPlayer? tp = null;
        var rs = int.MaxValue;
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

    public CustomPlayer? getPlayerBySteamID(ulong id)
    {
        return getPlayerBySteamID(id.ToString());
    }

    public CustomPlayer? getPlayerBySteamID(string id)
    {
        var name = id.ToString();
        if (Gameserver == null)
        {
            Tools.ConsoleLog("Unable to get player while Gameserver is null!");
            return null;
        }

        CustomPlayer? tp = null;
        var rs = int.MaxValue;
        foreach (var pp in Gameserver.AllPlayers)
        {
            var pn = pp.SteamID.ToString();
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
}

public abstract class CommandBase
{
    public CommandBase(string command, List<PluginAttributes.CommandPermissionAttribute>? perms = null)
    {
        Command = command;
        if (perms != null) Perms = perms;
    }


    public bool ServerOnlyCommand { get; set; } = false;

    public bool ServerCommandImplemented { get; set; } = false;
    public bool HideFromHelp { get; set; } = false;

    public string Command { get; set; }
    public string? HelpText { get; set; } = null;
    public string? Description { get; set; } = null;
    public string? Usage { get; set; } = null;
    public string[] Args { get; private set; }
    public CustomPlayer? Player { get; private set; }
    public ChatChannel Channel { get; private set; }
    public List<PluginAttributes.CommandPermissionAttribute> Perms { get; private set; } = new();

    public CustomGameServer? Gameserver { get; set; }

    public virtual string FormatForHelpCommand()
    {
        return $"/{Command} - {HelpText}";
    }

    public string GetCommand()
    {
        return Command;
    }

    public virtual void sendCommandUsage()
    {
        if (Player != null)
            Player.SayToChat($"Invalid usage of the command {Command}");
        else
            Tools.ConsoleLog($"Invalid usage of the command {Command}");
    }

    public bool TryRunCommand(string[] args, CustomPlayer sender, ChatChannel channel, CustomGameServer gs)
    {
        Gameserver = gs;
        Args = args;
        Player = sender;
        Channel = channel;


        bool r;
        if (Player != null)
        {
            var checkperms = CheckCommandPermissions();
            if (!checkperms)
            {
                onPermFail();
                return false;
            }

            r = RunPlayerCommand(Player, Args);
        }
        else
        {
            r = RunConsoleCommand(Args);
        }

        if (r) OnSuccess();
        else onFail();
        return false;
    }

    public bool CheckCommandPermissions(CustomPlayer p = null, bool log = true)
    {
        if (p == null) p = Player;
        if (p == null)
        {
            Tools.ConsoleLog("Error 112211");
            return false;
        }

        return PermissionManager.Instsance.CheckPerms(Perms, p, log);


        return true;
    }

    /// <summary>
    /// Return false to indicate that the command has failed.
    /// </summary>
    /// <returns></returns>
    public virtual bool RunConsoleCommand(string[] args)
    {
        Tools.ConsoleLog($"Console command not implemented for {Command}".Background(Color.Orange).Color(ConsoleColor.Gray));
        return true;
    }

    public abstract bool RunPlayerCommand(CustomPlayer sender, string[] args);


    public virtual void OnSuccess()
    {
    }

    public virtual void onFail()
    {
        Tools.ConsoleLog($"Command {Command} Failed!".Background(ConsoleColor.Red).Color(ConsoleColor.White));
    }

    public virtual void onPermFail()
    {
        Tools.ConsoleLog($"{Player ?? null} Tried to run command {Command} Failed!".Background(ConsoleColor.Red).Color(ConsoleColor.White));
        if (Player != null) Player.SayToChat($"Error could not run! You do not have the permissions needed! {Perms.Count}");
        foreach (var ppp in Perms) Tools.ConsoleLog($"DA PL {ppp.PermissionLevel} ||| PS {string.Join("|", ppp.PermissionStrings)}");
    }
}

public class CommandHideFromHelp : Attribute
{
    public bool Value { get; set; } = true;
}

public abstract class CommandBaseHelper : CommandBase
{
    public CommandBaseHelper(string command, List<PluginAttributes.CommandPermissionAttribute>? perms = null) : base(command, perms)
    {
    }
}