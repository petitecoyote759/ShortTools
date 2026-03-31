using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        /// Writes input to console, just shorter to write than Console.WriteLine(). <br/>
        /// This function is threadsafe with <see cref="ReadFunctions.ReadInput"/> to allow you to read inputs while printing on other threads.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"> The message to be printed. </param>
        public static void Print<T>(T message)
        {
            bool readActive = ReadFunctions.outputBuilder is not null;
            string outputString = message?.ToString() ?? "null";
            if (readActive) { Console.CursorLeft = 0; outputString = outputString.PadRight(ReadFunctions.outputBuilder!.Length, ' '); }
            Console.WriteLine(outputString);
            if (readActive) { Console.Write(ReadFunctions.outputBuilder!.ToString()); }
        }


        /// <summary>
        /// <para> Writes input to console, just shorter to write than Console.WriteLine(). </para>
        /// </summary>
        public static void Print()
        {
            Print(string.Empty);
        }


        /// <summary>
        /// <para> Writes input to console, writes arrays as [ item, item, item ]. </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inp"> The message to be printed. </param>
        public static void Print<T>(T[] inp)
        {
            bool readActive = ReadFunctions.outputBuilder is not null;
            // Delete the read text from the console
            if (readActive) { Console.CursorLeft = 0; Console.Write(new string(' ', ReadFunctions.outputBuilder!.Length)); Console.CursorLeft = 0; }


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


            // Re add the read text after completed
            if (readActive) { Console.Write(ReadFunctions.outputBuilder!.ToString()); }
        }


        /// <summary>
        /// <para> Writes input to console, writes list as { item, item, item }. </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inp"> The message to be printed. </param>
        public static void Print<T>(Collection<T> inp)
        {
            bool readActive = ReadFunctions.outputBuilder is not null;
            // Delete the read text from the console
            if (readActive) { Console.CursorLeft = 0; Console.Write(new string(' ', ReadFunctions.outputBuilder!.Length)); Console.CursorLeft = 0; }


            if (inp is null || inp.Count == 0) { Console.WriteLine(TR("List Open") + TR("Gap") + TR("List Close")); return; }
            if (inp.Count == 1) { Console.WriteLine("{ " + inp[0] + " }"); return; }

            Console.Write(TR("Gap"));
            for (int i = 0; i < inp.Count - 1; i++)
            {
                Console.Write(inp[i] + TR("List Seperator") + TR("Gap"));
            }
            Console.Write(inp.Last());
            Console.WriteLine(TR("Gap") + TR("List Close"));


            // Re add the read text after completed
            if (readActive) { Console.Write(ReadFunctions.outputBuilder!.ToString()); }
        }

        /// <summary>
        /// <para> Writes input to console, writes Dictionary as [ { key1 : value1 }, { key2, value2 } ]. </para>
        /// </summary>
        /// <typeparam name="T1"> Key type of the dictionary. </typeparam>
        /// <typeparam name="T2"> Value type of the dictionary. </typeparam>
        /// <param name="dictionary"> The dictionary to be printed. </param>
        public static void Print<T1, T2>(IDictionary<T1, T2> dictionary)
        {
            bool readActive = ReadFunctions.outputBuilder is not null;
            // Delete the read text from the console
            if (readActive) { Console.CursorLeft = 0; Console.Write(new string(' ', ReadFunctions.outputBuilder!.Length)); Console.CursorLeft = 0; }


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


            // Re add the read text after completed
            if (readActive) { Console.Write(ReadFunctions.outputBuilder!.ToString()); }
        }

        /// <summary>
        /// <para> Writes input to console, writes Dictionary as [ { key1 : value1 }, { key2, value2 } ]. </para>
        /// </summary>
        /// <param name="dictionary"> The dictionary to be printed. </param>
        public static void Print(IDictionary<object, object> dictionary)
        {
            bool readActive = ReadFunctions.outputBuilder is not null;
            // Delete the read text from the console
            if (readActive) { Console.CursorLeft = 0; Console.Write(new string(' ', ReadFunctions.outputBuilder!.Length)); Console.CursorLeft = 0; }


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


            // Re add the read text after completed
            if (readActive) { Console.Write(ReadFunctions.outputBuilder!.ToString()); }
        }



        // <<Coloured Printing>> //
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
    /// Class that provides a function for thread safe reading. (<see cref="ReadInput"/>)
    /// </summary>
    public static class ReadFunctions
    {
        internal static StringBuilder? outputBuilder = null;
        /// <summary>
        /// When this is used, if another thread calls <see cref="Prints.Print()"/> 
        /// then it will automatically move the text that you have typed onto the line below.
        /// </summary>
        /// <returns></returns>
        public static string ReadInput()
        {
            outputBuilder = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter) { Console.Write('\n'); break; }
                if (keyInfo.Key == ConsoleKey.Backspace) 
                { 
                    if (Console.CursorLeft == 0) { continue; }
                    Console.Write(' '); 
                    Console.CursorLeft -= 1;
                    _ = outputBuilder.Remove(outputBuilder.Length - 1, 1);
                    continue; 
                }
                _ = outputBuilder.Append(keyInfo.KeyChar);
            }

            string output = outputBuilder.ToString();
            outputBuilder = null;
            return output;
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






        // <<Tile Ray Functions>> //

