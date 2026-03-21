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

        /// <summary>
        /// Getter and setter for the minimum display level when the debugger is printing to console.
        /// </summary>
        public WarningLevel DisplayLevel { get => displayLevel; set => displayLevel = value; }



        // <<Private Variables>> //

        private Queue<Log> logs;
        private readonly Queue<Log> debugLogs;
        

        private WarningLevel defaultLevel;
        private WarningLevel displayLevel;
        
        private readonly HashSet<DebuggerFlag> flags;
        private readonly string name = "Debugger";
        private const int logCleanNum = 10;
        private const int debugLogLength = 20;
        private string fileName = "";
        private bool displayThread = false;

        private StreamWriter? logWriter = null;




        // <<Constructors>> //

        /// <summary>
        /// Constructor for the debugger with a flag input.
        /// </summary>
        /// <param name="name">The name of the debugger, used in the file name and printing.</param>
        /// <param name="displayLevel">The lowest warning level that will be displayed in the console when printing</param>
        /// <param name="flags"></param>
        public Debugger(string name = "Debugger", WarningLevel displayLevel = WarningLevel.Info, [NotNull] params DebuggerFlag[] flags)
        {
            this.name = name;
            this.displayLevel = displayLevel;

            this.flags = new HashSet<DebuggerFlag>(flags);
            this.logs = new Queue<Log>();
            this.debugLogs = new Queue<Log>();


            DoAddLog = DefaultAddLog;
            foreach (DebuggerFlag flag in flags)
            {
                DoFlagAction(flag);
            }

            CleanLogs();
        }



        // <<General Functions>> //
        private void DoFlagAction(DebuggerFlag flag)
        {
            Action FlagAction = flag switch
            {
                DebuggerFlag.PrintLogs => () => { DoAddLog += DisplayLog; },
                DebuggerFlag.WriteLogsToFile => SetupFileWriting,
                DebuggerFlag.DisplayThread => () => { displayThread = true; },

                _ => () => { }
            };
            FlagAction();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetupFileWriting()
        {
            fileName = Path.Combine($"{name}",$"{DateTimeOffset.Now.ToString("yyyy.MM.dd - HH.mm.ss", CultureInfo.InvariantCulture)}.log");

            // <<Crash Handling>> //
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CrashHandler);

            // <<Stream Creation>> //
            if (!Directory.Exists(Path.Combine($"Logs",$"{name}"))) { _ = Directory.CreateDirectory(Path.Combine($"Logs",$"{name}")); }
            if (!File.Exists(Path.Combine($"Logs", $"{fileName}"))) { File.Create(Path.Combine($"Logs", $"{fileName}")).Dispose(); }
            logWriter = new StreamWriter(Path.Combine($"Logs", $"{fileName}"));
        }


        /// <summary>
        /// Adds a log to the debugger with the text inputed, and the DefaultLevel variable as the warning level.
        /// </summary>
        /// <param name="data">Text to be added to the log.</param>
        public void AddLog(string data)
        {
            AddLog(data, defaultLevel);
        }
        /// <summary>
        /// Adds a log to the debugger with the text inputed, and the DefaultLevel variable as the warning level.
        /// </summary>
        /// <param name="data">Text to be added to the log.</param>
        /// <param name="level">The warning level of the log.</param>
        public void AddLog(string data, WarningLevel level)
        {
            DoAddLog(data, level);
        }
        private Action<string, WarningLevel> DoAddLog;
        private void DefaultAddLog(string inp, WarningLevel level)
        {
            if (level == WarningLevel.Debug)
            {
                if (debugLogs.Count >= debugLogLength) { _ = debugLogs.Dequeue(); }
                debugLogs.Enqueue(new Log(inp, level));
            }
            else
            {
                logs.Enqueue(new Log(inp, level));
            }
        }
        private void DisplayLog(string inp, WarningLevel level)
        {
            if (level < displayLevel) { return; }
            ShortTools.General.Prints.Print(new Log(inp, level).ToString(displayThread), WarnToConsColour[level]);
        }
        private void CleanLogs()
        {
            if (!Directory.Exists(Path.Combine($"Logs", $"{name}"))) { return; }

            string[] files = Directory.GetFiles(Path.Combine($"Logs", $"{name}"));

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
            while (logs.Count > 0) 
            {
                Log log;
                if (debugLogs.Count == 0) { log = logs.Dequeue(); }
                else if (debugLogs.Peek().logTime < logs.Peek().logTime)
                {
                    // debug log is next
                    log = debugLogs.Dequeue();
                }
                else { log = logs.Dequeue(); }
                WriteLog(log);
            }

            while (debugLogs.Count > 0)
            {
                WriteLog(debugLogs.Dequeue());
            }
        }
        private void WriteLog(Log log)
        {
            logWriter?.WriteLine(log.ToString());
        }
        private void CrashHandler(object sender, UnhandledExceptionEventArgs args)
        {
            if (disposed) { return; }
            Exception exception = (Exception)args.ExceptionObject;
            AddLog($"Exception raised : {exception}", WarningLevel.CriticalError);
            Dispose(true);
        }










        bool disposed = false;
        /// <summary>
        /// Saves the logs to the file, and safely closes the writing stream.
        /// </summary>
        public void Dispose(bool disposing)
        {
            if (disposed) { return; }
            disposed = true;

            if (flags.Contains(DebuggerFlag.WriteLogsToFile))
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
            Dispose(true);
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
            //Thread.CurrentThread.Name = "Main Thread";

            using Debugger debugger = new Debugger(
                name: "Test", displayLevel: WarningLevel.Debug, 
                DebuggerFlag.PrintLogs, DebuggerFlag.WriteLogsToFile, DebuggerFlag.DisplayThread);

            debugger.AddLog("Test", WarningLevel.Debug);
            debugger.AddLog("Test", WarningLevel.Info);
            debugger.AddLog("Test", WarningLevel.Warning);
            debugger.AddLog("Test", WarningLevel.Error);
            debugger.AddLog("Test", WarningLevel.CriticalError);

            for (int i = 0; i < 20; i++)
            {
                debugger.AddLog($"Debug Test at time {DateTimeOffset.Now.ToUnixTimeMilliseconds()}", WarningLevel.Debug);
            }

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
        public readonly DateTimeOffset logTime;

        public Log(string info, WarningLevel warningLevel)
        {
            this.info = info;
            this.warningLevel = warningLevel;
            logTime = DateTimeOffset.Now;
        }


        public static string ToString(Log log) => log.ToString();

        public readonly override string ToString() => ToString(false);
        public readonly string ToString(bool writeThread)
        {
            if (writeThread && !string.IsNullOrEmpty(Thread.CurrentThread.Name))
            {
                return $"{("[" + warningLevel.ToString() + "]").PadRight(15, ' ')} {logTime.ToString("HH:mm:ss.fff", CultureInfo.CurrentCulture)} [{Thread.CurrentThread.Name}] - {info}";
            }
            else
            {
                return $"{("[" + warningLevel.ToString() + "]").PadRight(15, ' ')} {logTime.ToString("HH:mm:ss.fff", CultureInfo.CurrentCulture)} - {info}";
            }
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
        /// Writes the logs to a file in the 'Logs' folder
        /// </summary>
        WriteLogsToFile,
        /// <summary>
        /// Displays the thread name in the logs, will not if there is no thread name
        /// </summary>
        DisplayThread,
    }
}
