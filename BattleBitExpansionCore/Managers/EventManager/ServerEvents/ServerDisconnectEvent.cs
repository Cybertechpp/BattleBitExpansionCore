namespace CyberTechBattleBit2.Events;

public class ServerDisconnectEvent : EventBase
{
    public ServerDisconnectEvent() : base(EventTypes.ServerDisconnectEvent)
    {
        // SaveType = new ServerConnectEventData();
    }

    public override object? fireEvent()
    {
        foreach (var customPlayer in GS.AllPlayers)
        {
            BattleBitExtenderMain.Instance.DSM.SavePlayerData(customPlayer);
            customPlayer.Kick("Gameserver shutting down!");
        }

        return base.fireEvent();
    }

    // public override void LoadData(EventBaseData data)
    // {
    //     var d = (ServerConnectEventData)data;
    //     GS = d.GS;
    //     base.LoadData(data);
    // }
}

public class ServerDisconnectEvent_Data : EventBaseData
{
    public ServerDisconnectEvent_Data(CustomGameServer gs) : base(gs)
    {
    }
}