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
    public abstract class Settings
    {
        protected Settings(string path = "")
        {
            if (path is null) { return; }
            if (path.Length != 0) { LoadSettings(path); }
        }




        private readonly Regex loadRegex = new Regex(@"(?:^|\W)([a-zA-Z]\w*) *= *((?:\d+\.\d+[fFdD]?)|(?:\d+)|(?:""\w*"")|(?:'\w'))[^.\W]*", RegexOptions.Compiled | RegexOptions.Multiline);
        /// <summary>
        /// Automatically called by the constructor if a path is passed in.
        /// </summary>
        /// <returns>True if the settings loaded correctly</returns>
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


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool MatchIsFloat(Match match, out float result)
        {
            return float.TryParse(match.Groups[2].ToString(), out result);
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









        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static bool FindDataInString([NotNull] string data, [NotNull] string item, out int index)
        {
            index = -1;

            for (int i = 0; i < data.Length - item.Length + 1; i++)
            {
                for (int j = 0; j < item.Length; j++)
                {
                    if (data[i + j] != item[j]) { goto End; }
                }
                index = i;
                return true;

                End:;
            }

            return false;
        }



        public void SaveSettings([NotNull] string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }




            List<string> data = File.ReadAllLines(path).ToList();

            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                for (int i = 0; i < data.Count - prop.Name.Length + 1; i++)
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
            { "Numbers", [typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong)] },
            { "Decimals", [typeof(float), typeof(double)] },
        };
    }







    internal class TestSettings : Settings
    {
        public TestSettings(string path = "") : base(path) { }

        public string Test { get; set; }
        public int AWd1 { get; set; }
        public char Test3 { get; set; }
        public float Test2 { get; set; }

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
}
