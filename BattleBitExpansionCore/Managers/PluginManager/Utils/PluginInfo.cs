using System.Reflection;
using CyberTechBattleBit2;
using CyberTechBattleBit2.DataSaver.Templates;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace BattleBitExpansionCore.Managers.PluginManager;

public class PluginInfo : BasePluginInfo
{
    public string Name;
    public string Directory;
    public Guid Guid;
    public Dictionary<string, Command> CommandList;
    public Type MainClassType;
    public PluginAttributes.PluginInfoAttribute InfoAttribute;
    public Type[] FoundTypes;
    public bool Loaded = false;
    public Dictionary<string, PluginAttributes.PluginDataSaverAttribute> DatasaverAttributes = new();
    // public Dictionary<string,CustomPlayerData.CustomPlayerData_PluginData> DatasaverObject = new Dictionary<string, CustomPlayerData.CustomPlayerData_PluginData>();

    public PluginBase MainClass
    {
        get => Loaded ? _mainClass : null;
        set
        {
            if (value == null) return;
            _mainClass = value;
            Loaded = true;
        }
    }


    public List<Type> EventTypes = new();
}

public class BasePluginInfo
{
    public List<Type> FoundCommands = new();
    public List<Type> FoundEvents = new();
    internal PluginBase _mainClass;

    internal Assembly Assembly;


    // static public Boolean operator ==(PluginInfo obj1, PluginInfo obj2)
    // {
    //     // If both are null, or both are same instance, return true.
    //     if (System.Object.ReferenceEquals(obj1, obj2))
    //     {
    //         return true;
    //     }
    //
    //     // If one is null, but not both, return false.
    //     if (((object)obj1 == null) || ((object)obj2 == null))
    //     {
    //         return false;
    //     }
    //
    //     return obj1.Guid == obj2.Guid;
    // }
    //
    // static public Boolean operator !=(PluginInfo obj1, PluginInfo obj2)
    // {
    //     return !(obj1 == obj2);
    // }
    //
    // public override bool Equals(object obj)
    // {
    //     if (obj is PluginInfo)
    //     {
    //         return this == (PluginInfo)obj;
    //     }
    //
    //     return false;
    // }
    //
    //
    // public override int GetHashCode()
    // {
    //     byte[] by = Guid.ToByteArray();
    //     int value = 0;
    //     for (int i = 0; i < by.GetLength(0); i++)
    //     {
    //         value += (int)(by[i] & 0xffL) << (8 * i);
    //     }
    //
    //     return value;
    // }
}