namespace CyberTechBattleBit2.Events;

public abstract class EventBase : IEventBase, IEvent
    // where TEventBaseData : EventBaseData
{
    private EventBaseData _data;
    public EventBaseData SaveType;


    internal bool SystemEvent = false;


    protected EventBase(EventTypes eventType, bool returnAfterTrue = false,
        EventPriority priority = EventPriority.MEDIUM)
    {
        // GS = gs;
        EventType = eventType;
        ReturnAfterTrue = returnAfterTrue;
        Priority = priority;
    }

    public virtual object EventBaseData { get; set; }

    public CustomGameServer? GS { get; set; }

    public override List<Type> GetConstructorTypes()
    {
        return new List<Type>
        {
            typeof(CustomGameServer)
        };
    }

    public virtual EventBaseData Data { get; set; }

    // public virtual object HandlePlayerCommand()
    // {
    //     return null;
    // }
    // public virtual object HandleServerCommand()
    // {
    //     return null;
    // }

    public virtual object? fireEvent()
    {
        // return GS == null ? HandleServerCommand() : HandlePlayerCommand();
        // throw new NotImplementedException();
        return null;
    }

    //IS THIS SITLL VISIBLE  OH NO!!!
    public virtual void LoadData<T>(T data) where T : EventBaseData
    {
        Data = data;
        GS = data.GS ?? null;
    }
}

public class EventBaseData
{
    public CustomGameServer GS;
    public object _PreviousData;

    public EventBaseData(CustomGameServer gs)
    {
        GS = gs;
    }
}