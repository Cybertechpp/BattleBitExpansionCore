using System.Reflection;
using System.Text.RegularExpressions;
using ANSIConsole;
using CyberTechBattleBit2;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.DataSaver.Templates;
using Spectre.Console.Rendering;

namespace BattleBitExpansionCore.DataSaver.Managers;

public class GameServerSettingsDataManager : BaseDataSaverClass
{
    public GameServerSettingsDataManager() : base("Data/Gameservers/", "-GS.json", typeof(GameServerSettingHolder))
    {
        Load();
    }
    // public string Name { get; set; }
    // public string IP { get; set; }
    // public string Port { get; set; }
    // public string T { get; set; }


    public Dictionary<ulong, GameServerSettingHolder> GameServerSettings { get; private set; } = new Dictionary<ulong, GameServerSettingHolder>();

    public void Load()
    {
        var currentpath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var gs_savelocation = Path.Combine(currentpath, SaveLocation);
        if (!Directory.Exists(gs_savelocation)) Directory.CreateDirectory(gs_savelocation);
        string[] files = Directory.GetFiles(gs_savelocation);
        List<string> jsons = new();
        foreach (var path in files)
        {
            var filename = Path.GetFileName(path);
            // if (filename.Contains(".dll")) jsons.Add(path);

            var rg = new Regex(@"^([0-9]+\-GS\.json)$");
            if (rg.IsMatch(filename))
            {
                jsons.Add(path);
            }
            // var matches = rg.Matches(path);
            // if (matches.Count > 0)
            // {
            //     
            // }
            // Console.WriteLine();
        }

        //Find All JSON Files that contain #########-GS.json
        Tools.ConsoleLog($"WE FOUND THISE FOLDERS {jsons.Count}|||| " + string.Join("|", jsons));

        foreach (var location in jsons)
        {
            GameServerSettingHolder a = (GameServerSettingHolder)ReadFromFile(location);
            if (a.ServerSettings.Hash != null)
            {
                a.OnAdd();
                GameServerSettings[(ulong)a.ServerSettings.Hash] = a;
            }
        }
    }

    public GameServerSettingHolder getGameServerSettings(CustomGameServer gs)
    {
        var hash = gs.ServerHash;
        if (GameServerSettings.TryGetValue(hash, out var settings))
        {
            settings.GS = gs;
            return settings;
        }
        var s = new GameServerSettingHolder();
        s.LoadGSValues(gs);
        s.OnAdd();
        GameServerSettings[hash] = s;
        return s;
    }

    public void updateGameServerSettings(GameServerSettingHolder d, CustomGameServer gs = null)
    {
        if (d.ServerSettings.Hash != null)
        {
            GameServerSettings[(ulong)d.ServerSettings.Hash] = d;
        }
        else
        {
            if (gs != null)
            {
                d.LoadGSValues(gs);
                GameServerSettings[gs.ServerHash] = d;
            }
        }
        // return s;
    }


    public override object CreateBlankObject()
    {
        return new GameServerSettingHolder();
    }

    public void SaveAll()
    {
        foreach (var gss in GameServerSettings)
        {
            // CustomGameServer? tgs = null;
            GameServerSettingHolder v = gss.Value;
            foreach (CustomGameServer? customGameServer in BattleBitExtenderMain.Instance.L.ConnectedGameServers.ToList())
            {
                if (customGameServer.ServerHash == v.ServerSettings.Hash)
                {
                    Tools.ConsoleLog("HYASSSSSSSSSSSSSSSSSSSSSSSS MISSSSSS".Color(ConsoleColor.Red));
                    v.LoadGSValues(customGameServer);
                }
            }

            var currentpath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Tools.ConsoleLog($"CCCCCCCCCCCCCCCCCC {SaveLocation}");
            var fp = Path.Join(currentpath, SaveLocation);
            var fp2 = Path.Join(fp, gss.Key + SaveFileName);
            SaveToFile(v, fp2);
        }
    }
}