using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ShortTools.General
{



    /// <summary>
    /// Static class for writing to the console, but with added features like automatically printing the data contained in an array, rather than just the array type.
    /// </summary>
    public static class Prints
    {
        internal static readonly Dictionary<string, string> textResources = new Dictionary<string, string>()
        {
            { "Gap", " " },

            { "Array Open", "[" },
            { "Array Close", "]" },
            { "Array Seperator", "," },

            { "List Open", "{" },
            { "List Close", "}" },
            { "List Seperator", "," },

            { "Dictionary Open", "[" },
            { "Dictionary Close", "]" },
            { "Dictionary Seperator", "," },
            { "Dictionary Entry Open", "{" },
            { "Dictionary Entry Close", "}" },
            { "Dictionary Entry Seperator", ":" },
        };



        /// <summary>
        /// Function that returns the text resource that relates to the input, just a shorter interface with the dictionary.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static string TR(string inp) => textResources[inp];
        /// <summary>
        /// Function that returns the text resource that relates to the input, just a shorter interface with the dictionary.
        /// </summary>
        internal static string TR(params string[] inp) 
        { 
            StringBuilder builder = new StringBuilder();
            foreach (string value in inp) 
            { 
                builder.Append(TR(value)); 
            }
            return builder.ToString(); 
        }





        /// <summary>
        /// <para> Writes input to console, just shorter to write than Console.WriteLine(). </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"> The message to be printed. </param>
        public static void Print<T>(T message)
        {
            Console.WriteLine(message);
        }


        /// <summary>
        /// <para> Writes input to console, just shorter to write than Console.WriteLine(). </para>
        /// </summary>
        public static void Print()
        {
            Console.WriteLine();
        }


        /// <summary>
        /// <para> Writes input to console, writes arrays as [ item, item, item ]. </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inp"> The message to be printed. </param>
        public static void Print<T>(T[] inp)
        {
            if (inp is null || inp.Length == 0) 
            { Console.WriteLine(TR("Array Open") + TR("Gap") + TR("Array Close")); return; }

            if (inp.Length == 1) 
            { Console.WriteLine(TR("Array Open") + TR("Gap") + inp[0] + TR("Gap") + TR("Array Close")); return; }

            Console.Write(TR("Array Open") + TR("Gap"));
            for (int i = 0; i < inp.Length - 1; i++)
            {
                Console.Write(inp[i] + TR("Array Seperator") + TR("Gap"));
            }
            Console.Write(inp.Last());
            Console.WriteLine(TR("Gap") + TR("Array Close"));
        }


        /// <summary>
        /// <para> Writes input to console, writes list as { item, item, item }. </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inp"> The message to be printed. </param>
        public static void Print<T>(Collection<T> inp)
        {
            if (inp is null || inp.Count == 0) { Console.WriteLine(TR("List Open") + TR("Gap") + TR("List Close")); return; }
            if (inp.Count == 1) { Console.WriteLine("{ " + inp[0] + " }"); return; }

            Console.Write(TR("Gap"));
            for (int i = 0; i < inp.Count - 1; i++)
            {
                Console.Write(inp[i] + TR("List Seperator") + TR("Gap"));
            }
            Console.Write(inp.Last());
            Console.WriteLine(TR("Gap") + TR("List Close"));
        }

        /// <summary>
        /// <para> Writes input to console, writes Dictionary as [ { key1 : value1 }, { key2, value2 } ]. </para>
        /// </summary>
        /// <typeparam name="T1"> Key type of the dictionary. </typeparam>
        /// <typeparam name="T2"> Value type of the dictionary. </typeparam>
        /// <param name="dictionary"> The dictionary to be printed. </param>
        public static void Print<T1, T2>(IDictionary<T1, T2> dictionary)
        {
            if (dictionary is null || dictionary.Count == 0) 
            { Console.WriteLine(TR("Dictionary Open", "Gap", "Dictionary Close")); return; }

            T1[] Keys = dictionary.Keys.ToArray();
            T2[] Values = dictionary.Values.ToArray();

            Console.Write(TR("Dictionary Open", "Gap"));
            for (int i = 0; i < dictionary.Count - 1; i++)
            {
                Console.Write(TR("Dictionary Entry Open", "Gap"));
                Console.Write(Keys[i] + TR("Gap", "Dictionary Entry Seperator" , "Gap") + Values[i]);
                Console.Write(TR("Gap", "Dictionary Entry Close", "Dictionary Seperator", "Gap"));
            }
            Console.Write(
$"{TR("Dictionary Entry Open")}{TR("Gap")}{Keys.Last()}{TR("Gap", "Dictionary Entry Seperator", "Gap")}{Values.Last()}{TR("Gap")}{TR("Dictionary Entry Close")}{TR("Gap", "Dictionary Close")}");
        }

        /// <summary>
        /// <para> Writes input to console, writes Dictionary as [ { key1 : value1 }, { key2, value2 } ]. </para>
        /// </summary>
        /// <param name="dictionary"> The dictionary to be printed. </param>
        public static void Print(IDictionary<object, object> dictionary)
        {
            if (dictionary is null || dictionary.Count == 0) { Console.WriteLine(TR("Dictionary Open", "Gap", "Dictionary Close")); return; }

            object[] Keys = dictionary.Keys.ToArray();
            object[] Values = dictionary.Values.ToArray();

            Console.Write(TR("Dictionary Open", "Gap"));
            for (int i = 0; i < dictionary.Count - 1; i++)
            {
                Console.Write(TR("Dictionary Entry Open", "Gap"));
                Console.Write(Keys[i] + TR("Gap", "Dictionary Entry Seperator", "Gap") + Values[i]);
                Console.Write(TR("Gap", "Dictionary Entry Close", "Dictionary Seperator", "Gap"));
            }
            Console.Write(
$"{TR("Dictionary Entry Open")}{TR("Gap")}{Keys.Last()}{TR("Gap", "Dictionary Entry Seperator", "Gap")}{Values.Last()}{TR("Gap")}{TR("Dictionary Entry Close")}{TR("Gap", "Dictionary Close")}");
        }




        /// <summary>
        /// <para> Writes input to console, just shorter to write than Console.WriteLine(). </para>
        /// </summary>
        /// <param name="colour"> The colour of the message to be printed. </param>
        /// <param name="message"> The message to be printed. </param>
        public static void Print<T>(T message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Print(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// <para> Writes input to console, just shorter to write than Console.WriteLine(). </para>
        /// </summary>
        /// <param name="colour"> The colour of the message to be printed. </param>
        /// <param name="message"> The message to be printed. </param>
        public static void Print<T>(T[] message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Print(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// <para> Writes input to console, just shorter to write than Console.WriteLine(). </para>
        /// </summary>
        /// <param name="colour"> The colour of the message to be printed. </param>
        /// <param name="message"> The message to be printed. </param>
        public static void Print<T>(Collection<T> message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Print(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// <para> Writes input to console, writes Dictionary as [ { key1 : value1 }, { key2, value2 } ]. </para>
        /// </summary>
        /// <typeparam name="T1"> Key type of the dictionary. </typeparam>
        /// <typeparam name="T2"> Value type of the dictionary. </typeparam>
        /// <param name="colour"> The colour of the message to be printed. </param>
        /// <param name="dictionary"> The dictionary to be printed. </param>
        public static void Print<T1, T2>(IDictionary<T1, T2> dictionary, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Print(dictionary);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// <para> Writes input to console, writes Dictionary as [ { key1 : value1 }, { key2, value2 } ]. </para>
        /// </summary>
        /// <param name="colour"> The colour of the message to be printed. </param>
        /// <param name="dictionary"> The dictionary to be printed. </param>
        public static void Print(IDictionary<object, object> dictionary, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Print(dictionary);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }








    /// <summary>
    /// Static class for game helps, like get the get dt function and more.
    /// </summary>
    public static class GameFunctions
    {
        /// <summary>
        /// <para> Returns the difference in time since the last frame given in milliseconds. </para>
        /// </summary>
        /// <param name="LastFrameTimeMS"> Time since last frame in milliseconds, automatically updated by GetDt</param>
        /// <returns></returns>
        public static long GetDt(ref long LastFrameTimeMS)
        {
            long millis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long dt = millis - LastFrameTimeMS;
            LastFrameTimeMS = millis;
            return dt;
        }
    }











    internal class MainRunning
    {
        private static void Main(string[] args)
        {
            IDictionary<string, int> values = new Dictionary<string, int>() { { "123", 123 }, { "143", 423 }, { "531", 535 } };
            //int[] values = new int[] { 2, 1, 6, 4, 9 };

            Prints.Print(values);
        }
    }
}