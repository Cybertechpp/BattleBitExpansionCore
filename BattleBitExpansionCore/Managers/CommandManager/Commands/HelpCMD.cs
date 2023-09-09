using System.Diagnostics.Contracts;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand]
public class HelpCMD : Command
{
    public HelpCMD() : base("help")
    {
    }


    public override bool RunPlayerCommand(CustomPlayer p, string[] args)
    {
        var page = 0;
        if (args.Length >= 1)
            try
            {
                page = int.Parse(args[0]) - 1;
            }
            catch (Exception e)
            {
                Tools.ConsoleLog("Error! Excption!!!!");
                Tools.ConsoleLog(e);
            }

        List<string> fs = new();
        var d = BattleBitExtenderMain.Instance.CM.CommandList.Values.ToList();
        d.OrderBy(i => i.Command);
        foreach (var v in d)
        {
            if (v.Command.ToLower() == "help"  || v.HideFromHelp || v.ServerOnlyCommand) continue;
            if (v.CheckCommandPermissions(Player, false)) fs.Add(v.FormatForHelpCommand());
        }

        p.SayToChat($"==== Help Page [{page + 1}/{Math.Ceiling((decimal)(fs.Count / 5)) + 1}] ({fs.Count} Commands)====");
        var ff = fs.Skip(page * 5).Take(5);
        p.SayToChat(string.Join("\n", ff));
        p.Message($"==== Help Page [{page + 1}/{Math.Ceiling((decimal)(fs.Count / 5)) + 1}] ({fs.Count} Commands)====\n" + string.Join("\n", ff), 60 * 3);

        return true;
    }

    public override bool RunConsoleCommand(string[] args)
    {
        var page = 0;
        if (args.Length >= 1)
            try
            {
                page = int.Parse(args[0]) - 1;
            }
            catch (Exception e)
            {
                Tools.ConsoleLog("Error! Excption!!!!");
                Tools.ConsoleLog(e);
            }

        List<string> fs = new();
        var d = BattleBitExtenderMain.Instance.CM.CommandList.Values.ToList();
        d.OrderBy(i => i.Command);
        foreach (var v in d)
        {
            // if(v.Command.ToLower() == "help" || !v.ServerCommandImplemented)continue;
            if (v.Command.ToLower() == "help"|| v.HideFromHelp) continue;
            fs.Add(v.FormatForHelpCommand());
        }

        Tools.ConsoleLog($"==== Help Page [{page + 1}/{Math.Ceiling((decimal)(fs.Count / 10)) + 1}] ({fs.Count} Commands)====");
        var ff = fs.Skip(page * 10).Take(10);
        Tools.ConsoleLog(string.Join("\n", ff));

        return true;
    }

    public override void OnSuccess()
    {
    }
}