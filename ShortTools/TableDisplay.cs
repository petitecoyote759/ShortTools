using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortTools.General
{
    /// <summary>
    /// Functions to display tables in the <see cref="Console"/>.
    /// </summary>
    public static class TableDisplay
    {
        /// <summary>
        /// Displays a row in a table.
        /// </summary>
        /// <param name="items">The items to be displayed.</param>
        /// <param name="columnLengths">The column lengths.</param>
        /// <param name="createLine">Boolean representing if a line should be printed after.</param>
        public static void DisplayTableRow(
            [NotNull] IEnumerable<string> items,
            [NotNull] IEnumerable<int> columnLengths,
            bool createLine = true)
        {
            IEnumerator<string> itemEnumerator = items.GetEnumerator();
            IEnumerator<int> columnLengthsEnumerator = columnLengths.GetEnumerator();

            Console.CursorLeft = 0;
            bool ran = false;
            while (itemEnumerator.MoveNext())
            {
                ran = true;
                _ = columnLengthsEnumerator.MoveNext();

                int currentWidth = columnLengthsEnumerator.Current;

                string currentItem = itemEnumerator.Current;
                if (currentItem.Length > currentWidth) { currentItem = $"{currentItem.Substring(0, currentWidth - 1)}-"; }
                else if (currentItem.Length < currentWidth)
                {
                    int gapWidth = currentWidth - currentItem.Length;
                    int leftGapWidth = gapWidth / 2;
                    currentItem = currentItem.PadLeft(leftGapWidth + currentItem.Length, ' ').PadRight(currentWidth, ' ');
                }
                Console.Write(currentItem);
                Console.Write('|');
            }

            if (ran)
            {
                Console.CursorLeft -= 1;
                Console.Write(' ');
                Console.CursorLeft -= 1;
            }

            if (createLine)
            {
                Console.Write('\n');
                ran = false;
                foreach (int length in columnLengths)
                {
                    ran = true;
                    Console.Write(new string('-', length));
                    Console.Write('+');
                }
                if (ran)
                {
                    Console.CursorLeft -= 1;
                    Console.Write(' ');
                    Console.CursorLeft -= 1;
                }
            }
            Console.Write('\n');
            Console.Write(ReadFunctions.outputBuilder?.ToString());
        }
    }
}
