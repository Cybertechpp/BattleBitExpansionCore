using System.Drawing;
using ANSIConsole;
using CyberTechBattleBit2.Managers.PluginManager.Utils;

namespace CyberTechBattleBit2;

public static class LogTools
{
    // public class PluginLogHelper<T> where T : PluginBase
    // {
    //     public void LogToConsole(object str)
    //     {
    //         var pt = $"[{typeof(T).Name}]".Color(Color.Orange).Background(ConsoleColor.White);
    //         Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag + Tools.TextTemplates.SecondLevelTags.PluginManager + pt + " > " + str);
    //     }
    // }


    public class ModuleLogHelper<TT> : LogHelperBase<TT>
    {
        public ModuleLogHelper(ConsoleColor textColor = ConsoleColor.Yellow, ConsoleColor backgroundColor = ConsoleColor.Green, string moduleName = null) : base(ConsoleColorToColor(textColor), ConsoleColorToColor(backgroundColor), moduleName)
        {
        }

        public ModuleLogHelper(Color? textColor = null, Color? backgroundColor = null, string moduleName = null) : base(textColor, backgroundColor, moduleName)
        {
        }

        protected ModuleLogHelper() : base(null, null, null)
        {
            // throw new NotImplementedException();
        }
    }

    public class LogHelperBase<T>
    {
        public string Name;

        public static Color? ConsoleColorToColor(ConsoleColor? color)
        {
            if (color == null) return null;
            return new Color?(ANSIString.FromConsoleColor((ConsoleColor)color));
        }

        // public string ModuleName;
        public Color ReturnTextColor = Color.Orange;
        public Color TextColor = Color.White;
        public Color BackgroundColor = Color.Purple;

        public LogHelperBase(Color? textColor = null, ConsoleColor backgroundColor = ConsoleColor.White, string moduleName = null) : this(textColor, ConsoleColorToColor(backgroundColor), moduleName)
        {
        }

        public LogHelperBase(ConsoleColor textColor = ConsoleColor.Yellow, ConsoleColor backgroundColor = ConsoleColor.Green, string moduleName = null) : this(ConsoleColorToColor(textColor), ConsoleColorToColor(backgroundColor), moduleName)
        {
        }

        public LogHelperBase(ConsoleColor textColor = ConsoleColor.Yellow, Color? backgroundColor = null, string moduleName = null) : this(ConsoleColorToColor(textColor), backgroundColor, moduleName)
        {
        }

        protected LogHelperBase(Color? textColor = null, Color? backgroundColor = null, string moduleName = null)
        {
            if (backgroundColor != null) BackgroundColor = (Color)backgroundColor;
            if (textColor != null) TextColor = (Color)textColor;
            if (moduleName == null)
                Name = typeof(T).Name;
            else Name = moduleName;

            // new LogHelperBase<T>
            Init();
        }

        public void Init()
        {
            Log = new ModuleLogHelperLog(this);
        }


        public ModuleLogHelperLog Log;


        public class ModuleLogHelperLog
        {
            private static LogHelperBase<T> H;

            public ModuleLogHelperLog(LogHelperBase<T> h)
            {
                H = h;
            }


            public void Info(object str)
            {
                var pt = $"[{H.Name}]".Color(H.TextColor).Background(H.BackgroundColor);
                var prefix = Tools.TextTemplates.FirstLevelTags.BBECTag.ToString() + pt + Tools.TextTemplates.ThridLevelTags.LogInfoLevel;
                var format = prefix + (" > " + str).ToString().Color(ConsoleColor.Black).Background(Color.CornflowerBlue);
                Tools.ConsoleLog(format.Background(Color.CornflowerBlue));
                // Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag.ToString() + pt + Tools.TextTemplates.LogInfoLevel + " > " + str);
            }
     public void Warn(object str)
            {
                var pt = $"[{H.Name}]".Color(H.TextColor).Background(H.BackgroundColor);
                var prefix = Tools.TextTemplates.FirstLevelTags.BBECTag.ToString() + pt + Tools.TextTemplates.ThridLevelTags.WarnInfoLevel;
                var format = prefix + (" > " + str).ToString().Color(ConsoleColor.Black).Background(Color.Orange);
                Tools.ConsoleLog(format.Background(Color.Orange));
                // Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag.ToString() + pt + Tools.TextTemplates.LogInfoLevel + " > " + str);
            }


            public void Success(object str)
            {
                var pt = $"[{H.Name}]".Color(H.TextColor).Background(H.BackgroundColor);
                var prefix = Tools.TextTemplates.FirstLevelTags.BBECTag.ToString() + pt + Tools.TextTemplates.ThridLevelTags.SuccessLogLevel;
                var format = prefix + (" > " + str).ToString().Color(ConsoleColor.White).Background(Color.Green);
                Tools.ConsoleLog(format);
            }
        }

        public void LogToConsole(object str)
        {
            var pt = $"[{Name}]".Color(TextColor).Background(ConsoleColor.White);
            Tools.ConsoleLog(Tools.TextTemplates.FirstLevelTags.BBECTag.ToString() + pt + " > " + str.ToString().Background(ConsoleColor.DarkBlue));
        }

        public void LogRawToConsole(object str)
        {
            Tools.ConsoleLog(str);
        }
    }
}