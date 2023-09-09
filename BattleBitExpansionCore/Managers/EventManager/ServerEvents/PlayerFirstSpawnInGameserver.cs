namespace CyberTechBattleBit2.Events;

public class PlayerFirstSpawnInGameserver : EventBase
{
    public PlayerFirstSpawnInGameserver() : base(EventTypes.PlayerFirstSpawnInGameserver)
    {
        // SaveType = new ServerConnectEventData();
    }

    public override object? fireEvent()
    {
        var p = ((PlayerFirstSpawnInGameserver_Data)Data).Player;


        p.Message(BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.JoinMessage, 180);
        p.SayToChat(BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.JoinChatMessage);
        // if (p.SessionStats.Deaths == 0 && p.SessionStats.Spawns == 0)
        // {
        //     //FIRE FIRST SPAWN EVENT
        //     var e = new 
        //     Program.Instance.EM.CallEvent()
        // }
        // p.SessionStats.Spawns++;
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

public class PlayerFirstSpawnInGameserver_Data : EventBaseData
{
    public CustomPlayer Player;

    public PlayerFirstSpawnInGameserver_Data(CustomGameServer gs, CustomPlayer p) : base(gs)
    {
        Player = p;
    }
}