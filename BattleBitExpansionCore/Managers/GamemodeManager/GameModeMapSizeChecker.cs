using BattleBitAPI.Common;
using BattleBitExpansionCore.DataSaver.Managers;
using Terminal.Gui;

namespace CyberTechBattleBit2.Managers.GamemodeManager;

public class GameModeMapSizeChecker
{
    public GameModeMapDataEntry Data;
    public GameModeMapDataEntry.Gamemodes Gamemode;
    public GameModeMapDataEntry.Maps Map;
    public MapSize MapSize;

    private Dictionary<GameModeMapDataEntry.Maps, GMMS_Entry> Checkers = new();


    public GameModeMapSizeChecker()
    {
        // Data = data;
        // Gamemode = data.GM;
        // Map = data.Map;
        // MapSize = data.Size;

        Checkers[GameModeMapDataEntry.Maps.Azagor] = (new GMMS_Entry(GameModeMapDataEntry.Maps.Azagor, new List<AllowedGamemodeCriteria>()
        {
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.TDM, new List<MapSize>()
            {
                MapSize._8v8,
                MapSize._16vs16,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.RUSH, new List<MapSize>()
            {
                MapSize._16vs16,
                MapSize._32vs32,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.CONQ, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.DOMI, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.FRONTLINE, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.INFCONQ, new List<MapSize>()
            {
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.ELI, new List<MapSize>()
            {
                MapSize._16vs16
            }),
        }));

        Checkers[GameModeMapDataEntry.Maps.Basra] = (new GMMS_Entry(GameModeMapDataEntry.Maps.Basra, new List<AllowedGamemodeCriteria>()
        {
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.TDM, new List<MapSize>()
            {
                MapSize._8v8,
                MapSize._16vs16,
            }),

            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.CONQ, new List<MapSize>()
            {
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.FRONTLINE, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.INFCONQ, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.ELI, new List<MapSize>()
            {
                MapSize._8v8,
                MapSize._16vs16,
            }),
        }));

        Checkers[GameModeMapDataEntry.Maps.Construction] = (new GMMS_Entry(GameModeMapDataEntry.Maps.Construction, new List<AllowedGamemodeCriteria>()
        {
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.TDM, new List<MapSize>()
            {
                MapSize._8v8,
                MapSize._16vs16,
            }),

            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.CONQ, new List<MapSize>()
            {
                MapSize._32vs32,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.DOMI, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.CashRun, new List<MapSize>()
            {
                MapSize._16vs16,
                MapSize._32vs32,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.ELI, new List<MapSize>()
            {
                MapSize._16vs16
            }),
        }));


        Checkers[GameModeMapDataEntry.Maps.District] = (new GMMS_Entry(GameModeMapDataEntry.Maps.District, new List<AllowedGamemodeCriteria>()
        {
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.RUSH, new List<MapSize>()
            {
                MapSize._16vs16,
                MapSize._32vs32,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.INFCONQ, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.CaptureTheFlag, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.CONQ, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.DOMI, new List<MapSize>()
            {
                MapSize._16vs16,
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.FRONTLINE, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64
            }),
        }));

        Checkers[GameModeMapDataEntry.Maps.Dustydew] = (new GMMS_Entry(GameModeMapDataEntry.Maps.Dustydew, new List<AllowedGamemodeCriteria>()
        {
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.TDM, new List<MapSize>()
            {
                MapSize._8v8,
                MapSize._16vs16,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.RUSH, new List<MapSize>()
            {
                MapSize._16vs16,
                MapSize._32vs32,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.CONQ, new List<MapSize>()
            {
                MapSize._32vs32,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.INFCONQ, new List<MapSize>()
            {
                MapSize._16vs16,
                MapSize._64vs64,
                MapSize._64vs64,
                MapSize._127vs127,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.GunGameFFA, new List<MapSize>()
            {
                MapSize._8v8
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.GunGameTeam, new List<MapSize>()
            {
                MapSize._8v8,
                MapSize._16vs16,
            }),
            new AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes.FFA, new List<MapSize>()
            {
                MapSize._8v8
            }),
        }));
    }


    public bool Validate(GameModeMapDataEntry data)
    {
        ;
        var gamemode = data.GM;
        var map = data.Map;
        var mapSize = data.Size;
        GMMS_Entry v;
        if (!Checkers.TryGetValue(data.Map, out v)) return true;
        return v.Validate(gamemode, mapSize);
    }
}

public class AllowedGamemodeCriteria
{
    public GameModeMapDataEntry.Gamemodes Gamemode;
    public List<MapSize> AllowedMapSizes;

    public AllowedGamemodeCriteria(GameModeMapDataEntry.Gamemodes gamemode, List<MapSize> allowedMapSizes)
    {
        Gamemode = gamemode;
        AllowedMapSizes = allowedMapSizes;
    }
}

public class GMMS_Entry
{
    public GameModeMapDataEntry.Maps Map;
    public List<AllowedGamemodeCriteria> AllowedGamemodes;

    public GMMS_Entry(GameModeMapDataEntry.Maps map, List<AllowedGamemodeCriteria> allowedGamemodes)
    {
        Map = map;
        AllowedGamemodes = allowedGamemodes;
    }

    public bool Validate(GameModeMapDataEntry.Gamemodes gamemode, MapSize mapSize)
    {
        foreach (var gm in AllowedGamemodes)
        {
            if (gm.Gamemode.Value == gamemode.Value)
            {
                if (gm.AllowedMapSizes.Contains(mapSize))return true;
                return false;
            }
        }

        return false;
    }
}