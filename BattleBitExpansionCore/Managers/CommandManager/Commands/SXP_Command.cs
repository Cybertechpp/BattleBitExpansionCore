using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand]
public class SXP_Command : Command
{
    public SXP_Command() : base("XP")
    {
    }
    
    
    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        var sxp = Player.ServerXP;
        // Gameserver.MessageToPlayer(Player,"Test!!!!!");
        Gameserver.SayToChat($"{BBColors.Aqua}You Have {BBColors.Orange}{sxp} {BBColors.Aqua}Server XP and are Level  {BBColors.Orange+Player.Level}", Player);
        var d = Player.Deaths;
        if (d == 0) d = 1;
        Gameserver.SayToChat($"{BBColors.Aqua}You Have {BBColors.Green+Player.Kills} Kills / {BBColors.Red+Player.Deaths} Deaths / {BBColors.DarkCyan+(Player.Kills / d)} KDR", Player);
        Gameserver.SayToChat($"{BBColors.Aqua}You are currently ona {BBColors.Green+Player.KillStreak} Killstreak", Player);

        return true;
    }

    public override void OnSuccess()
    {
        // 
    }
}