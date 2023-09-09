using System.Reflection;
using BattleBitExpansionCore.DataSaver.Managers;
using CyberTechBattleBit2.DataSaver.Templates;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CyberTechBattleBit2.DataSaver;

public class PlayerDataManager
{
    private string[] SubFolders =
    {
        "Players"
    };

    public void init()
    {
        var cp = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var dp = Path.Combine(cp, "Data");
        var pp = Path.Combine(dp, "Player");
        if (!Directory.Exists(cp)) Directory.CreateDirectory(cp);
        if (!Directory.Exists(dp)) Directory.CreateDirectory(dp);
        if (!Directory.Exists(pp)) Directory.CreateDirectory(pp);

        //Make Data Folder
        //Make Sub Folders
    }

    public void SavePlayerData(CustomPlayer player)
    {
        var SteamID = player.SteamID.ToString();

        var cp = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var dp = Path.Combine(cp, "Data");
        var pp = Path.Combine(dp, "Player");
        var sp = Path.Combine(pp, SteamID + ".json");

        var data = new CustomPlayerData();
        data.LoadDataFromPlayer(player);

        var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Converters = new List<JsonConverter>()
            {
                new GameModeMapDataEntry.GamemodesConvert()
            }
        });
        // var json = JsonConvert.SerializeObject(data);
        File.WriteAllText(sp, json);
    }

    public CustomPlayerData GetPlayer(CustomPlayer player)
    {
        return GetPlayer(player.SteamID);
    }

    public CustomPlayerData GetPlayer(ulong steamID)
    {
        var SteamID = steamID;

        var cp = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var dp = Path.Combine(cp, "Data");
        var pp = Path.Combine(dp, "Player");
        var sp = Path.Combine(pp, SteamID + ".json");

        var fileName = sp;

        if (!File.Exists(fileName)) return null;

        var jsonString = File.ReadAllText(fileName);
        var weatherForecast = JsonConvert.DeserializeObject<CustomPlayerData>(jsonString, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Converters = new List<JsonConverter>()
            {
                new GameModeMapDataEntry.GamemodesConvert()
            }
        });
        // var weatherForecast = JsonConvert.DeserializeObject<CustomPlayerData>(jsonString, new JsonSerializerSettings()
        // {
        // Error = (sender, error) => error.ErrorContext.Handled = true
        // });


        return weatherForecast;
    }
}