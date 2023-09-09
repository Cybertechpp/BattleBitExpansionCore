using BattleBitAPI.Common;
using CyberTechBattleBit2;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleBitExpansionCore.DataSaver.Managers;

public class GameModeMapData
{
    public bool ForceRestartGameserverToMatchMapRotation { get; set; } = false;

    [JsonIgnore] public bool LoopRanOnce { get; set; } = false;

    public bool AllowRandomVotingAfter1FullPlaylistRotation { get; set; } = false;


    public List<GameModeMapDataEntry> RotationData { get; set; } = new List<GameModeMapDataEntry>();
    public int CurrentKey { get; set; } = 0;

    public GameModeMapDataEntry NextGame { get; set; }

    public GameModeMapData()
    {
    }

    public async Task ForceStartNewGame(CustomGameServer gs, GameModeMapDataEntry? d = null)
    {
        SetNextGamemodeMapSize(gs, d);
        // await Task.Delay(2000);
        gs.ForceEndGame(Team.None);
        await Task.Delay(5000);
        gs.ForceStartGame();
        await Task.Delay(1000);
        gs.RoundSettings.PlayersToStart = BattleBitExtenderMain.Instance.GameServerSettings.getGameServerSettings(gs).RoundSettings.PlayersToStart;
        //Force Game Over
    }

    public async Task OnStart(CustomGameServer gs)
    {
        if (ForceRestartGameserverToMatchMapRotation)
        {
            var a = RotationData[0];
            if (gs.Map.ToString() == a.Map.ToString() && gs.MapSize.ToString() == a.Size.ToString())
            {
                CurrentKey = 0;
                return;
            }
            else
            {
                Tools.ConsoleLog($"!!!!!!!!! >> {gs.Map} {a.Map} {gs.MapSize} {a.Size}");
            }

            CurrentKey = -1;
            await ForceStartNewGame(gs);
        }
        else
        {
            SetNextGamemodeMapSize(gs, RotationData[CurrentKey]);
        }
    }

    public async Task OnNext(CustomGameServer gs)
    {
        SetNextGamemodeMapSize(gs);
    }

    public int GetNextKey()
    {
        var cc = CurrentKey;
        if (cc + 1 >= RotationData.Count)
        {
            cc = 0;
        }
        else
        {
            cc = cc + 1;
        }

        return cc;
    }

    public void SetNextGamemodeMapSize(CustomGameServer gs, GameModeMapDataEntry? next = null)
    {
        Tools.ConsoleLog("Setting Next Gamemode and Mapsize");
        var nk = GetNextKey();
        if (next == null) next = RotationData[nk];
        var ns = next.Size;
        if (next.IncreaseMapSizeToAccomidateCurrentPlayers || BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.AutoExpandServer)
        {
            var cp = gs.AllPlayers.ToArray().Length + BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.AutoExpandThreashHolad;
            if (cp >= (int)next.Size * 2)
            {
                ns = Tools.GetCorrectMapSize(gs.AllPlayers.ToArray().Length, BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.AutoExpandThreashHolad);
            }
        }

        Tools.ConsoleLog($"Setting Next Gamemode {next.Map.ToString()} and Mapsize {next.Size.ToString()} V {next.Size.ToString()} {next.IncreaseMapSizeToAccomidateCurrentPlayers}");
        Tools.ConsoleLog($"Setting NextGM TO {ns}");

        gs.SetServerSizeForNextMatch(ns);
        gs.MapRotation.ClearRotation();
        gs.MapRotation.SetRotation(new string[]
        {
            next.Map.ToString()
        });
        gs.GamemodeRotation.ClearRotation();
        gs.GamemodeRotation.SetRotation(new string[]
        {
            next.GM.Value
        });
        CurrentKey = nk;
    }


    public void AddDefaultMaps()
    {
        RotationData.Clear();
        RotationData.Add(new GameModeMapDataEntry(GameModeMapDataEntry.Gamemodes.TDM, GameModeMapDataEntry.Maps.Azagor, MapSize._16vs16));
        RotationData.Add(new GameModeMapDataEntry(GameModeMapDataEntry.Gamemodes.RUSH, GameModeMapDataEntry.Maps.Azagor, MapSize._16vs16));
        RotationData.Add(new GameModeMapDataEntry(GameModeMapDataEntry.Gamemodes.TDM, GameModeMapDataEntry.Maps.Basra, MapSize._16vs16));
        RotationData.Add(new GameModeMapDataEntry(GameModeMapDataEntry.Gamemodes.TDM, GameModeMapDataEntry.Maps.Construction, MapSize._16vs16));
    }
}

