using CyberTechBattleBit2.DataSaver.Templates;

namespace BattleBitExpansionCore_TestPlugin;

public class TestSaveData: CustomPlayerData.CustomPlayerData_PluginData
{
    public Dictionary<string, string> TestData { get; set; } = new Dictionary<string, string>();
}