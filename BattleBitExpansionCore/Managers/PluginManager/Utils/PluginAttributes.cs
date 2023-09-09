using BattleBitExpansionCore.Managers.PluginManager;
using CyberTechBattleBit2.DataSaver;
using CyberTechBattleBit2.Events;

namespace CyberTechBattleBit2.Managers.PluginManager.Utils;

public class PluginAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginInfoAttribute : Attribute
    {
        public string Name;
        public string Author;
        public string Version;
        public string Description;
        public string URL;
        public string Copyright;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PluginCommandAttribute : Attribute
    {
        public bool ServerCommand { get; internal set; } = false;
        public string? CommandDescription { get; private set; }
        public string? CommandUsage { get; private set; }

        public PluginInfo? Plugin { get; set; } = null;

        public PluginCommandAttribute() : this(null)
        {
        }

        public PluginCommandAttribute(string commandDescription) : this(commandDescription, null)
        {
        }

        public PluginCommandAttribute(string commandDescription, string commandUsage)
        {
            // ServerCommand = false;
            CommandDescription = commandDescription;
            CommandUsage = commandUsage;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class ServerCommandAttribute : PluginCommandAttribute
    {
        public ServerCommandAttribute()
        {
            ServerCommand = true;
        }

        public ServerCommandAttribute(string commandDescription) : base(commandDescription)
        {
            ServerCommand = true;
        }

        public ServerCommandAttribute(string commandDescription, string commandUsage) : base(commandDescription, commandUsage)
        {
            ServerCommand = true;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PluginEventAttribute : Attribute
    {
        public EventTypes Type;
        public EventPriority Priority;

        public PluginEventAttribute(EventTypes t, EventPriority p = EventPriority.MEDIUM)
        {
            Type = t;
            Priority = p;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandPermissionAttribute : Attribute
    {
        public ServerBasicPermissionLevel PermissionLevel = ServerBasicPermissionLevel.None;
        public string[] PermissionStrings = new List<string>().ToArray();

        public CommandPermissionAttribute(ServerBasicPermissionLevel level)
        {
            PermissionLevel = level;
        }

        public CommandPermissionAttribute(string[] permissionString)
        {
            PermissionStrings = permissionString;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PluginDataSaverAttribute : Attribute
    {
        public Type SaveType;
        public bool Sharable = false;
        public string FileName;
        public string AccessName;


        public PluginDataSaverAttribute(Type saveType, string fileName, string accessName = null)
        {
            SaveType = saveType;
            FileName = fileName;
            AccessName = accessName == null ? fileName : accessName;
        }
        // public PluginDataSaverAttribute(EventTypes t){
        //
        //     SaveType = t;
        // }
    }
}