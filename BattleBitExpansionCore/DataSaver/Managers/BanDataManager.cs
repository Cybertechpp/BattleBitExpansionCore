using CyberTechBattleBit2.DataSaver.Templates;
using Newtonsoft.Json;

namespace CyberTechBattleBit2.DataSaver;

public class BanDataManager : BaseDataSaverClass
{
    public BanDataManager() : base("../../", "BannedPlayers.json", typeof(ServerBanSettingsData))
    {
        Load();
    }

    // public PlayerPermissionDataHolder getPlayerPermission(CustomPlayer player)
    // {
    //     // if()
    // }

    public static ServerBanSettingsData BanList = new();

    public void AddBan(CustomPlayer player, string bannedBy, string reason, TimeSpan duration)
    {
        BanList.AddBanEntry(player, duration, reason, bannedBy);
        Save(BanList);
    }

    public BanEntry? GetServerBans(CustomPlayer player, bool checkvalid = true)
    {
        return GetServerBans(player.SteamID, checkvalid);
    }

    public BanEntry? GetServerBans(ulong steamid, bool checkvalid = true)
    {
        return BanList.TryGetBan(steamid, checkvalid);
    }

    public void onClose()
    {
        Save(BanList);
    }

    public void Save(ServerBanSettingsData data)
    {
        // var json = JsonConvert.SerializeObject(data);
        SaveToFile(data, ParsedSaveLocation);
    }

    public override object CreateBlankObject()
    {
        var a = new ServerBanSettingsData();
        return a;
    }

    public void Load()
    {
        var a = ReadFromFile();
        BanList = (ServerBanSettingsData)a;
    }
}