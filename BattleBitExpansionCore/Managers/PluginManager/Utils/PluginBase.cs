using System.Drawing;
using System.Runtime.CompilerServices;
using ANSIConsole;
using BattleBitAPI.Server;
using CyberTechBattleBit2.DataSaver.Templates;
using CyberTechBattleBit2.Events;

namespace CyberTechBattleBit2.Managers.PluginManager.Utils;

public abstract class PluginBase : IPlugin<PluginBase>
{

    public CustomPlayerData.CustomPlayerData_PluginData? GetDatastore(string key)
    {
        return BattleBitExpansionCore.Managers.PluginManager.PluginManager.getInstance().getDataSaverObject(key);
    }

    public T GetDatastore<T>(string key) where T : CustomPlayerData.CustomPlayerData_PluginData
    {
        return (T)BattleBitExpansionCore.Managers.PluginManager.PluginManager.getInstance().getDataSaverObject(key);
    }

    public static PluginLogHelper Log;
    public PluginAttributes.PluginInfoAttribute PluginInfo;

    public PluginBase(PluginAttributes.PluginInfoAttribute info) : base(info.Name)
    {
        PluginInfo = info;
        setInstance(this);
        Log = new PluginLogHelper(this, info.Name);
    }

    public virtual object HandlePlayerCommand(CustomPlayer p, CustomGameServer server, string command, string[] args)
    {
        return null;
    }

    public virtual object HandleServerCommand()
    {
        return null;
    }

    // public PluginBase Instance;
    public abstract void onDisable();
    public abstract void onEnable();
    public abstract void onLoad();

    public virtual void OnCommand(CustomPlayer p, CustomGameServer server, string command, string[] args)
    {
        if (p == null || server == null)
        {
            HandleServerCommand();
            return;
        }

        HandlePlayerCommand(p, server, command, args);
    }
}

public class PluginLogHelper
{
    private string PluginName;
    private PluginBase P;

    public PluginLogHelper(PluginBase pluginBase, string pluginName = null)
    {
        P = pluginBase;

        if (pluginName == null)
            pluginName = pluginBase.GetType().Name;
        else
            PluginName = pluginName;
    }

    public void Info(object str)
    {
        var pt = $"[{P.PluginName}]".Color(Color.Orange).Background(ConsoleColor.White);
        Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.PluginManager + pt + Tools.TextTemplates.ThridLevelTags.LogInfoLevel + " > " + str);
    }
}
// public   PluginBase2 : PluginBase,IEvent
// {
//     
// }