namespace CyberTechBattleBit2.Events;

public class PlayerDisconnectLeaveGameserver : EventBase
{
    public PlayerDisconnectLeaveGameserver() : base(EventTypes.PlayerLeaveGameserver)
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

public class PlayerDisconnectLeaveGameserver_Data : EventBaseData
{
    public CustomPlayer Player;

    public PlayerDisconnectLeaveGameserver_Data(CustomGameServer gs, CustomPlayer p) : base(gs)
    {
        Player = p;
    }
}