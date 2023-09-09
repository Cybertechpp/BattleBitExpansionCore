using System.Drawing;
using ANSIConsole;
using BattleBitAPI.Server;
using CyberTechBattleBit2.Events.ExtenderEvents;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Events;

public class EventManager : LogTools.ModuleLogHelper<EventManager>
{
    private readonly Dictionary<EventTypes, EventListHolder<EventBase>> EventList = new();

    private readonly BattleBitExtenderMain P;

    public GenericDictionary EventListTypes = new();
    private CustomGameServer GS;

    public EventManager(BattleBitExtenderMain program)
    {
        // GS = gs;
        P = program;
        //BASE EVENTS BASEEVENTS
        EventListTypes.Add(new ServerConnectEvent());
        EventListTypes.Add(new ServerDisconnectEvent());
        EventListTypes.Add(new ServerCreatingPlayerInstance());
        EventListTypes.Add(new ServerCreatingServerInstance());
        EventListTypes.Add(new ServerLogEvent());
        EventListTypes.Add(new SererConnectingToAPI());
        EventListTypes.Add(new PlayerKillEvent());
        EventListTypes.Add(new PlayerDisconnectLeaveGameserver());
        EventListTypes.Add(new PlayerConnectJoinGameserver());
        EventListTypes.Add(new PlayerSpawnInGameserver());
        EventListTypes.Add(new PlayerFirstSpawnInGameserver());


        EventListTypes.Add(new ExtenderStartEvent());
        EventListTypes.Add(new ExtenderStopEvent());
        // EventListTypes.Add(new PlayerConnectJoinGameserver());
        // EventListTypes.Add(new PlayerConnectJoinGameserver());


        EventListTypes.Add(new TestServerConnect());
        EventListTypes.Add(new TestServerCreatePlayer());
        RegisterLocalEvents();
        RegisterListenersToGameServer();
    }

    public Dictionary<EventTypes, EventBase> EventListTypes2<T>() where T : EventBaseData
    {
        return new Dictionary<EventTypes, EventBase>();
    }

    private void RegisterListenersToGameServer()
    {
        P.L.OnGameServerConnected += async server =>
        {
            if(BattleBitExtenderMain.DebugMode)Tools.ConsoleLog("EVENT OnGameServerConnected".Color(ConsoleColor.Red));
            var ev = new ServerConnectEvent();
            ev.LoadData(new ServerConnectEventData((CustomGameServer)server));
            CallEvent(ev);
            // callEvent<>(ev);
        };

        P.L.OnCreatingPlayerInstance += steamID =>
        {
            if(BattleBitExtenderMain.DebugMode) Tools.ConsoleLog("EVENT OnCreatingPlayerInstance".Color(ConsoleColor.Red));
            var ev = new ServerCreatingPlayerInstance();
            ev.LoadData(new ServerCreatingPlayerInstanceData(null, steamID));
            return (CustomPlayer)CallEvent(ev);
            // callEvent<>(ev);
        };

        P.L.OnGameServerConnecting += async address =>
        {
            try
            {
                if(BattleBitExtenderMain.DebugMode)  Tools.ConsoleLog("EVENT OnGameServerConnecting".Color(ConsoleColor.DarkCyan));
                var ev = new SererConnectingToAPI();
                ev.LoadData(new SererConnectingToAPI_Data(null, address));
                var r = CallEvent(ev);
                if(BattleBitExtenderMain.DebugMode)   Tools.ConsoleLog("EVENT OnGameServerConnecting".Color(ConsoleColor.Red) + "|||| " +
                                                                       $"{r} AND {r == null}".Color(ConsoleColor.Green));
                if (r == null) r = false;
                return (bool)r;
            }
            catch (Exception e)
            {
                Tools.ConsoleLog("Heyyyeyeye !!!!");
                Tools.ConsoleLog(e);
            }

            return true;
        };

        P.L.OnGameServerDisconnected += async server =>
        {
            if(BattleBitExtenderMain.DebugMode)  Tools.ConsoleLog("EVENT OnGameServerDisconnected".Color(ConsoleColor.DarkCyan));
            var ev = new ServerDisconnectEvent();
            ev.LoadData(new ServerDisconnectEvent_Data((CustomGameServer)server));
            CallEvent(ev);
        };

        P.L.OnCreatingGameServerInstance += (address, port) =>
        {
            if(BattleBitExtenderMain.DebugMode)    Tools.ConsoleLog("EVENT OnCreatingGameServerInstance".Color(ConsoleColor.DarkCyan)
                .Background(Color.Purple));
            var ev = new ServerCreatingServerInstance();
            ev.LoadData(new ServerCreatingServerInstance_Data(null, address, port));
            var aa = (CustomGameServer)CallEvent(ev);
            if(BattleBitExtenderMain.DebugMode) Tools.ConsoleLog($"I GOTTZZZZZ {aa?.GetType()} {aa == null}".Background(ConsoleColor.Red));
            return aa;
        };

        // P.L.OnLog += async (level, s, arg3) =>
        // {
        //     Tools.ConsoleLog("EVENT OnLog".Color(ConsoleColor.Red));
        //     var ev = new ServerLogEvent();
        //     ev.LoadData(new ServerLogEvent_Data(null, s, level, arg3));
        //     CallEvent(ev);
        // };
    }


