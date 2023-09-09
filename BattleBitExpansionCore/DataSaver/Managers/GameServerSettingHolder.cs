using System.Net;
using BattleBitAPI.Common;
using CyberTechBattleBit2;
using Newtonsoft.Json;
using Spectre.Console.Rendering;

namespace BattleBitExpansionCore.DataSaver.Managers;

public class GameServerSettingHolder
{
    [JsonIgnore] public CustomGameServer GS { get; set; }


    public Dictionary<string,bool> EnabledPlugins { get; set; } = new ();
    public ExtenderSettingsHolder ExtenderSettings { get; set; } = new ExtenderSettingsHolder();
    public int RoundCountDown { get; set; } = 15;

    public class ExtenderSettingsHolder
    {
        public string? LoadingText { get; set; }
        public string? ServerRulesText { get; set; }
    }


    public class ServerSettingsHolder
    {
        public string Name { get; set; }
        public float DamageMultiplier { get; set; } = 1;
        public bool FriendlyFireEnabled { get; set; }
        public bool OnlyWinnerTeamCanVote { get; set; }
        public bool PlayerCollision { get; set; }
        public bool HideMapVotes { get; set; }
        public bool CanVoteDay { get; set; }= true;
        public bool CanVoteNight { get; set; } = false;
        public byte MedicLimitPerSquad { get; set; }
        public byte EngineerLimitPerSquad { get; set; }
        public byte SupportLimitPerSquad { get; set; }
        public byte ReconLimitPerSquad { get; set; }
        public bool TeamlessMode { get; set; }
        public bool UnlockAllAttachments { get; set; }
        public bool SquadRequiredToChangeRole { get; set; }
        public float TankSpawnDelayMultipler { get; set; }
        public float TransportSpawnDelayMultipler { get; set; }
        public float SeaVehicleSpawnDelayMultipler { get; set; }
        public float APCSpawnDelayMultipler { get; set; }
        public float HelicopterSpawnDelayMultipler { get; set; }
        public ulong? Hash { get; set; } = null;
        public string IP { get; set; }
        public int Port { get; set; }
        public string T { get; set; }
        public string RulesText { get; set; }
    }

    public class MapRotationSettingsHolder
    {
        public List<String> MapRotation { get; set; }
    }

    public class GameModeSettingsHolder
    {
        public List<String> GameModeRotation { get; set; }
    }

    public class RoundSettingsHolder
    {
        public double TeamATickets { get; set; }
        public double TeamBTickets { get; set; }
        public double MaxTickets { get; set; }
        public int PlayersToStart { get; set; } = 2;
        public int SecondsLeft { get; set; }
    }

    public ServerSettingsHolder ServerSettings = new ServerSettingsHolder();
    public MapRotationSettingsHolder MapRotationSettings = new MapRotationSettingsHolder();
    public GameModeSettingsHolder GameModeSettings = new GameModeSettingsHolder();
    public RoundSettingsHolder RoundSettings = new RoundSettingsHolder();

    public GameModeMapData ExtenderGamemodeMapData = new GameModeMapData();

