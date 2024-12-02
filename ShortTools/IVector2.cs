using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;



namespace ShortTools.General
{
    /// <summary>
    /// An integer vector, contains 2 values and functions like the dot product can be calculated.
    /// </summary>
    public struct IVector2 : IEquatable<IVector2>
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


        #region Equality

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

        #endregion
    }
}
