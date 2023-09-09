namespace CyberTechBattleBit2.Events;

public class TestServerConnect : ServerConnectEvent
{
    // public override object HandlePlayerCommand()
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public override object HandleServerCommand()
    // {
    //     throw new NotImplementedException();
    // }

    public override object fireEvent()
    {
        Tools.ConsoleLog("ON JOIN WAS CALLED AND THE SERVER INFO IS!!!");

        Tools.ConsoleLog(
            $"ON JOIN WAS CALLED AND THE SERVER INFO IS!!! {GS?.GameIP}:{GS?.GamePort} ###{GS?.ServerHash} ||| {GS?.ServerName} {Data.GS == null}");
        return base.fireEvent();
    }
}

public class TestServerCreatePlayer : ServerCreatingPlayerInstance
{
    public override CustomPlayer fireEvent()
    {
        Tools.ConsoleLog("Create Player called!");
        Tools.ConsoleLog($"Checking Data!!! {((ServerCreatingPlayerInstanceData)Data)?.SteamID}");
        return base.fireEvent();
    }
}