public class GameModeMapDataEntry
{
    // public int Order { get; set; } = 0;

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public MapSize Size { get; private set; }

    public Gamemodes GM { get; private set; }

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public Maps Map { get; private set; }

    public bool HasError { get; set; } = false;
    
    public bool IncreaseMapSizeToAccomidateCurrentPlayers { get; set; } = true;

    public override string ToString()
    {
        return GM.Value + "-" + Map.ToString() + "-" + ((int)Size).ToString();
    }

    public GameModeMapDataEntry(Gamemodes gm, Maps map, MapSize size = MapSize.None)
    {
        GM = gm;
        Map = map;
        Size = size;
    }


    public enum Maps
    {
        Azagor,
        Basra,
        Construction,
        District,
        Dustydew,
        Eduardovo,
        Frugis,
        Isle,
        Lonovo,
        MultuIslands,
        Namak,
        OilDunes,
        River,
        Salhan,
        SandySunset,
        TensaTown,
        Valley,
        Wakistan,
        WineParadise
    }
    public class GamemodesConvert : JsonConverter<Gamemodes>
    {
        public override void WriteJson(JsonWriter writer, Gamemodes value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Value);
        }

        public override Gamemodes ReadJson(JsonReader reader, Type objectType, Gamemodes existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;

            return Gamemodes.FromString(s);
        }
    }

    public class Gamemodes
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Name + "|" + Value;
        }

        public static readonly Gamemodes Default = TDM;
        public static readonly Gamemodes TDM = new Gamemodes("TDM", "TeamDeathMatch");
        public static readonly Gamemodes DOMI = new Gamemodes("DOMI", "Domination");
        public static readonly Gamemodes CONQ = new Gamemodes("CONQ", "Conquest");
        public static readonly Gamemodes AAS = new Gamemodes("AAS", "AAS");
        public static readonly Gamemodes RUSH = new Gamemodes("RUSH", "RUSH");
        public static readonly Gamemodes ELI = new Gamemodes("ELI", "ELI"); //UNKNOWN
        public static readonly Gamemodes INFCONQ = new Gamemodes("INFCONQ", "InfantryConquest");
        public static readonly Gamemodes FRONTLINE = new Gamemodes("FRONTLINE", "Frontline");
        public static readonly Gamemodes GunGameFFA = new Gamemodes("GunGameFFA", "GunGameFreeForAll");
        public static readonly Gamemodes FFA = new Gamemodes("FFA", "FreeForAll");
        public static readonly Gamemodes GunGameTeam = new Gamemodes("GunGameTeam", "GunGameTeam");
        public static readonly Gamemodes SuicideRush = new Gamemodes("SuicideRush", "SuicideRush");
        public static readonly Gamemodes CatchGame = new Gamemodes("CatchGame", "CatchGame");
        public static readonly Gamemodes Infected = new Gamemodes("Infected", "Infected");
        public static readonly Gamemodes CashRun = new Gamemodes("CashRun", "CashRun");
        public static readonly Gamemodes VoxelFortify = new Gamemodes("VoxelFortify", "VoxelFortify");
        public static readonly Gamemodes VoxelTrench = new Gamemodes("VoxelTrench", "VoxelTrench");
        public static readonly Gamemodes CaptureTheFlag = new Gamemodes("CaptureTheFlag", "CaptureTheFlag");


        public static Gamemodes? FromString(string s)
        {
            if (s == "TDM") return TDM;
            else if (s == "DOMI") return DOMI;
            else if (s == "CONQ") return CONQ;
            else if (s == "AAS") return AAS;
            else if (s == "RUSH") return RUSH;
            else if (s == "ELI") return ELI;
            else if (s == "INFCONQ") return INFCONQ;
            else if (s == "FRONTLINE") return FRONTLINE;
            else if (s == "GunGameFFA") return GunGameFFA;
            else if (s == "GunGameTeam") return GunGameTeam;
            else if (s == "SuicideRush") return SuicideRush;
            else if (s == "CatchGame") return CatchGame;
            else if (s == "Infected") return Infected;
            else if (s == "VoxelFortify") return VoxelFortify;
            else if (s == "VoxelTrench") return VoxelTrench;
            else if (s == "CaptureTheFlag") return CaptureTheFlag;
            return null;
        }
        
        public override bool Equals(object? obj)
        {
            if ((Gamemodes)obj != null)
            {
                return ((Gamemodes)obj).ToString() == this.ToString();
            }

            return false;
        }

        public Gamemodes(string value, string name)
        {
            Name = name;
            Value = value;
        }

        // private Gamemodes()
    }
}