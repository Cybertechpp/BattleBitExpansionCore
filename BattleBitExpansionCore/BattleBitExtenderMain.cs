#region

using System.Drawing;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ANSIConsole;
using BattleBitAPI.Common;
using BattleBitAPI.Server;
using BattleBitExpansionCore.DataSaver.Managers;
using BattleBitExpansionCore.Managers.PluginManager;
using Colorful;
using CyberTechBattleBit2;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Events;
using CyberTechBattleBit2.Events.ExtenderEvents;
using CyberTechBattleBit2.Managers;
using Console = System.Console;

#endregion

public class BattleBitExtenderMain : LogTools.ModuleLogHelper<BattleBitExtenderMain>
{
    public static JsonSerializerOptions GetDefualtJSONSettings()
    {
        var stringEnumConverter = new JsonStringEnumConverter();
        var opts = new JsonSerializerOptions();
        opts.Converters.Add(stringEnumConverter);
        return opts;
    }


    private readonly Dictionary<int, ulong>
        GameServerHashList = new();

    internal readonly Dictionary<int, GameServerModuleHolder>
        GameServerHolder = new();

    private readonly Dictionary<int, GameServer<CustomPlayer>> GameServersList = new();
    public PlayerDataManager DSM;


    public ServerListener<CustomPlayer, CustomGameServer> L;
    public ServerConfigDataManager ExtenderServerConfig;

    public PluginManager PluginManager;

    //
    public PermissionManager PermissionManager;

    public static BattleBitExtenderMain Instance;

    private static void Main(string[] args)
    {
        if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
        Instance = new BattleBitExtenderMain();
        Instance.Start();
    }

    public void Stop()
    {
        Log.Info("Stopping Server");
        ExtenderServerConfig.Save();
        DBM.onClose();
        var ne = new ExtenderStopEvent();
        ne.LoadData(new ExtenderStopEvent_Data(null, L, this));
        EM.CallEvent(ne);
        
        GameServerSettings.SaveAll();

        PluginManager.CloseAllPlugins();

        foreach (var lConnectedGameServer in L.ConnectedGameServers)
        foreach (var customPlayer in lConnectedGameServer.AllPlayers)
            DSM.SavePlayerData(customPlayer);
        // customPlayer.Kick("Server shutting down!");
        // Thread.Sleep(1000);
        // lConnectedGameServer.StopServer();
        L.Stop();
        // Application.Exit();
        Environment.Exit(1);
    }


    public BanDataManager DBM;

    public static bool DebugMode = false;

    public GameServerSettingsDataManager GameServerSettings { get; private set; }
    public void Start()
    {
        // Console.Q += (sender, e) =>
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            Stop();
        };

        Log.Info("Loading...");
        ExtenderServerConfig = new ServerConfigDataManager();
        DebugMode = ExtenderServerConfig.ExtenderConfig.DebugMode;
        
        GameServerSettings = new GameServerSettingsDataManager();
        DBM = new BanDataManager();
        DSM = new PlayerDataManager();
        DSM.init();
        
        
        
        
        
        var port = ExtenderServerConfig.ExtenderConfig.Port;
        LogRawToConsole(new Figlet().ToAscii("-----| Welcome to Battle Bit Expansion Core |-----").ToString().Background(Color.Purple).Color(ConsoleColor.White));
        Log.Info(" Core Loading!");
        Console.WriteLine();
        L = new ServerListener<CustomPlayer, CustomGameServer>();
        Log.Info(" Core Listener Added!".Background(ConsoleColor.Green).Color(ConsoleColor.Black).ToString());
        // Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + " Core Listener Added!".Background(ConsoleColor.Green).Color(ConsoleColor.Black).ToString());


        //DEBUG ONLY
        L.LogLevel = LogLevel.All;
        var zz = (LogLevel level, string s, object? arg3) =>
        {
            Console.Out.WriteLine(level.ToString().Color(ConsoleColor.DarkCyan) + " > " +
                                  s.Background(ConsoleColor.Black) +
                                  (arg3 != null ? "|" + arg3?.ToString().Background(ConsoleColor.Black) : null));
        };

        L.OnLog += zz;


        Log.Info(" Basic Console Logging Added!".Background(ConsoleColor.Green).Color(ConsoleColor.Black).ToString());


        L.OnGameServerConnected += OnGameServerConnected;
        EM = new EventManager(this);
        CM = new CommandManager(this);
        PluginManager = new PluginManager(this);
        PermissionManager = new PermissionManager(this);

        // new MainPage();

        L.Start(port);
        Log.Info($" Listening on port {port}!".Background(ConsoleColor.Green).Color(ConsoleColor.Black).ToString());
        //LOAD Cyber Modules


        // Thread.Sleep(1000 * 2);
        // this.DEBUG();


