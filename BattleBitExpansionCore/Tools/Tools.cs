using System.Drawing;
using ANSIConsole;
using BattleBitAPI.Common;
using CyberTechBattleBit2.Events;
using Newtonsoft.Json;

namespace CyberTechBattleBit2;

public static class Tools
{
    public static T CloneJson<T>(this T source)
    {
        // Don't serialize a null object, simply return the default for that object
        if (ReferenceEquals(source, null)) return default;

        // initialize inner objects individually
        // for example in default constructor some list property initialized with some values,
        // but in 'source' these items are cleaned -
        // without ObjectCreationHandling.Replace default constructor values will be added to result
        var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

        //JsonSerializer.Serialize JsonSerializer.Deserializ
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
    }

    
    public static void ConsoleLog(object str)
    {
        Console.Out.WriteLine(str);
    }

    public static void ConsoleLog(string str)
    {
        Console.Out.WriteLine(str);
    }

    public static void DebugLog(string str)
    {
        Console.Out.WriteLine(str);
    }

    public static void ConsoleLog(ANSIString str)
    {
        Console.Out.WriteLine(str);
    }

    public static MapSize Increase(this MapSize m)
    {
        if (m == MapSize.None) return m;
        if (m == MapSize._8v8) return MapSize._16vs16;
        if (m == MapSize._16vs16) return MapSize._64vs64;
        if (m == MapSize._64vs64) return MapSize._127vs127;
        if (m == MapSize._127vs127) return m;
        return m;
    }

    public static MapSize GetCorrectMapSize( int count,int openslots = 8)
    {
        var cc = count + openslots;
        if (cc < 16) return MapSize._8v8;
        if (cc < 32) return MapSize._16vs16;
        if (cc < 128) return MapSize._64vs64;
        return MapSize._127vs127;
    }
    public static MapSize Decrease(this MapSize m)
    {
        if (m == MapSize.None) return m;
        if (m == MapSize._8v8) return m;
        if (m == MapSize._16vs16) return MapSize._8v8;
        if (m == MapSize._64vs64) return MapSize._16vs16;
        if (m == MapSize._127vs127)return MapSize._64vs64;
        return m;
    }
    
    public static string BlackBackgroud(this string ss, ConsoleColor c = ConsoleColor.White)
    {
        return ss.Background(ConsoleColor.Black).Color(c).ToString();
    }

    public static class TextTemplates
    {
        public static class FirstLevelTags
        {
            public static ANSIString BBECTag = "[BBEC]".Background(Color.Purple).Color(ConsoleColor.White);
            public static string GrayArrow = " > ".Color(ConsoleColor.Gray).ToString();

            public static ANSIString BattleBitExpansionCore =
                "BattleBitExpansionCore".Background(Color.Purple).Color(ConsoleColor.White);
        }

        public static class SecondLevelTags
        {
            public static string EventManager = "[".Background(Color.Purple).ToString() + "EventManager".Color(Color.Gold).Background(Color.Purple) + "]".Background(Color.Purple).Color(ConsoleColor.White);
            public static string PluginManager = "[".Background(Color.Purple).ToString() + "PluginManager".Color(Color.Cyan).Background(Color.Purple) + "]".Background(Color.Purple).Color(ConsoleColor.White);
        }

        public static class ThridLevelTags
        {
            public static string LogInfoLevel = "[INFO]".Color(Color.CornflowerBlue).Background(ConsoleColor.DarkBlue).ToString();
            public static string WarnInfoLevel = "[WARN]".Color(Color.White).Background(Color.Orange).ToString();

            public static string SuccessLogLevel = "[SUCCESS]".Color(Color.White).Background(Color.Green).ToString();
        }
    }
}

public class GenericDictionary
{
    private readonly Dictionary<EventTypes, List<EventBase>> _dict = new();

    public Dictionary<EventTypes, List<EventBase>> getDict()
    {
        return _dict;
    }

    //ADD EventBase
    public void Add(EventBase value)
    {
        Add(value.EventType, value);
    }

    public void Add(EventTypes key, EventBase value)
    {
        if (!_dict.ContainsKey(key)) _dict[key] = new List<EventBase>();
        var a = _dict[key];
        a.Add(value);
        // _dict.Add(key, value);
    }

    public List<EventBase> GetValue(EventTypes key)
    {
        return _dict[key];
    }
}

public static class StaticStuff
{
}