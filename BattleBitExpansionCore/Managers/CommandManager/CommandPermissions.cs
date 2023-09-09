using CyberTechBattleBit2.DataSaver;

namespace CyberTechBattleBit2;

public class CommandPermissions
{
    public ServerBasicPermissionLevel Level = ServerBasicPermissionLevel.None;
    public string PermissionString = "";

    public bool CheckPermissions(CustomPlayer p)
    {
        var pl = p.Permissions.PermissionLevelLevel;
        if (Level == ServerBasicPermissionLevel.None)
            if ((int)pl >= (int)Level)
            {
            }

        return false;
    }
}