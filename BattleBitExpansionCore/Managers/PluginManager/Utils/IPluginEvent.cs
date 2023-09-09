namespace CyberTechBattleBit2.Managers.PluginManager.Utils;

public interface IPluginEvent
{
}

public abstract class IPlugin<T>
{
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