    //LOAD BASE EVENTS BASEEVENTS
    private void RegisterLocalEvents()
    {
        // Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + " PreLoaded 111111" + EventListTypes.getDict().Count);
        foreach (var ee in EventListTypes.getDict())
        {
            var k = ee;
            // Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.EventManager +$" initializing base event {ee.ToString()}");
            foreach (var kk in k.Value) _GetOrAdd(k.Key, kk);

            if(BattleBitExtenderMain.DebugMode)Log.Success($" Loaded base event {ee.ToString()}");
        }
    }

    private void RegisterEvents(EventBase eb)
    {
        // Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.EventManager +" ABOUT TO LOAD 1 EVENT!!!!!".Background(ConsoleColor.Yellow).Color(ConsoleColor.Black).ToString() + EventListTypes.getDict().Count);

        var ee = eb.EventType;
        Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.EventManager + $" initializing base event {ee.ToString()}");
        _GetOrAdd(eb.EventType, eb);

        Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.EventManager + $" Loaded base event {ee.ToString()}");
    }

    public void RegisterPluginEvents(EventBase eb, PluginAttributes.PluginInfoAttribute pluginInfoAttribute = null)
    {
        var pluginname = "Unknown";
        if (pluginInfoAttribute != null) pluginname = pluginInfoAttribute.Name;

        // Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.EventManager +" ABOUT TO LOAD 1 EVENT!!!!!".Background(ConsoleColor.Yellow).Color(ConsoleColor.Black).ToString() + EventListTypes.getDict().Count);

        var ee = eb.EventType;
        // Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.EventManager +$" initializing base event {ee.ToString()}");
        _GetOrAdd(eb.EventType, eb);

        Log.Success($" Loaded plugin event {ee.ToString()} from {pluginname}");
    }


    private EventListHolder<EventBase> _GetOrAdd(EventTypes type, EventBase evnt = null)
    {
        // Tools.ConsoleLog("Just an FYI ");
        // Tools.ConsoleLog($"Just an FYI {type} AND {evnt}");
        var ns = evnt.GetType().Namespace;
        if (evnt == null)
        {
            Tools.ConsoleLog("SDadasd adasdad asd - CHcek me I think.... ".Color(ConsoleColor.Green));
            return null;
        }

        if (!EventList.ContainsKey(type))
            // evnt = (EventBase<EventBaseData>)evnt;
            // var tt = EventListTypes[type];
            EventList[type] = new EventListHolder<EventBase>();

        if (evnt != null) EventList[type].addEvent(evnt);

        return EventList[type];
    }

    private string MyDictionaryToJson(Dictionary<int, List<int>> dict)
    {
        var entries = dict.Select(d =>
            string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));
        return "{" + string.Join(",", entries) + "}";
    }

    public object CallEvent(EventBase evnt)
    {
        var aa = evnt?.EventType.ToString().Color(ConsoleColor.Yellow);
        Log.Info($"Event {aa} Has been called".ToString());
        // Tools.ConsoleLog(JsonConvert.SerializeObject( evnt.Data));

        try
        {
            var et = evnt.EventType;
            if (EventList.TryGetValue(et, out var a))
            {
                var v = a.callEvent(evnt);
                // if (v == null)
                // {
                //     Tools.ConsoleLog("POCKET WATCHING AFTER CALL");
                //     Tools.ConsoleLog(v?.GetType());
                // }

                return v;
            }
            else
            {
                Tools.DebugLog($"THIS TTT IS {et} || ".Color(ConsoleColor.Green) + string.Join(" | ", EventList.Keys));
                Tools.ConsoleLog("Ummm It looks like this Key was not In the Event Dictionary!");
            }

            Tools.ConsoleLog($"No Event found!?!?!??!  IS {a} {a == null}");
        }
        catch (Exception e)
        {
            Console.WriteLine("BROOOOOOOOOOOOOOOOO");
            Console.WriteLine("BROOOOOOOOOOOOOOOOO");
            Console.WriteLine(e);
            throw;
        }

        return null;
    }


    public void addEvent(EventTypes et, EventBase evnt)
    {
        _GetOrAdd(et, evnt);
    }
}

// internal class TTEvent : EventBase
// {
// }

public enum EventTypes
{
    NONE = 0,

    // NONE = 0,
    // NONE = 0,
    // NONE = 0,
    // NONE = 0,
    // NONE = 0,
    // NONE = 0,
    // NONE = 0,
    ServerConnectEvent,
    ServerDisconnectEvent,
    ServerCreatingPlayer,
    ServerConnectingToAPI,
    ServerCreatingServer,
    ServerLogEvent,
    PlayerJoinGameserver,
    PlayerLeaveGameserver,
    PlayerKillEvent,
    PlayerSpawnGameserver,
    PlayerFirstSpawnInGameserver
}

public enum EventPriority
{
    HIGHEST,
    HIGH,
    MEDIUM,
    LOW,
    LOWEST
}

// public delegate TResult EventBase<in T1, out TResult>(T1 arg1);