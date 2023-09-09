using System.CodeDom;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using CyberTechBattleBit2.DataSaver.Templates;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CyberTechBattleBit2.DataSaver;

public abstract class BaseDataSaverClass
{
    public string SaveLocation;
    public string BaseSaveLocation = "Data/Plugins";
    public string SaveFileName;
    public Type SaveStructure;
    public string ParsedSaveLocation { get; private set; } = null;

    public BaseDataSaverClass(string saveLocation, string saveFileName, Type saveStructure)
    {
        SaveLocation = saveLocation;
        SaveFileName = saveFileName;
        SaveStructure = saveStructure;
        init();
    }

    public string ReParseSaveLocation(string filename, string extension = ".json")
    {
        var fl1 = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var fl2 = Path.Combine(fl1, BaseSaveLocation);
        var fl3 = Path.Combine(fl2, SaveLocation);
        var fl4 = Path.Combine(fl3, filename + extension);
        return fl4;
    }

    public void init()
    {
        var fl1 = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var fl2 = Path.Combine(fl1, BaseSaveLocation);
        var fl3 = Path.Combine(fl2, SaveLocation);
        var fl4 = Path.Combine(fl3, SaveFileName);
        ParsedSaveLocation = fl4;
        if (!Directory.Exists(fl1)) Directory.CreateDirectory(fl1);
        if (!Directory.Exists(fl2)) Directory.CreateDirectory(fl2);
        if (!Directory.Exists(fl3)) Directory.CreateDirectory(fl3);

        //Make Data Folder
        //Make Sub Folders
    }

    // public abstract void SaveData(params object[] data);
    // public abstract void LoadData(params object[] data);

    public void SaveToFile(object data, string location = null)
    {
        var datas = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        if (location == null) location = ParsedSaveLocation;
        File.WriteAllText(location, datas);
    }

    public abstract object CreateBlankObject();

    public object ReadFromFile(string location = null)
    {
        if (location == null) location = ParsedSaveLocation;

        if (!File.Exists(location) || File.ReadAllText(location).Length == 0)
        {
            var cn = CreateBlankObject();
            if (cn == null) return null;
            SaveToFile(cn, location);
            return cn;
        }

        try
        {
            return JsonConvert.DeserializeObject(File.ReadAllText(location), SaveStructure, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
        catch (Exception e)
        {
            Tools.ConsoleLog("ERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
            Tools.ConsoleLog(e);
            var cn = CreateBlankObject();
            if (cn == null) return null;
            SaveToFile(cn, location);
            return cn;
        }

        ;
    }
}