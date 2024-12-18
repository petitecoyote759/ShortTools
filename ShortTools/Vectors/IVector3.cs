using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using static ShortTools.General.Prints;


namespace ShortTools.General.Vectors
{
    /// <summary>
    /// An integer vector, contains 3 values and functions like the dot product can be calculated.
    /// </summary>
    public struct IVector3 : IEquatable<IVector3>, ICloneable, IComparable<Vector3>, IComparable<IVector3>, IFormattable
    {
        #region x, y, and z
#pragma warning disable CA1051 // Dont declare visible instance fields, i dont care cause this is faster
        /// <summary>
        /// The x component of the vector.
        /// </summary>
        public int x;
        /// <summary>
        /// The y component of the vector.
        /// </summary>
        public int y;
        /// <summary>
        /// The z component of the vector.
        /// </summary>
        public int z;
#pragma warning restore CA1051
        #endregion


        #region Constructors
        /// <summary>
        /// Default constructor, initialises x, y, and z to 0.
        /// </summary>
        public IVector3()
        {
            x = 0; y = 0; z = 0;
        }
        /// <summary>
        /// Constructor that takes x, y, and z as inputs.
        /// </summary>
        /// <param name="x">An integer input, stored in the x variable.</param>
        /// <param name="y">An integer input, stored in the y variable.</param>
        /// <param name="z">An integer input, stored in the z variable.</param>
        public IVector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// Constructor that takes x, y, and z as float inputs.
        /// </summary>
        /// <param name="x">A float input which is cast via (int)x, stored in the x variable.</param>
        /// <param name="y">A float input which is cast via (int)y, stored in the y variable.</param>
        /// <param name="z">A float input which is cast via (int)z, stored in the z variable.</param>
        public IVector3(float x, float y, float z)
        {
            this.x = (int)x;
            this.y = (int)y;
            this.z = (int)z;
        }
        /// <summary>
        /// Constructor that takes x, y, and z as double inputs.
        /// </summary>
        /// <param name="x">A double input which is cast via (int)x, stored in the x variable.</param>
        /// <param name="y">A double input which is cast via (int)y, stored in the y variable.</param>
        /// <param name="z">A double input which is cast via (int)z, stored in the z variable.</param>
        public IVector3(double x, double y, double z)
        {
            this.x = (int)x;
            this.y = (int)y;
            this.z = (int)z;
        }
        #endregion


        #region Converters
        /// <summary>
        /// Explicit conversion that calls the ToVector3 function.
        /// </summary>
        /// <param name="SIVect">Integer vector to be converted.</param>
        public static explicit operator Vector3([NotNull] IVector3 SIVect) => ToVector3(SIVect);
        /// <summary>
        /// Explicit conversion that calls the FromVector3 function.
        /// </summary>
        /// <param name="Vect">Vector to be converted.</param>
        public static explicit operator IVector3([NotNull] Vector3 Vect) => FromVector3(Vect);
        /// <summary>
        /// Explicit conversion that calls the ToTuple function.
        /// </summary>
        /// <param name="SIVect">Integer vector to be converted, Item1 is the x value, Item2 is the y value, and Item3 is the z value.</param>
        public static explicit operator Tuple<int, int, int>([NotNull] IVector3 SIVect) => ToTuple(SIVect);
        /// <summary>
        /// Explicit conversion that calls the FromTuple function.
        /// </summary>
        /// <param name="tuple">Tuple to be converted, Item1 is the x value, Item2 is the y value, Item3 is the z value.</param>
        public static explicit operator IVector3([NotNull] Tuple<int, int, int> tuple) => FromTuple(tuple);



