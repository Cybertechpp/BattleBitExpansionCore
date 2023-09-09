using ANSIConsole;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;
using Terminal.Gui;
using Color = System.Drawing.Color;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand]
[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class SayCommand : Command
{
    public SayCommand() : base("say")
    {
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        foreach (var p in Gameserver.AllPlayers) p.SayToChat(string.Join(" ", args));

        return true;
    }

    public override bool RunCommandConsole(string[] args)
    {
        foreach (var p in BattleBitExtenderMain.Instance.L.ConnectedGameServers)
        foreach (var pp in p.AllPlayers)
            pp.SayToChat(BBColors.Aqua + "[GameServer] > " + BBColors.Green + string.Join(" ", args));
        Tools.ConsoleLog("[GameServer] > ".Color(Color.Aqua)  + string.Join(" ", args).Color(ConsoleColor.Green).ToString()) ;
        return true;
    }

    public override void OnSuccess()
    {
    }
}