#pragma warning disable
        /// <summary>
        /// Where a line hits a tile, on the top, left, bottom, or right.
        /// </summary>
        public enum HitPosition : byte
        {
            Top, 
            Bottom, 
            Left, 
            Right
        }


        /// <summary>
        /// Intersection information, containing tile hit coordinates and the hit direction
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public readonly struct IntersectInfo
        {
#pragma warning restore
            /// <summary>
            /// X coordinate of the tile that was hit.
            /// </summary>
            public readonly int x;
            /// <summary>
            /// Y coordinate of the tile that was hit.
            /// </summary>
            public readonly int y;

            /// <summary>
            /// X coordinate of the intersection point.
            /// </summary>
            public readonly float hitX;
            /// <summary>
            /// Y coordinate of the intersection point.
            /// </summary>
            public readonly float hitY;

            /// <summary>
            /// The direction in which the tile was hit from.
            /// </summary>
            public readonly HitPosition hitDirection;

            /// <summary>
            /// Creates an Intersect Info instance.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="hitX"></param>
            /// <param name="hitY"></param>
            /// <param name="hitPosition"></param>
            public IntersectInfo(int x, int y, float hitX, float hitY, HitPosition hitPosition)
            {
                this.x = x;
                this.y = y;

                this.hitX = hitX;
                this.hitY = hitY;

                this.hitDirection = hitPosition;
            }
        }

        /// <summary>
        /// Gets the tiles that a line goes through in a 2d plane, not including the current tile.
        /// </summary>
        /// <param name="x">The x coordinate the line starts at.</param>
        /// <param name="y">The y coordinate the line starts at.</param>
        /// <param name="dx">The x direction of the line.</param>
        /// <param name="dy">The y direction of the line.</param>
        /// <param name="count">The total amount of tiles to get.</param>
        /// <returns>An array of <see cref="IntersectInfo"/>'s, not including the tile that the line spawned from.</returns>
        public static IntersectInfo[] GetIntersectPoints(float x, float y, float dx, float dy, int count)
        {
            // Main maths at https://www.desmos.com/calculator/x2pzte0do0

            IntersectInfo[] output = new IntersectInfo[count];

            int startX = (int)MathF.Floor(x) + (dx < 0 ? 0 : 1);
            int startY = (int)MathF.Floor(y) + (dy < 0 ? 0 : 1);

            int xIndex = 0; // x is the ones that intersect the X axis
            int yIndex = 0; // y is the ones that intersect the Y axis

            int targetX;
            int targetY;
            float hitX;
            float hitY;

            bool above = dy < 0; // hits will be the top of the square if true
            HitPosition yHit = above ? HitPosition.Top : HitPosition.Bottom;
            bool left = dx < 0; // hits will be on the left if true
            HitPosition xHit = left ? HitPosition.Left : HitPosition.Right;

            for (int i = 0; i < count; i++)
            {
                // Distance to the next line
                float xDist = (startX + (dx < 0 ? -xIndex : xIndex) - x) / dx;
                float yDist = (startY + (dy < 0 ? -yIndex : yIndex) - y) / dy;

                // Which one is closer, the x lines or the y lines
                HitPosition hitDirection;
                if (xDist < yDist)
                {
                    hitX = x + (xDist * dx);
                    hitY = y + (xDist * dy);

                    targetX = (int)(hitX) + (left ? -1 : 0);
                    targetY = (int)MathF.Floor(hitY);
                    xIndex++;
                    hitDirection = xHit;
                }
                else
                {
                    hitX = x + (yDist * dx);
                    hitY = y + (yDist * dy);

                    targetX = (int)MathF.Floor(hitX);
                    targetY = (int)(hitY) + (above ? -1 : 0);
                    yIndex++;
                    hitDirection = yHit;
                }

                // If not already in the set
                if (i == 0 || targetX != output[i - 1].x || targetY != output[i - 1].y)
                {
                    output[i] = new IntersectInfo(targetX, targetY, hitX, hitY, hitDirection);
                }
                else
                {
                    i--; // already in set, go back to keep looking. (index will be updated already)
                }
            }

            return output;
        }
    }











    internal class MainRunning
    {
        private static void Main(string[] args)
        {
            Thread thread = new Thread(new ThreadStart(() => { Thread.Sleep(5000); Prints.Print(new int[50]); } ));
            thread.Start();
            ReadFunctions.ReadInput();
        }
    }
}