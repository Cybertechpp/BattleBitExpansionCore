using System.Net;
using BattleBitAPI.Common;

namespace CyberTechBattleBit2.Events;

public class ServerCreatingServerInstance : EventBase, IEventWrapper<ServerCreatingServerInstance_Data, CustomGameServer>
{
    public ServerCreatingServerInstance() : base(EventTypes.ServerCreatingServer)
    {
        // SaveType = new ServerConnectEventData();
    }

    public override object? fireEvent()
    {
        return new CustomGameServer();
    }

    // public override void LoadData(EventBaseData data)
    // {
    //     var d = (ServerConnectEventData)data;
    //     GS = d.GS;
    //     base.LoadData(data);
    // }
    public ServerCreatingServerInstance_Data Data { get; set; }
}

public class ServerCreatingServerInstance_Data : EventBaseData
{
    public IPAddress IpAddress;
    public int Port;

    public ServerCreatingServerInstance_Data(CustomGameServer gs, IPAddress ipAddress, int port) : base(gs)
    {
        IpAddress = ipAddress;
        Port = port;
    }
}

public class ServerLogEvent : EventBase
{
    public ServerLogEvent() : base(EventTypes.ServerLogEvent)
    {
        // SaveType = new ServerConnectEventData();
    }


    public ServerLogEvent_Data Data { get; set; }

    public Array callEvent()
    {
        return new List<object>()
        {
            Data.LogLvl,
            Data.Message,
            Data.Args
        }.ToArray();
    }
}

public class ServerLogEvent_Data : EventBaseData
{
    public LogLevel LogLvl;
    public string Message;
    public object? Args;

    public ServerLogEvent_Data(CustomGameServer gs, string message, LogLevel logLvl, object? args) : base(gs)
    {
        Message = message;
        LogLvl = logLvl;
        Args = args;
    }
}