using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2.Managers;

public class PermissionManager
{
    public static PlayerPermissionDataHolder GEN_DEFAULT_PERMS(ulong steamId)
    {
        var r = new PlayerPermissionDataHolder();
        // r.Name = player.Name;
        r.SteamID = steamId;
        r.Permissions.Add("BattleBitExpansionCore.Default");
        r.PermissionLevelLevel = ServerBasicPermissionLevel.Public;
        MasterPlayerPermission[steamId] = r;

        return r;
    }

    private readonly BattleBitExtenderMain P;
    private readonly PermissionDataManager PDM;

    internal static Dictionary<ulong, PlayerPermissionDataHolder> MasterPlayerPermission = new();
    public static PermissionManager Instsance { get; private set; }

    public PermissionManager(BattleBitExtenderMain commandManager)
    {
        P = commandManager;
        Instsance = this;
        PDM = new PermissionDataManager();
        PDM.Load();
    }


    internal void RegisterSavedPermission(CustomPlayer p)
    {
        MasterPlayerPermission[p.SteamID] = p.Permissions;
    }

    public bool CheckPerms(List<PluginAttributes.CommandPermissionAttribute> perms, CustomPlayer player, bool log = true)
    {
        //CHECK PERM LEVEL
        var pp = player.Permissions;
        var ppl = pp.PermissionLevelLevel;
        var PLFail = false;
        object PLFailo = null;

        foreach (var p in perms)
            if (p.PermissionLevel != ServerBasicPermissionLevel.None)
                if ((int)p.PermissionLevel > (int)ppl)
                {
                    PLFail = true;
                    PLFailo = p;
                    break;
                }

        if (PLFail)
        {
            if (log) Tools.ConsoleLog("FLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
            if (log) Tools.ConsoleLog($"FAILED AT {pp.GetType()} {string.Join("|", pp.Permissions)}");
            //FAILED AT PermLvl
            return false;
        }


        //Wildcard / String Perm Fields
        var FL = new List<string>();
        foreach (var p in perms)
            if (p.PermissionStrings.Length > 0)
                FL.AddRange(p.PermissionStrings);

        if (FL.Count == 0) return true;
        var dd = new PermStringDictionary(FL);
        var rr = dd.CheckListOfPerms(pp.Permissions);
        // if (ppl > perm)
        return rr;
    }


    public class PermStringDictionary
    {
        public List<string> OrgData = new();
        public PermDict PD = new("Root");

        public PermStringDictionary(List<string> d)
        {
            OrgData = d;
            foreach (var s in d)
            {
                var subs = s.Split(".").ToList();
                var z = subs[0];
                subs.RemoveAt(0);
                PD.Add(z, subs);
            }
        }


        public class PermDict
        {
            public string Namespace;
            public Dictionary<string, PermDict> Data = new();
            public bool Value = false;

            public PermDict(string namesapce)
            {
                Namespace = namesapce;
            }

            public void Add(List<string> value)
            {
                if (value.Count == 0)
                {
                    Value = true;
                    return;
                }

                var subs = value[0];
                var v = value.ToList();
                v.RemoveAt(0);
                Add(subs, v);
            }

            public void Add(string value)
            {
                var subs = value.Split(".").ToList();
                var a = subs[0];
                subs.RemoveAt(0);
                Add(a, subs);
            }

            public void Add(string key, List<string> value)
            {
                if (!Data.ContainsKey(key))
                {
                    var f = new PermDict(key);
                    f.Add(value);
                    Data[key] = f;
                }

                Data[key].Add(value);
            }

            public void Add(string key, string value)
            {
                if (!Data.ContainsKey(key))
                {
                    var f = new PermDict(key);
                    f.Add(value);
                }
            }

            public void Add(string key, PermDict value)
            {
                Data[key] = value;
            }


            public bool CheckPerms(List<string> ppPermissions)
            {
                if (ppPermissions.Count == 0) return Value;

                if (Data.ContainsKey(ppPermissions[0]))
                {
                    ppPermissions.RemoveAt(0);
                    return CheckPerms(ppPermissions);
                }

                return false;
            }

            // public bool CheckPerms(string key, List<string> ppPermissions)
            // {
            //     
            // }
            //
            //
            // public bool CheckPerms(string ppPermissions)
            // {
            //     var subs = ppPermissions.Split(".").ToList();
            //     // if (subs.Count == 0) return true;
            //     return CheckPerms(subs);
            // }
        }

        public bool CheckListOfPerms(List<string> ppPermissions)
        {
            if (ppPermissions.Count == 0) return true;

            foreach (var stringPerm in ppPermissions)
            {
                var subs = stringPerm.Split(".").ToList();
                // if(subs.Count == 1 &&  PD.)
                var r = PD.CheckPerms(subs);

                // var a = subs[0];
                // subs.RemoveAt(0);
                // var r = CheckPerms(stringPerm);
                if (r) return r;
            }

            return false;
        }
    }
}

public static class PermissionManagerHelper
{
    // public static bool CheckPerms(this CustomPlayer p, PluginAttributes.PluginCommandPermissionAttribute perm)
    // {
    //     PlayerPermissionDataHolder? pp = p.Permissions;
    //     if ()
    // }
}