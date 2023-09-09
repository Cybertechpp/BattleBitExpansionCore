using BattleBitAPI.Common;
using JetBrains.Annotations;

namespace CyberTechBattleBit2.Events;

public class PlayerKillEvent : EventBase
{
    public PlayerKillEvent() : base(EventTypes.PlayerKillEvent)
    {
        // SaveType = new ServerConnectEventData();
    }

    public override object? fireEvent()
    {
        var data = (PlayerKillEvent_Data)Data;
        var killer = data.Killer;
        var victim = data.Victim;
        killer.OnPlayerKill(victim);
        victim.OnPlayerDeath(killer);
        return base.fireEvent();
    }

    // public override EventBaseData Data { get; set; }

    // public override void LoadData(EventBaseData data)
    // {
    //     var d = (ServerConnectEventData)data;
    //     GS = d.GS;
    //     base.LoadData(data);
    // }
    // public override object HandlePlayerCommand()
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public override object HandleServerCommand()
    // {
    //     throw new NotImplementedException();
    // }

    // public abstract override object? fireEvent()
    // {
    //     return base.fireEvent();
    // }
}

public class PlayerKillEvent_Data : EventBaseData
{
    public CustomPlayer Killer;
    public CustomPlayer Victim;
    public OnPlayerKillArguments<CustomPlayer> Args;

    public PlayerKillEvent_Data(CustomGameServer gs, CustomPlayer killer, CustomPlayer victim,
        OnPlayerKillArguments<CustomPlayer> onPlayerKillArguments) : base(gs)
    {
        Args = onPlayerKillArguments;
        Killer = killer;
        Victim = victim;
    }

    public PlayerKillEvent_Data(CustomGameServer gs, CustomPlayer killer, CustomPlayer victim) : base(gs)
    {
        Killer = killer;
        Victim = victim;
    }
}