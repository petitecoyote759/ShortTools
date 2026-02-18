using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ShortTools.General.Prints;


namespace ShortTools.General
{
    /// <summary>
    /// A class designed for settings, where you inherit this class to add a function to save the data within the class to a file.
    /// <code>
    /// public class Settings : ShortTools.General.Settings
    /// {
    ///     public string Test { get; set; } = "test123"; 
    /// }
    /// </code>
    /// Will be saved on a call of
    /// <code>
    /// Settings settings = new Settings();
    /// settings.SaveSettings("Settings.ini")
    /// </code>
    /// You can also load these files to a class using either the constructor or the LoadSettings function
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Searches for a word, with either a non word character or the start of the line before it, then an equals, and then either an int, float, string, or char,
        /// in the format: <br/>
        /// <example>
        /// <![CDATA[varName = 3               <- for ints   ]]>   <br/>   
        /// <![CDATA[varName2 = 3.14f          <- for floats ]]>   <br/>
        /// <![CDATA[varName3 = "testing123"   <- for strings]]>   <br/>
        /// <![CDATA[varName4 = 'a'            <- for chars  ]]>   <br/>
        /// </example>
        /// </summary>
        private static readonly Regex loadRegex = new Regex(@"(?:^|\W)([a-zA-Z]\w*) *= *((?:\d+\.\d+[fFdD]?)|(?:\d+)|(?:""\w*"")|(?:'\w'))[^.\W]*", RegexOptions.Compiled | RegexOptions.Multiline);
        /// <summary>
        /// Automatically called by the constructor if a path is passed in.
        /// </summary>
        /// <param name="path">The path of the settings file.</param>
        /// <param name="obj">The object for the settings to be loaded into.</param>
        public static void LoadSettings<T>(string path, T obj)
        {
            if (obj is null) { return; }

            if (!File.Exists(path)) { throw new FileNotFoundException($"Could not find the file designated at {path}"); }

            string data = File.ReadAllText(path);
            
            MatchCollection group = loadRegex.Matches(data);
            



            foreach (Match match in group.Cast<Match>()) // Grrr
            {
                PropertyInfo? info = obj.GetType().GetProperty(match.Groups[1].ToString()); // Gets the property with the name given by the settings file

                if (info is null) { continue; } // if there was no property with that name, go to the next one.


                if (MatchIsInt(match, out long result))
                {
                    if (info.PropertyType == typeof(int))
                    {
                        info.SetValue(obj, (int)result);
                    }
                    else if (info.PropertyType == typeof(long))
                    {
                        info.SetValue(obj, result);
                    }
                }

                else if (MatchIsFloat(match, out float fresult))
                {
                    if (IsFloatType(info)) // if the property is a float, so it matches the float type of the match.
                    {
                        info.SetValue(obj, fresult);
                    }
                }

                else if (MatchIsString(match))
                {
                    if (IsStringType(info))
                    {
                        info.SetValue(obj, match.Groups[2].ToString()[1..^1]);
                    }
                }

                else if (MatchIsChar(match))
                {
                    if (IsCharType(info))
                    {
                        info.SetValue(obj, match.Groups[2].ToString()[1]);
                    }
                }
            }
        }





        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool MatchIsInt(Match match, out long result)
        {
            return long.TryParse(match.Groups[2].ToString(), out result);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool IsIntType(PropertyInfo info)
        {
            return typesToOthers["Numbers"].Contains(info.PropertyType);
        }


        private const string floatEnders = "fFdD";
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool MatchIsFloat(Match match, out float result)
        {
            string num = match.Groups[2].ToString();
            if (floatEnders.Contains(num.Last(), StringComparison.InvariantCulture)) { num = num[..^1]; }
            return float.TryParse(num, out result);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool IsFloatType(PropertyInfo info)
        {
            return typesToOthers["Decimals"].Contains(info.PropertyType);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool MatchIsString(Match match)
        {
            return match.Groups[2].ToString()[0] == '\"';
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool IsStringType(PropertyInfo info)
        {
            return info.PropertyType == typeof(string);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool MatchIsChar(Match match)
        {
            return match.Groups[2].ToString()[0] == '\'';
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool IsCharType(PropertyInfo info)
        {
            return info.PropertyType == typeof(char);
        }







        /// <summary>
        /// Saves the current settings values to the path given, overrides values if they are present, and if they are not it appends them to the bottom.
        /// </summary>
        /// <param name="path">The path of where to save the settings.</param>
        /// <param name="obj">The object for the settings to be saved to.</param>
        public static void SaveSettings<T>([NotNull] string path, T obj) 
        {
            if (obj is null) { return; }

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }




            List<string> data = File.ReadAllLines(path).ToList();

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i][..prop.Name.Length] != prop.Name) { continue; }

                    data[i] = GetDisplayString(prop, obj);

                    goto Next;
                }

                data.Add(GetDisplayString(prop, obj));

                Next:;
            }

            File.WriteAllLines(path, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static string GetDisplayString<T>(PropertyInfo prop, T obj)
        {
            if (IsStringType(prop))
            {
                return $"{prop.Name} = \"{prop.GetValue(obj)}\";";
            }
            else if (IsCharType(prop))
            {
                return $"{prop.Name} = \'{prop.GetValue(obj)}\';";
            }
            else if (IsFloatType(prop))
            {
                return $"{prop.Name} = {prop.GetValue(obj)}f;";
            }
            else if (IsIntType(prop))
            {
                return $"{prop.Name} = {prop.GetValue(obj)};";
            }



            return $"{prop.Name} = {prop.GetValue(obj)};";
        }






        internal static Dictionary<string, Type[]> typesToOthers = new Dictionary<string, Type[]>()
        {
            { "Numbers", new Type[] { typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong) } },
            { "Decimals", new Type[] { typeof(float), typeof(double) } },
        };
    }




    #region Tests
#pragma warning disable



    internal class TestSettings
    {
        public TestSettings(string path = "") { Settings.LoadSettings(path, this); }

        public string Test { get; set; } = "";
        public int AWd1 { get; set; } = 0;
        public char Test3 { get; set; } = 'L';
        public float Test2 { get; set; } = 1.34f;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SaveSettings(string path) => Settings.SaveSettings(path, this); 


        private static void Main(string[] args)
        {
            TestSettings settings = new TestSettings();
            Settings.LoadSettings("Settings.ini", settings);
        }



        public override string ToString()
        {
            return Misc.GetDisplayString(this);
        }
    }

    #endregion Tests
}
