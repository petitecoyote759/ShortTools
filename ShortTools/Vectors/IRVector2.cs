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
    public readonly struct IRVector2 : IEquatable<IRVector2>, ICloneable, IComparable<Vector2>, IComparable<IVector2>, IComparable<IRVector2>, IFormattable
    {
        #region x and y
#pragma warning disable CA1051 // Dont declare visible instance fields, i dont care cause this is faster
        /// <summary>
        /// The x component of the vector.
        /// </summary>
        public readonly int x;
        /// <summary>
        /// The y component of the vector.
        /// </summary>
        public readonly int y;
#pragma warning restore CA1051
        #endregion


        #region Constructors
        /// <summary>
        /// Default constructor, initialises x and y to 0.
        /// </summary>
        public IRVector2()
        {
            x = 0; y = 0;
            GenVariables(out Magnitude, out MagnitudeSquared, out RoughMag, out thisString);
        }
        /// <summary>
        /// Constructor that takes x and y as inputs.
        /// </summary>
        /// <param name="x">An integer input, stored in the x variable.</param>
        /// <param name="y">An integer input, stored in the y variable.</param>
        public IRVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
            GenVariables(out Magnitude, out MagnitudeSquared, out RoughMag, out thisString);
        }
        /// <summary>
        /// Constructor that takes x and y as float inputs.
        /// </summary>
        /// <param name="x">A float input which is cast via (int)x, stored in the x variable.</param>
        /// <param name="y">A float input which is cast via (int)y, stored in the y variable.</param>
        public IRVector2(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
            GenVariables(out Magnitude, out MagnitudeSquared, out RoughMag, out thisString);
        }
        /// <summary>
        /// Constructor that takes x and y as double inputs.
        /// </summary>
        /// <param name="x">A double input which is cast via (int)x, stored in the x variable.</param>
        /// <param name="y">A double input which is cast via (int)y, stored in the y variable.</param>
        public IRVector2(double x, double y)
        {
            this.x = (int)x;
            this.y = (int)y;
            GenVariables(out Magnitude, out MagnitudeSquared, out RoughMag, out thisString);
        }



        internal IRVector2(int x, int y, float mag, int magSquared, int rougMag, string thisString)
        {
            this.x = x; this.y = y; this.Magnitude = mag; this.thisString = thisString;
            this.MagnitudeSquared = magSquared; this.RoughMag = rougMag;
        }
        #endregion


        #region Converters
        /// <summary>
        /// Explicit conversion that calls the <see cref="ToVector2(IRVector2)">ToVector2</see> function.
        /// </summary>
        /// <param name="SIVect">Integer vector to be converted.</param>
        public static explicit operator Vector2([NotNull] IRVector2 SIVect) => ToVector2(SIVect);
        /// <summary>
        /// Explicit conversion that calls the <see cref="FromVector2(Vector2)">FromVector2</see> function.
        /// </summary>
        /// <param name="Vect">Vector to be converted.</param>
        public static explicit operator IRVector2([NotNull] Vector2 Vect) => FromVector2(Vect);
        /// <summary>
        /// Explicit conversion that calls the <see cref="ToTuple(IRVector2)">ToTuple</see> function.
        /// </summary>
        /// <param name="SIVect">Integer vector to be converted, Item1 is the x value, Item2 is the y value.</param>
        public static explicit operator Tuple<int, int>([NotNull] IRVector2 SIVect) => ToTuple(SIVect);
        /// <summary>
        /// Explicit conversion that calls the <see cref="FromTuple(Tuple{int, int})">FromTuple</see> function.
        /// </summary>
        /// <param name="tuple">Tuple to be converted, Item1 is the x value, Item2 is the y value.</param>
        public static explicit operator IRVector2([NotNull] Tuple<int, int> tuple) => FromTuple(tuple);
        /// <summary>
        /// Explicit conversion that calls the <see cref="ToIVector2(IRVector2)">ToIVector2</see> function.
        /// </summary>
        /// <param name="IRVect"><inheritdoc cref="ToIVector2(IRVector2)"/></param>
        public static explicit operator IVector2([NotNull] IRVector2 IRVect) => ToIVector2(IRVect);
        /// <summary>
        /// Explicit conversion that calls the <see cref="FromIVector2(IVector2)">FromIVector2</see> function.
        /// </summary>
        /// <param name="IRVect"><inheritdoc cref="FromIVector2(IVector2)"/></param>
        public static explicit operator IRVector2([NotNull] IVector2 IRVect) => FromIVector2(IRVect);





        /// <summary>
        /// Converts a readonly integer vector into a short tools integer vector using the IVector2(int, int) constructor.
        /// </summary>
        /// <param name="IRVect">The readonly integer vector to be converted.</param>
        /// <returns>A Short_Tools.General.Vectors.IVector2 struct.</returns>
        public static IVector2 ToIVector2([NotNull] IRVector2 IRVect) => new IVector2(IRVect.x, IRVect.y);

        /// <summary>
        /// Converts an integer vector into a short tools readonly integer vector using the IRVector2(int, int) constructor.
        /// </summary>
        /// <param name="IRVect">The integer vector to be converted.</param>
        /// <returns>A Short_Tools.General.Vectors.IRVector2 readonly struct.</returns>
        public static IRVector2 FromIVector2([NotNull] IVector2 IRVect) => new IRVector2(IRVect.x, IRVect.y);



        /// <summary>
        /// Converts an integer vector into a System.Numerics vector using the Vector2(float, float) constructor.
        /// </summary>
        /// <param name="IVect">The integer vector to be converted.</param>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public static Vector2 ToVector2([NotNull] IRVector2 IVect) => new Vector2(IVect.x, IVect.y);
        /// <summary>
        /// Converts this integer vector into a System.Numerics vector using the Vector2(float, float) constructor.
        /// </summary>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public readonly Vector2 ToVector2() => ToVector2(this);
        /// <summary>
        /// Creates an integer vector from a given System.Numerics Vector2.
        /// </summary>
        /// <returns>A ShortTools.General.IRVector2 struct.</returns>
        public static IRVector2 FromVector2([NotNull] Vector2 Vect) => new IRVector2(Vect.X, Vect.Y);


        /// <summary>
        /// Converts this integer vector into a System tuple using the Tuple(int, int) constructor.
        /// </summary>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public static Tuple<int, int> ToTuple([NotNull] IRVector2 IVect) => new Tuple<int, int>(IVect.x, IVect.y);
        /// <summary>
        /// Converts this integer vector into a System tuple using the Tuple(int, int) constructor.
        /// </summary>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public readonly Tuple<int, int> ToTuple() => ToTuple(this);
        /// <summary>
        /// Converts this integer vector into a System tuple using the Vector2(float, float) constructor.
        /// </summary>
        /// <returns>A System.Numerics Vector2 struct.</returns>
        public static IRVector2 FromTuple([NotNull] Tuple<int, int> tuple) => new IRVector2(tuple.Item1, tuple.Item2);
        #endregion




        readonly string thisString;
        /// <summary>
        /// Converts the vector to the form (x, y)
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString()
        {
            return thisString;
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
        public static IRVector2 operator +(IRVector2 left, IRVector2 right) => Add(left, right);
        /// <summary>
        /// Adds the 2 vectors using regular vector addition.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IRVector2 Add(IRVector2 left, IRVector2 right) => new IRVector2(left.x + right.x, left.y + right.y);
        /// <summary>
        /// Adds this vector to the other vector using the Add function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly IRVector2 Add(IRVector2 right) => Add(this, right);


        /// <summary>
        /// Subtracts the 2 vectors via the Subtract function, doing left - right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IRVector2 operator -(IRVector2 left, IRVector2 right) => Subtract(left, right);
        /// <summary>
        /// Subtracts the 2 vectors via normal vector subtraction, doing left - right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IRVector2 Subtract(IRVector2 left, IRVector2 right) => new IRVector2(left.x - right.x, left.y - right.y);
        /// <summary>
        /// Subtracts the given vector from this vector, doing this - right using the Subtract function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly IRVector2 Subtract(IRVector2 right) => Subtract(this, right);


        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 operator *(IRVector2 vect, int mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 operator *(IRVector2 vect, double mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 operator *(IRVector2 vect, float mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 Multiply(IRVector2 vect, int mult) => new IRVector2(vect.x * mult, vect.y * mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 Multiply(IRVector2 vect, double mult) => new IRVector2(vect.x * mult, vect.y * mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 Multiply(IRVector2 vect, float mult) => new IRVector2(vect.x * mult, vect.y * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IRVector2 Multiply(int mult) => new IRVector2(x * mult, y * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IRVector2 Multiply(double mult) => new IRVector2(x * mult, y * mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IRVector2 Multiply(float mult) => new IRVector2(x * mult, y * mult);


        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 operator /(IRVector2 vect, int mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 operator /(IRVector2 vect, double mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IRVector2 operator /(IRVector2 vect, float mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IRVector2 Divide(IRVector2 vect, int divisor) => new IRVector2(vect.x / divisor, vect.y / divisor);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IRVector2 Divide(IRVector2 vect, double divisor) => new IRVector2(vect.x / divisor, vect.y / divisor);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IRVector2 Divide(IRVector2 vect, float divisor) => new IRVector2(vect.x / divisor, vect.y / divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IRVector2 Divide(int divisor) => Divide(this, divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IRVector2 Divide(double divisor) => Divide(this, divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IRVector2 Divide(float divisor) => Divide(this, divisor);

        #endregion


        #region Cross and Dot product

        /// <summary>
        /// Calculates the cross product of the 2 vectors via (x1, y1) x (x2, y2) = x1y2 - y1x2.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int CrossProduct(IRVector2 left, IRVector2 right) => (left.x * right.y) - (left.y * right.x);
        /// <summary>
        /// Calculates the cross product of a vector and this via the CrossProduct function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly int CrossProduct(IRVector2 right) => CrossProduct(this, right);


        /// <summary>
        /// Calculates the dot product of the 2 given vectors via (x1, y1) * (x2, y2) = x1x2 + y1y2.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int DotProduct(IRVector2 left, IRVector2 right) => (left.x * right.x) + (left.y * right.y);
        /// <summary>
        /// Calculates the dot product of the given vector and this via the DotProduct function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly int DotProduct(IRVector2 right) => DotProduct(this, right);




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

            if (obj is IRVector2 vect)
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
        public readonly bool Equals(IRVector2 other)
        {
            return x == other.x && y == other.y;
        }




        /// <summary>
        /// Returns true if both the x and the y values are equal in both the given vectors.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(IRVector2 left, IRVector2 right) => left.x == right.x && left.y == right.y;
        /// <summary>
        /// Returns false if either the x or the y values are not equal in the given vectors.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(IRVector2 left, IRVector2 right) => left.x != right.x || left.y != right.y;


        /// <summary>
        /// Compares the magnitudes of the 2 vectors.
        /// </summary>
        /// <param name="left">The first vector to be compared</param>
        /// <param name="right">The second vector to be compared</param>
        /// <returns>True, if the magnitude of the left value is less than the magnitude of the right</returns>
        public static bool operator <(IRVector2 left, IRVector2 right) => left.MagnitudeSquared < right.MagnitudeSquared;
        /// <summary><inheritdoc cref="operator {(IRVector2, IRVector2)"/></summary>
        /// <returns>True, if the magnitude of the left value is less than or equal to the magnitude of the right</returns>
        public static bool operator <=(IRVector2 left, IRVector2 right) => left.MagnitudeSquared <= right.MagnitudeSquared;
        /// <summary><inheritdoc cref="operator {(IRVector2, IRVector2)"/></summary>
        /// <returns>True, if the magnitude of the left value is greater than than the magnitude of the right</returns>
        public static bool operator >(IRVector2 left, IRVector2 right) => left.MagnitudeSquared > right.MagnitudeSquared;
        /// <summary><inheritdoc cref="operator {(IRVector2, IRVector2)"/></summary>
        /// <returns>True, if the magnitude of the left value is greater than than or equal to the magnitude of the right</returns>
        public static bool operator >=(IRVector2 left, IRVector2 right) => left.MagnitudeSquared >= right.MagnitudeSquared;


        #endregion


        #region Indexing
        /*
        public T? UseIndex<T>([NotNullIfNotNull(nameof(data))] IList<IList<T>> data)
        {
            if (data is null) { return default; }
            return data[x][y];
        }

        public T? UseIndex<T>([NotNullIfNotNull(nameof(data))] this IList<IList<T>> data, IRVector2 vector)
        {
            if (data is null) { return default; }
            return data[vector.x][vector.y];
        }
        */
        #endregion


        #region Others

#pragma warning disable CA1051

        /// <summary>
        /// 
        /// </summary>
        public readonly int MagnitudeSquared;
        /// <summary>
        /// 
        /// </summary>
        public readonly float Magnitude;
        /// <summary>
        /// 
        /// </summary>
        public readonly int RoughMag;

        /// <summary>
        /// Unused variable, use <see cref="Magnitude">Magnitude</see> instead.
        /// </summary>
        [Obsolete("This is not used, use Magnitude instead.", true)]
        public readonly int? Length = null;



        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private readonly void GenVariables(out float mag, out int magSquared, out int roughMag, out string thisString)
        {
            magSquared = DotProduct(this);
            mag = MathF.Sqrt(magSquared);
            roughMag = Math.Abs(x) + Math.Abs(y);
            thisString = $"({x}, {y})";
        }


        #endregion






        #region Cloning

        /// <summary>
        /// Clones the IRVector2, returning a copy with the same x and y values.
        /// </summary>
        /// <returns></returns>
        public readonly object Clone()
        {
            return new IRVector2(x, y, Magnitude, MagnitudeSquared, RoughMag, thisString);
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
            float dist = Magnitude - other.Length();
            if (dist < 0) { return -1; }
            else if (dist > 0) { return 1; }
            return 0;
        }


        /// <inheritdoc cref="CompareTo(Vector2)"/>
        public readonly int CompareTo(IRVector2 other)
        {
            float dist = Magnitude - other.Magnitude;
            if (dist < 0) { return -1; }
            else if (dist > 0) { return 1; }
            return 0;
        }


        /// <inheritdoc cref="CompareTo(Vector2)"/>
        public readonly int CompareTo(IVector2 other)
        {
            float dist = Magnitude - other.Magnitude();
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
            Print(new IRVector2(1, 4).ToString("wow so many cool words this is so cool!!! ({x} + {y})", CultureInfo.InvariantCulture));
        }

        #endregion
    }
}