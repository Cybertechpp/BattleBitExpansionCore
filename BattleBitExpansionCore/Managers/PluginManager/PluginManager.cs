using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ANSIConsole;
using CyberTechBattleBit2;
using CyberTechBattleBit2.DataSaver.Templates;
using CyberTechBattleBit2.Events;
using System.Text.Json;
using BattleBitAPI.Pooling;
using BattleBitExpansionCore.DataSaver.Managers;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Managers.PluginManager.Utils;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BattleBitExpansionCore.Managers.PluginManager;

public class PluginManager
{
    public BattleBitExtenderMain Main;


    public List<PluginInfo> PlugList = new();
    public Dictionary<string, CustomPlayerData.CustomPlayerData_PluginData> DatasaverObject = new();

    private static PluginManager Instance;

    public static PluginManager getInstance()
    {
        return Instance;
    }

    public CustomPlayerData.CustomPlayerData_PluginData? getDataSaverObject(string key)
    {
        return DatasaverObject[key] ?? null;
    }


    public void CloseAllPlugins()
    {
        foreach (var pi in PlugList)
        {
            var mc = pi.MainClass;
            Tools.ConsoleLog($"2222222222222222222222222222222>>>>>> {mc == null} {pi.Loaded} {pi.Directory} {pi.DatasaverAttributes.Count}");
            mc.onDisable();


            //SAVE Data stores
            foreach (var qqq in pi.DatasaverAttributes)
            {
                var k = qqq.Key;
                var v = qqq.Value;

                //SAVE THIS
                var vv = DatasaverObject[k];

                var location = pi.Directory;
                var sp = Path.Combine(location, v.FileName + ".json");

                var az = JsonConvert.SerializeObject(vv, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Converters = new List<JsonConverter>()
                    {
                        new GameModeMapDataEntry.GamemodesConvert()
                    }
                });
                File.WriteAllText(sp, az);
            }
        }
    }

    public PluginManager(BattleBitExtenderMain program)
    {
        Instance = this;
        Main = program;

        var currentpath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var p_plugin = Path.Combine(currentpath, "Plugin");
        if (!Directory.Exists(p_plugin)) Directory.CreateDirectory(p_plugin);
        string[] files = Directory.GetFiles(p_plugin);
        if (files.Length == 0)
        {
            Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.PluginManager +
                             " 0 Plugins Found".Background(ConsoleColor.Yellow));
        }
        else
        {
            List<string> dlls = new();
            foreach (var path in files)
            {
                var filename = Path.GetFileName(path);
                if (filename.Contains(".dll")) dlls.Add(path);

                var rg = new Regex(@"[/\\\\]([\\w,\\s]+.dll)");
                // var matches = rg.Matches(path);
                // if (matches.Count > 0)
                // {
                //     
                // }
                // Console.WriteLine();
            }

            Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.PluginManager +
                             $" {dlls.Count} Plugins Found".Background(ConsoleColor.DarkYellow)
                                 .Color(ConsoleColor.White));
            TryLoadDLLs(dlls);
        }
    }

    private void TryLoadDLLs(List<string> dlls)
    {
        List<Assembly> validAssemblies = new();
        List<Assembly> validClassTypes = new();
        List<Assembly> validMethodTypes = new();


        foreach (var pathdll in dlls)
        {
            Type MainType = null;
            List<Type> CommandTypes = new();
            List<Type> EventTypes = new();


            //LOAD SINGLE DLL
            var assembly = Assembly.LoadFile(pathdll);
            var pluginInfo = new PluginInfo();

            var a = Path.GetDirectoryName(pathdll);
            pluginInfo.Directory = Path.Join(a, "../Data/Plugins/");

            foreach (var type in assembly.GetExportedTypes())
            {
                var aa = type.Attributes.ToString();
                var aaa = type.GetCustomAttributesData();
                //IN CLASS/FILE
                if (BattleBitExtenderMain.DebugMode)
                    Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.PluginManager +
                                     "HEY Found Class Files".Background(Color.Orange).Color(ConsoleColor.White) + " > " +
                                     type?.Name +
                                     $" ||| {aaa.Count}");
                //CHECK ATTRIBUTES
                var validAttr = false;


                //PLUGIN INFO ATTRIBUTE
                List<PluginAttributes.PluginInfoAttribute> pa = type.GetCustomAttributes<PluginAttributes.PluginInfoAttribute>().ToList();
                var mainType = pa.Count > 0 ? type : null;


                //ONLY RUN IN MAIN CLASS
                if (mainType != null)
                {
                    //PLUGIN DATA SAVER ATTRIBUTE

                    List<PluginAttributes.PluginDataSaverAttribute> da = type.GetCustomAttributes<PluginAttributes.PluginDataSaverAttribute>().ToList();
                    if (da.Count > 0)
                        foreach (var sda in da)
                        {
                            var an = sda.AccessName;
                            pluginInfo.DatasaverAttributes[an] = sda;
                        }
                }


                List<Type> commandTypes =
                    type.GetCustomAttributes<PluginAttributes.PluginCommandAttribute>().ToList().Count == 0
                        ? new List<Type>()
                        : new List<Type>() { type };
                List<Type> eventTypes =
                    type.GetCustomAttributes<PluginAttributes.PluginEventAttribute>().ToList().Count == 0
                        ? new List<Type>()
                        : new List<Type>() { type };

                if (type.IsAssignableFrom(typeof(IPluginEvent)) || type.IsAssignableTo(typeof(IPluginEvent)))
                {
                    EventTypes.Add(type);
                }
                else
                {
                    // Tools.ConsoleLog("'|||||||||".Color(ConsoleColor.Red).ToString()+$"|||>> FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
                }


                pluginInfo.FoundEvents.AddRange(eventTypes);

                if (MainType == null && mainType != null) MainType = mainType;
                if (commandTypes.Count > 0)
                {
                    // Tools.ConsoleLog($"ADDING P{commandTypes.Count}".Background(ConsoleColor.Red));
                    CommandTypes.AddRange(commandTypes);
                }

                // if (eventTypes.Count > 0)
                // {
                //     Tools.ConsoleLog($"ADDING eventTypeseventTypeseventTypes P{commandTypes.Count}".Background(ConsoleColor.Red));
                //     CommandTypes.AddRange(commandTypes);
                // }
                if (pa != null && pa.Count > 0)
                    pluginInfo.InfoAttribute =
                        type.GetCustomAttributes<PluginAttributes.PluginInfoAttribute>().ToList()[0];


                foreach (var z in aaa)
                {
                    if (BattleBitExtenderMain.DebugMode)
                        Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.PluginManager +
                                         "HEY Found ATTRIBUTE FOR THIS CLASS  Files".Background(Color.Orange)
                                             .Color(ConsoleColor.White) + " > " +
                                         type?.Name + $" ||| {z} TYPE {type}");
                    var c1 = aaa.Where(a => a.AttributeType == typeof(PluginAttributes.PluginInfoAttribute))
                        // .Select(a => new CustomAttributeData(a))
                        .ToList();
                }
            }

            pluginInfo.Assembly = assembly;
            pluginInfo.EventTypes = EventTypes;
            pluginInfo.MainClassType = MainType;
            pluginInfo.FoundCommands = CommandTypes;
            PlugList.Add(pluginInfo);
            Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.PluginManager +
                             $" Found {pluginInfo.InfoAttribute.Name} wth {pluginInfo.FoundCommands.Count} " //|| {CommandTypes.Count} Commands!
                                 .Color(ConsoleColor.Yellow));

            //Fond Class Files


            // var c = Activator.CreateInstance(type);
            // type.InvokeMember("Output", BindingFlags.InvokeMethod, null, c, new object[] {@"Hello"});
        }


        Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.PluginManager +
                         $" Tring to load {PlugList.Count} Plugins!".Color(ConsoleColor.Yellow));


        ////LOADING PROCESSS
        ////LOADING PROCESSS
        ////LOADING PROCESSS
        ////LOADING PROCESSS


        foreach (var plug in PlugList)
        {
            var MC = plug.MainClassType;
            var MMC = (PluginBase)Activator.CreateInstance(MC, new object[] { plug.InfoAttribute });
            plug.MainClass = MMC;
            MMC.onLoad();
            // plug.Loaded = true;
            MMC.onEnable();
            //TODO register Commands to Command Manager
            //TODO register Events to Event Manager
            // GameServerModuleHolder z = Main.GameServerHolder[0];
            var z = Main.CM;
            //TODO?
            foreach (var cmd in plug.FoundCommands)
            {
                var zz = (Command?)Activator.CreateInstance(cmd);
                if (zz == null)
                {
                    Tools.ConsoleLog("ERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
                    continue;
                }

                //CHECK FOR PERMS
                List<PluginAttributes.CommandPermissionAttribute> rawPermList = cmd.GetCustomAttributes<PluginAttributes.CommandPermissionAttribute>().ToList();
                // foreach (PluginAttributes.PluginCommandPermissionAttribute r in rawPermList)
                // {
                //     zz.Perms.Add(r);
                // }

                if (rawPermList.Count == 0)
                    zz.Perms.Add(new PluginAttributes.CommandPermissionAttribute(ServerBasicPermissionLevel.Public));
                else
                    zz.Perms.AddRange(rawPermList);

                // if(zz) // DEBUG MESSAGE
                if(BattleBitExtenderMain.DebugMode)Tools.ConsoleLog($"Hey we got thissSSSSSSSSSSSSSSSSSSSSSSSSSSSS TYPE:{zz.GetType()} {zz.Perms.Count}");
                z.AddCommand(zz);
            }


            var log = 0;
            //Parse Event Types!
            foreach (var et in plug.EventTypes)
            {
                //Check Each type Methods:
                List<object> good = new();
                foreach (var m in et.GetMethods())
                {
                    PluginAttributes.PluginEventAttribute[] foundCustomAttr = (PluginAttributes.PluginEventAttribute[])m.GetCustomAttributes(typeof(PluginAttributes.PluginEventAttribute), false);
                    foreach (var singlefound in foundCustomAttr)
                    {
                        PluginMethodEventWrapper ee = PluginMethodEventWrapper.createInstance(singlefound.Type);
                        ee.Priority = singlefound.Priority; // ?? EventPriority.MEDIUM;
                        ee.fireEventFunc += async data =>
                        {
                            var z = m.Invoke(MMC, new List<object>() { data }.ToArray());
                            return z;
                        };
                        Main.EM.RegisterPluginEvents(ee, plug.InfoAttribute);
                    }
                }
            }

        if(BattleBitExtenderMain.DebugMode)    Tools.ConsoleLog(">>>>>>>>>>>>>>>>>>>>>>>>>3333333333333333333>>>>>>>".Color(ConsoleColor.Red) + plug.DatasaverAttributes.Count.ToString());
            foreach (var tds in plug.DatasaverAttributes)
            {
                var t = tds.Value.SaveType;

                var k = tds.Key;
                //SAVE THIS
                var v = tds.Value;


                var location = plug.Directory;
                var sp = Path.Combine(location, v.FileName + ".json");
                CustomPlayerData.CustomPlayerData_PluginData o;
                if (!File.Exists(sp))
                {
                    var obj = JsonConvert.DeserializeObject(@"", t, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        Converters = new List<JsonConverter>()
                        {
                            new GameModeMapDataEntry.GamemodesConvert()
                        }
                    });
                    o = (CustomPlayerData.CustomPlayerData_PluginData)Activator.CreateInstance(t, new object[] { });
                    var az = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        Converters = new List<JsonConverter>()
                        {
                            new GameModeMapDataEntry.GamemodesConvert()
                        }
                    });
                    File.WriteAllText(sp, az);
                }
                else
                {
                    try
                    {
                        // Tools.ConsoleLog("YOOoaosdoasdasdasdasdasdasdsa dasd asd asd ");
                        var jsonString = File.ReadAllText(sp);
                        // Tools.ConsoleLog(jsonString);
                        o = (CustomPlayerData.CustomPlayerData_PluginData)JsonConvert.DeserializeObject(jsonString, t, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            Converters = new List<JsonConverter>()
                            {
                                new GameModeMapDataEntry.GamemodesConvert()
                            }
                        });
                        // o = (CustomPlayerData.CustomPlayerData_PluginData)JsonConvert.DeserializeObject(jsonString,t);
                    }
                    catch (Exception e)
                    {
                        Tools.ConsoleLog("EXCEPTUON");
                        Tools.ConsoleLog(e);

                        var obj = JsonConvert.DeserializeObject(@"", t, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            Converters = new List<JsonConverter>()
                            {
                                new GameModeMapDataEntry.GamemodesConvert()
                            }
                        });
                        o = (CustomPlayerData.CustomPlayerData_PluginData)Activator.CreateInstance(t, new object[] { });
                        var az = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            Converters = new List<JsonConverter>()
                            {
                                new GameModeMapDataEntry.GamemodesConvert()
                            }
                        });
                        File.WriteAllText(sp, az);
                    }
                }


                // Tools.ConsoleLog($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {t.Name.ToString()}".Color(ConsoleColor.Red));
                // Tools.ConsoleLog($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> {obj == null}>> {o == null}".Color(ConsoleColor.Red));
                // Tools.ConsoleLog(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>".Color(ConsoleColor.Red)+obj.GetType().ToString());
                // plug.DatasaverAttributes
                DatasaverObject[tds.Key] = o;
                // Tools.ConsoleLog($"JUST AND {sp} FFFFFFFFFFFF" + JsonConvert.SerializeObject(o));
                // var weatherForecast = JsonConvert.DeserializeObject<CustomPlayerData>("")!;
            }
        }
    }
}