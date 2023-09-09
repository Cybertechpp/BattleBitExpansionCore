namespace CyberTechBattleBit2.Events;

public class ServerConnectEvent : EventBase
{
    public ServerConnectEvent() : base(EventTypes.ServerConnectEvent)
    {
        // SaveType = new ServerConnectEventData();
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

public class ServerConnectEventData : EventBaseData
{
    public ServerConnectEventData(CustomGameServer gs) : base(gs)
    {
    }
}