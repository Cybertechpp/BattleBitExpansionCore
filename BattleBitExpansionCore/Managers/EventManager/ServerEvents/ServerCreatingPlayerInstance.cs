using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.DataSaver.Templates;
using CyberTechBattleBit2.Managers;

namespace CyberTechBattleBit2.Events;

public class ServerCreatingPlayerInstance : EventBase //, IEventWrapper<ServerCreatingPlayerInstanceData, CustomPlayer>

{
    public ServerCreatingPlayerInstance() : base(EventTypes.ServerCreatingPlayer)
    {
    }

    // public ServerCreatingPlayerInstanceData Data { get; set; }

    public override CustomPlayer fireEvent()
    {
        var player = new CustomPlayer();
        var sid = ((ServerCreatingPlayerInstanceData)Data)?.SteamID;
        if (sid == null) return new CustomPlayer();
        player.TempSteamID = (ulong)sid;
        // var p = Program.Instance;
        // CustomPlayer.Internal i;
        // var player = p.L.mInstanceDatabase.GetPlayerInstance((ulong)sid, out i,null);
        // CustomPlayer.Internal
        // player.SteamID = Data.s

        // i.SteamID = (ulong)sid;
        Tools.ConsoleLog($"SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS {sid}");
        Tools.ConsoleLog("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
        Tools.ConsoleLog("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");

        var zz = BattleBitExtenderMain.Instance.DBM.GetServerBans((ulong)sid, false);
        var a = BattleBitExtenderMain.Instance.DSM.GetPlayer((ulong)sid);
        if (zz != null) Tools.ConsoleLog($"SSSSSSS{zz?.IsValid()} {zz?.BannedOn}SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
        if (a != null)
        {
            player = a.LoadDataToPlayer(player);
            // player.SetXp(a.XP);
            // player.Kills = a.Kills;
            // player.Deaths = a.Deaths;
            // player.KillStreak = a.KillStreak;
            Tools.ConsoleLog("JUST LOADED SAVED PLAYER DATA!!!");
        }

        var aa = BattleBitExtenderMain.Instance.DBM.GetServerBans((ulong)sid);
        if (aa != null)
        {
            Tools.ConsoleLog("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK");
            Tools.ConsoleLog("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK");
            Tools.ConsoleLog("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK");
            Tools.ConsoleLog($"KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK {sid.GetType()}");
            Tools.ConsoleLog("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK" + sid);
            // Data.GS.Kick((ulong)sid,"You are banned from the server!");
            player.Banned = true;
            // player.Kick("You are BANNNEDDDDDD");
            return player;
        }


        if (player.Permissions == null)
        {
            // Tools.ConsoleLog("11111111111111111111111111111111111111111111111111111111111111111111111111111111");
            // Tools.ConsoleLog("11111111111111111111111111111111111111111111111111111111111111111111111111111111");
            // Tools.ConsoleLog("11111111111111111111111111111111111111111111111111111111111111111111111111111111");
            // Tools.ConsoleLog("11111111111111111111111111111111111111111111111111111111111111111111111111111111");
            player.Permissions = PermissionManager.GEN_DEFAULT_PERMS((ulong)sid);
        }
        else
        {
            // Tools.ConsoleLog("2222322322222232232222223223222222322322222232232222223223222222322322");
            // Tools.ConsoleLog("2222322322222232232222223223222222322322222232232222223223222222322322");
            // Tools.ConsoleLog("2222322322222232232222223223222222322322222232232222223223222222322322");
            // Tools.ConsoleLog("2222322322222232232222223223222222322322222232232222223223222222322322");
            // Tools.ConsoleLog("2222322322222232232222223223222222322322222232232222223223222222322322");
            // Tools.ConsoleLog("2222322322222232232222223223222222322322222232232222223223222222322322");
            // Tools.ConsoleLog("2222322322222232232222223223222222322322222232232222223223222222322322");
        }

        Tools.ConsoleLog($"chekkkkkKk {player.Permissions.GetType()} {player.RunningXP} {player.ServerXP}");
        return player;
    }

    // public void LoadData(ServerCreatingPlayerInstanceData data)
    // {
    //     base.LoadData(data);
    // }

    // public override void LoadData(EventBaseData data)
    // {
    //     base.LoadData(data);
    // }
}

public class ServerCreatingPlayerInstanceData : EventBaseData
{
    public ServerCreatingPlayerInstanceData(CustomGameServer gs, ulong steamId) : base(gs)
    {
        SteamID = steamId;
    }

    public ulong SteamID { get; set; }
}