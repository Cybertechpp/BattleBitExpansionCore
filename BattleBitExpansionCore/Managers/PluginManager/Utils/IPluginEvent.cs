namespace CyberTechBattleBit2.Managers.PluginManager.Utils;

public interface IPluginEvent
{
}

public abstract class IPlugin<T>
{

    public string PluginName = "NotSet";
    public IPlugin(string pluginName)
    {
        PluginName = pluginName;
    }
    // public IPlugin(T t)
    // {
    //     Instance = t;
    // }
    public static T Instance;

    public static T getInstance()
    {
        return Instance;
    }

    public static void setInstance(T i)
    {
        Instance = i;
    }
}