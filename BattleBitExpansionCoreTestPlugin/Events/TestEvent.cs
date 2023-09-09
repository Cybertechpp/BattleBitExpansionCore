using CyberTechBattleBit2;
using CyberTechBattleBit2.Events;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace BattleBitExpansionCore_DemoPlugin.Events;

[PluginAttributes.PluginEvent(EventTypes.ServerConnectEvent)]
public class TestEvent : ServerConnectEvent
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

    public override object? fireEvent()
    {
        // Tools.ConsoleLog("HEY^YYYYYYYYYYYYYYYYY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        return base.fireEvent();
    }
}