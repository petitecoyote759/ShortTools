using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShortTools.General
{
    /// <summary>
    /// Some functions for hiding information, 
    /// </summary>
    public static class Secrets
    {



        // <<Encoding>> //

        // Encoding works via combining the input in a certain way and applying a function onto each character
        // It combines the input to be the first and last letter, and then moves in, so
        //
        // Testing
        // ^     ^ - First step
        //  ^   ^  - Second step
        //   ^ ^   - Third step
        //    ^    - End
        // 
        // So Testing -> Tgensit
        // 
        // The function applied is simple, an XOR with the 'characterModifier'
        // It is also XORed with the index of the letter.


        const ushort characterModifier = 0b_00000000_01010101;
        const ushort indexMax = 0b00111111;

        /// <summary>
        /// Encodes the input using a reversable algorithm to obfuscate text.
        /// </summary>
        /// <param name="input">The string to be encoded.</param>
        /// <returns>A new string that is encoded.</returns>
        public static string Encode([NotNull] string input)
        {
            char[] buffer = new char[input.Length];

            // Combine letters
            for (int i = 0; i < input.Length / 2; i++)
            {
                buffer[i * 2] = ApplyModification(input[i], i);
                buffer[(i * 2) + 1] = ApplyModification(input[input.Length - i - 1], i);
            }

            // Final letter if odd
            if (input.Length % 2 == 1)
            {
                buffer[^1] = (char)(input[input.Length / 2] ^ characterModifier);
            }

            return new string(buffer);
        }

        /// <summary>
        /// Decodes the text encoded by <see cref="Encode(string)"/>.
        /// </summary>
        /// <param name="input">The encoded string.</param>
        /// <returns>The decoded string.</returns>
        public static string Decode([NotNull] string input)
        {
            char[] buffer = new char[input.Length];

            // Combine Letters in reverse
            for (int i = 0; i < input.Length / 2; i++)
            {
                buffer[i] = ApplyModification(input[i * 2], i);
                buffer[input.Length - i - 1] = ApplyModification(input[(i * 2) + 1], i);
            }

            // Final letter if odd
            if (input.Length % 2 == 1)
            {
                buffer[input.Length / 2] = (char)(input[^1] ^ characterModifier);
            }

            return new string(buffer);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char ApplyModification(char input, int index)
        {
            return (char)(input ^ (characterModifier ^ (2 * (index & indexMax))));
        }
    }
}
