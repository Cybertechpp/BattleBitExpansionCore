using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using BattleBitAPI;
using BattleBitAPI.Common;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Events;
using CyberTechBattleBit2.Managers;

namespace CyberTechBattleBit2;

public class CustomPlayer : Player<CustomPlayer>
{
    public static void setIns()
    {
    }

    public CustomPlayer()
    {
    }

    public override async Task OnConnected()
    {
        if (Banned)
        {
            var a = BattleBitExtenderMain.Instance.DBM.GetServerBans(SteamID.ToString().Length == 0 ? TempSteamID : SteamID);
            if (a == null || !a.IsValid())
            {
                Banned = false;
                return;
            }

            var diff = (a.BannedOn - a.BanReleased).Duration();
            // Tools.ConsoleLog($"HHaysda hdadh ashd ashdashdhas dhasdh asd{a == null}");
            // Tools.ConsoleLog($"HHaysda hdadh ashd ashdashdhas dhasdh asd{a?.IsValid()}");

            Kick($"You are currently banned for: {diff.Days}Days {diff.Hours}Hrs {diff.Minutes}Mins\n Come Back After Your Ban To Play Again! \n Banned By: {a.BannedBy} \n Reason: {a.BanReason}");
        }

        base.OnConnected();
        return;
    }


    internal ulong TempSteamID { get; set; }

    public string Prefix;
    public string DisplayName;
    public int RunningXP { get; private set; }
    public int ServerXP { get; private set; }
    public int Level { get; private set; }

    public bool Banned { get; set; } = false;

    public SessionStatsClass SessionStats = new();

    public class SessionStatsClass
    {
        public int Spawns { get; set; } = 0;
        public int Kills { get; set; } = 0;
        public int Deaths { get; set; } = 0;

        public bool FirstSpawn => Spawns > 0 && Deaths == 0;
    }

    public int ServerJoins { get; set; } = 0;

    public PlayerPermissionDataHolder? Permissions = null;


    [Newtonsoft.Json.JsonIgnore] public int KillStreak { get; set; } = 0;
    public int Kills { get; set; }
    public int Deaths { get; set; }

    public void SetXp(int xp, bool recalculate = true)
    {
        SetRunningXp(xp, recalculate);
    }

    public void SetServerXp(int xp)
    {
        ServerXP = xp;
    }

    public int GetServerXP()
    {
        return ServerXP;
    }

    public override Task OnSpawned()
    {
        var e = new PlayerSpawnInGameserver();
        e.LoadData(new PlayerSpawnInGameserver_Data((CustomGameServer)GameServer, this));
        BattleBitExtenderMain.Instance.EM.CallEvent(e);

        return base.OnSpawned();
    }

    public void AddServerXP(int amt)
    {
        ServerXP += amt;
    }

    public void BanPlayer(string reason, string bannedBy, TimeSpan duration)
    {
        BattleBitExtenderMain.Instance.DBM.AddBan(this, bannedBy, reason, duration);
        Kick($"You have been banned from the server for {duration.Days}Days {duration.Hours}Hrs {duration.Minutes}Mins \n" +
             $"Reason: {reason} | BannedBy: {bannedBy}");
    }

    public int GetLevel()
    {
        return Level;
    }

    public void AddXP(int xp)
    {
        SetRunningXp(RunningXP + xp);
    }

    public void TakeXP(int xp)
    {
        SetRunningXp(RunningXP * xp);
    }

    // public void setSteamID(ulong s)
    // {
    //     mInternal.SteamID = s;
    // }

    public void OnPlayerKill(CustomPlayer args)
    {
        Kills++;
        KillStreak++;
        SessionStats.Kills++;
    }

    public void OnPlayerDeath(CustomPlayer args)
    {
        Deaths++;
        KillStreak = 0;
        SessionStats.Deaths++;
    }

    /// <summary>
    ///     Set Running XP for player
    /// </summary>
    /// <param name="xp">Int Value of XP</param>
    /// <param name="recalculate">Bool to Recalculate XP and Level vars</param>
    private void SetRunningXp(int xp, bool recalculate = true)
    {
        RunningXP = xp;
        if (recalculate) ReCalculateXPandLevel();
    }

    public void ReCalculateXPandLevel()
    {
        var cl = 0;
        var cxp = 0;
        var rxp = RunningXP;
        while (rxp > 0)
        {
            var nxtlvlxp = getMaxXPForLevel(cl + 1);
            if (nxtlvlxp > rxp)
            {
                cxp = rxp;
                rxp = 0;
            }
            else
            {
                cl++;
                rxp -= nxtlvlxp;
            }
        }

        Level = cl;
        ServerXP = cxp;
    }

    /// <summary>
    ///     Start with Level 1 or Value 1
    /// </summary>
    /// <param name="level">Starts at 1</param>
    /// <returns></returns>
    public int getMaxXPForLevel(int level)
    {
        if (level == 0)
            //TODO Throw Error
            //EXCEPTION
            return -1;
        var x = level;
        return x * 25 * (x + 1) - x * 20 / 5 * 9;
        //https://i.imgur.com/Zd7kVch.png
        //https://www.desmos.com/calculator/xpjvopn33c
    }


    public struct OnPlayerDeathArguments<TPlayer> where TPlayer : Player<TPlayer>
    {
        public TPlayer Killer;
        public Vector3 KillerPosition;

        public TPlayer Victim;
        public Vector3 VictimPosition;

        public string KillerTool;
        public PlayerBody BodyPart;
        public ReasonOfDamage SourceOfDamage;
    }

    public struct OnPlayerKillPlayerArguments<TPlayer> where TPlayer : Player<TPlayer>
    {
        public TPlayer Killer;
        public Vector3 KillerPosition;

        public TPlayer Victim;
        public Vector3 VictimPosition;

        public string KillerTool;
        public PlayerBody BodyPart;
        public ReasonOfDamage SourceOfDamage;
    }
}