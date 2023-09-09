using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;

namespace CyberTechBattleBit2.DataSaver.Templates;

public class CustomPlayerData : BaseDataSaverTemplate
{
    public int Killstreak { get; set; }
    public int XP { get; set; }
    public bool Banned { get; set; }
    public int ServerXP { get; set; }
    public int Level { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public string Username { get; set; }
    public PlayerPermissionDataHolder Permissions { get; set; }

    public string SteamID { get; set; }
    // public int SteamID { get; set; }

    [OnError]
    internal void OnError(StreamingContext context, ErrorContext errorContext)
    {
        errorContext.Handled = true;
    }

    public CustomPlayer LoadDataToPlayer(CustomPlayer p)
    {
        var player = new CustomPlayer();
        player.SetXp(XP);
        player.Kills = Kills;
        player.Deaths = Deaths;
        player.KillStreak = Killstreak;
        player.Permissions = Permissions;
        player.Banned = Banned;
        player.SetServerXp(ServerXP);
        Tools.ConsoleLog($"JUST LOADED SAVED PLAYER DATA for {Username} ? {SteamID}!!!");
        return player;
    }

    public void LoadDataFromPlayer(CustomPlayer player)
    {
        XP = player.RunningXP;
        ServerXP = player.ServerXP;
        Level = player.Level;
        Kills = player.Kills;
        Deaths = player.Deaths;
        Banned = player.Banned;
        SteamID = player.SteamID.ToString();
        Username = player.Name;
        Killstreak = player.KillStreak;
        Permissions = player.Permissions;

        Tools.ConsoleLog($"JUST SAVED SAVED PLAYER DATA for {Username} ? {SteamID}!!!");
    }

    public class CustomPlayerData_PluginData
    {
        public string PluginName { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
        public string DllName { get; set; }
        public string DLLHash { get; set; }
    }

    public class CustomPlayerData_PluginDataManager
    {
        public List<CustomPlayerData_PluginData> List = new();

        public void AddPluginData(CustomPlayerData_PluginData data)
        {
            List.Add(data);
        }
    }
}