        var ne = new ExtenderStartEvent();
        ne.LoadData(new ExtenderStartEvent_Data(null, L, this));
        EM.CallEvent(ne);


        //SEPERATE THREAD!!!!
        //SEPERATE THREAD!!!!
        do
        {
            var r = Console.ReadLine(); //read command
            if (!(r == null || r.Length == 0))
            {
                Log.Info(Tools.TextTemplates.FirstLevelTags.BBECTag + $" Console Command was ran: {r}!".Background(Color.Purple).Color(ConsoleColor.Black).ToString());
                if(TargetGameServer == null)Log.Warn("Target Gameserver not set! Please use /gameserver to switch or choose one!".Background(ConsoleColor.Yellow).Color(ConsoleColor.Black));
                CM.HandleCommands(TargetGameServer, r.Split(" ")[0], r.Split(" ").Skip(1).ToArray(), null, ChatChannel.AllChat);
            }
        } while (true);

        Thread.Sleep(-1);
    }

    public EventManager EM { get; set; }
    public CustomGameServer? TargetGameServer { get; set; } = null;


    /// <summary>
    ///     Register Gameserver Events to Event Manager
    /// </summary>
    /// <param name="arg"></param>
    private async Task OnGameServerConnected(GameServer<CustomPlayer> arg)
    {
        //get index
        var l = GameServersList.Count;
        while (GameServersList.ContainsKey(l)) l++;

        var svr = arg;

        GameServersList.Add(l, svr);
        GameServerHolder.Add(l, new GameServerModuleHolder((CustomGameServer)svr));
        GameServerHashList.Add(l, svr.ServerHash);
        var ip = svr.GameIP;
        var port = svr.GamePort;
        Tools.ConsoleLog($"Adding new Gameserver(#{l}) to API: {ip}:{port} HASH {svr.ServerHash}");
        RegisterTasksOnGameserver((CustomGameServer)svr, l);
    }

    public CommandManager CM;

    private void LoadModules(CustomGameServer gs, int index)
    {
        //COMMAND MANGER LISTENER
        gs.CommandTaskList += async (gs, player, channel, arg3) =>
        {
            var c = arg3.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList();
            var cs = string.Join("|", c);
            var z = new List<string>(c);
            z.RemoveAt(0);
            var zs = string.Join("|", z);
            Tools.ConsoleLog("Just as an FYIIII AGAIN".Color(ConsoleColor.Red) +
                             $"{c[0].Replace("/", "").Replace("!", "")} |||| {cs} {zs}");
            var a = CM.HandleCommands(gs, c[0].Replace("/", "").Replace("!", ""), z.ToArray(),
                player, channel);
            Tools.ConsoleLog("COMMAND " + c[0] + " SENT W " + c.Take(1).ToArray());
            return true;
        };


        //PLAYER CONNECT LISENER
        gs.PlayerOnConnectTaskList += async player =>
        {
            Tools.ConsoleLog("PLAYER JOIN EVENT CALLEDDD!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            var a = DSM.GetPlayer(player);
            if (a != null)
            {
                a.LoadDataToPlayer(player);
                Tools.ConsoleLog("JUST LOADED SAVED PLAYER DATA!!!");
            }

            var e = new PlayerConnectJoinGameserver();
            e.LoadData(new PlayerConnectJoinGameserver_Data(gs, player));
            EM.CallEvent(e);
        };

        gs.PlayerLeaveServerTaskList += async player =>
        {
            Log.Info($"Trying to save {player.Name}");
            try
            {
                DSM.SavePlayerData(player);
            }
            catch (Exception ee)
            {
                Log.Info("WTFFFFFF IS THISSS");
                Log.Info(ee);
            }

            Log.Info($"Trying to save {player.Name}");
            var e = new PlayerDisconnectLeaveGameserver();
            e.LoadData(new PlayerDisconnectLeaveGameserver_Data(gs, player));
            EM.CallEvent(e);
        };
        gs.PlayerKillEvent += async (server, arguments) =>
        {
            var e = new PlayerKillEvent();
            e.LoadData(new PlayerKillEvent_Data(gs, arguments.Killer, arguments.Victim, arguments));
            EM.CallEvent(e);
        };
    }


    public void RegisterTasksOnGameserver(CustomGameServer gs, int index)
    {
        // gs.OnPlayerTypedCommand = new OnPlayerTypedCommand();
        LoadModules(gs, index);
    }
}

internal class GameServerModuleHolder
{
    public CommandManager CommandManager;
    private CustomGameServer GameServer;

    public GameServerModuleHolder(CustomGameServer gs)
    {
        GameServer = gs;
        // CommandManager = new CommandManager(gs);
    }

    public void InstallListeners()
    {
    }
}