using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using static ShortTools.General.Prints;




namespace ShortTools.General.Vectors
{
    /// <summary>
    /// An integer vector, contains 3 values and functions like the dot product can be calculated.
    /// </summary>
    public readonly struct IVectorn : IEquatable<IVectorn>, ICloneable, IComparable<IVectorn>
    {
        #region data values

        /// <summary>
        /// The dimention of the vector.
        /// </summary>
        public readonly int Dimension { get => data.Length; }
        readonly int[] data = Array.Empty<int>();

        /// <summary>
        /// <inheritdoc cref="GetValue(int)"/>
        /// There is no error checking in this version to speed it up, if you want error checking do GetValue.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly int this[int index] { get => data[index]; set => data[index] = value; }

        /// <summary>
        /// Gets the data from the vector, where if it were n = 3, then an index of 0 would be x, 1 would be y, and 2 would be z. <br/>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public readonly int? GetValue(int index) 
        {  
            if (index < 0) { return null; }

            if (data is null || data.Length == 0) { return null; }
            if (data.Length <= index) { return null; }

            return data[index];

        }

        #endregion


        #region Constructors
        /// <summary>
        /// Default constructor, initialises all data to the data inputed.
        /// </summary>
        public IVectorn(int dimentions, int defaultData = 0)
        {
            data = Enumerable.Repeat(defaultData, dimentions).ToArray();
        }
        /// <summary>
        /// A params constructor, that sets the data and dimentions to the data inputed.
        /// </summary>
        /// <param name="data"></param>
        public IVectorn(params int[] data)
        {
            this.data = data;
        }

        #endregion


        #region Converters

        /// <inheritdoc cref="ToInt32Array(IVectorn)"/>
        public static explicit operator int[]([NotNull] IVectorn IVect) => ToInt32Array(IVect);
        /// <summary>
        /// Converts the given vector to an array.
        /// </summary>
        /// <param name="IVect"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int[] ToInt32Array(IVectorn IVect) => IVect.data;



        /// <inheritdoc cref="FromInt32Array(int[])"/>
        public static explicit operator IVectorn([NotNull] int[] data) => FromInt32Array(data);
        /// <summary>
        /// Converts the given array to a vector.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static IVectorn FromInt32Array(int[] data) => new IVectorn(data);

        #endregion



        /// <summary>
        /// Converts the vector to the form (x, y, z)
        /// </summary>
        /// <returns></returns>
        public readonly override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('(');

            foreach (int value in data)
            {
                builder.Append(value);
                if (data.Last() != value)
                {
                    builder.Append(", ");
                }
            }

            builder.Append(')');
            return builder.ToString();
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
        public static IVectorn operator +(IVectorn left, IVectorn right) => Add(left, right);
        /// <summary>
        /// Adds the 2 vectors using regular vector addition.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="DifferentVectorSizeException">Thrown when the 2 vectors are different dimensions.</exception>
        public static IVectorn Add(IVectorn left, IVectorn right) 
        { 
            if (left.Dimension != right.Dimension) 
            { throw new DifferentVectorSizeException($"The left vector of dimension {left.Dimension
                }, and the right vector of dimension {right.Dimension} have different sizes, so cannot be added."); 
            }

            for (int i = 0; i < right.Dimension; i++)
            {
                left[i] += right[i];
            }

            return left;
        }
        /// <summary>
        /// Adds this vector to the other vector using the Add function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly IVectorn Add(IVectorn right) => Add(this, right);


        /// <summary>
        /// Subtracts the 2 vectors via the Subtract function, doing left - right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IVectorn operator -(IVectorn left, IVectorn right) => Subtract(left, right);
        /// <summary>
        /// Subtracts the 2 vectors via normal vector subtraction, doing left - right.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <exception cref="DifferentVectorSizeException">Thrown when the 2 vectors are different dimensions.</exception>
        /// <returns></returns>
        public static IVectorn Subtract(IVectorn left, IVectorn right)
        {
            if (left.Dimension != right.Dimension)
            {
                throw new DifferentVectorSizeException($"The left vector of dimension {left.Dimension
                    }, and the right vector of dimension {right.Dimension} have different sizes, so cannot be added.");
            }

            for (int i = 0; i < right.Dimension; i++)
            {
                left[i] -= right[i];
            }

            return left;
        }
        /// <summary>
        /// Subtracts the given vector from this vector, doing this - right using the Subtract function.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public readonly IVectorn Subtract(IVectorn right) => Subtract(this, right);


        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn operator *(IVectorn vect, int mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn operator *(IVectorn vect, double mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar using the Multiply function.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn operator *(IVectorn vect, float mult) => Multiply(vect, mult);
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn Multiply(IVectorn vect, int mult)
        {
            for (int i = 0; i < vect.Dimension; i++)
            {
                vect[i] *= mult;
            }

            return vect;
        }
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn Multiply(IVectorn vect, double mult)
        {
            for (int i = 0; i < vect.Dimension; i++)
            {
                vect[i] = (int)(vect[i] * mult);
            }

            return vect;
        }
        /// <summary>
        /// Multiplies the Vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn Multiply(IVectorn vect, float mult)
        {
            for (int i = 0; i < vect.Dimension; i++)
            {
                vect[i] = (int)(vect[i] * mult);
            }

            return vect;
        }
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVectorn Multiply(int mult) => Multiply(this, mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVectorn Multiply(double mult) => Multiply(this, mult);
        /// <summary>
        /// Multiplies this Vector by a scalar.
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public readonly IVectorn Multiply(float mult) => Multiply(this, mult);


        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn operator /(IVectorn vect, int mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn operator /(IVectorn vect, double mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static IVectorn operator /(IVectorn vect, float mult) => Divide(vect, mult);
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVectorn Divide(IVectorn vect, int divisor)
        {
            for (int i = 0; i < vect.Dimension; i++)
            {
                vect[i] /= divisor;
            }

            return vect;
        }
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVectorn Divide(IVectorn vect, double divisor)
        {
            for (int i = 0; i < vect.Dimension; i++)
            {
                vect[i] = (int)(vect[i] / divisor);
            }

            return vect;
        }
        /// <summary>
        /// Divides the vector by a scalar.
        /// </summary>
        /// <param name="vect"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IVectorn Divide(IVectorn vect, float divisor)
        {
            for (int i = 0; i < vect.Dimension; i++)
            {
                vect[i] = (int)(vect[i] / divisor);
            }

            return vect;
        }
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVectorn Divide(int divisor) => Divide(this, divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVectorn Divide(double divisor) => Divide(this, divisor);
        /// <summary>
        /// Divides this vector by a scalar.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public readonly IVectorn Divide(float divisor) => Divide(this, divisor);

        #endregion


        #region Cross and Dot product

        /// <summary>
        /// Only works forn = 3 as i dont know how to do it for any higher dimensions.
        /// </summary>
        [Obsolete(
            "Do not use if n != 3, i dont know how to make this work for any dimentions higher than 3. Also this is inefficient if you are using it for n = 3, use a IVector3 instead.", 
            false, DiagnosticId = WarningCodes.NotFullyCoded, UrlFormat = WarningCodes.URL)]
        public static Vector3 CrossProduct(IVectorn left, IVectorn right)
        {
            if (left.Dimension != right.Dimension) {
                throw new DifferentVectorSizeException($"The left vector of dimension {left.Dimension}, and the right vector of dimension {right.Dimension
                    } have different sizes, so cannot be added.");
            }

            if (left.Dimension == 3)
            {
                IVector3 left3 = new IVector3(left[0], left[1], left[2]);
                IVector3 right3 = new IVector3(right[0], right[1], right[2]);

                return left3.CrossProduct(right3);
            }
            else
            {
                throw new ShortException(ErrorCode.VectorsAreNot3D, "The vectors were not a dimention of 3, which is the only functional version for this cross product function");
            }
        }
        /// <inheritdoc cref="CrossProduct(IVectorn, IVectorn)"/>
        [Obsolete(
            "Do not use if n > 3, i dont know how to make this work for any dimentions higher than 3. Also this is inefficient if you are using it for n = 3, use a IVector3 instead.", 
            false, DiagnosticId = WarningCodes.NotFullyCoded, UrlFormat = WarningCodes.URL)]
        public readonly Vector3 CrossProduct(IVectorn right) => CrossProduct(this, right);


        /// <summary>
        /// Calculates the dot product of the 2 given vectors via (x1, y1, z1) * (x2, y2, z2) = x1x2 + y1y2 + z1z2.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int DotProduct(IVectorn left, IVectorn right)
        {
            if (left.Dimension != right.Dimension) {
                throw new DifferentVectorSizeException($"The left vector of dimension {left.Dimension}, and the right vector of dimension {right.Dimension
                    } have different sizes, so cannot be added.");
            }


            int sum = 0;
            for (int i = 0; i < left.Dimension; i++)
            {
                sum += left[i] * right[i];
            }
            return sum;
        }
        /// <inheritdoc cref="DotProduct(IVectorn, IVectorn)"/>
        public readonly int DotProduct(IVectorn right) => DotProduct(this, right);




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

            if (obj is IVectorn vect)
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
        public readonly bool Equals(IVectorn other)
        {
            if (Dimension != other.Dimension) { return false; }
            for (int i = 0; i < Dimension; i++)
            {
                if (this[i] != other[i]) { return false; }
            }
            return true;
        }




        /// <summary>
        /// Returns true if both the x and the y values are equal in both the given vectors.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(IVectorn left, IVectorn right) => left.Equals(right);
        /// <summary>
        /// Returns false if either the x or the y values are not equal in the given vectors.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(IVectorn left, IVectorn right) => !left.Equals(right);


        /// <summary>
        /// Compares the magnitudes of the 2 vectors.
        /// </summary>
        /// <param name="left">The first vector to be compared</param>
        /// <param name="right">The second vector to be compared</param>
        /// <returns>True, if the magnitude of the left value is less than the magnitude of the right</returns>
        public static bool operator <(IVectorn left, IVectorn right) => left.MagSquared() < right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVectorn, IVectorn)"/></summary>
        /// <returns>True, if the magnitude of the left value is less than or equal to the magnitude of the right</returns>
        public static bool operator <=(IVectorn left, IVectorn right) => left.MagSquared() <= right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVectorn, IVectorn)"/></summary>
        /// <returns>True, if the magnitude of the left value is greater than than the magnitude of the right</returns>
        public static bool operator >(IVectorn left, IVectorn right) => left.MagSquared() > right.MagSquared();
        /// <summary><inheritdoc cref="operator {(IVectorn, IVectorn)"/></summary>
        /// <returns>True, if the magnitude of the left value is greater than than or equal to the magnitude of the right</returns>
        public static bool operator >=(IVectorn left, IVectorn right) => left.MagSquared() >= right.MagSquared();


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
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public readonly int RoughMag()
        {
            int sum = 0;
            for (int i = 0; i < Dimension; i++)
            {
                sum += Math.Abs(data[i]);
            }
            return sum;
        }
        /// <inheritdoc cref="RoughMag()"/>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public readonly int RoughMag(IVectorn vector) => (vector - this).RoughMag();

        #endregion






        #region Cloning

        /// <summary>
        /// Clones the IVectorn, returning a copy with the same x, y, and z values.
        /// </summary>
        /// <returns></returns>
        public readonly object Clone()
        {
            return new IVectorn(data);
        }

        #endregion


        #region Comparable

        /// <summary>
        /// Compares the lengths of the 2 vectors. When this vector has a lower length than the parameter vector, -1 will be returned. If the vectors are the same length
        /// then it will return 0. And if the parameter vector is smaller than this vector, then it will return 1.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly int CompareTo(IVectorn other)
        {
            float dist = MagSquared() - other.MagSquared();
            if (dist < 0) { return -1; }
            else if (dist > 0) { return 1; }
            return 0;
        }

        #endregion





        #region Tests

        internal static void Main()
        {

        }

        #endregion
    }






    #region Exceptions



    /// <summary>
    /// An exception for when an issue with vector dimentions occurs in a short tools based program.
    /// </summary>
    public class DifferentVectorSizeException : ShortException
    {

        /// <inheritdoc cref="DifferentVectorSizeException"/>
        /// <param name="message">The message to be displayed.</param>
        public DifferentVectorSizeException(string message) : base(ErrorCode.DifferentVectorSizes, message)
        { }

        /// <inheritdoc cref="DifferentVectorSizeException"/>
        public DifferentVectorSizeException() : base(ErrorCode.DifferentVectorSizes, "The 2 vectors were different sizes.")
        { }

        /// <inheritdoc cref="DifferentVectorSizeException"/>
        /// <param name="message"><inheritdoc cref="DifferentVectorSizeException(string)"/></param>
        /// <param name="innerException"></param>
        public DifferentVectorSizeException(string message, Exception innerException) : base(ErrorCode.DifferentVectorSizes, message, innerException)
        { }
    }



    #endregion
}