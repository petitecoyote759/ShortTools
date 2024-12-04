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
    public abstract class Settings
    {
        /// <inheritdoc cref="Settings"/>
        protected Settings(string path = "")
        {
            if (path is null) { return; }
            if (path.Length != 0) { LoadSettings(path); }
        }



        /// <summary>
        /// Searches for a word, with either a non word character or the start of the line before it, then an equals, and then either an int, float, string, or char,
        /// in the format:
        /// <code>
        /// varName = 3               // for ints
        /// varName2 = 3.14f          // for floats
        /// varName3 = "testing123"   // for strings
        /// varName4 = 'a'            // for chars
        /// </code>
        /// </summary>
        private readonly Regex loadRegex = new Regex(@"(?:^|\W)([a-zA-Z]\w*) *= *((?:\d+\.\d+[fFdD]?)|(?:\d+)|(?:""\w*"")|(?:'\w'))[^.\W]*", RegexOptions.Compiled | RegexOptions.Multiline);
        /// <summary>
        /// Automatically called by the constructor if a path is passed in.
        /// </summary>
        public void LoadSettings(string path)
        {
            if (!File.Exists(path)) { throw new FileNotFoundException($"Could not find the file designated at {path}"); }

            string data = File.ReadAllText(path);
            
            MatchCollection group = loadRegex.Matches(data);
            



            foreach (Match match in group.Cast<Match>()) // Grrr
            {
                PropertyInfo? info = GetType().GetProperty(match.Groups[1].ToString());

                if (info is null) { continue; }


                if (MatchIsInt(match, out long result))
                {
                    if (info.PropertyType == typeof(int))
                    {
                        info.SetValue(this, (int)result);
                    }
                    else if (info.PropertyType == typeof(long))
                    {
                        info.SetValue(this, result);
                    }
                }

                else if (MatchIsFloat(match, out float fresult))
                {
                    if (IsFloatType(info))
                    {
                        info.SetValue(this, fresult);
                    }
                }

                else if (MatchIsString(match))
                {
                    if (IsStringType(info))
                    {
                        info.SetValue(this, match.Groups[2].ToString()[1..^1]);
                    }
                }

                else if (MatchIsChar(match))
                {
                    if (IsCharType(info))
                    {
                        info.SetValue(this, match.Groups[2].ToString()[1]);
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
        private bool IsIntType(PropertyInfo info)
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
        private bool IsFloatType(PropertyInfo info)
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
        public void SaveSettings([NotNull] string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }




            List<string> data = File.ReadAllLines(path).ToList();

            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i][..prop.Name.Length] != prop.Name) { continue; }

                    data[i] = GetDisplayString(prop);

                    goto Next;
                }

                data.Add(GetDisplayString(prop));

                Next:;
            }

            File.WriteAllLines(path, data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private string GetDisplayString(PropertyInfo prop)
        {
            if (IsStringType(prop))
            {
                return $"{prop.Name} = \"{prop.GetValue(this)}\";";
            }
            else if (IsCharType(prop))
            {
                return $"{prop.Name} = \'{prop.GetValue(this)}\';";
            }
            else if (IsFloatType(prop))
            {
                return $"{prop.Name} = {prop.GetValue(this)}f;";
            }
            else if (IsIntType(prop))
            {
                return $"{prop.Name} = {prop.GetValue(this)};";
            }



            return $"{prop.Name} = {prop.GetValue(this)};";
        }






        internal Dictionary<string, Type[]> typesToOthers = new Dictionary<string, Type[]>()
        {
            { "Numbers", new Type[] { typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong) } },
            { "Decimals", new Type[] { typeof(float), typeof(double) } },
        };
    }




    #region Tests
#pragma warning disable


    internal class TestSettings : Settings
    {
        public TestSettings(string path = "") : base(path) { }

        public string Test { get; set; } = "";
        public int AWd1 { get; set; } = 0;
        public char Test3 { get; set; } = 'L';
        public float Test2 { get; set; } = 1.34f;



        private static void Main(string[] args)
        {
            
            
            TestSettings settings = new TestSettings("Settings.ini");

            Print(settings.ToString());
            
            
            settings.SaveSettings("Settings.ini");




        }



        public override string ToString()
        {
            return Misc.GetDisplayString(this);
        }
    }

    #endregion Tests
}