        /// <summary>
        /// Converts an integer vector into a System.Numerics vector using the Vector3(float, float, float) constructor.
        /// </summary>
        /// <param name="IVect">The integer vector to be converted.</param>
        /// <returns>A <see cref="System.Numerics.Vector3">System.Numerics Vector3</see> struct.</returns>
        public static Vector3 ToVector3([NotNull] IVector3 IVect) => new Vector3(IVect.x, IVect.y, IVect.z);
        /// <summary>
        /// Converts this integer vector into a System.Numerics vector using the Vector3(float, float, float) constructor.
        /// </summary>
        /// <returns>A <see cref="System.Numerics.Vector3">System.Numerics Vector3</see> struct.</returns>
        public readonly Vector3 ToVector3() => ToVector3(this);
        /// <summary>
        /// Creates an integer vector from a given <see cref="System.Numerics.Vector3">System.Numerics Vector3</see>.
        /// </summary>
        /// <returns>A ShortTools.General.IVector3 struct.</returns>
        public static IVector3 FromVector3([NotNull] Vector3 Vect) => new IVector3(Vect.X, Vect.Y, Vect.Z);


        /// <summary>
        /// Converts this integer vector into a System tuple using the Tuple(int, int, int) constructor.
        /// </summary>
        /// <returns>A <see cref="System.Numerics.Vector3">System.Numerics Vector3</see> struct.</returns>
        public static Tuple<int, int, int> ToTuple([NotNull] IVector3 IVect) => new Tuple<int, int, int>(IVect.x, IVect.y, IVect.z);
        /// <summary>
        /// Converts this integer vector into a System tuple using the Tuple(int, int, int) constructor.
        /// </summary>
        /// <returns>A <see cref="System.Numerics.Vector3">System.Numerics Vector3</see> struct.</returns>
        public readonly Tuple<int, int, int> ToTuple() => ToTuple(this);
        /// <summary>
        /// Converts this integer vector into a System tuple using the Vector3(float, float, float) constructor.
        /// </summary>
        /// <returns>A <see cref="System.Numerics.Vector3">System.Numerics Vector3</see> struct.</returns>
        public static IVector3 FromTuple([NotNull] Tuple<int, int, int> tuple) => new IVector3(tuple.Item1, tuple.Item2, tuple.Item3);
        #endregion



        /// <summary>
        /// Converts the vector to the form (x, y, z)
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        /// <summary>
        /// Calls the base GetHashCode function.
        /// </summary>
        /// <returns></returns>
        public readonly override int GetHashCode()
        {
            return base.GetHashCode();
        }




        #region Operators


        /// <summary>
        /// Adds the 2 vectors via the Add function, uses regular vector addition.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IVector3 operator +(IVector3 left, IVector3 right) => Add(left, right);
        /// <summary>
        /// Adds the 2 vectors using regular vector addition.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IVector3 Add(IVector3 left, IVector3 right) => new IVector3(left.x + right.x, left.y + right.y, left.z + right.z);
        /// <summary>
        /// Adds this vector to the other vector using the Add function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly IVector3 Add(IVector3 right) => Add(this, right);


