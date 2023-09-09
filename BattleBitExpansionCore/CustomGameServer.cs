using BattleBitAPI;
using BattleBitAPI.Common;
using BattleBitAPI.Server;
using JetBrains.Annotations;

namespace CyberTechBattleBit2;

public class CustomGameServer : GameServer<CustomPlayer>
{
    public Func<CustomGameServer, CustomPlayer, ChatChannel, string, Task<bool>>? CommandTaskList { get; set; }

    public Func<ulong, PlayerJoiningArguments, Task>? PlayerJoinServerTaskList { get; set; }
    public Func<CustomGameServer, OnPlayerKillArguments<CustomPlayer>, Task>? PlayerKillEvent { get; set; }
    public Func<CustomPlayer, Task>? PlayerLeaveServerTaskList { get; set; }
    public Func<CustomPlayer, Task>? PlayerOnConnectTaskList { get; set; }


    // this.ServerSettings = new ServerSettings<TPlayer>(this);
    // this.MapRotation = new MapRotation<TPlayer>(this);
    // this.GamemodeRotation = new GamemodeRotation<TPlayer>(this);
    // this.RoundSettings = new RoundSettings<TPlayer>(this);
    // this.mChangedModifications = new Queue<(ulong steamID, PlayerModifications<TPlayer>.mPlayerModifications)>(254);

    // public override void AddPlayer(Player<TPlayer> player)
    // {
    //     base.AddPlayer(player);
    // }

    public override Task OnAPlayerDownedAnotherPlayer(OnPlayerKillArguments<CustomPlayer> args)
    {
        if (PlayerKillEvent != null) PlayerKillEvent(this, args);
        var victim = args.Victim;
        var killer = args.Killer;
        var damage = 100;
        var killxp = 0;
        if (killer.KillStreak > 2) killxp += 50 * killer.KillStreak;

        killxp += damage;
        killer.AddXP(killxp);
        victim.TakeXP(100 / 5); //20XP
        return Task.CompletedTask;
        //TODO Calculate Distance
        //TODO Calculate Damage Done
        //TODO Give XP
        //
        // if (args.Victim.IsZombie)
        // {
        //     args.Victim.IsZombie = false;
        //     args.Victim.Message("You are no longer zombie");
        //
        //     AnnounceShort("Choosing new zombie in 5");
        //     await Task.Delay(1000);
        //     AnnounceShort("Choosing new zombie in 4");
        //     await Task.Delay(1000);
        //     AnnounceShort("Choosing new zombie in 3");
        //     await Task.Delay(1000);
        //     AnnounceShort("Choosing new zombie in 2");
        //     await Task.Delay(1000);
        //     AnnounceShort("Choosing new zombie in 1");
        //     await Task.Delay(1000);
        //
        //     args.Killer.IsZombie = true;
        //     args.Killer.SetHeavyGadget(Gadgets.SledgeHammer.ToString(), 0, true);
        //
        //     // var position = args.Killer.GetPosition();
        // }
    }

    public override async Task OnPlayerDisconnected(CustomPlayer player)
    {
        if (PlayerLeaveServerTaskList != null) await PlayerLeaveServerTaskList(player);
        // await base.OnPlayerDisconnected(player);
    }

    public override async Task OnPlayerConnected(CustomPlayer player)
    {
        if (PlayerOnConnectTaskList != null) await PlayerOnConnectTaskList(player);
        // await base.OnPlayerConnected(player);
    }

    // playerjoi

    public override async Task OnPlayerJoiningToServer(ulong steamID, PlayerJoiningArguments args)
    {
        // Tools.ConsoleLog($"--------------------------------------- ");
        // Tools.ConsoleLog($"--------------------------------------- {steamID} {args == null}");
        try
        {
            if (PlayerJoinServerTaskList != null) await PlayerJoinServerTaskList(steamID, args);
        }
        catch (Exception e)
        {
            Tools.ConsoleLog("YOOOO WTF IS THIS SHITTT");
            Tools.ConsoleLog("YOOOO WTF IS THIS SHITTT");
            Tools.ConsoleLog(e);
        }

        // Tools.ConsoleLog("--------------------------------------55555-");

        // await base.OnPlayerJoiningToServer(steamID, args);
    }

    public override async Task OnRoundEnded()
    {
        if (BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.AutoExpandServer)
        {
            var c = AllPlayers.ToArray().Length;
            var d = BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.AutoExpandThreashHolad;
            MapSize m = this.MapSize;

            if (m == MapSize.None)
            {
                
                SetServerSizeForNextMatch(Tools.GetCorrectMapSize(c,d));
                return;
            }
            var mc = (int)(byte)m;

            var a = ((mc * 2) - 5);

            if (a <= c)
            {
                //INCREASE SIZE
                SetServerSizeForNextMatch(m.Increase());
                
            }

        }
    }
    
    

    public virtual async Task<bool> OnPlayerTypedCommand(CustomPlayer player, ChatChannel channel, string msg)
    {
        Tools.ConsoleLog($"{player.Name} Ran Command > {msg}");
        var r = await CommandTaskList(this, player, channel, msg);
        return false; //Do not Sent Back
    }

    public override async Task<bool> OnPlayerTypedMessage(CustomPlayer player, ChatChannel channel, string msg)
    {
        if (msg.Substring(0, 1) == "!" || msg.Substring(0, 1) == "/")
            //Command
            return await OnPlayerTypedCommand(player, channel, msg);

        //Allow Chat -- TODO add settings
        return true; //await base.OnPlayerTypedMessage(player, channel, msg);
    }


    
    
    public override async Task OnConnected()
    {
        await Console.Out.WriteLineAsync("Current state: " + RoundSettings.State);
        var a = BattleBitExtenderMain.Instance.GameServerSettings.getGameServerSettings(this);
        a.SetGSValues(this);
        await a.ExtenderGamemodeMapData.OnStart(this);
    }

    public override async Task OnGameStateChanged(GameState oldState, GameState newState)
    {
        await Console.Out.WriteLineAsync("State changed to -> " + newState);
        if (newState == GameState.WaitingForPlayers)
        {
            RoundSettings.PlayersToStart = BattleBitExtenderMain.Instance.GameServerSettings.getGameServerSettings(this).RoundSettings.PlayersToStart;
        }else if (newState == GameState.CountingDown)
        {
            RoundSettings.SecondsLeft = BattleBitExtenderMain.Instance.GameServerSettings.getGameServerSettings(this).RoundCountDown;;
        }else if (newState == GameState.EndingGame)
        {
            RoundSettings.PlayersToStart = BattleBitExtenderMain.Instance.GameServerSettings.getGameServerSettings(this).RoundSettings.PlayersToStart;
            RoundSettings.SecondsLeft = 5;
        }
    }
}