    public void LoadGSValues(CustomGameServer gs)
    {
        GS = gs;
        try
        {
            ExtenderSettings.LoadingText = gs.LoadingScreenText ?? "";
        }
        catch (Exception e)
        {
            
            ExtenderSettings.LoadingText = "";
        }
        try
        {
            ExtenderSettings.ServerRulesText = gs.ServerRulesText ?? "";
        }
        catch (Exception e)
        {
            
            ExtenderSettings.ServerRulesText = "";
        }
        RoundSettings.TeamATickets = gs.RoundSettings.TeamATickets;
        RoundSettings.TeamBTickets = gs.RoundSettings.TeamBTickets;
        RoundSettings.MaxTickets = gs.RoundSettings.MaxTickets;
        RoundSettings.PlayersToStart = gs.RoundSettings.PlayersToStart;
        RoundSettings.SecondsLeft = gs.RoundSettings.SecondsLeft;
        GameModeSettings.GameModeRotation = gs.GamemodeRotation.GetGamemodeRotation().ToList();
        MapRotationSettings.MapRotation = gs.MapRotation.GetMapRotation().ToList();
        ServerSettings.IP = gs.GameIP.ToString();
        ServerSettings.Port = gs.GamePort;
        ServerSettings.Hash = gs.ServerHash;
        ServerSettings.Name = gs.ServerName;
        ServerSettings.RulesText = gs.ServerRulesText;
        ServerSettings.DamageMultiplier = gs.ServerSettings.DamageMultiplier;
        ServerSettings.FriendlyFireEnabled = gs.ServerSettings.FriendlyFireEnabled;
        ServerSettings.OnlyWinnerTeamCanVote = gs.ServerSettings.OnlyWinnerTeamCanVote;
        ServerSettings.PlayerCollision = gs.ServerSettings.PlayerCollision;
        ServerSettings.HideMapVotes = gs.ServerSettings.HideMapVotes;
        ServerSettings.CanVoteDay = gs.ServerSettings.CanVoteDay;
        ServerSettings.CanVoteNight = gs.ServerSettings.CanVoteNight;
        ServerSettings.MedicLimitPerSquad = gs.ServerSettings.MedicLimitPerSquad;
        ServerSettings.EngineerLimitPerSquad = gs.ServerSettings.EngineerLimitPerSquad;
        ServerSettings.SupportLimitPerSquad = gs.ServerSettings.SupportLimitPerSquad;
        ServerSettings.ReconLimitPerSquad = gs.ServerSettings.ReconLimitPerSquad;
        ServerSettings.ReconLimitPerSquad = gs.ServerSettings.ReconLimitPerSquad;
        ServerSettings.UnlockAllAttachments = gs.ServerSettings.UnlockAllAttachments;
        ServerSettings.SquadRequiredToChangeRole = gs.ServerSettings.SquadRequiredToChangeRole;
        ServerSettings.TankSpawnDelayMultipler = gs.ServerSettings.TankSpawnDelayMultipler;
        ServerSettings.TransportSpawnDelayMultipler = gs.ServerSettings.TransportSpawnDelayMultipler;
        ServerSettings.SeaVehicleSpawnDelayMultipler = gs.ServerSettings.SeaVehicleSpawnDelayMultipler;
        ServerSettings.APCSpawnDelayMultipler = gs.ServerSettings.APCSpawnDelayMultipler;
        ServerSettings.HelicopterSpawnDelayMultipler = gs.ServerSettings.HelicopterSpawnDelayMultipler;
    }

    public void SetGSValues(CustomGameServer gs)
    {
        gs.LoadingScreenText = ExtenderSettings.LoadingText;
        gs.ServerRulesText = ExtenderSettings.ServerRulesText;
        gs.ServerRulesText = ServerSettings.RulesText;
        gs.ServerSettings.DamageMultiplier = ServerSettings.DamageMultiplier;
        gs.ServerSettings.FriendlyFireEnabled = ServerSettings.FriendlyFireEnabled;
        gs.ServerSettings.OnlyWinnerTeamCanVote = ServerSettings.OnlyWinnerTeamCanVote;
        gs.ServerSettings.PlayerCollision = ServerSettings.PlayerCollision;
        gs.ServerSettings.HideMapVotes = ServerSettings.HideMapVotes;
        gs.ServerSettings.CanVoteDay = ServerSettings.CanVoteDay;
        gs.ServerSettings.CanVoteNight = ServerSettings.CanVoteNight;
        gs.ServerSettings.MedicLimitPerSquad = ServerSettings.MedicLimitPerSquad;
        gs.ServerSettings.EngineerLimitPerSquad = ServerSettings.EngineerLimitPerSquad;
        gs.ServerSettings.SupportLimitPerSquad = ServerSettings.SupportLimitPerSquad;
        gs.ServerSettings.ReconLimitPerSquad = ServerSettings.ReconLimitPerSquad;
        gs.ServerSettings.ReconLimitPerSquad = ServerSettings.ReconLimitPerSquad;
        gs.ServerSettings.UnlockAllAttachments = ServerSettings.UnlockAllAttachments;
        gs.ServerSettings.SquadRequiredToChangeRole = ServerSettings.SquadRequiredToChangeRole;
        gs.ServerSettings.TankSpawnDelayMultipler = ServerSettings.TankSpawnDelayMultipler;
        gs.ServerSettings.TransportSpawnDelayMultipler = ServerSettings.TransportSpawnDelayMultipler;
        gs.ServerSettings.SeaVehicleSpawnDelayMultipler = ServerSettings.SeaVehicleSpawnDelayMultipler;
        gs.ServerSettings.APCSpawnDelayMultipler = ServerSettings.APCSpawnDelayMultipler;
        gs.ServerSettings.HelicopterSpawnDelayMultipler = ServerSettings.HelicopterSpawnDelayMultipler;
    }
}