using BattleBitAPI.Common;
using CyberTechBattleBit2;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleBitExpansionCore.DataSaver.Managers;

public class GameModeMapData
{
    public bool ForceRestartGameserverToMatchMapRotation { get; set; } = false;
    
    [JsonIgnore]
    public bool LoopRanOnce { get; set; } = false;
    
    public bool AllowRandomVotingAfter1FullPlaylistRotation { get; set; } = false;
    
    
    public List<GameModeMapDataEntry> RotationData { get; set; } = new List<GameModeMapDataEntry>();
    public int CurrentKey { get; set; } = 0;

    public GameModeMapDataEntry NextGame { get; set; }
    
    public GameModeMapData()
    {
        
    }

    public async Task ForceStartNewGame(CustomGameServer gs,GameModeMapDataEntry? d = null)
    {
        SetNextGamemodeMapSize(gs,d);
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
            if (gs.Map.ToString() == a.Map.ToString() && gs.MapSize.ToString() == a.Map.ToString())
            {
                CurrentKey = 0;
                return;
            }

            CurrentKey = -1;
            await ForceStartNewGame(gs);
        }
        else
        {
            SetNextGamemodeMapSize(gs,RotationData[CurrentKey]);
        }
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
            cc =  cc + 1;
        }

        return cc;
    }
    
    public void SetNextGamemodeMapSize(CustomGameServer gs,GameModeMapDataEntry? next = null)
    {
        Tools.ConsoleLog("Setting Next Gamemode and Mapsize");
        var nk = GetNextKey();
        if(next == null) next = RotationData[nk];
        var ns = next.Size;
        if (next.IncreaseMapSizeToAccomidateCurrentPlayers)
        {
            var cp = gs.AllPlayers.ToArray().Length + BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.AutoExpandThreashHolad;
            if (cp >= (int)next.Size * 2)
            {
                ns = Tools.GetCorrectMapSize(gs.AllPlayers.ToArray().Length , BattleBitExtenderMain.Instance.ExtenderServerConfig.ExtenderConfig.AutoExpandThreashHolad);
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
        CurrentKey =nk;
    }
    

    public void AddDefaultMaps()
    {
        RotationData.Clear();
        RotationData.Add(new GameModeMapDataEntry(GameModeMapDataEntry.Gamemodes.TDM,GameModeMapDataEntry.Maps.Azagor,MapSize._32vs32));
        RotationData.Add(new GameModeMapDataEntry(GameModeMapDataEntry.Gamemodes.TDM,GameModeMapDataEntry.Maps.Basra,MapSize._32vs32));
        RotationData.Add(new GameModeMapDataEntry(GameModeMapDataEntry.Gamemodes.TDM,GameModeMapDataEntry.Maps.Frugis,MapSize._32vs32));
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
    public bool IncreaseMapSizeToAccomidateCurrentPlayers { get;  set; } = true;
    
    public override string ToString()
    {
        return GM.Value + "-" + Map.ToString() + "-" +((int)Size).ToString();
    }

    public GameModeMapDataEntry(Gamemodes gm, Maps map,MapSize size = MapSize.None)
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

    public class Gamemodes
    {
        public string Name { get;  set; }
        public string Value { get;  set; }

        public override string ToString()
        {
            return Name+"|"+Value;
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

        public override bool Equals(object? obj)
        {
            if ((Gamemodes)obj != null)
            {
                return ((Gamemodes)obj).ToString() == this.ToString();
            }

            return false;
        }

        public Gamemodes( string value,string name)
        {
            Name = name;
            Value = value;
        }

        // private Gamemodes()
    }
}