#region

using CyberTechBattleBit2;
using CyberTechBattleBit2.Events;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

#endregion

namespace BattleBitExpansionCore_TestPlugin;

[PluginAttributes.PluginDataSaver(typeof(TestSaveData), "testFile")] //Creates a JsonFIle of Var Class Type
[PluginAttributes.PluginInfo(Author = "Yungtechboy1", Name = "Test Plugin / RevengeBoost",
    Copyright = "All rights Reserved 2023", Description = "Test Plugin demonstrating how to use BBEC API",
    URL = "yungtechboy1.com", Version = "0.0.1")]
public class TestPlugin : PluginBase, IPluginEvent
{
    //This plugin will be called "RevengeBoost". Killing a player that has killed you within 2 of your deaths will add XP and Perks!
    //We will also add a command to so players can see who they need to kill inorder to gain perks.
    public TestPlugin(PluginAttributes.PluginInfoAttribute info) : base(info)
    {
        setInstance(this);
    }

    public new static TestPlugin getInstance()
    {
        return (TestPlugin)Instance;
    }

    [PluginAttributes.PluginEvent(EventTypes.PlayerJoinGameserver)]
    public object PlayerJoinEvent(PlayerConnectJoinGameserver_Data data)
    {
        var p = data.Player;
        Rdata[p.SteamID] = new List<string>();
        return true;
    }


    [PluginAttributes.PluginEvent(EventTypes.PlayerKillEvent)]
    public object PlayerKillEvent(PlayerKillEvent_Data data)
    {
        var killer = data.Killer;
        var victim = data.Victim;
        if (!Rdata.ContainsKey(victim.SteamID)) Rdata[victim.SteamID] = new List<string>();
        if (!Rdata.ContainsKey(killer.SteamID)) Rdata[killer.SteamID] = new List<string>();
        var vrd = Rdata[victim.SteamID];
        var krd = Rdata[killer.SteamID];
        while (vrd.Count >= 2) vrd.RemoveAt(0);

        //VICTIM Tasks
        vrd.Add($"{killer.SteamID}|{killer.Name}");
        victim.SayToChat(
            $"{killer.Name} has been added to your Revenge List!! \n Killing this player will add Server 500 XP");


        //CHECK KILL
        var i = 0;
        foreach (var bpi in krd)
        {
            if (bpi.Contains(victim.SteamID + ""))
            {
                krd.RemoveAt(i);
                Tools.ConsoleLog(
                    $"{killer.Name}[{killer.SteamID}] Killed {victim.Name}[{victim.SteamID}] for an extra 500XP");
                killer.SayToChat($"Congratulations you killed {victim.Name} and gained 500XP");
                killer.AddXP(500);
                // killer.Squad.SquadPoints
                data.GS.SetSquadPointsOf(killer.Team, killer.SquadName, killer.Squad.SquadPoints + 500);
                return true;
            }

            i++;
        }


        return true;
    }


    [PluginAttributes.PluginEvent(EventTypes.ServerConnectEvent)]
    public object ServerConnectEvent(ServerConnectEventData data)
    {
        return true;
    }


    public Dictionary<ulong, List<string>> Rdata = new();


    public override void onDisable()
    {
        //Access DataSaver Attribute/Object
        var d = GetDatastore<TestSaveData>("testFile");
        Log.Info("Test Plugin has been disable!");
    }

    public override void onEnable()
    {
        Log.Info("Test Plugin has been enabled!");
    }

    public override void onLoad()
    {
        Log.Info("Test Plugin has been loaded!");
    }
}