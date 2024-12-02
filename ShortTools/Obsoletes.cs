using ShortTools.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortTools.General
{





    /// <summary>
    /// An unused class to make anyone using the old Short_Tools.General.Print see the new Prints class.
    /// </summary>
    [Obsolete("This item has been depreciated, please use Prints.Print instead, or statically import the ShortTools.General.Prints class", true,
        DiagnosticId = $"{WarningCodes.Depreciated}", UrlFormat = WarningCodes.URL)]
    public static class Print { }




    /// <summary>
    /// An unused class to make anyone using the old Short_Tools.General.GetTimeString to see a better way of doing it.
    /// <code>
    /// DateTimeOffset.Now.ToString("HH:mm:ss");
    /// </code>
    /// </summary>
    [Obsolete("This item has been depreciated as there is an easier way to do it. Just do DateTimeOffset.Now.ToString(\"HH:mm:ss\");", true,
        DiagnosticId = $"{WarningCodes.Depreciated}", UrlFormat = WarningCodes.URL)]
    public static class GetTimeString { }




    /// <summary>
    /// An unused class to make anyone using the old Short_Tools.General.GetDateString to see a better way of doing it.
    /// <code>
    /// DateTimeOffset.Now.ToString("yy:MM:dd");
    /// </code>
    /// </summary>
    [Obsolete("This item has been depreciated as there is an easier way to do it. Just do DateTimeOffset.Now.ToString(\"yy:MM:dd\");", true,
        DiagnosticId = $"{WarningCodes.Depreciated}", UrlFormat = WarningCodes.URL)]
    public static class GetDateString { }




    /// <summary>
    /// An unused class replacing GetPath as it is not here anymore.
    /// <code>
    /// DateTimeOffset.Now.ToString("yy:MM:dd");
    /// </code>
    /// </summary>
    [Obsolete("This item has been removed.", true,
        DiagnosticId = $"{WarningCodes.Depreciated}", UrlFormat = WarningCodes.URL)]
    public static class GetPath { }




    /// <summary>
    /// An unused class replacing getMousePos as it has been moved to ShortTools.SDL as GetMousePos.
    /// <code>
    /// DateTimeOffset.Now.ToString("yy:MM:dd");
    /// </code>
    /// </summary>
    [Obsolete("This item has been moved to ShortTools.SDL as GetMousePos.", true,
        DiagnosticId = $"{WarningCodes.Depreciated}", UrlFormat = WarningCodes.URL)]
    public static class getMousePos { }
}




namespace ShortTools
{
    /// <summary>
    /// An unused class replacing getMousePos as it has been moved to ShortTools.SDL as GetMousePos.
    /// <code>
    /// DateTimeOffset.Now.ToString("yy:MM:dd");
    /// </code>
    /// </summary>
    [Obsolete("This item has been moved to ShortTools.General.Debugger.", true,
        DiagnosticId = $"{WarningCodes.Depreciated}", UrlFormat = WarningCodes.URL)]
    public static class ShortDebugger { }
}