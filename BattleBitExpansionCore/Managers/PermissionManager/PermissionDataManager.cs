using System.Reflection;
using System.Text.Json.Serialization;
using CyberTechBattleBit2.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CyberTechBattleBit2.DataSaver;

public class PermissionDataManager : BaseDataSaverClass
{
    public PermissionDataManager() : base("../../", "Permissions.json", typeof(ServerGroupPermissionList_DataHolder))
    {
    }

    // public PlayerPermissionDataHolder getPlayerPermission(CustomPlayer player)
    // {
    //     // if()
    // }

    public static ServerGroupPermissionList_DataHolder PermissionGroups = new();

    public void SavePermissionData(CustomPlayer player)
    {
        SavePermissionData(player.Permissions);
    }

    public void SavePermissionData(PlayerPermissionDataHolder data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        SaveToFile(json, ParsedSaveLocation);
    }

    public override object CreateBlankObject()
    {
        var a = new ServerGroupPermissionList_DataHolder();
        var aa = new ServerGroupPermission_DataHolder();
        aa.Name = "Default Group";
        aa.Default = true;
        aa.Permissions.Add("BattleBitExtensionCore.Default");
        aa.PermissionLevelLevel = ServerBasicPermissionLevel.Public;
        a.Data.Add(aa);
        return a;
    }

    public void Load()
    {
        var a = ReadFromFile();
        PermissionGroups = (ServerGroupPermissionList_DataHolder)a;
        foreach (var d in PermissionGroups.Data) Tools.ConsoleLog($"LOADED GROUP >>> {d.Name}");
    }
}

public enum ServerBasicPermissionLevel
{
    None,
    Public,
    Community,
    Restricted,
    Private,
    Mod,
    Admin,
    SuperAdmin
}

public class PlayerPermissionDataHolder : BaseDataSaverTemplate
{
    public PlayerPermissionDataHolder()
    {
        PermissionLevelLevel = ServerBasicPermissionLevel.Public;
    }

    public string Name { get; set; }
    public ulong SteamID { get; set; }
    public List<string> Permissions { get; set; } = new();
    public List<string> Groups { get; set; } = new();
    // {}

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]

    public ServerBasicPermissionLevel PermissionLevelLevel { get; set; }
}

public class ServerGroupPermission_DataHolder : BaseDataSaverTemplate
{
    public string Name { get; set; }
    public int Priority { get; set; }
    public bool Default { get; set; } = false;

    public List<ulong> SteamID { get; set; }

    // public List<string> User { get; set; }
    public List<string> Permissions { get; set; } = new();
    public List<string> InheritedGroups { get; set; } = new();

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public ServerBasicPermissionLevel PermissionLevelLevel = ServerBasicPermissionLevel.None;
}

public class ServerGroupPermissionList_DataHolder : BaseDataSaverTemplate
{
    public List<ServerGroupPermission_DataHolder> Data { get; set; } = new();
}