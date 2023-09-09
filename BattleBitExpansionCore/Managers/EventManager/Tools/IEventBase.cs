namespace CyberTechBattleBit2.Events;

public abstract class IEventBase //: IEvent
{
    // public abstract void LoadData(EventBaseData data);

    // public EventBaseData Data { get; internal set; }

    // public T Data<T>
    // public virtual void LoadData<T>(T data)
    // {
    //     throw new NotImplementedException();
    // }
    public EventTypes EventType { get; set; }
    public EventPriority Priority { get; set; }
    public bool ReturnAfterTrue { get; set; }

    public virtual List<Type> GetConstructorTypes()
    {
        return new List<Type>();
    }

    // public abstract object callEvent();
}