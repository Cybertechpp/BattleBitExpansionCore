using System.Reflection;
using BattleBitExpansionCore.DataSaver.Managers;
using CyberTechBattleBit2.DataSaver.Templates;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CyberTechBattleBit2;

public class ServerConfigDataManager
{
    public ServerConfigDataManager()
    {
        //Create Config file if it does not exist

        var cp = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var dp = Path.Combine(cp, "config.yml");
        if (!File.Exists(dp))
        {
            var a = new ExtenderServerSettingsData();
            a.Port = 29294;
            Save(a);
            ExtenderConfig = a;
        }
        else
        {
            ExtenderConfig = Load();
        }

    }

    public ExtenderServerSettingsData ExtenderConfig { get; private set; }

    public ExtenderServerSettingsData Load()
    {
        var cp = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var dp = Path.Combine(cp, "config.yml");

        var fileName = dp;

        if (!File.Exists(fileName)) return null;
        var jsonString = File.ReadAllText(fileName);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance) // see height_in_inches in sample yml 
            .Build();

//yml contains a string containing your YAML
        try
        {
            var p = deserializer.Deserialize<ExtenderServerSettingsData>(jsonString);

            return p;
        }
        catch (Exception e)
        {
            Tools.ConsoleLog("Error while loading YAML!");
            Tools.ConsoleLog(e);
            return null;
        }
    }

    public void Save(ExtenderServerSettingsData? settings = null)
    {
        if (settings == null) settings = ExtenderConfig;
        var cp = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var dp = Path.Combine(cp, "config.yml");

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();


        var yaml = serializer.Serialize(settings);


        File.WriteAllText(dp, yaml);
    }
}