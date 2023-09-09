using BattleBitExpansionCore_TestPlugin;
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
        TestPlugin.Log.Info(("A new Gameserver has connected!"));
        return base.fireEvent();
    }
}