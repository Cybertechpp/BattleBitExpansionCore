namespace CyberTechBattleBit2.Events;

public interface IEvent
    //<TEventDataa,TReturn>

{
    // public EventTypes EventType { get; internal set; }
    // public EventPriority Priority { get; internal set; }
    // public bool ReturnAfterTrue { get; internal set; }
    // public List<Type> GetConstructorTypes();

    public EventBaseData Data { get; internal set; }
    public object? fireEvent();
    public void LoadData<T>(T data) where T : EventBaseData;

    // public void LoadData<T>(T data);
    // public EventBase<> Data { get; set; }
}

public interface IEventWrapper<T, TT> : IEvent
{
    // public EventBaseData Data { get; internal set; }
    public T Data { get; internal set; }
    // public TT? callEvent();

    public void LoadData(T data)
    {
        Data = data;
    }
}