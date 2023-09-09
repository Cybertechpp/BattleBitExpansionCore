using BattleBitAPI.Server;

namespace CyberTechBattleBit2.Events.ExtenderEvents;

public class ExtenderStartEvent : EventBase
{
    public ExtenderStartEvent() : base(EventTypes.ServerConnectingToAPI)
    {
    }

    // public SererConnectingToAPI_Data Data { get; set; }

    public override object fireEvent()
    {
        return true;
    }

    // public bool callEvent()
    // {
    //     return true;
    // }

    public virtual void LoadData(SererConnectingToAPI_Data data)
    {
        base.LoadData(data);
    }

    // public override void LoadData(EventBaseData data)
    // {
    //     base.LoadData(data);
    // }
}

public class ExtenderStartEvent_Data : EventBaseData
{
    private readonly object ServerListener;

    public ExtenderStartEvent_Data(CustomGameServer gs, ServerListener<CustomPlayer, CustomGameServer> sl, BattleBitExtenderMain program) : base(gs)
    {
        ServerListener = sl;
    }
}