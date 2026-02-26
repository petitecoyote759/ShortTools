using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;



namespace ShortTools.General
{
    /// <summary>
    /// Debugger class to allow for more interesting looking console printing and error log saving. Has a functionality to write to files.
    /// </summary>
    public sealed class Debugger : IDisposable
    {
        // <<Public Variables>> //


        /// <summary>
        /// Getter and setter to set what the default addlog function does when there is no warning level input.
        /// </summary>
        public WarningLevel DefaultLevel { get => defaultLevel; set => defaultLevel = value; }




        // <<Private Variables>> //

        private Queue<Log> logs = new Queue<Log>();
        

        private WarningLevel defaultLevel = WarningLevel.Debug;
        
        private readonly DebuggerFlag flags = DebuggerFlag.None;
        private readonly string name = "Debugger";
        private int logDeleteNum = -1;
        private int logCleanNum = -1;
        private string fileName = "";

        private StreamWriter? logWriter = null;




        // <<Constructors>> //

        /// <summary>
        /// Constructor for the debugger with a flag input.
        /// </summary>
        /// <param name="flags"></param>
        public Debugger([NotNull] params DebuggerFlag[] flags)
        {
            this.flags = CoalesceFlags(flags);
            Setup();
        }
        /// <summary>
        /// Constructor for the debugger with a flag input.
        /// </summary>
        /// <param name="flags"></param>
        public Debugger(DebuggerFlag flags)
        {
            this.flags = flags;
            Setup();
        }
        /// <summary>
        /// Constructor for the debugger with a flag input. 
        /// </summary>
        /// <param name="name">The name of the file that the debugger saves to.</param>
        /// <param name="flags"></param>
        public Debugger(string name, [NotNull] params DebuggerFlag[] flags)
        {
            this.name = name; this.flags = CoalesceFlags(flags);
            Setup();
        }
        /// <summary>
        /// Constructor for the debugger with a flag input.
        /// </summary>
        /// <param name="name">The name of the file that the debugger saves to.</param>
        /// <param name="flags"></param>
        public Debugger(string name, DebuggerFlag flags)
        {
            this.flags = flags; this.name = name;
            Setup();
        }



        // <<General Functions>> //

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static DebuggerFlag CoalesceFlags(params DebuggerFlag[] flags)
        {
            DebuggerFlag flag = DebuggerFlag.None;
            foreach (DebuggerFlag IFlag in flags)
            {
                flag |= IFlag;
            }
            return flag;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Setup()
        {
            fileName = $"[{name}] {DateTimeOffset.Now.ToString("dd.MM.yy - HH.mm.ss", CultureInfo.CurrentCulture)}.log";

            DoAddLog += DefaultAddLog;

            if (flags.HasFlag(DebuggerFlag.PrintLogs)) { DoAddLog += DisplayLog; }

            if (flags.HasFlag(DebuggerFlag.DestroyLogsAt10)) { logDeleteNum = 10; 
                if (!flags.HasFlag(DebuggerFlag.WriteLogsToFile)) { DoAddLog += DestroyLogs; } }
            else if (flags.HasFlag(DebuggerFlag.DestroyLogsAt20)) { logDeleteNum = 20; 
                if (!flags.HasFlag(DebuggerFlag.WriteLogsToFile)) { DoAddLog += DestroyLogs; } }
            else if (flags.HasFlag(DebuggerFlag.DestroyLogsAt50)) { logDeleteNum = 50; 
                if (!flags.HasFlag(DebuggerFlag.WriteLogsToFile)) { DoAddLog += DestroyLogs; } }
            else if (flags.HasFlag(DebuggerFlag.DestroyLogsAt100)) { logDeleteNum = 100; 
                if (!flags.HasFlag(DebuggerFlag.WriteLogsToFile)) { DoAddLog += DestroyLogs; } }

            logCleanNum = 50; // default setting
            if (flags.HasFlag(DebuggerFlag.CleanupLogsAt5)) { logCleanNum = 5; }
            if (flags.HasFlag(DebuggerFlag.CleanupLogsAt10)) { logCleanNum = 10; }
            if (flags.HasFlag(DebuggerFlag.CleanupLogsAt20)) { logCleanNum = 20; }
            if (flags.HasFlag(DebuggerFlag.CleanupLogsAt50)) { logCleanNum = 50; }


            if (flags.HasFlag(DebuggerFlag.WriteLogsToFile)) 
            { 
                if (logDeleteNum == -1) { logDeleteNum = 20; }
                DoAddLog += Write;


                // <<Auto Writing>> //

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CrashHandler);

                if (!Directory.Exists($"Logs\\{name}")) { _ = Directory.CreateDirectory($"Logs\\{name}"); }
                if (!File.Exists($"Logs\\{fileName}")) { File.Create($"Logs\\{fileName}").Dispose(); }
                logWriter = new StreamWriter($"Logs\\{fileName}");
            }
        }


