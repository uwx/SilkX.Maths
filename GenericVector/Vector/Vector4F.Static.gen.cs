
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace GenericVector.Experimental;

public static class Vector4F
{

    #region CopyTo
    /// <summary>Copies the elements of the vector to a specified array.</summary>
    /// <param name="self">The vector to be copied.</param>
    /// <param name="array">The destination array.</param>
    /// <remarks><paramref name="array" /> must have at least two elements. The method copies the vector's elements starting at index 0.</remarks>
    /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
    /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo<TScalar>(in this Vector4F<TScalar> self, TScalar[] array) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
    
        ArgumentOutOfRangeException.ThrowIfLessThan(array.Length, Vector4F<TScalar>.ElementCount, nameof(array));
    
        Unsafe.WriteUnaligned(ref Unsafe.As<TScalar, byte>(ref array[0]), self);
    }
    
    /// <summary>Copies the elements of the vector to a specified array starting at a specified index position.</summary>
    /// <param name="self">The vector to be copied.</param>
    /// <param name="array">The destination array.</param>
    /// <param name="index">The index at which to copy the first element of the vector.</param>
    /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the two vector elements. In other words, elements <paramref name="index" /> through <paramref name="index" /> + 2 must already exist in <paramref name="array" />.</remarks>
    /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
    /// -or-
    /// <paramref name="index" /> is greater than or equal to the array length.</exception>
    /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo<TScalar>(in this Vector4F<TScalar> self, TScalar[] array, int index) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons
    
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)array.Length);
        ArgumentOutOfRangeException.ThrowIfLessThan((array.Length - index), Vector4F<TScalar>.ElementCount);
    
        Unsafe.WriteUnaligned(ref Unsafe.As<TScalar, byte>(ref array[index]), self);
    }
    
    /// <summary>Copies the vector to the given <see cref="Span{TScalar}" />. The length of the destination span must be at least 2.</summary>
    /// <param name="self">The vector to be copied.</param>
    /// <param name="destination">The destination span which the values are copied into.</param>
    /// <exception cref="ArgumentException">If number of elements in source vector is greater than those available in destination span.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyTo<TScalar>(in this Vector4F<TScalar> self, Span<TScalar> destination) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, Vector4F<TScalar>.ElementCount, nameof(destination));
    
        Unsafe.WriteUnaligned(ref Unsafe.As<TScalar, byte>(ref MemoryMarshal.GetReference(destination)), self);
    }
    
    /// <summary>Attempts to copy the vector to the given <see cref="Span{Single}" />. The length of the destination span must be at least 2.</summary>
    /// <param name="self">The vector to be copied.</param>
    /// <param name="destination">The destination span which the values are copied into.</param>
    /// <returns><see langword="true" /> if the source vector was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCopyTo<TScalar>(in this Vector4F<TScalar> self, Span<TScalar> destination) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        if (destination.Length < Vector4F<TScalar>.ElementCount)
        {
            return false;
        }
    
        Unsafe.WriteUnaligned(ref Unsafe.As<TScalar, byte>(ref MemoryMarshal.GetReference(destination)), self);
        return true;
    }
    #endregion
    
    #region Extension
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<TScalar> AsSpan<TScalar>(this Vector4F<TScalar> vec) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<Vector4F<TScalar>, TScalar>(ref Unsafe.AsRef(in vec)), Vector4F<TScalar>.ElementCount);
    }
    
    #endregion
    
    #region Operator Shortcuts
    
    /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The element-wise product vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Multiply<TScalar>(Vector4F<TScalar> left, Vector4F<TScalar> right) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return left * right;
    }
    
    /// <summary>Multiplies a vector by a specified scalar.</summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Multiply<TScalar>(Vector4F<TScalar> left, TScalar right) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return left * right;
    }
    
    /// <summary>Multiplies a scalar value by a specified vector.</summary>
    /// <param name="left">The scaled value.</param>
    /// <param name="right">The vector.</param>
    /// <returns>The scaled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Multiply<TScalar>(TScalar left, Vector4F<TScalar> right) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return left * right;
    }
    
    /// <summary>Negates a specified vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Negate<TScalar>(Vector4F<TScalar> value) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return -value;
    }
    
    /// <summary>Subtracts the second vector from the first.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The difference vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Subtract<TScalar>(Vector4F<TScalar> left, Vector4F<TScalar> right) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return left - right;
    }
    
    /// <summary>Adds two vectors together.</summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The summed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Add<TScalar>(Vector4F<TScalar> left, Vector4F<TScalar> right) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return left + right;
    }
    
    /// <summary>Divides the first vector by the second.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The vector resulting from the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Divide<TScalar>(Vector4F<TScalar> left, Vector4F<TScalar> right) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return left / right;
    }
    
    /// <summary>Divides the specified vector by a specified scalar value.</summary>
    /// <param name="left">The vector.</param>
    /// <param name="divisor">The scalar value.</param>
    /// <returns>The vector that results from the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Divide<TScalar>(Vector4F<TScalar> left, TScalar divisor) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return left / divisor;
    }
    
    #endregion
    
    #region Other
    
    /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
    /// <param name="value">A vector.</param>
    /// <returns>The absolute value vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Abs<TScalar>(Vector4F<TScalar> value) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return SpeedHelpers2.Abs<Vector4F<TScalar>, TScalar>(value);
    }
    
    /// <summary>Restricts a vector between a minimum and a maximum value.</summary>
    /// <param name="value1">The vector to restrict.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The restricted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Clamp<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> min, Vector4F<TScalar> max) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        // We must follow HLSL behavior in the case user specified min value is bigger than max value.
        return SpeedHelpers2.Clamp<Vector4F<TScalar>, TScalar>(value1, min, max);
    }
    
    /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance squared.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar DistanceSquared<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return SpeedHelpers2.DistanceSquared<Vector4F<TScalar>, TScalar>(value1, value2);
    }
    
    /// <summary>Returns the dot product of two vectors.</summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The dot product.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar Dot<TScalar>(Vector4F<TScalar> vector1, Vector4F<TScalar> vector2) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return SpeedHelpers2.Dot<Vector4F<TScalar>, TScalar>(vector1, vector2);
    }
    
    /// <summary>Returns a vector whose elements are the maximum of each of the pairs of elements in two specified vectors.</summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The maximized vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Max<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return SpeedHelpers2.Max<Vector4F<TScalar>, TScalar>(value1, value2);
    }
    
    /// <summary>Returns a vector whose elements are the minimum of each of the pairs of elements in two specified vectors.</summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The minimized vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Min<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return SpeedHelpers2.Min<Vector4F<TScalar>, TScalar>(value1, value2);
    }
    
    // CANNOT BE DONE
    /*
    /// <summary>Transforms a vector by a specified 4x4 matrix.</summary>
    /// <param name="position">The vector to transform.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Transform<TScalar>(Vector4F<TScalar> position, Matrix4X4<TScalar> matrix) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return (Vector4F<TScalar>)Vector4D.Transform(position, matrix);
    }
    
    /// <summary>Transforms a vector by the specified Quaternion rotation value.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TReturn> Transform<TScalar, TQuat, TReturn>(Vector4F<TScalar> value, Quaternion<TQuat> rotation) where TScalar : IBinaryFloatingPointIeee754<TScalar> where TReturn : INumberBase<TReturn> where TQuat : ITrigonometricFunctions<TQuat>, IRootFunctions<TQuat>
    {
        var  = rotation.X + rotation.X;
        var  = rotation.Y + rotation.Y;var x2 = rotation.X + rotation.X;
        var y2 = rotation.Y + rotation.Y;
        var z2 = rotation.Z + rotation.Z;
    
    
        var  = rotation.X + rotation.X;
        var  = rotation.Y + rotation.Y;var wx2 = TReturn.CreateTruncating(rotation.W * x2);
        var wy2 = TReturn.CreateTruncating(rotation.W * y2);
        var wz2 = TReturn.CreateTruncating(rotation.W * z2);
        var xx2 = TReturn.CreateTruncating(rotation.X * x2);
        var xy2 = TReturn.CreateTruncating(rotation.X * y2);
        var xz2 = TReturn.CreateTruncating(rotation.X * z2);
        var yy2 = TReturn.CreateTruncating(rotation.Y * y2);
        var yz2 = TReturn.CreateTruncating(rotation.Y * z2);
        var zz2 = TReturn.CreateTruncating(rotation.Z * z2);
    
        return new Vector4F<TReturn>(
            TReturn.Sqrt(TReturn.CreateTruncating(value.X)),
            TReturn.Sqrt(TReturn.CreateTruncating(value.Y))
        );
    
        return new Vector4F<TReturn>(
            TReturn.CreateTruncating(value.X) * (TReturn.One - yy2 - zz2) + TReturn.CreateTruncating(value.Y) * (xy2 - wz2) + TReturn.CreateTruncating(value.Z) * (xz2 + wy2),
            TReturn.CreateTruncating(value.X) * (xy2 + wz2) + TReturn.CreateTruncating(value.Y) * (TReturn.One - xx2 - zz2) + TReturn.CreateTruncating(value.Z) * (yz2 - wx2),
            TReturn.CreateTruncating(value.X) * (xz2 - wy2) + TReturn.CreateTruncating(value.Y) * (yz2 + wx2) + TReturn.CreateTruncating(value.Z) * (TReturn.One - xx2 - yy2)
        );
    }
    
    // /// <summary>Transforms a vector normal by the given 4x4 matrix.</summary>
    // /// <param name="normal">The source vector.</param>
    // /// <param name="matrix">The matrix.</param>
    // /// <returns>The transformed vector.</returns>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // internal static Vector4F<TScalar> TransformNormal<TScalar>(Vector4F<TScalar> normal, in Matrix4x4 matrix) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    // {
    //     var matrixX = new Vector4(matrix.M11, matrix.M12, matrix.M13, matrix.M14);
    //     var matrixY = new Vector4(matrix.M21, matrix.M22, matrix.M23, matrix.M24);
    //     var matrixZ = new Vector4(matrix.M31, matrix.M32, matrix.M33, matrix.M34);
    //     // var matrixW = new Vector4(matrix.M41, matrix.M42, matrix.M43, matrix.M44);
    //
    //     var result = matrixX * normal.X;
    //     result += matrixY * normal.Y;
    //     result += matrixZ * normal.Z;
    //     return result.AsVector128().AsVector3();
    // }
    */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Remainder<TScalar>(this Vector4F<TScalar> left, Vector4F<TScalar> right) where TScalar : IBinaryFloatingPointIeee754<TScalar>, IModulusOperators<TScalar, TScalar, TScalar>
    {
        return SpeedHelpers2.Remainder<Vector4F<TScalar>, TScalar>(left, right);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Remainder<TScalar>(this Vector4F<TScalar> left, TScalar right) where TScalar : IBinaryFloatingPointIeee754<TScalar>, IModulusOperators<TScalar, TScalar, TScalar>
    {
        return SpeedHelpers2.Remainder<Vector4F<TScalar>, TScalar>(left, right);
    }
    #endregion
    
    #region Specializations
    
    /// <summary>Returns the length of the vector squared.</summary>
    /// <returns>The vector's length squared.</returns>
    /// <remarks>This operation offers better performance than a call to the <see cref="Length{TScalar}" /> method.</remarks>
    /// <altmember cref="Length{TScalar}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar LengthSquared<TScalar>(this Vector4F<TScalar> vec) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return SpeedHelpers2.LengthSquared<Vector4F<TScalar>, TScalar>(vec);
    }
    #endregion
    
    // INumber<TScalar>
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> CopySign<TScalar>(Vector4F<TScalar> value, Vector4F<TScalar> sign) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.CopySign(value.X, sign.X), TScalar.CopySign(value.Y, sign.Y), TScalar.CopySign(value.Z, sign.Z), TScalar.CopySign(value.W, sign.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> CopySign<TScalar>(Vector4F<TScalar> value, TScalar sign) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.CopySign(value.X, sign), TScalar.CopySign(value.Y, sign), TScalar.CopySign(value.Z, sign), TScalar.CopySign(value.W, sign));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> MaxNumber<TScalar>(Vector4F<TScalar> x, Vector4F<TScalar> y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.MaxNumber(x.X, y.X), TScalar.MaxNumber(x.Y, y.Y), TScalar.MaxNumber(x.Z, y.Z), TScalar.MaxNumber(x.W, y.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> MinNumber<TScalar>(Vector4F<TScalar> x, Vector4F<TScalar> y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.MinNumber(x.X, y.X), TScalar.MinNumber(x.Y, y.Y), TScalar.MinNumber(x.Z, y.Z), TScalar.MinNumber(x.W, y.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Sign<TScalar>(Vector4F<TScalar> value) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.CreateChecked(TScalar.Sign(value.X)), TScalar.CreateChecked(TScalar.Sign(value.Y)), TScalar.CreateChecked(TScalar.Sign(value.Z)), TScalar.CreateChecked(TScalar.Sign(value.W)));


    // Float-specific stuff
    /// <summary>Returns the length of this vector object.</summary>
    /// <returns>The vector's length.</returns>
    /// <altmember cref="LengthSquared{TScalar}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar Length<TScalar>(this Vector4F<TScalar> vec) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        var lengthSquared = vec.LengthSquared();
        return TScalar.Sqrt(lengthSquared);
    }

    /// <summary>Computes the Euclidean distance between the two given points.</summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TScalar Distance<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        var distanceSquared = DistanceSquared(value1, value2);
        return TScalar.Sqrt(distanceSquared);
    }

    /// <summary>Performs a linear interpolation between two vectors based on the given weighting.</summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">A value between 0 and 1 that indicates the weight of <paramref name="value2" />.</param>
    /// <returns>The interpolated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Lerp<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2, TScalar amount) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return (value1 * (TScalar.One - amount)) + (value2 * amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4F<TScalar> LerpUnchecked<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2, TScalar amount) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return (value1 * (TScalar.One - amount)) + (value2 * amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> LerpClamped<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2, TScalar amount) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        amount = TScalar.Clamp(amount, TScalar.Zero, TScalar.One);
        return Lerp(value1, value2, amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4F<TScalar> LerpClampedUnchecked<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2, TScalar amount) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static TScalar ClampT(TScalar value, TScalar min, TScalar max)
        {
            return TScalar.MaxMagnitude(TScalar.MaxMagnitude(value, min), max);
        }

        amount = ClampT(amount, TScalar.Zero, TScalar.One);
        return LerpUnchecked(value1, value2, amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Lerp<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2, Vector4F<TScalar> amount) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return (value1 * (Vector4F<TScalar>.One - amount)) + (value2 * amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4F<TScalar> LerpUnchecked<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2, Vector4F<TScalar> amount) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return (value1 * (Vector4F<TScalar>.One - amount)) + (value2 * amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> LerpClamped<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2, Vector4F<TScalar> amount) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        amount = Clamp(amount, Vector4F<TScalar>.Zero, Vector4F<TScalar>.One);
        return Lerp(value1, value2, amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4F<TScalar> LerpClampedUnchecked<TScalar>(Vector4F<TScalar> value1, Vector4F<TScalar> value2, Vector4F<TScalar> amount) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        amount = Clamp(amount, Vector4F<TScalar>.Zero, Vector4F<TScalar>.One);
        return LerpUnchecked(value1, value2, amount);
    }

    /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Normalize<TScalar>(Vector4F<TScalar> value) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return value / value.Length();
    }

    /// <summary>Returns the reflection of a vector off a surface that has the specified normal.</summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="normal">The normal of the surface being reflected off.</param>
    /// <returns>The reflected vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Reflect<TScalar>(Vector4F<TScalar> vector, Vector4F<TScalar> normal) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        var dot = Dot(vector, normal);
        return vector - (Scalar<TScalar>.Two * (dot * normal));
    }

    /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
    /// <param name="value">A vector.</param>
    /// <returns>The square root vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4F<TScalar> Sqrt<TScalar>(Vector4F<TScalar> value) where TScalar : IBinaryFloatingPointIeee754<TScalar>
    {
        return new Vector4F<TScalar>(
            TScalar.Sqrt(value.X),
            TScalar.Sqrt(value.Y)
        );
    }

    // Even more float-specific stuff
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Acosh<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Acosh(x.X), TScalar.Acosh(x.Y), TScalar.Acosh(x.Z), TScalar.Acosh(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Asinh<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Asinh(x.X), TScalar.Asinh(x.Y), TScalar.Asinh(x.Z), TScalar.Asinh(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Atanh<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Atanh(x.X), TScalar.Atanh(x.Y), TScalar.Atanh(x.Z), TScalar.Atanh(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Cosh<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Cosh(x.X), TScalar.Cosh(x.Y), TScalar.Cosh(x.Z), TScalar.Cosh(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Sinh<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Sinh(x.X), TScalar.Sinh(x.Y), TScalar.Sinh(x.Z), TScalar.Sinh(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Tanh<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Tanh(x.X), TScalar.Tanh(x.Y), TScalar.Tanh(x.Z), TScalar.Tanh(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Acos<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Acos(x.X), TScalar.Acos(x.Y), TScalar.Acos(x.Z), TScalar.Acos(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> AcosPi<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.AcosPi(x.X), TScalar.AcosPi(x.Y), TScalar.AcosPi(x.Z), TScalar.AcosPi(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Asin<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Asin(x.X), TScalar.Asin(x.Y), TScalar.Asin(x.Z), TScalar.Asin(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> AsinPi<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.AsinPi(x.X), TScalar.AsinPi(x.Y), TScalar.AsinPi(x.Z), TScalar.AsinPi(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Atan<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Atan(x.X), TScalar.Atan(x.Y), TScalar.Atan(x.Z), TScalar.Atan(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> AtanPi<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.AtanPi(x.X), TScalar.AtanPi(x.Y), TScalar.AtanPi(x.Z), TScalar.AtanPi(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Cos<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Cos(x.X), TScalar.Cos(x.Y), TScalar.Cos(x.Z), TScalar.Cos(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> CosPi<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.CosPi(x.X), TScalar.CosPi(x.Y), TScalar.CosPi(x.Z), TScalar.CosPi(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> DegreesToRadians<TScalar>(Vector4F<TScalar> degrees) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.DegreesToRadians(degrees.X), TScalar.DegreesToRadians(degrees.Y), TScalar.DegreesToRadians(degrees.Z), TScalar.DegreesToRadians(degrees.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> RadiansToDegrees<TScalar>(Vector4F<TScalar> radians) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.RadiansToDegrees(radians.X), TScalar.RadiansToDegrees(radians.Y), TScalar.RadiansToDegrees(radians.Z), TScalar.RadiansToDegrees(radians.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Sin<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Sin(x.X), TScalar.Sin(x.Y), TScalar.Sin(x.Z), TScalar.Sin(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> SinPi<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.SinPi(x.X), TScalar.SinPi(x.Y), TScalar.SinPi(x.Z), TScalar.SinPi(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Tan<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Tan(x.X), TScalar.Tan(x.Y), TScalar.Tan(x.Z), TScalar.Tan(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> TanPi<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.TanPi(x.X), TScalar.TanPi(x.Y), TScalar.TanPi(x.Z), TScalar.TanPi(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static (Vector4F<TScalar> Sin, Vector4F<TScalar> Cos) SinCos<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => (
        new(TScalar.Sin(x.X), TScalar.Sin(x.Y), TScalar.Sin(x.Z), TScalar.Sin(x.W)),
        new(TScalar.Cos(x.X), TScalar.Cos(x.Y), TScalar.Cos(x.Z), TScalar.Cos(x.W))
    );
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static (Vector4F<TScalar> SinPi, Vector4F<TScalar> CosPi) SinCosPi<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => (
        new(TScalar.SinPi(x.X), TScalar.SinPi(x.Y), TScalar.SinPi(x.Z), TScalar.SinPi(x.W)),
        new(TScalar.CosPi(x.X), TScalar.CosPi(x.Y), TScalar.CosPi(x.Z), TScalar.CosPi(x.W))
    );
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Log<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Log(x.X), TScalar.Log(x.Y), TScalar.Log(x.Z), TScalar.Log(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Log<TScalar>(Vector4F<TScalar> x, Vector4F<TScalar> newBase) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Log(x.X, newBase.X), TScalar.Log(x.Y, newBase.Y), TScalar.Log(x.Z, newBase.Z), TScalar.Log(x.W, newBase.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Log<TScalar>(Vector4F<TScalar> x, TScalar newBase) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Log(x.X, newBase), TScalar.Log(x.Y, newBase), TScalar.Log(x.Z, newBase), TScalar.Log(x.W, newBase));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> LogP1<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.LogP1(x.X), TScalar.LogP1(x.Y), TScalar.LogP1(x.Z), TScalar.LogP1(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] private static TScalar Log2ILogarithmicFunctions<TScalar>(TScalar x) where TScalar : ILogarithmicFunctions<TScalar> => TScalar.Log2(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Log2<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(Log2ILogarithmicFunctions(x.X), Log2ILogarithmicFunctions(x.Y), Log2ILogarithmicFunctions(x.Z), Log2ILogarithmicFunctions(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Log2P1<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Log2P1(x.X), TScalar.Log2P1(x.Y), TScalar.Log2P1(x.Z), TScalar.Log2P1(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Log10<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Log10(x.X), TScalar.Log10(x.Y), TScalar.Log10(x.Z), TScalar.Log10(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Log10P1<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Log10P1(x.X), TScalar.Log10P1(x.Y), TScalar.Log10P1(x.Z), TScalar.Log10P1(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Exp<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Exp(x.X), TScalar.Exp(x.Y), TScalar.Exp(x.Z), TScalar.Exp(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> ExpM1<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.ExpM1(x.X), TScalar.ExpM1(x.Y), TScalar.ExpM1(x.Z), TScalar.ExpM1(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Exp2<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Exp2(x.X), TScalar.Exp2(x.Y), TScalar.Exp2(x.Z), TScalar.Exp2(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Exp2M1<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Exp2M1(x.X), TScalar.Exp2M1(x.Y), TScalar.Exp2M1(x.Z), TScalar.Exp2M1(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Exp10<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Exp10(x.X), TScalar.Exp10(x.Y), TScalar.Exp10(x.Z), TScalar.Exp10(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Exp10M1<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Exp10M1(x.X), TScalar.Exp10M1(x.Y), TScalar.Exp10M1(x.Z), TScalar.Exp10M1(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Pow<TScalar>(Vector4F<TScalar> x, Vector4F<TScalar> y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Pow(x.X, y.X), TScalar.Pow(x.Y, y.Y), TScalar.Pow(x.Z, y.Z), TScalar.Pow(x.W, y.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Pow<TScalar>(Vector4F<TScalar> x, TScalar y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Pow(x.X, y), TScalar.Pow(x.Y, y), TScalar.Pow(x.Z, y), TScalar.Pow(x.W, y));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Cbrt<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Cbrt(x.X), TScalar.Cbrt(x.Y), TScalar.Cbrt(x.Z), TScalar.Cbrt(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Hypot<TScalar>(Vector4F<TScalar> x, Vector4F<TScalar> y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Hypot(x.X, y.X), TScalar.Hypot(x.Y, y.Y), TScalar.Hypot(x.Z, y.Z), TScalar.Hypot(x.W, y.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Hypot<TScalar>(Vector4F<TScalar> x, TScalar y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Hypot(x.X, y), TScalar.Hypot(x.Y, y), TScalar.Hypot(x.Z, y), TScalar.Hypot(x.W, y));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> RootN<TScalar>(Vector4F<TScalar> x, int n) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.RootN(x.X, n), TScalar.RootN(x.Y, n), TScalar.RootN(x.Z, n), TScalar.RootN(x.W, n));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Round<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Round(x.X), TScalar.Round(x.Y), TScalar.Round(x.Z), TScalar.Round(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Round<TScalar>(Vector4F<TScalar> x, int digits) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Round(x.X, digits), TScalar.Round(x.Y, digits), TScalar.Round(x.Z, digits), TScalar.Round(x.W, digits));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Round<TScalar>(Vector4F<TScalar> x, MidpointRounding mode) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Round(x.X, mode), TScalar.Round(x.Y, mode), TScalar.Round(x.Z, mode), TScalar.Round(x.W, mode));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Round<TScalar>(Vector4F<TScalar> x, int digits, MidpointRounding mode) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Round(x.X, digits, mode), TScalar.Round(x.Y, digits, mode), TScalar.Round(x.Z, digits, mode), TScalar.Round(x.W, digits, mode));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Truncate<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Truncate(x.X), TScalar.Truncate(x.Y), TScalar.Truncate(x.Z), TScalar.Truncate(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Atan2<TScalar>(Vector4F<TScalar> x, Vector4F<TScalar> y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Atan2(x.X, y.X), TScalar.Atan2(x.Y, y.Y), TScalar.Atan2(x.Z, y.Z), TScalar.Atan2(x.W, y.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Atan2Pi<TScalar>(Vector4F<TScalar> x, Vector4F<TScalar> y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Atan2Pi(x.X, y.X), TScalar.Atan2Pi(x.Y, y.Y), TScalar.Atan2Pi(x.Z, y.Z), TScalar.Atan2Pi(x.W, y.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Atan2<TScalar>(Vector4F<TScalar> x, TScalar y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Atan2(x.X, y), TScalar.Atan2(x.Y, y), TScalar.Atan2(x.Z, y), TScalar.Atan2(x.W, y));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> Atan2Pi<TScalar>(Vector4F<TScalar> x, TScalar y) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.Atan2Pi(x.X, y), TScalar.Atan2Pi(x.Y, y), TScalar.Atan2Pi(x.Z, y), TScalar.Atan2Pi(x.W, y));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> BitDecrement<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.BitDecrement(x.X), TScalar.BitDecrement(x.Y), TScalar.BitDecrement(x.Z), TScalar.BitDecrement(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> BitIncrement<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.BitIncrement(x.X), TScalar.BitIncrement(x.Y), TScalar.BitIncrement(x.Z), TScalar.BitIncrement(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> FusedMultiplyAdd<TScalar>(Vector4F<TScalar> left, Vector4F<TScalar> right, Vector4F<TScalar> addend) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.FusedMultiplyAdd(left.X, right.X, addend.X), TScalar.FusedMultiplyAdd(left.Y, right.Y, addend.Y), TScalar.FusedMultiplyAdd(left.Z, right.Z, addend.Z), TScalar.FusedMultiplyAdd(left.W, right.W, addend.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> ReciprocalEstimate<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.ReciprocalEstimate(x.X), TScalar.ReciprocalEstimate(x.Y), TScalar.ReciprocalEstimate(x.Z), TScalar.ReciprocalEstimate(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> ReciprocalSqrtEstimate<TScalar>(Vector4F<TScalar> x) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.ReciprocalSqrtEstimate(x.X), TScalar.ReciprocalSqrtEstimate(x.Y), TScalar.ReciprocalSqrtEstimate(x.Z), TScalar.ReciprocalSqrtEstimate(x.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> ScaleB<TScalar>(Vector4F<TScalar> x, Vector2D<int> n) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.ScaleB(x.X, n.X), TScalar.ScaleB(x.Y, n.Y), TScalar.ScaleB(x.Z, n.Z), TScalar.ScaleB(x.W, n.W));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector4F<TScalar> ScaleB<TScalar>(Vector4F<TScalar> x, int n) where TScalar : IBinaryFloatingPointIeee754<TScalar> => new(TScalar.ScaleB(x.X, n), TScalar.ScaleB(x.Y, n), TScalar.ScaleB(x.Z, n), TScalar.ScaleB(x.W, n));
}