        /// <summary>
        /// Subtracts the 2 vectors via the Subtract function, doing left - right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IVector3 operator -(IVector3 left, IVector3 right) => Subtract(left, right);
        /// <summary>
        /// Subtracts the 2 vectors via normal vector subtraction, doing left - right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IVector3 Subtract(IVector3 left, IVector3 right) => new IVector3(left.x - right.x, left.y - right.y, left.z - right.z);
        /// <summary>
        /// Subtracts the given vector from this vector, doing this - right using the Subtract function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly IVector3 Subtract(IVector3 right) => Subtract(this, right);


        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 operator *(IVector3 vect, int mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 operator *(IVector3 vect, double mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 operator *(IVector3 vect, float mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 Multiply(IVector3 vect, int mult) => new IVector3(vect.x * mult, vect.y * mult, vect.z * mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 Multiply(IVector3 vect, double mult) => new IVector3(vect.x * mult, vect.y * mult, vect.z * mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 Multiply(IVector3 vect, float mult) => new IVector3(vect.x * mult, vect.y * mult, vect.z * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVector3 Multiply(int mult) => new IVector3(x * mult, y * mult, z * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVector3 Multiply(double mult) => new IVector3(x * mult, y * mult, z * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVector3 Multiply(float mult) => new IVector3(x * mult, y * mult, z * mult);


        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 operator /(IVector3 vect, int mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 operator /(IVector3 vect, double mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector3 operator /(IVector3 vect, float mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVector3 Divide(IVector3 vect, int divisor) => new IVector3(vect.x / divisor, vect.y / divisor, vect.z / divisor);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVector3 Divide(IVector3 vect, double divisor) => new IVector3(vect.x / divisor, vect.y / divisor, vect.z / divisor);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVector3 Divide(IVector3 vect, float divisor) => new IVector3(vect.x / divisor, vect.y / divisor, vect.z / divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVector3 Divide(int divisor) => Divide(this, divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVector3 Divide(double divisor) => Divide(this, divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVector3 Divide(float divisor) => Divide(this, divisor);

        #endregion


        #region Cross and Dot product

        /// <summary>
        /// Calculates the cross product of the 3 vectors via <br/>
        /// (x1)‎ ‎ ‎ ‎   (x2)‎ ‎ ‎ ‎ ‎     [ z2y1 - z1y2 ] <br/>
        /// (y1)                          X (y2) =                                   [ z2x1 - z1x2 ] <br/>
        /// (z1)‎ ‎ ‎ ‎   (z2)‎ ‎ ‎ ‎ ‎     [ y2x1 - y1x2 ] <br/>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Vector3 CrossProduct(IVector3 left, IVector3 right) => new Vector3( 
            (right.z * left.y) - (left.z * right.y),
            (right.z * left.x) - (left.z * right.x),
            (right.y * left.x) - (left.y * right.x));
        /// <inheritdoc cref="CrossProduct(IVector3, IVector3)"/>
        public readonly Vector3 CrossProduct(IVector3 right) => CrossProduct(this, right);


        /// <summary>
        /// Calculates the dot product of the 2 given vectors via (x1, y1, z1) * (x2, y2, z2) = x1x2 + y1y2 + z1z2.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int DotProduct(IVector3 left, IVector3 right) => (left.x * right.x) + (left.y * right.y) + (left.z * right.z);
        /// <inheritdoc cref="DotProduct(IVector3, IVector3)"/>
        public readonly int DotProduct(IVector3 right) => DotProduct(this, right);




        #endregion


        #region Equality and Comparisons

        /// <summary>
        /// Checks if the object is an integer vector, and then compares the content
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly override bool Equals(object? obj)
        {
            if (obj is null) { return false; }

            if (obj is IVector3 vect)
            {
                return this == vect;
            }
            return false;
        }

        /// <summary>
        /// Returns true if both the x and the y values are equal in both the given vectors.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(IVector3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }




        /// <summary>
        /// Returns true if both the x and the y values are equal in both the given vectors.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(IVector3 left, IVector3 right) => left.x == right.x && left.y == right.y && left.z == right.z;
        /// <summary>
        /// Returns false if either the x or the y values are not equal in the given vectors.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(IVector3 left, IVector3 right) => left.x != right.x || left.y != right.y || left.z != right.z;


        /// <summary>
        /// Compares the magnitudes of the 2 vectors.
        /// </summary>
        /// <param name="left">The first vector to be compared</param>
        /// <param name="right">The second vector to be compared</param>
        /// <returns>True, if the magnitude of the left value is less than the magnitude of the right</returns>
        public static bool operator <(IVector3 left, IVector3 right) => left.MagSquared() < right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVector3, IVector3)"/></summary>
        /// <returns>True, if the magnitude of the left value is less than or equal to the magnitude of the right</returns>
        public static bool operator <=(IVector3 left, IVector3 right) => left.MagSquared() <= right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVector3, IVector3)"/></summary>
        /// <returns>True, if the magnitude of the left value is greater than than the magnitude of the right</returns>
        public static bool operator >(IVector3 left, IVector3 right) => left.MagSquared() > right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVector3, IVector3)"/></summary>
        /// <returns>True, if the magnitude of the left value is greater than than or equal to the magnitude of the right</returns>
        public static bool operator >=(IVector3 left, IVector3 right) => left.MagSquared() >= right.MagSquared();


        #endregion


        #region Others


        /// <summary>
        /// Returns the magnitude of this vector squared.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public readonly float MagSquared()
        {
            return DotProduct(this);
        }


        /// <summary>
        /// Returns the magnitude of this vector.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public readonly float Magnitude()
        {
            return MathF.Sqrt(MagSquared());
        }
        /// <inheritdoc cref="Magnitude"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Mag() => Magnitude();
        /// <inheritdoc cref="Magnitude"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Length() => Magnitude();


        /// <summary>
        /// Returns the rough mag, given by <br/>
        /// |x1 - x2| + |y1 - y2| + |z1 - z2|
        /// </summary>
        /// <returns></returns>
        public readonly int RoughMag()
        {
            return Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
        }
        /// <inheritdoc cref="RoughMag()"/>
        public readonly int RoughMag(IVector3 vector)
        {
            return Math.Abs(x - vector.x) + Math.Abs(y - vector.y) + Math.Abs(z - vector.z);
        }

        #endregion






        #region Cloning

        /// <summary>
        /// Clones the IVector3, returning a copy with the same x, y, and z values.
        /// </summary>
        /// <returns></returns>
        public readonly object Clone()
        {
            return new IVector3(x, y, z);
        }

        #endregion


        #region Comparable

        /// <summary>
        /// Compares the lengths of the 2 vectors. When this vector has a lower length than the parameter vector, -1 will be returned. If the vectors are the same length
        /// then it will return 0. And if the parameter vector is smaller than this vector, then it will return 1.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly int CompareTo(Vector3 other)
        {
            float dist = Magnitude() - other.Length();
            if (dist < 0) { return -1; }
            else if (dist > 0) { return 1; }
            return 0;
        }


        /// <inheritdoc cref="CompareTo(Vector3)"/>
        public readonly int CompareTo(IVector3 other)
        {
            float dist = Magnitude() - other.Magnitude();
            if (dist < 0) { return -1; }
            else if (dist > 0) { return 1; }
            return 0;
        }


        #endregion


        #region Serialization

        /// <summary>
        /// Creates a string representing the data of the vector, allowing you to do
        /// <code>
        /// vector.ToString("I can put any words in here but then to get the values i do {x} or {y}");
        /// </code>
        /// The string will then have {x}, {y}, and {z} be replaced with the x, y, and z values.
        /// </summary>
        /// <param name="format">The string to have its {x}, {y}, and {z} to be changed</param>
        /// <param name="formatProvider">The format provider used for the <see cref="int">int</see>.<see cref="int.ToString()">ToString</see>, defaults to 
        /// <see cref="CultureInfo.InvariantCulture">CultureInfo.InvariantCulture</see>
        /// </param>
        /// <returns></returns>
        public readonly string ToString(string? format, IFormatProvider? formatProvider = null)
        {
            if (format is null) { return this.ToString(); }
            if (format.Length < 3) { return this.ToString(); }

            formatProvider ??= CultureInfo.InvariantCulture;


            bool checking = true;
            while (checking)
            {
                int i;
                for (i = 0; i < format.Length - 2; i++)
                {
                    if (i == format.Length - 3) { checking = false; }
                    // if format part goes {x}
                    if (format[i] == '{' && format[i + 2] == '}')
                    {
                        if (IsX(format[i + 1]))
                        {
                            format = format[..i] + x.ToString(formatProvider) + format[(i + 3)..];
                        }
                        else if (IsY(format[i + 1]))
                        {
                            format = format[..i] + y.ToString(formatProvider) + format[(i + 3)..];
                        }
                        else if (IsZ(format[i + 1]))
                        {
                            format = format[..i] + z.ToString(formatProvider) + format[(i + 3)..];
                        }
                    }
                }
            }

            return format;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static bool IsX(char test)
        {
            return "x".Contains(test, StringComparison.InvariantCultureIgnoreCase);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static bool IsY(char test)
        {
            return "y".Contains(test, StringComparison.InvariantCultureIgnoreCase);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static bool IsZ(char test)
        {
            return "z".Contains(test, StringComparison.InvariantCultureIgnoreCase);
        }



        #endregion






        #region Tests

        internal static void Main()
        {

        }

        #endregion
    }
}