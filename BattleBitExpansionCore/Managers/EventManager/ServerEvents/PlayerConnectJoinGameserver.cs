namespace CyberTechBattleBit2.Events;

public class PlayerConnectJoinGameserver : EventBase
{
    public PlayerConnectJoinGameserver() : base(EventTypes.PlayerJoinGameserver)
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

public class PlayerConnectJoinGameserver_Data : EventBaseData
{
    public CustomPlayer Player;

    public PlayerConnectJoinGameserver_Data(CustomGameServer gs, CustomPlayer p) : base(gs)
    {
        Player = p;
    }
}