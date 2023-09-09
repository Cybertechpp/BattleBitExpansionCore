using CyberTechBattleBit2.Events;

namespace CyberTechBattleBit2.Managers.PluginManager.Utils;

public class PluginMethodEventWrapper : EventBase
{
    public Func<object, Task<object>> fireEventFunc { get; set; }

    public static PluginMethodEventWrapper createInstance(EventTypes et)
    {
        return new PluginMethodEventWrapper(et);
    }

    public PluginMethodEventWrapper(EventTypes eventType, bool returnAfterTrue = false, EventPriority priority = EventPriority.MEDIUM) : base(eventType, returnAfterTrue, priority)
    {
    }


    public override object fireEvent()
    {
        // Tools.ConsoleLog("EVEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE FIREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
        try
        {
            return fireEventFunc(Data).Result;
        }
        catch (Exception e)
        {
            Tools.ConsoleLog("EEEE11EEEEEEEEEEEEE");
            Tools.ConsoleLog(e);
        }

        return base.fireEvent();
    }
}