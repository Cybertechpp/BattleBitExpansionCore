using System.Numerics;
using BattleBitAPI.Common;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Commands;

[PluginAttributes.PluginCommand]
// [PluginAttributes.CommandPermission(ServerBasicPermissionLevel.Admin)]
public class ReviveCommand : Command
{
    public ReviveCommand() : base("revive")
    {
        HideFromHelp = true;
    }


    public override bool RunPlayerCommand(CustomPlayer sender, string[] args)
    {
        //WIP
        var tp = Player;
        tp.Modifications.RespawnTime = 0;
        // tp.Modifications. = 0;
        // tp.Modifications.rev = 0;
        var lp = new Vector3(tp.Position.X, tp.Position.Y, tp.Position.Z);
        // var ld = Tools.CloneJson(tp);
        var lpl = Tools.CloneJson(tp.CurrentLoadout);
        var lpw = Tools.CloneJson(tp.CurrentWearings);
        Console.WriteLine("PREEEEEEEEEEEEEEE " + tp.IsBleeding + " | " + tp.IsDead + " | " + tp.IsDown);
        tp.SetHP(100);

        // tp.Kill();
        // Thread.Sleep(1000);
        // tp.Heal(35);
        // Vector3 ld;

        Task.Delay(1000).ContinueWith(t =>
        {
            Console.WriteLine("GOIGOGOGOOGOGOGOG" + tp.IsBleeding + " | " + tp.IsDead + " | " + tp.IsDown);
            tp.SpawnPlayer(lpl, lpw, lp, lp, PlayerStand.Standing, 2);
            Console.WriteLine("GOIGOGOGOOGOGOGOG" + tp.IsBleeding + " | " + tp.IsDead + " | " + tp.IsDown);
        });

        return true;
    }

    public override void OnSuccess()
    {
    }
}