        /// <summary>
        /// Adds a log to the debugger with the text inputed, and the DefaultLevel variable as the warning level.
        /// </summary>
        /// <param name="data">Text to be added to the log.</param>
        public void AddLog([NotNull] string data)
        {
            AddLog(data, defaultLevel);
        }
        /// <summary>
        /// Adds a log to the debugger with the text inputed, and the DefaultLevel variable as the warning level.
        /// </summary>
        /// <param name="data">Text to be added to the log.</param>
        /// <param name="level">The warning level of the log.</param>
        public void AddLog([NotNull] string data, WarningLevel level)
        {
            DoAddLog(data, level);
        }
        private Action<string, WarningLevel> DoAddLog;
        private void DefaultAddLog(string inp, WarningLevel level)
        {
            logs.Enqueue(new Log(inp, level));
        }
        private void DisplayLog(string inp, WarningLevel level)
        {
            ShortTools.General.Prints.Print(new Log(inp, level).ToString(), WarnToConsColour[level]);
        }
        private void DestroyLogs(string inp, WarningLevel level)
        {
            while (logs.Count > logDeleteNum) { _ = logs.Dequeue(); }
            if (logCleanNum != -1) 
            {
                CleanLogs();
            }
        }
        private void CleanLogs()
        {
            if (!Directory.Exists($"Logs\\{name}")) { return; }

            string[] files = Directory.GetFiles($"Logs\\{name}");

            if (files.Length <= logCleanNum) { return; }

            string[] sortedFiles = (
                from file in files
                orderby File.GetCreationTime(file) ascending
                select file).ToArray();

            for (int i = 0; i < files.Length - logCleanNum; i++)
            {
                if (File.GetCreationTime(sortedFiles[i]) == File.GetCreationTime(fileName)) { continue; }
                File.Delete(sortedFiles[i]);
            }
        }




        // <<Writing>> //
        private void WriteAll()
        {
            while (logs.Count > 0) { WriteLog(logs.Dequeue()); }
        }
        private void Write(string inp = "", WarningLevel level = WarningLevel.Debug)
        {
            WriteLog(logs.Dequeue());
        }
        private void WriteLog(Log log)
        {
            logWriter?.WriteLine(log.ToString());
        }
        private void CrashHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception exception = (Exception)args.ExceptionObject;
            AddLog($"Exception raised : {exception}", WarningLevel.CriticalError);
            WriteAll();
            Dispose();
        }










        bool disposed = false;
        /// <summary>
        /// Saves the logs to the file, and safely closes the writing stream.
        /// </summary>
        public void Dispose()
        {
            if (disposed) { return; }
            disposed = true;

            if (flags.HasFlag(DebuggerFlag.WriteLogsToFile))
            {
                AddLog("Disposing...", WarningLevel.Debug);
                WriteAll();
                logWriter?.Close();
                logWriter?.Dispose();
                logWriter = null;
                logs = new Queue<Log>();
            }
            CleanLogs();
        }
#pragma warning disable CA1063
        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose();
        }
