namespace CyberTechBattleBit2.DataSaver.Templates;

public class ServerBanSettingsData : BaseDataSaverTemplate
{
    public Dictionary<ulong, BanEntry> BanList = new();

    public void AddBanEntry(CustomPlayer player, TimeSpan duration, string reason, string bannedBy)
    {
        BanEntry a;
        var aa = new BanEntry();
        if (BanList.TryGetValue(player.SteamID, out a))
        {
            aa.PreviousBans.Add(a);
            // a.UsernameAtBan = player.Name;
            aa.BannedBy = bannedBy;
            aa.UsernameAtBan = player.Name;
            aa.BanReason = reason;
            aa.Duration = duration;
            aa.BannedOn = DateTime.Now;
            aa.BanReleased = DateTime.Now.Add(aa.Duration);
            aa.SteamID = player.SteamID;
            aa.BannedBy = bannedBy;
            //Unneeded probably
            if (aa.UsernameAtBan.ToLower() != player.Name.ToLower() && !aa.AliasUsernames.Contains(player.Name.ToLower())) aa.AliasUsernames.Add(player.Name.ToLower());
        }
        else
        {
            aa.BannedBy = bannedBy;
            aa.UsernameAtBan = player.Name;
            aa.BanReason = reason;
            aa.Duration = duration;
            aa.BannedOn = DateTime.Now;
            aa.BanReleased = DateTime.Now.Add(aa.Duration);
            aa.SteamID = player.SteamID;
            aa.BannedBy = bannedBy;
        }

        BanList[player.SteamID] = aa;
    }

    public BanEntry? TryGetBan(CustomPlayer player, bool checkvalid = true)
    {
        return TryGetBan(player.SteamID, checkvalid);
    }

    public BanEntry? TryGetBan(ulong steamid, bool checkvalid = true)
    {
        BanEntry a;
        if (BanList.TryGetValue(steamid, out a))
        {
            // Tools.ConsoleLog($"GOOOTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT {a.IsValid()}");
            if (a.IsValid() && checkvalid) return a;
            else if (!checkvalid) return a;
        }

        // Tools.ConsoleLog($"FFFFFFFFFFFFFFFFFFFFFFFFF");

        return null;
    }
}

public class BanEntry
{
    public ulong SteamID { get; set; }
    public string UsernameAtBan { get; set; }
    public string BannedBy { get; set; }
    public List<string> AliasUsernames { get; set; }
    public DateTime BannedOn { get; set; }
    public string BanReason { get; set; }
    public DateTime BanReleased { get; set; }
    public TimeSpan Duration { get; set; }
    public List<BanEntry> PreviousBans { get; set; } = new();

    public bool IsValid()
    {
        Tools.ConsoleLog($"CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC {DateTime.Compare(DateTime.Now, BanReleased)}");
        return DateTime.Compare(DateTime.Now, BanReleased) == -1;
    }
}