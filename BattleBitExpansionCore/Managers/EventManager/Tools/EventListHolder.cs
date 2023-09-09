using System.Drawing;
using ANSIConsole;

namespace CyberTechBattleBit2.Events;

public class EventListHolder<TEvent> where TEvent : EventBase
{
    private readonly Dictionary<EventPriority, List<EventBase>> Data = new();

    public readonly List<EventPriority> EventOrderList = new()
    {
        EventPriority.HIGHEST,
        EventPriority.HIGH,
        EventPriority.MEDIUM,
        EventPriority.LOW,
        EventPriority.LOWEST
    };

    public EventListHolder()
    {
    }

    public EventListHolder(List<Type> types)
    {
        Types = types;
    }

    public List<Type> Types { get; set; }

    public object callEvent(EventBase eventbeingcalled)
    {
        object r = null;
        var fullbreak = false;
        foreach (var kp in EventOrderList)
        {
            // Tools.ConsoleLog("ON PRIO222RITY".Color(ConsoleColor.Red).ToString() + kp);
            var cpri = kp;
            if (!Data.ContainsKey(cpri)) continue;
            if(BattleBitExtenderMain.DebugMode)Tools.ConsoleLog($"Checking priority for {eventbeingcalled.GetType().ToString().Color(ConsoleColor.Green)} ".Color(ConsoleColor.Red).ToString() + kp + $" | CUrrent Lenght is {Data[cpri].Count}");
            // Tools.ConsoleLog("ON PRIORITY PAS 1".Color(ConsoleColor.Red).Blink().ToString() + kp);
            try
            {
                var priorityEventList = Data[cpri];
                // Tools.ConsoleLog("ON PRIO25555555522RITY".Color(ConsoleColor.Red).Background(Color.Blue).ToString() + kp);
                foreach (var singleEventBase in priorityEventList)
                {
                    // Tools.ConsoleLog("ON 9999999999".Color(ConsoleColor.Red).Background(Color.Blue).ToString() + kp);

                    singleEventBase.LoadData(eventbeingcalled.Data);
                    // ei.
                    //SET DATA
                    if (BattleBitExtenderMain.DebugMode) Tools.ConsoleLog("ABOUT TO FIRE EVENT".Gradient(Color.LightBlue, new[] { Color.Green, Color.Yellow }));
                    var rr = singleEventBase.fireEvent();
                    if (rr != null) r = rr;
                    if (BattleBitExtenderMain.DebugMode)
                        if (r == null)
                            Tools.ConsoleLog("1st WATCHER SAID RETURN WAS NULL".Color(Color.Orange) + $"|||| {singleEventBase == null} {singleEventBase.GetType().Namespace}");
                        else Tools.ConsoleLog($"1st WATCHER SAID RETURN TYPE {r.GetType()} {r == null}!!!!!!!!!!!!!!===============================".Color(Color.Orange));
                    // Tools.ConsoleLog(r);
                    // Tools.ConsoleLog(r==null);
                    if ((r == (object)true || (r != (object)false && r != null)) && singleEventBase.ReturnAfterTrue)
                    {
                        Tools.ConsoleLog("Hey Just FYIII44444I".Color(Color.Purple));
                        fullbreak = true;
                        //Debug
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Tools.ConsoleLog("Exxxxxx");
                Tools.ConsoleLog(e);
            }

            if(BattleBitExtenderMain.DebugMode)Tools.ConsoleLog("ON End PRIORITY".Color(ConsoleColor.Red).Background(ConsoleColor.Black).ToString() + kp);
            if (fullbreak) break;
        }

        return r;
    }

    public void addEvent(EventBase eventBase)
    {
        var p = eventBase.Priority;
        if (!Data.ContainsKey(p))
            Data[p] = new List<EventBase>
            {
                eventBase
            };
        else Data[p].Add(eventBase);
        var z = Data[p];
    }


    // private Dictionary<string, int> getSamePriorityEvents(EventPriority ep)
    // {
    //     
    // }
}