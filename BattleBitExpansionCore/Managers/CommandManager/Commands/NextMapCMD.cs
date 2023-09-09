using System.Reflection;
using BattleBitAPI.Common;
using BattleBitExpansionCore.DataSaver.Managers;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand("Force Next map to start", "/nextmap")]
[PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class NextMapCMD : Command
{
    public NextMapCMD() : base("nextmap")
    {
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
            GameServerSettingHolder gsd = BattleBitExtenderMain.Instance.GameServerSettings.getGameServerSettings(Gameserver);
        if (args.Length == 0)
        {
            // gsd.ExtenderGamemodeMapData.SetNextGamemodeMapSize(Gameserver);
            // Gameserver.ForceEndGame(Team.None);
            // Gameserver.SayToAllChat("Current Game is ending!  The next map will be");
            gsd.ExtenderGamemodeMapData.ForceStartNewGame(Gameserver);
            return true;
        }
        
        var subarg = args[0].ToLower();
        var al = args.ToList();
        al.RemoveAt(0);
        args = al.ToArray();

        if (subarg == "info")
        {
            var nmk = gsd.ExtenderGamemodeMapData.GetNextKey();
            var nm = gsd.ExtenderGamemodeMapData.RotationData[nmk];
                sender.SayToChat($"Next map will be {nm.Map} {nm.Size} V X {nm.IncreaseMapSizeToAccomidateCurrentPlayers} Increase Bool");
        
        }

        return true;
    }

    public override void OnSuccess()
    {
    }
}