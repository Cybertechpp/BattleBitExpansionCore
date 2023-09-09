namespace CyberTechBattleBit2.DataSaver.Templates;

public class ExtenderServerSettingsData
{
    public bool DebugMode { get; set; } = true;
    public int Port { get; set; } = 29294;
    public bool SavePlayerData { get; set; } = true;
    public bool AutoExpandServer { get; set; } = true;
    public int AutoExpandThreashHolad { get; set; } = 5;

    public string SaveType { get; set; } = "JSON";
    public string JoinChatMessage { get; set; } = "Welcome to a BattleBitExpansionCore Server - An Extend BB API\n Use `/help` for a list of commands!";

    public string JoinMessage { get; set; } = "Welcome to a CyberTech TestServer! Earn a cool Killstreak every 3,5,and 9 Kills!\n" +
                                              "Every kill earns you ServerXP which can be spent at the `/xpshop\n" +
                                              "For example, once you have enough XP you can run\n" +
                                              "`/xpshop uav` or `/xpshop g` or `/xpshop grenade`\n" +
                                              "Or just run `/xpshop` to se a list of ServerXP perks!\n\n" +
                                              "As always.... HAVE FUN! BE SAFE! AND DONT TAKE IT TOO FAR!";
    // public string JoinMessage { get; set; } = "Welcome to a BattleBitExpansionCore Server - An Extend BB API";
    // public string  { get; set; }
    // public string IPAddress { get; set; }
}