namespace CyberTechBattleBit2.Events;

public class PlayerSpawnInGameserver : EventBase
{
    public PlayerSpawnInGameserver() : base(EventTypes.PlayerSpawnGameserver)
    {
        // SaveType = new ServerConnectEventData();
    }

    public override object? fireEvent()
    {
        var p = ((PlayerSpawnInGameserver_Data)Data).Player;

        if (p.SessionStats.Deaths == 0 && p.SessionStats.Spawns == 0)
        {
            //FIRE FIRST SPAWN EVENT
            var e = new PlayerFirstSpawnInGameserver();
            e.LoadData(new PlayerFirstSpawnInGameserver_Data(Data.GS, p));
            BattleBitExtenderMain.Instance.EM.CallEvent(e);
        }

        p.SessionStats.Spawns++;
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

public class PlayerSpawnInGameserver_Data : EventBaseData
{
    public CustomPlayer Player;

    public PlayerSpawnInGameserver_Data(CustomGameServer gs, CustomPlayer p) : base(gs)
    {
        Player = p;
    }
}