#pragma warning restore CA1063



        private readonly Dictionary<WarningLevel, ConsoleColor> WarnToConsColour = new Dictionary<WarningLevel, ConsoleColor>()
        {
            { WarningLevel.Debug, ConsoleColor.Cyan },
            { WarningLevel.Warning, ConsoleColor.Yellow },
            { WarningLevel.Error, ConsoleColor.Red },
            { WarningLevel.CriticalError, ConsoleColor.DarkRed },
            { WarningLevel.Info, ConsoleColor.White },
        };

















        internal static void Main()
        {
            using Debugger debugger = new Debugger("Test", DebuggerFlag.ShortDefault);

            debugger.AddLog("Test", WarningLevel.Debug);
            debugger.AddLog("Test", WarningLevel.Info);
            debugger.AddLog("Test", WarningLevel.Warning);
            debugger.AddLog("Test", WarningLevel.Error);
            debugger.AddLog("Test", WarningLevel.CriticalError);

            int value = int.MaxValue;
            checked
            {
                Console.WriteLine(value + 16);
            }


        }
    }












    internal struct Log
    {
        public string info;
        public WarningLevel warningLevel;
        private readonly DateTimeOffset now;

        public Log(string info, WarningLevel warningLevel)
        {
            this.info = info;
            this.warningLevel = warningLevel;
            now = DateTimeOffset.Now;
        }


        public static string ToString(Log log) => log.ToString();

        public readonly override string ToString()
        {
            return $"{("[" + warningLevel.ToString() + "]").PadRight(15, ' ')} {now.ToString("HH:mm:ss.fff", CultureInfo.CurrentCulture)} - {info}";
        }
    }









    /// <summary>
    /// The warning level of a log.
    /// </summary>
    public enum WarningLevel : int
    { 
        /// <summary>
        /// 
        /// </summary>
        Debug,
        /// <summary>
        /// 
        /// </summary>
        Info,
        /// <summary>
        /// 
        /// </summary>
        Warning,
        /// <summary>
        /// 
        /// </summary>
        Error,
        /// <summary>
        /// An error that could not be handled and is forcing a shutdown.
        /// </summary>
        CriticalError,
    }

    /// <summary>
    /// Flags for the ShortTools.Debugger object.
    /// </summary>
    [Flags]
#pragma warning disable CA1711
    public enum DebuggerFlag : int
#pragma warning restore CA1711
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// Displays the logs to the console.
        /// </summary>
        PrintLogs,
        /// <summary>
        /// defaults to 20
        /// </summary>
        WriteLogsToFile,
        /// <summary>
        /// Makes the log list be capped at 10 logs, reducing ram usage.
        /// </summary>
        DestroyLogsAt10 = 4,
        /// <summary>
        /// Makes the log list be capped at 20 logs, reducing ram usage.
        /// </summary>
        DestroyLogsAt20 = 8,
        /// <summary>
        /// Makes the log list be capped at 50 logs.
        /// </summary>
        DestroyLogsAt50 = 16,
        /// <summary>
        /// Makes the log list be capped at 100 logs.
        /// </summary>
        DestroyLogsAt100 = 32,

        /// <summary>
        /// Makes the saved log files only be the last 5 logs.
        /// </summary>
        CleanupLogsAt5 = 64,
        /// <summary>
        /// Makes the saved log files only be the last 10 logs.
        /// </summary>
        CleanupLogsAt10 = 128,
        /// <summary>
        /// Makes the saved log files only be the last 20 logs.
        /// </summary>
        CleanupLogsAt20 = 256,
        /// <summary>
        /// Makes the saved log files only be the last 50 logs.
        /// </summary>
        CleanupLogsAt50 = 512,



        /// <summary>
        /// The general settings advised, including printing of logs and writing of logs to files, at a max log amount of 20 in ram and 10 files stored.
        /// </summary>
        ShortDefault = PrintLogs | WriteLogsToFile | DestroyLogsAt20 | CleanupLogsAt10,
    }
}
