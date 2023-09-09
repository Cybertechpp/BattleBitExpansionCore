using System.Net;

namespace CyberTechBattleBit2.Events;

public class SererConnectingToAPI : EventBase, IEventWrapper<SererConnectingToAPI_Data, bool>

{
    public SererConnectingToAPI() : base(EventTypes.ServerConnectingToAPI)
    {
    }

    public SererConnectingToAPI_Data Data { get; set; }

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

public class SererConnectingToAPI_Data : EventBaseData
{
    public SererConnectingToAPI_Data(CustomGameServer gs, IPAddress ipaddress) : base(gs)
    {
        IPAddress = ipaddress;
    }

    public IPAddress IPAddress { get; set; }
}