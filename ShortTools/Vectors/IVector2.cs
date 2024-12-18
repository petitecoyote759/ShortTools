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
    /// An integer vector, contains 2 values and functions like the dot product can be calculated.
    /// </summary>
    public struct IVector2 : IEquatable<IVector2>, ICloneable, IComparable<Vector2>, IComparable<IVector2>, IFormattable
    {
        #region x and y
#pragma warning disable CA1051 // Dont declare visible instance fields, i dont care cause this is faster
        /// <summary>
        /// The x component of the vector.
        /// </summary>
        public int x;
        /// <summary>
        /// The y component of the vector.
        /// </summary>
        public int y;
#pragma warning restore CA1051
        #endregion


        #region Constructors
        /// <summary>
        /// Default constructor, initialises x and y to 0.
        /// </summary>
        public IVector2()
        {
            x = 0; y = 0;
        }
        /// <summary>
        /// Constructor that takes x and y as inputs.
        /// </summary>
        /// <param name="x">An integer input, stored in the x variable.</param>
        /// <param name="y">An integer input, stored in the y variable.</param>
        public IVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Constructor that takes x and y as float inputs.
        /// </summary>
        /// <param name="x">A float input which is cast via (int)x, stored in the x variable.</param>
        /// <param name="y">A float input which is cast via (int)y, stored in the y variable.</param>
        public IVector2(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }
        /// <summary>
        /// Constructor that takes x and y as double inputs.
        /// </summary>
        /// <param name="x">A double input which is cast via (int)x, stored in the x variable.</param>
        /// <param name="y">A double input which is cast via (int)y, stored in the y variable.</param>
        public IVector2(double x, double y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }
        #endregion


        #region Converters
        /// <summary>
        /// Explicit conversion that calls the ToVector2 function.
        /// </summary>
        /// <param name="SIVect">Integer vector to be converted.</param>
        public static explicit operator Vector2([NotNull] IVector2 SIVect) => ToVector2(SIVect);
        /// <summary>
        /// Explicit conversion that calls the FromVector2 function.
        /// </summary>
        /// <param name="Vect">Vector to be converted.</param>
        public static explicit operator IVector2([NotNull] Vector2 Vect) => FromVector2(Vect);
        /// <summary>
        /// Explicit conversion that calls the ToTuple function.
        /// </summary>
        /// <param name="SIVect">Integer vector to be converted, Item1 is the x value, Item2 is the y value.</param>
        public static explicit operator Tuple<int, int>([NotNull] IVector2 SIVect) => ToTuple(SIVect);
        /// <summary>
        /// Explicit conversion that calls the FromTuple function.
        /// </summary>
        /// <param name="tuple">Tuple to be converted, Item1 is the x value, Item2 is the y value.</param>
        public static explicit operator IVector2([NotNull] Tuple<int, int> tuple) => FromTuple(tuple);



        /// <summary>
        /// Converts an integer vector into a System.Numerics vector using the Vector2(float, float) constructor.
        /// </summary>
        /// <param name="IVect">The integer vector to be converted.</param>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public static Vector2 ToVector2([NotNull] IVector2 IVect) => new Vector2(IVect.x, IVect.y);
        /// <summary>
        /// Converts this integer vector into a System.Numerics vector using the Vector2(float, float) constructor.
        /// </summary>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public readonly Vector2 ToVector2() => ToVector2(this);
        /// <summary>
        /// Creates an integer vector from a given System.Numerics Vector2.
        /// </summary>
        /// <returns>A ShortTools.General.IVector2 struct.</returns>
        public static IVector2 FromVector2([NotNull] Vector2 Vect) => new IVector2(Vect.X, Vect.Y);


        /// <summary>
        /// Converts this integer vector into a System tuple using the Tuple(int, int) constructor.
        /// </summary>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public static Tuple<int, int> ToTuple([NotNull] IVector2 IVect) => new Tuple<int, int>(IVect.x, IVect.y);
        /// <summary>
        /// Converts this integer vector into a System tuple using the Tuple(int, int) constructor.
        /// </summary>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public readonly Tuple<int, int> ToTuple() => ToTuple(this);
        /// <summary>
        /// Converts this integer vector into a System tuple using the Vector2(float, float) constructor.
        /// </summary>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public static IVector2 FromTuple([NotNull] Tuple<int, int> tuple) => new IVector2(tuple.Item1, tuple.Item2);
        #endregion



        /// <summary>
        /// Converts the vector to the form (x, y)
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString()
        {
            return "(" + x + ", " + y + ")";
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
        public static IVector2 operator +(IVector2 left, IVector2 right) => Add(left, right);
        /// <summary>
        /// Adds the 2 vectors using regular vector addition.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IVector2 Add(IVector2 left, IVector2 right) => new IVector2(left.x + right.x, left.y + right.y);
        /// <summary>
        /// Adds this vector to the other vector using the Add function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly IVector2 Add(IVector2 right) => Add(this, right);


        /// <summary>
        /// Subtracts the 2 vectors via the Subtract function, doing left - right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IVector2 operator -(IVector2 left, IVector2 right) => Subtract(left, right);
        /// <summary>
        /// Subtracts the 2 vectors via normal vector subtraction, doing left - right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IVector2 Subtract(IVector2 left, IVector2 right) => new IVector2(left.x - right.x, left.y - right.y);
        /// <summary>
        /// Subtracts the given vector from this vector, doing this - right using the Subtract function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly IVector2 Subtract(IVector2 right) => Subtract(this, right);


        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 operator *(IVector2 vect, int mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 operator *(IVector2 vect, double mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 operator *(IVector2 vect, float mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 Multiply(IVector2 vect, int mult) => new IVector2(vect.x * mult, vect.y * mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 Multiply(IVector2 vect, double mult) => new IVector2(vect.x * mult, vect.y * mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 Multiply(IVector2 vect, float mult) => new IVector2(vect.x * mult, vect.y * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVector2 Multiply(int mult) => new IVector2(x * mult, y * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVector2 Multiply(double mult) => new IVector2(x * mult, y * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVector2 Multiply(float mult) => new IVector2(x * mult, y * mult);


        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 operator /(IVector2 vect, int mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 operator /(IVector2 vect, double mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVector2 operator /(IVector2 vect, float mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVector2 Divide(IVector2 vect, int divisor) => new IVector2(vect.x / divisor, vect.y / divisor);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVector2 Divide(IVector2 vect, double divisor) => new IVector2(vect.x / divisor, vect.y / divisor);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVector2 Divide(IVector2 vect, float divisor) => new IVector2(vect.x / divisor, vect.y / divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVector2 Divide(int divisor) => Divide(this, divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVector2 Divide(double divisor) => Divide(this, divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVector2 Divide(float divisor) => Divide(this, divisor);

        #endregion


        #region Cross and Dot product

        /// <summary>
        /// Calculates the cross product of the 2 vectors via (x1, y1) x (x2, y2) = x1y2 - y1x2.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int CrossProduct(IVector2 left, IVector2 right) => (left.x * right.y) - (left.y * right.x);
        /// <summary>
        /// Calculates the cross product of a vector and this via the CrossProduct function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly int CrossProduct(IVector2 right) => CrossProduct(this, right);


        /// <summary>
        /// Calculates the dot product of the 2 given vectors via (x1, y1) * (x2, y2) = x1x2 + y1y2.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int DotProduct(IVector2 left, IVector2 right) => (left.x * right.x) + (left.y * right.y);
        /// <summary>
        /// Calculates the dot product of the given vector and this via the DotProduct function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly int DotProduct(IVector2 right) => DotProduct(this, right);




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

            if (obj is IVector2 vect)
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
        public readonly bool Equals(IVector2 other)
        {
            return x == other.x && y == other.y;
        }




        /// <summary>
        /// Returns true if both the x and the y values are equal in both the given vectors.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(IVector2 left, IVector2 right) => left.x == right.x && left.y == right.y;
        /// <summary>
        /// Returns false if either the x or the y values are not equal in the given vectors.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(IVector2 left, IVector2 right) => left.x != right.x || left.y != right.y;


        /// <summary>
        /// Compares the magnitudes of the 2 vectors.
        /// </summary>
        /// <param name="left">The first vector to be compared</param>
        /// <param name="right">The second vector to be compared</param>
        /// <returns>True, if the magnitude of the left value is less than the magnitude of the right</returns>
        public static bool operator <(IVector2 left, IVector2 right) => left.MagSquared() < right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVector2, IVector2)"/></summary>
        /// <returns>True, if the magnitude of the left value is less than or equal to the magnitude of the right</returns>
        public static bool operator <=(IVector2 left, IVector2 right) => left.MagSquared() <= right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVector2, IVector2)"/></summary>
        /// <returns>True, if the magnitude of the left value is greater than than the magnitude of the right</returns>
        public static bool operator >(IVector2 left, IVector2 right) => left.MagSquared() > right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVector2, IVector2)"/></summary>
        /// <returns>True, if the magnitude of the left value is greater than than or equal to the magnitude of the right</returns>
        public static bool operator >=(IVector2 left, IVector2 right) => left.MagSquared() >= right.MagSquared();


        #endregion


        #region Indexing
        /*
        public T? UseIndex<T>([NotNullIfNotNull(nameof(data))] IList<IList<T>> data)
        {
            if (data is null) { return default; }
            return data[x][y];
        }

        public T? UseIndex<T>([NotNullIfNotNull(nameof(data))] this IList<IList<T>> data, IVector2 vector)
        {
            if (data is null) { return default; }
            return data[vector.x][vector.y];
        }
        */
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
        /// |x1 - x2| + |y1 - y2|
        /// </summary>
        /// <returns></returns>
        public readonly int RoughMag()
        {
            return Math.Abs(x) + Math.Abs(y);
        }
        /// <inheritdoc cref="RoughMag()"/>
        public readonly int RoughMag(IVector2 vector)
        {
            return Math.Abs(x - vector.x) + Math.Abs(y - vector.y);
        }

        #endregion






        #region Cloning

        /// <summary>
        /// Clones the IVector2, returning a copy with the same x and y values.
        /// </summary>
        /// <returns></returns>
        public readonly object Clone()
        {
            return new IVector2(x, y);
        }

        #endregion


        #region Comparable

        /// <summary>
        /// Compares the lengths of the 2 vectors. When this vector has a lower length than the parameter vector, -1 will be returned. If the vectors are the same length
        /// then it will return 0. And if the parameter vector is smaller than this vector, then it will return 1.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly int CompareTo(Vector2 other)
        {
            float dist = Magnitude() - other.Length();
            if (dist < 0) { return -1; }
            else if (dist > 0) { return 1; }
            return 0;
        }


        /// <inheritdoc cref="CompareTo(Vector2)"/>
        public readonly int CompareTo(IVector2 other)
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
        /// The string will then have {x} and {y} be replaced with the x and y values.
        /// </summary>
        /// <param name="format">The string to have its {x} and {y} to be changed</param>
        /// <param name="formatProvider">The format provider used for the int.ToString, defaults to 
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



        #endregion


        







        #region Tests

        internal static void Main()
        {
            Print(new IVector2(1, 4).ToString("wow so many cool words this is so cool!!! ({x} + {y})", CultureInfo.InvariantCulture));
        }

        #endregion
    }
}