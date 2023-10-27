using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Serialization;
using System.Text.Unicode;

namespace GenericVector;


// Vector3D<T>
[StructLayout(LayoutKind.Sequential), DataContract, Serializable]
public readonly partial struct Vector3D<T> : IVector<Vector3D<T>, T>, IVectorAlso<Vector3D<T>, T>, IEquatable<Vector3>, ISpanFormattable
    where T : INumberBase<T>
{
    /// <summary>The X component of the vector.</summary>
    [DataMember]
    public readonly T X;
    /// <summary>The Y component of the vector.</summary>
    [DataMember]
    public readonly T Y;
    /// <summary>The Z component of the vector.</summary>
    [DataMember]
    public readonly T Z;

    internal const int Count = 3;

    /// <summary>Creates a new <see cref="Vector3D{T}" /> object whose three elements have the same value.</summary>
    /// <param name="value">The value to assign to all three elements.</param>
    public Vector3D(T value) : this(value, value, value)
    {
    }

    /// <summary>Creates a new <see cref="Vector3D{T}" /> object from the specified <see cref="Vector3D{T}" /> object X and a Y and a Z and a W component.</summary>
    /// <param name="value">The vector to use for the [,  and ] components.</param>
    /// <param name="z">The Z component.</param>
    public Vector3D(Vector2D<T> value, T z) : this(value.X, value.Y, z)
    {
    }

    /// <summary>Creates a vector whose elements have the specified values.</summary>
    /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
    /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
    /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
    public Vector3D(T x, T y, T z)
    {
        Unsafe.SkipInit(out this);

        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{T}" />. The span must contain at least 4 elements.</summary>
    /// <param name="values">The span of elements to assign to the vector.</param>
    public Vector3D(ReadOnlySpan<T> values)
    {
        Unsafe.SkipInit(out this);

        ArgumentOutOfRangeException.ThrowIfLessThan(values.Length, Count, nameof(values));

        this = Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(values)));
    }

    /// <summary>Gets a vector whose 4 elements are equal to zero.</summary>
    /// <value>A vector whose three elements are equal to zero (that is, it returns the vector <c>(0,0,0)</c>.</value>
    public static Vector3D<T> Zero => new(T.Zero);

    /// <summary>Gets a vector whose 4 elements are equal to one.</summary>
    /// <value>Returns <see cref="Vector3D{T}" />.</value>
    /// <remarks>A vector whose three elements are equal to one (that is, it returns the vector <c>(1,1,1)</c>.</remarks>
    public static Vector3D<T> One => new(T.One);

    /// <summary>Gets the vector (1,0,0)).</summary>
    /// <value>The vector <c>(1,0,0)</c>.</value>
    public static Vector3D<T> UnitX => new(T.One, T.Zero, T.Zero);
    /// <summary>Gets the vector (0,1,0)).</summary>
    /// <value>The vector <c>(0,1,0)</c>.</value>
    public static Vector3D<T> UnitY => new(T.Zero, T.One, T.Zero);
    /// <summary>Gets the vector (0,0,1)).</summary>
    /// <value>The vector <c>(0,0,1)</c>.</value>
    public static Vector3D<T> UnitZ => new(T.Zero, T.Zero, T.One);

    public ReadOnlySpan<T> Components
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => MemoryMarshal.CreateReadOnlySpan<T>(ref Unsafe.AsRef(in X), Count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static Vector3D<T> IVector<Vector3D<T>, T>.CreateFromRepeatingComponent(T scalar) => new(scalar);

    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="index">The index of the element to get or set.</param>
    /// <returns>The the element at <paramref name="index" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> was less than zero or greater than the number of elements.</exception>
    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Components[index];
    }

    #region Operators
    /// <summary>Adds two vectors together.</summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The summed vector.</returns>
    /// <remarks>The <see cref="op_Addition" /> method defines the addition operation for <see cref="Vector3D{T}" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> operator +(Vector3D<T> left, Vector3D<T> right)
    {
        // NOTE: COMPLETELY UNTESTED. MIGHT BE SLOW.
        unsafe
        {
            if (Vector64<T>.IsSupported && Vector64.IsHardwareAccelerated)
            {
                Vector64<T> v0 = default;
                Vector64<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v1), right);
                
                v0 = v0 + v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0));
            }
        
            if (Vector128<T>.IsSupported && Vector128.IsHardwareAccelerated)
            {
                Vector128<T> v0 = default;
                Vector128<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v1), right);
                
                v0 = v0 + v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0));
            }
        
            if (Vector256<T>.IsSupported && Vector256.IsHardwareAccelerated)
            {
                Vector256<T> v0 = default;
                Vector256<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v1), right);
                
                v0 = v0 + v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0));
            }
        
            if (Vector512<T>.IsSupported && Vector512.IsHardwareAccelerated)
            {
                Vector512<T> v0 = default;
                Vector512<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v1), right);
                
                v0 = v0 + v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0));
            }
        }


        return new Vector3D<T>(
            left.X + right.X,
            left.Y + right.Y,
            left.Z + right.Z
        );
    }

    /// <summary>Subtracts the second vector from the first.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The vector that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
    /// <remarks>The <see cref="op_Subtraction" /> method defines the subtraction operation for <see cref="Vector3D{T}" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> operator -(Vector3D<T> left, Vector3D<T> right)
    {
        // NOTE: COMPLETELY UNTESTED. MIGHT BE SLOW.
        unsafe
        {
            if (Vector64<T>.IsSupported && Vector64.IsHardwareAccelerated)
            {
                Vector64<T> v0 = default;
                Vector64<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v1), right);
                
                v0 = v0 - v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0));
            }
        
            if (Vector128<T>.IsSupported && Vector128.IsHardwareAccelerated)
            {
                Vector128<T> v0 = default;
                Vector128<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v1), right);
                
                v0 = v0 - v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0));
            }
        
            if (Vector256<T>.IsSupported && Vector256.IsHardwareAccelerated)
            {
                Vector256<T> v0 = default;
                Vector256<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v1), right);
                
                v0 = v0 - v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0));
            }
        
            if (Vector512<T>.IsSupported && Vector512.IsHardwareAccelerated)
            {
                Vector512<T> v0 = default;
                Vector512<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v1), right);
                
                v0 = v0 - v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0));
            }
        }


        return new Vector3D<T>(
            left.X - right.X,
            left.Y - right.Y,
            left.Z - right.Z
        );
    }

    /// <summary>Negates the specified vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    /// <remarks>The <see cref="op_UnaryNegation" /> method defines the unary negation operation for <see cref="Vector3D{T}" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> operator -(Vector3D<T> value)
    {
        return Zero - value;
    }

    /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The element-wise product vector.</returns>
    /// <remarks>The <see cref="Vector3D{T}.op_Multiply" /> method defines the multiplication operation for <see cref="Vector3D{T}" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> operator *(Vector3D<T> left, Vector3D<T> right)
    {
        // NOTE: COMPLETELY UNTESTED. MIGHT BE SLOW.
        unsafe
        {
            if (Vector64<T>.IsSupported && Vector64.IsHardwareAccelerated)
            {
                Vector64<T> v0 = default;
                Vector64<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v1), right);
                
                v0 = v0 * v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0));
            }
        
            if (Vector128<T>.IsSupported && Vector128.IsHardwareAccelerated)
            {
                Vector128<T> v0 = default;
                Vector128<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v1), right);
                
                v0 = v0 * v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0));
            }
        
            if (Vector256<T>.IsSupported && Vector256.IsHardwareAccelerated)
            {
                Vector256<T> v0 = default;
                Vector256<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v1), right);
                
                v0 = v0 * v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0));
            }
        
            if (Vector512<T>.IsSupported && Vector512.IsHardwareAccelerated)
            {
                Vector512<T> v0 = default;
                Vector512<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v1), right);
                
                v0 = v0 * v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0));
            }
        }


        return new Vector3D<T>(
            left.X * right.X,
            left.Y * right.Y,
            left.Z * right.Z
        );
    }

    /// <summary>Multiplies the specified vector by the specified scalar value.</summary>
    /// <param name="left">The vector.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    /// <remarks>The <see cref="Vector3D{T}.op_Multiply" /> method defines the multiplication operation for <see cref="Vector3D{T}" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> operator *(Vector3D<T> left, T right)
    {
        return left * new Vector3D<T>(right);
    }

    /// <summary>Multiplies the scalar value by the specified vector.</summary>
    /// <param name="left">The vector.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    /// <remarks>The <see cref="Vector3D{T}.op_Multiply" /> method defines the multiplication operation for <see cref="Vector3D{T}" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> operator *(T left, Vector3D<T> right)
    {
        return right * left;
    }

    /// <summary>Divides the first vector by the second.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The vector that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
    /// <remarks>The <see cref="Vector3D{T}.op_Division" /> method defines the division operation for <see cref="Vector3D{T}" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> operator /(Vector3D<T> left, Vector3D<T> right)
    {
        // NOTE: COMPLETELY UNTESTED. MIGHT BE SLOW.
        unsafe
        {
            if (Vector64<T>.IsSupported && Vector64.IsHardwareAccelerated)
            {
                Vector64<T> v0 = default;
                Vector64<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v1), right);
                
                v0 = v0 / v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0));
            }
        
            if (Vector128<T>.IsSupported && Vector128.IsHardwareAccelerated)
            {
                Vector128<T> v0 = default;
                Vector128<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v1), right);
                
                v0 = v0 / v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0));
            }
        
            if (Vector256<T>.IsSupported && Vector256.IsHardwareAccelerated)
            {
                Vector256<T> v0 = default;
                Vector256<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v1), right);
                
                v0 = v0 / v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0));
            }
        
            if (Vector512<T>.IsSupported && Vector512.IsHardwareAccelerated)
            {
                Vector512<T> v0 = default;
                Vector512<T> v1 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0), left);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v1), right);
                
                v0 = v0 / v1;
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0));
            }
        }


        return new Vector3D<T>(
            left.X / right.X,
            left.Y / right.Y,
            left.Z / right.Z
        );
    }

    /// <summary>Divides the specified vector by a specified scalar value.</summary>
    /// <param name="value1">The vector.</param>
    /// <param name="value2">The scalar value.</param>
    /// <returns>The result of the division.</returns>
    /// <remarks>The <see cref="Vector3D{T}.op_Division" /> method defines the division operation for <see cref="Vector3D{T}" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> operator /(Vector3D<T> value1, T value2)
    {
        return value1 / new Vector3D<T>(value2);
    }

    /// <summary>Returns a value that indicates whether each pair of elements in two specified vectors is equal.</summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    /// <remarks>Two <see cref="Vector3D{T}" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector3D<T> left, Vector3D<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>Returns a value that indicates whether two specified vectors are not equal.</summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector3D<T> left, Vector3D<T> right)
    {
        return !(left == right);
    }
    #endregion

    #region CopyTo
    /// <summary>Copies the elements of the vector to a specified array.</summary>
    /// <param name="array">The destination array.</param>
    /// <remarks><paramref name="array" /> must have at least three elements. The method copies the vector's elements starting at index 0.</remarks>
    /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
    /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array)
    {
        // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons

        ArgumentOutOfRangeException.ThrowIfLessThan(array.Length, Count, nameof(array));

        Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref array[0]), this);
    }

    /// <summary>Copies the elements of the vector to a specified array starting at a specified index position.</summary>
    /// <param name="array">The destination array.</param>
    /// <param name="index">The index at which to copy the first element of the vector.</param>
    /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the three vector elements. In other words, elements <paramref name="index" /> through <paramref name="index" /> + 3 must already exist in <paramref name="array" />.</remarks>
    /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
    /// -or-
    /// <paramref name="index" /> is greater than or equal to the array length.</exception>
    /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array, int index)
    {
        // We explicitly don't check for `null` because historically this has thrown `NullReferenceException` for perf reasons

        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)array.Length);
        ArgumentOutOfRangeException.ThrowIfLessThan((array.Length - index), Count);

        Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref array[index]), this);
    }

    /// <summary>Copies the vector to the given <see cref="Span{T}" />. The length of the destination span must be at least 4.</summary>
    /// <param name="destination">The destination span which the values are copied into.</param>
    /// <exception cref="ArgumentException">If number of elements in source vector is greater than those available in destination span.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(Span<T> destination)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, Count, nameof(destination));

        Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(destination)), this);
    }

    /// <summary>Attempts to copy the vector to the given <see cref="Span{Single}" />. The length of the destination span must be at least 4.</summary>
    /// <param name="destination">The destination span which the values are copied into.</param>
    /// <returns><see langword="true" /> if the source vector was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryCopyTo(Span<T> destination)
    {
        if (destination.Length < Count)
        {
            return false;
        }

        Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(destination)), this);
        return true;
    }
    #endregion

    #region Equality
    /// <summary>Returns a value that indicates whether this instance and another vector are equal.</summary>
    /// <param name="other">The other vector.</param>
    /// <returns><see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" />.</returns>
    /// <remarks>Two vectors are equal if their <see cref="X" />, <see cref="Y" />, <see cref="Z" />, and <see cref="W" /> elements are equal.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector3D<T> other)
    {
        /*// NOTE: COMPLETELY UNTESTED. MIGHT BE SLOW.
unsafe
{
    if (Vector64<T>.IsSupported && Vector64.IsHardwareAccelerated)
    {
        Vector64<T> v0 = default;
        Vector64<T> v1 = default;
        
        Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0), left);
        Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v1), right);
        
        v0 = v0.Equals(v1);
        return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0));
    }

    if (Vector128<T>.IsSupported && Vector128.IsHardwareAccelerated)
    {
        Vector128<T> v0 = default;
        Vector128<T> v1 = default;
        
        Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0), left);
        Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v1), right);
        
        v0 = v0.Equals(v1);
        return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0));
    }

    if (Vector256<T>.IsSupported && Vector256.IsHardwareAccelerated)
    {
        Vector256<T> v0 = default;
        Vector256<T> v1 = default;
        
        Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0), left);
        Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v1), right);
        
        v0 = v0.Equals(v1);
        return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0));
    }

    if (Vector512<T>.IsSupported && Vector512.IsHardwareAccelerated)
    {
        Vector512<T> v0 = default;
        Vector512<T> v1 = default;
        
        Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0), left);
        Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v1), right);
        
        v0 = v0.Equals(v1);
        return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0));
    }
}
*/

        //return SpeedHelpers.FastEqualsUpTo4<Vector3D<T>, T>(this, other);
        return
            X.Equals(other.X) &&
            Y.Equals(other.Y) &&
            Z.Equals(other.Z);
    }

    /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
    /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Vector3D{T}" /> object and their corresponding elements are equal.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return (obj is Vector3D<T> other) && Equals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector3 other)
    {
        if (typeof(T) == typeof(float) && Vector128.IsHardwareAccelerated)
        {
            return Unsafe.BitCast<Vector3D<T>, Vector3>(this).AsVector128().Equals(other.AsVector128());
        }


        return
            float.CreateTruncating(X).Equals(other.X) &&
            float.CreateTruncating(Y).Equals(other.Y) &&
            float.CreateTruncating(Z).Equals(other.Z);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
    #endregion

    #region Format
    /// <summary>Returns the string representation of the current instance using default formatting.</summary>
    /// <returns>The string representation of the current instance.</returns>
    /// <remarks>This method returns a string in which each element of the vector is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
    public override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
    /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
    /// <returns>The string representation of the current instance.</returns>
    /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
    /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
    /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
    /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
    /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the current instance.</returns>
    /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "&lt;" and "&gt;" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
    /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
    /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        Span<char> initialBuffer = stackalloc char[Math.Min((2 + (Count - 1) + (separator.Length * (Count - 1)) + (Count * 2)), 256)];

        // We can't use an interpolated string here because it won't allow us to pass `format`
        var handler = new DefaultInterpolatedStringHandler(
            4 + (separator.Length * 2),
            Count,
            formatProvider,
            initialBuffer
        );

        handler.AppendLiteral("<");
        handler.AppendFormatted(X, format);
        handler.AppendLiteral(separator);
        handler.AppendLiteral(" ");
        handler.AppendFormatted(Y, format);
        handler.AppendLiteral(separator);
        handler.AppendLiteral(" ");
        handler.AppendFormatted(Z, format);
        handler.AppendLiteral(">");

        return handler.ToStringAndClear();
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        // Possible fast path for failure case:
        // if (destination.Length < 4) return false;

        var separator = NumberFormatInfo.GetInstance(provider).NumberGroupSeparator;

        // We can't use an interpolated string here because it won't allow us to pass `format`
        var handler = new MemoryExtensions.TryWriteInterpolatedStringHandler(
            4 + (separator.Length * 2),
            Count,
            destination,
            provider,
            out var shouldAppend
        );
        if (!shouldAppend)
        {
            charsWritten = 0;
            return false;
        }

        // Annoyingly we need to turn the span into a string for the string handler
        string? formatString = format.Length > 0 ? new string(format) : null;

        _ =
            handler.AppendLiteral("<") &&
            handler.AppendFormatted(X, formatString) &&
            handler.AppendLiteral(separator) &&
            handler.AppendLiteral(" ") &&
            handler.AppendFormatted(Y, formatString) &&
            handler.AppendLiteral(separator) &&
            handler.AppendLiteral(" ") &&
            handler.AppendFormatted(Z, formatString) &&
            handler.AppendLiteral(">");

        return destination.TryWrite(ref handler, out charsWritten);
    }
    #endregion

    #region Casts
    public Vector3D<TOther> As<TOther>() where TOther : INumberBase<TOther>
    {
        if (SpeedHelpers.TryFastConvert<Vector3D<T>, T, Vector3D<TOther>, TOther>(this, out var result))
        {
            return result;
        }

        return new Vector3D<TOther>(
            TOther.CreateTruncating(X),
            TOther.CreateTruncating(Y),
            TOther.CreateTruncating(Z)
        );
    }

    private Vector3D<TOther> AsChecked<TOther>() where TOther : INumberBase<TOther>
    {
        return new Vector3D<TOther>(
            TOther.CreateChecked(X),
            TOther.CreateChecked(Y),
            TOther.CreateChecked(Z)
        );
    }
    public static explicit operator Vector3D<byte>(Vector3D<T> self) => self.As<byte>();
    public static explicit operator Vector3D<sbyte>(Vector3D<T> self) => self.As<sbyte>();
    public static explicit operator Vector3D<short>(Vector3D<T> self) => self.As<short>();
    public static explicit operator Vector3D<ushort>(Vector3D<T> self) => self.As<ushort>();
    public static explicit operator Vector3D<int>(Vector3D<T> self) => self.As<int>();
    public static explicit operator Vector3D<uint>(Vector3D<T> self) => self.As<uint>();
    public static explicit operator Vector3D<long>(Vector3D<T> self) => self.As<long>();
    public static explicit operator Vector3D<ulong>(Vector3D<T> self) => self.As<ulong>();
    public static explicit operator Vector3D<Int128>(Vector3D<T> self) => self.As<Int128>();
    public static explicit operator Vector3D<UInt128>(Vector3D<T> self) => self.As<UInt128>();
    public static explicit operator Vector3D<Half>(Vector3D<T> self) => self.As<Half>();
    public static explicit operator Vector3D<float>(Vector3D<T> self) => self.As<float>();
    public static explicit operator Vector3D<double>(Vector3D<T> self) => self.As<double>();
    public static explicit operator Vector3D<decimal>(Vector3D<T> self) => self.As<decimal>();
    public static explicit operator Vector3D<Complex>(Vector3D<T> self) => self.As<Complex>();
    public static explicit operator Vector3D<BigInteger>(Vector3D<T> self) => self.As<BigInteger>();

    public static explicit operator checked Vector3D<byte>(Vector3D<T> self) => self.AsChecked<byte>();
    public static explicit operator checked Vector3D<sbyte>(Vector3D<T> self) => self.AsChecked<sbyte>();
    public static explicit operator checked Vector3D<short>(Vector3D<T> self) => self.AsChecked<short>();
    public static explicit operator checked Vector3D<ushort>(Vector3D<T> self) => self.AsChecked<ushort>();
    public static explicit operator checked Vector3D<int>(Vector3D<T> self) => self.AsChecked<int>();
    public static explicit operator checked Vector3D<uint>(Vector3D<T> self) => self.AsChecked<uint>();
    public static explicit operator checked Vector3D<long>(Vector3D<T> self) => self.AsChecked<long>();
    public static explicit operator checked Vector3D<ulong>(Vector3D<T> self) => self.AsChecked<ulong>();
    public static explicit operator checked Vector3D<Int128>(Vector3D<T> self) => self.AsChecked<Int128>();
    public static explicit operator checked Vector3D<UInt128>(Vector3D<T> self) => self.AsChecked<UInt128>();
    public static explicit operator checked Vector3D<Half>(Vector3D<T> self) => self.AsChecked<Half>();
    public static explicit operator checked Vector3D<float>(Vector3D<T> self) => self.AsChecked<float>();
    public static explicit operator checked Vector3D<double>(Vector3D<T> self) => self.AsChecked<double>();
    public static explicit operator checked Vector3D<decimal>(Vector3D<T> self) => self.AsChecked<decimal>();
    public static explicit operator checked Vector3D<Complex>(Vector3D<T> self) => self.AsChecked<Complex>();
    public static explicit operator checked Vector3D<BigInteger>(Vector3D<T> self) => self.AsChecked<BigInteger>();

    // Cast to System.Numerics.Vector3
    public static explicit operator Vector3(Vector3D<T> self) => new(float.CreateTruncating(self.X), float.CreateTruncating(self.Y), float.CreateTruncating(self.Z));
    public static explicit operator checked Vector3(Vector3D<T> self) => new(float.CreateChecked(self.X), float.CreateChecked(self.Y), float.CreateChecked(self.Z));

    // Downcast
    public static explicit operator Vector2D<T>(Vector3D<T> self) => new(self.X, self.Y);

    // Upcast
    public static explicit operator Vector4D<T>(Vector3D<T> self) => new(self, T.Zero);

    // Upcast from System.Numerics.Vector < 3
    public static explicit operator Vector3D<T>(Vector2 self) => new(T.CreateTruncating(self.X), T.CreateTruncating(self.Y), T.Zero);
    public static explicit operator checked Vector3D<T>(Vector2 self) => new(T.CreateChecked(self.X), T.CreateChecked(self.Y), T.Zero);

    // Downcast from System.Numerics.Vector >= 3
    public static explicit operator Vector3D<T>(Vector3 self) => new(T.CreateTruncating(self.X), T.CreateTruncating(self.Y), T.CreateTruncating(self.Z));
    public static explicit operator checked Vector3D<T>(Vector3 self) => new(T.CreateChecked(self.X), T.CreateChecked(self.Y), T.CreateChecked(self.Z));
    public static explicit operator Vector3D<T>(Vector4 self) => new(T.CreateTruncating(self.X), T.CreateTruncating(self.Y), T.CreateTruncating(self.Z));
    public static explicit operator checked Vector3D<T>(Vector4 self) => new(T.CreateChecked(self.X), T.CreateChecked(self.Y), T.CreateChecked(self.Z));

    public static implicit operator Vector3D<T>((T X, T Y, T Z) components)
        => new(components.X, components.Y, components.Z);

    #endregion

    public void Deconstruct(out T x, out T y, out T z)
    {
        x = X;
        y = Y;
        z = Z;
    }
}

file interface IVec3
{
    // Returns null if incompatible. Throws OverflowException if overflowing
    Vector3D<T>? GetChecked<T>() where T : INumberBase<T>;
    Vector3D<T>? GetSaturating<T>() where T : INumberBase<T>;
    Vector3D<T>? GetTruncating<T>() where T : INumberBase<T>;
}

// Vector3D<T>.INumber
public readonly partial struct Vector3D<T> :
    IDivisionOperators<Vector3D<T>, T, Vector3D<T>>,
    IMultiplyOperators<Vector3D<T>, T, Vector3D<T>>,
    INumberBase<Vector3D<T>>,
    IVec3
{
    /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
    /// <param name="value">A vector.</param>
    /// <returns>The absolute value vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static Vector3D<T> INumberBase<Vector3D<T>>.Abs(Vector3D<T> value) => Vector3D.Abs(value);

    static Vector3D<T> IParsable<Vector3D<T>>.Parse(string s, IFormatProvider? provider)
        => Parse(s.AsSpan(), NumberStyles.None, provider);

    static Vector3D<T> ISpanParsable<Vector3D<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => Parse(s, NumberStyles.None, provider);

    public static Vector3D<T> Parse(string s, NumberStyles style = default, IFormatProvider? provider = null)
        => Parse(s.AsSpan(), style, provider);

    public static Vector3D<T> Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.None, IFormatProvider? provider = null)
        => TryParse(s, style, provider, out var result) ? result : throw new ArgumentException($"Failed to parse {nameof(Vector3)}<{typeof(T)}>");

    public static bool TryParse(string? s, IFormatProvider? provider, out Vector3D<T> result)
        => TryParse(s.AsSpan(), NumberStyles.None, provider, out result);

    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Vector3D<T> result)
        => TryParse(s.AsSpan(), style, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Vector3D<T> result)
        => TryParse(s, NumberStyles.None, provider, out result);

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Vector3D<T> result)
    {
        result = default;

        if (s[0] != '<') return false;
        if (s[^1] != '>') return false;

        var separator = NumberFormatInfo.GetInstance(provider).NumberGroupSeparator;

        s = s[1..^1];

        T? x, y, z;

        {
            if (s.Length == 0) return false;

            var nextNumber = s.IndexOf(separator);
            if (nextNumber == -1)
            {
                return false;
            }

            if (!T.TryParse(s[..nextNumber], style, provider, out x)) return false;

            s = s[(nextNumber + separator.Length)..];
        }
        {
            if (s.Length == 0) return false;

            var nextNumber = s.IndexOf(separator);
            if (nextNumber == -1)
            {
                return false;
            }

            if (!T.TryParse(s[..nextNumber], style, provider, out y)) return false;

            s = s[(nextNumber + separator.Length)..];
        }

        {
            if (s.Length == 0) return false;

            if (!T.TryParse(s, style, provider, out z)) return false;
        }

        result = new Vector3D<T>(x, y, z);
        return true;
    }

    static Vector3D<T> IAdditiveIdentity<Vector3D<T>, Vector3D<T>>.AdditiveIdentity => Zero;
    static Vector3D<T> IMultiplicativeIdentity<Vector3D<T>, Vector3D<T>>.MultiplicativeIdentity => One;

    static Vector3D<T> IDecrementOperators<Vector3D<T>>.operator --(Vector3D<T> value) => value - One;
    static Vector3D<T> IIncrementOperators<Vector3D<T>>.operator ++(Vector3D<T> value) => value + One;

    static Vector3D<T> IUnaryPlusOperators<Vector3D<T>, Vector3D<T>>.operator +(Vector3D<T> value) => value;

    static bool INumberBase<Vector3D<T>>.IsCanonical(Vector3D<T> value) => T.IsCanonical(value.X) && T.IsCanonical(value.Y) && T.IsCanonical(value.Z);

    static bool INumberBase<Vector3D<T>>.IsComplexNumber(Vector3D<T> value) => T.IsComplexNumber(value.X) || T.IsComplexNumber(value.Y) || T.IsComplexNumber(value.Z);

    static bool INumberBase<Vector3D<T>>.IsEvenInteger(Vector3D<T> value) => T.IsEvenInteger(value.X) && T.IsEvenInteger(value.Y) && T.IsEvenInteger(value.Z);

    static bool INumberBase<Vector3D<T>>.IsFinite(Vector3D<T> value) => T.IsFinite(value.X) && T.IsFinite(value.Y) && T.IsFinite(value.Z);

    static bool INumberBase<Vector3D<T>>.IsImaginaryNumber(Vector3D<T> value) => T.IsImaginaryNumber(value.X) || T.IsImaginaryNumber(value.Y) || T.IsImaginaryNumber(value.Z);

    static bool INumberBase<Vector3D<T>>.IsInfinity(Vector3D<T> value) => T.IsInfinity(value.X) && T.IsInfinity(value.Y) && T.IsInfinity(value.Z);

    static bool INumberBase<Vector3D<T>>.IsInteger(Vector3D<T> value) => T.IsInteger(value.X) && T.IsInteger(value.Y) && T.IsInteger(value.Z);

    static bool INumberBase<Vector3D<T>>.IsNaN(Vector3D<T> value) => T.IsNaN(value.X) || T.IsNaN(value.Y) || T.IsNaN(value.Z);

    static bool INumberBase<Vector3D<T>>.IsNegative(Vector3D<T> value) => T.IsNegative(value.X) && T.IsNegative(value.Y) && T.IsNegative(value.Z);

    static bool INumberBase<Vector3D<T>>.IsNegativeInfinity(Vector3D<T> value) => T.IsNegativeInfinity(value.X) && T.IsNegativeInfinity(value.Y) && T.IsNegativeInfinity(value.Z);

    static bool INumberBase<Vector3D<T>>.IsNormal(Vector3D<T> value) => T.IsNormal(value.X) && T.IsNormal(value.Y) && T.IsNormal(value.Z);

    static bool INumberBase<Vector3D<T>>.IsOddInteger(Vector3D<T> value) => T.IsOddInteger(value.X) && T.IsOddInteger(value.Y) && T.IsOddInteger(value.Z);

    static bool INumberBase<Vector3D<T>>.IsPositive(Vector3D<T> value) => T.IsPositive(value.X) && T.IsPositive(value.Y) && T.IsPositive(value.Z);

    static bool INumberBase<Vector3D<T>>.IsPositiveInfinity(Vector3D<T> value) => T.IsPositiveInfinity(value.X) && T.IsPositiveInfinity(value.Y) && T.IsPositiveInfinity(value.Z);

    static bool INumberBase<Vector3D<T>>.IsRealNumber(Vector3D<T> value) => T.IsRealNumber(value.X) && T.IsRealNumber(value.Y) && T.IsRealNumber(value.Z);

    static bool INumberBase<Vector3D<T>>.IsSubnormal(Vector3D<T> value) => T.IsSubnormal(value.X) && T.IsSubnormal(value.Y) && T.IsSubnormal(value.Z);

    static bool INumberBase<Vector3D<T>>.IsZero(Vector3D<T> value) => T.IsZero(value.X) && T.IsZero(value.Y) && T.IsZero(value.Z);

    static Vector3D<T> INumberBase<Vector3D<T>>.MaxMagnitude(Vector3D<T> x, Vector3D<T> y) => new(T.MaxMagnitude(x.X, y.X), T.MaxMagnitude(x.Y, y.Y), T.MaxMagnitude(x.Z, y.Z));

    static Vector3D<T> INumberBase<Vector3D<T>>.MaxMagnitudeNumber(Vector3D<T> x, Vector3D<T> y) => new(T.MaxMagnitudeNumber(x.X, y.X), T.MaxMagnitudeNumber(x.Y, y.Y), T.MaxMagnitudeNumber(x.Z, y.Z));

    static Vector3D<T> INumberBase<Vector3D<T>>.MinMagnitude(Vector3D<T> x, Vector3D<T> y) => new(T.MinMagnitude(x.X, y.X), T.MinMagnitude(x.Y, y.Y), T.MinMagnitude(x.Z, y.Z));

    static Vector3D<T> INumberBase<Vector3D<T>>.MinMagnitudeNumber(Vector3D<T> x, Vector3D<T> y) => new(T.MinMagnitudeNumber(x.X, y.X), T.MinMagnitudeNumber(x.Y, y.Y), T.MinMagnitudeNumber(x.Z, y.Z));

    public static bool TryConvertFromChecked<TOther>(TOther value, out Vector3D<T> result) where TOther : INumberBase<TOther>
    {
        if (value is Vector3D<T> v)
        {
            result = v;
            return true;
        }

        if (value is IVec3 IVec3 && IVec3.GetChecked<T>() is {} r)
        {
            result = r;
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryConvertFromSaturating<TOther>(TOther value, out Vector3D<T> result) where TOther : INumberBase<TOther>
    {
        if (value is Vector3D<T> v)
        {
            result = v;
            return true;
        }

        if (value is IVec3 IVec3 && IVec3.GetSaturating<T>() is {} r)
        {
            result = r;
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryConvertFromTruncating<TOther>(TOther value, out Vector3D<T> result) where TOther : INumberBase<TOther>
    {
        if (value is Vector3D<T> v)
        {
            result = v;
            return true;
        }

        if (value is IVec3 IVec3 && IVec3.GetTruncating<T>() is {} r)
        {
            result = r;
            return true;
        }

        result = default;
        return false;
    }

    public static bool TryConvertToChecked<TOther>(Vector3D<T> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
    {
        return TOther.TryConvertFromChecked(value, out result);
    }

    public static bool TryConvertToSaturating<TOther>(Vector3D<T> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
    {
        return TOther.TryConvertFromSaturating(value, out result);
    }

    public static bool TryConvertToTruncating<TOther>(Vector3D<T> value, [MaybeNullWhen(false)]out TOther result) where TOther : INumberBase<TOther>
    {
        return TOther.TryConvertFromTruncating(value, out result);
    }

    static int INumberBase<Vector3D<T>>.Radix => T.Radix;

    Vector3D<T1>? IVec3.GetChecked<T1>() => T1.TryConvertFromChecked(X, out var x) ? new(x, T1.CreateChecked(Y), T1.CreateChecked(Z)) : null;
    Vector3D<T1>? IVec3.GetSaturating<T1>() => T1.TryConvertFromSaturating(X, out var x) ? new(x, T1.CreateSaturating(Y), T1.CreateSaturating(Z)) : null;
    Vector3D<T1>? IVec3.GetTruncating<T1>() => T1.TryConvertFromTruncating(X, out var x) ? new(x, T1.CreateTruncating(Y), T1.CreateTruncating(Z)) : null;
}

// Vector3D<T>.IReadOnlyList
public readonly partial struct Vector3D<T> : IReadOnlyList<T>
{
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    int IReadOnlyCollection<T>.Count => Count;
}

// Vector3D<T>.IUtf8SpanParsableFormattable
public partial struct Vector3D<T> :
    IUtf8SpanFormattable,
    IUtf8SpanParsable<Vector3D<T>>
{
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        // Possible fast path for failure case:
        // if (destination.Length < 4) return false;

        var separator = NumberFormatInfo.GetInstance(provider).NumberGroupSeparator;

        // We can't use an interpolated string here because it won't allow us to pass `format`
        var handler = new Utf8.TryWriteInterpolatedStringHandler(
            4 + (separator.Length * 2),
            Count,
            utf8Destination,
            provider,
            out var shouldAppend
        );
        if (!shouldAppend)
        {
            bytesWritten = 0;
            return false;
        }

        // Annoyingly we need to turn the span into a string for the string handler
        string? formatString = format.Length > 0 ? new string(format) : null;

        // JIT will automagically convert literals to utf8
        _ =
            handler.AppendLiteral("<") &&
            handler.AppendFormatted(X, formatString) &&
            handler.AppendLiteral(separator) &&
            handler.AppendLiteral(" ") &&
            handler.AppendFormatted(Y, formatString) &&
            handler.AppendLiteral(separator) &&
            handler.AppendLiteral(" ") &&
            handler.AppendFormatted(Z, formatString) &&
            handler.AppendLiteral(">");

        return Utf8.TryWrite(utf8Destination, ref handler, out bytesWritten);
    }

    public static Vector3D<T> Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
        => TryParse(utf8Text, provider, out var result) ? result : throw new ArgumentException($"Failed to parse {nameof(Vector3D)}<{typeof(T)}>");

    public static bool TryParse(ReadOnlySpan<byte> s, IFormatProvider? provider, out Vector3D<T> result)
    {
        result = default;

        if (s[0] != (byte)'<') return false;
        if (s[^1] != (byte)'>') return false;

        var separator = NumberGroupSeparatorTChar<byte>(NumberFormatInfo.GetInstance(provider));

        s = s[1..^1];

        T? x, y, z;

        {
            if (s.Length == 0) return false;

            var nextNumber = s.IndexOf(separator);
            if (nextNumber == -1)
            {
                return false;
            }

            if (!T.TryParse(s[..nextNumber], provider, out x)) return false;

            s = s[(nextNumber + separator.Length)..];
        }
        {
            if (s.Length == 0) return false;

            var nextNumber = s.IndexOf(separator);
            if (nextNumber == -1)
            {
                return false;
            }

            if (!T.TryParse(s[..nextNumber], provider, out y)) return false;

            s = s[(nextNumber + separator.Length)..];
        }

        {
            if (s.Length == 0) return false;

            if (!T.TryParse(s, provider, out z)) return false;
        }

        result = new Vector3D<T>(x, y, z);
        return true;

        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(NumberGroupSeparatorTChar))]
        static extern ReadOnlySpan<TChar> NumberGroupSeparatorTChar<TChar>(NumberFormatInfo? c) where TChar : unmanaged;
    }
}

// Vector3D
public static partial class Vector3D
{
    #region Extension

    /// <summary>Returns the length of this vector object.</summary>
    /// <returns>The vector's length.</returns>
    /// <altmember cref="LengthSquared{T,TReturn}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TReturn Length<T, TReturn>(this Vector3D<T> vec) where T : INumberBase<T> where TReturn : INumberBase<TReturn>, IRootFunctions<TReturn>
    {
        var lengthSquared = vec.LengthSquared<T, TReturn>();
        return TReturn.Sqrt(lengthSquared);
    }

    /// <summary>Returns the length of the vector squared.</summary>
    /// <returns>The vector's length squared.</returns>
    /// <remarks>This operation offers better performance than a call to the <see cref="Length{T,TReturn}" /> method.</remarks>
    /// <altmember cref="Length{T,TReturn}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TReturn LengthSquared<T, TReturn>(this Vector3D<T> vec) where T : INumberBase<T> where TReturn : INumberBase<TReturn>
    {
        return Dot<T, TReturn>(vec, vec);
    }

    #endregion

    #region Basic

    /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The element-wise product vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Multiply<T>(Vector3D<T> left, Vector3D<T> right) where T : INumberBase<T>
    {
        return left * right;
    }

    /// <summary>Multiplies a vector by a specified scalar.</summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Multiply<T>(Vector3D<T> left, T right) where T : INumberBase<T>
    {
        return left * right;
    }

    /// <summary>Multiplies a scalar value by a specified vector.</summary>
    /// <param name="left">The scaled value.</param>
    /// <param name="right">The vector.</param>
    /// <returns>The scaled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Multiply<T>(T left, Vector3D<T> right) where T : INumberBase<T>
    {
        return left * right;
    }

    /// <summary>Negates a specified vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Negate<T>(Vector3D<T> value) where T : INumberBase<T>
    {
        return -value;
    }

    /// <summary>Subtracts the second vector from the first.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The difference vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Subtract<T>(Vector3D<T> left, Vector3D<T> right) where T : INumberBase<T>
    {
        return left - right;
    }

    /// <summary>Adds two vectors together.</summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The summed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Add<T>(Vector3D<T> left, Vector3D<T> right) where T : INumberBase<T>
    {
        return left + right;
    }

    /// <summary>Divides the first vector by the second.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The vector resulting from the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Divide<T>(Vector3D<T> left, Vector3D<T> right) where T : INumberBase<T>
    {
        return left / right;
    }

    /// <summary>Divides the specified vector by a specified scalar value.</summary>
    /// <param name="left">The vector.</param>
    /// <param name="divisor">The scalar value.</param>
    /// <returns>The vector that results from the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Divide<T>(Vector3D<T> left, T divisor) where T : INumberBase<T>
    {
        return left / divisor;
    }

    #endregion

    #region Other

    /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
    /// <param name="value">A vector.</param>
    /// <returns>The absolute value vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Abs<T>(Vector3D<T> value) where T : INumberBase<T>
    {
        // NOTE: COMPLETELY UNTESTED. MIGHT BE SLOW.
        unsafe
        {
            if (Vector64<T>.IsSupported && Vector64.IsHardwareAccelerated)
            {
                Vector64<T> v0 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0), value);
                
                v0 = Vector64.Abs(v0);
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0));
            }
        
            if (Vector128<T>.IsSupported && Vector128.IsHardwareAccelerated)
            {
                Vector128<T> v0 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0), value);
                
                v0 = Vector128.Abs(v0);
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0));
            }
        
            if (Vector256<T>.IsSupported && Vector256.IsHardwareAccelerated)
            {
                Vector256<T> v0 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0), value);
                
                v0 = Vector256.Abs(v0);
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0));
            }
        
            if (Vector512<T>.IsSupported && Vector512.IsHardwareAccelerated)
            {
                Vector512<T> v0 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0), value);
                
                v0 = Vector512.Abs(v0);
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0));
            }
        }


        return new(T.Abs(value.X), T.Abs(value.Y), T.Abs(value.Z));
    }

    /// <summary>Restricts a vector between a minimum and a maximum value.</summary>
    /// <param name="value1">The vector to restrict.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The restricted vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Clamp<T>(Vector3D<T> value1, Vector3D<T> min, Vector3D<T> max) where T : INumberBase<T>, IComparisonOperators<T, T, bool>
    {
        // NOTE: COMPLETELY UNTESTED. MIGHT BE SLOW.
        unsafe
        {
            if (Vector64<T>.IsSupported && Vector64.IsHardwareAccelerated)
            {
                Vector64<T> v0 = default;
                Vector64<T> v1 = default;
                Vector64<T> v2 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0), value1);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v1), min);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v2), max);
                
                v0 = Vector64.Min(Vector64.Max(v0, v1), v2);
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector64<T>, byte>(ref v0));
            }
        
            if (Vector128<T>.IsSupported && Vector128.IsHardwareAccelerated)
            {
                Vector128<T> v0 = default;
                Vector128<T> v1 = default;
                Vector128<T> v2 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0), value1);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v1), min);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v2), max);
                
                v0 = Vector128.Min(Vector128.Max(v0, v1), v2);
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector128<T>, byte>(ref v0));
            }
        
            if (Vector256<T>.IsSupported && Vector256.IsHardwareAccelerated)
            {
                Vector256<T> v0 = default;
                Vector256<T> v1 = default;
                Vector256<T> v2 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0), value1);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v1), min);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v2), max);
                
                v0 = Vector256.Min(Vector256.Max(v0, v1), v2);
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector256<T>, byte>(ref v0));
            }
        
            if (Vector512<T>.IsSupported && Vector512.IsHardwareAccelerated)
            {
                Vector512<T> v0 = default;
                Vector512<T> v1 = default;
                Vector512<T> v2 = default;
                
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0), value1);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v1), min);
                Unsafe.WriteUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v2), max);
                
                v0 = Vector512.Min(Vector512.Max(v0, v1), v2);
                return Unsafe.ReadUnaligned<Vector3D<T>>(ref Unsafe.As<Vector512<T>, byte>(ref v0));
            }
        }


        // We must follow HLSL behavior in the case user specified min value is bigger than max value.
        return Min(Max(value1, min), max);
    }

    /// <summary>Computes the Euclidean distance between the two given points.</summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TReturn Distance<T, TReturn>(Vector3D<T> value1, Vector3D<T> value2) where T : INumberBase<T> where TReturn : INumberBase<TReturn>, IRootFunctions<TReturn>
    {
        var distanceSquared = DistanceSquared<T, TReturn>(value1, value2);
        return TReturn.Sqrt(distanceSquared);
    }

    /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance squared.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T DistanceSquared<T>(Vector3D<T> value1, Vector3D<T> value2) where T : INumberBase<T>
    {
        var difference = value1 - value2;
        return Dot(difference, difference);
    }

    /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance squared.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TReturn DistanceSquared<T, TReturn>(Vector3D<T> value1, Vector3D<T> value2) where T : INumberBase<T> where TReturn : INumberBase<TReturn>
    {
        var difference = value1 - value2;
        return Dot<T, TReturn>(difference, difference);
    }

    /// <summary>Returns the dot product of two vectors.</summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The dot product.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Dot<T>(Vector3D<T> vector1, Vector3D<T> vector2) where T : INumberBase<T>
    {
        // TODO: vectorize return scalar
        return
            vector1.X * vector2.X +
            vector1.Y * vector2.Y +
            vector1.Z * vector2.Z;
    }

    /// <summary>Returns the dot product of two vectors.</summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The dot product.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TReturn Dot<T, TReturn>(Vector3D<T> vector1, Vector3D<T> vector2) where T : INumberBase<T> where TReturn : INumberBase<TReturn>
    {
        // TODO vectorize return converted (maybe not possible)
        return
            TReturn.CreateTruncating(vector1.X) * TReturn.CreateTruncating(vector2.X) +
            TReturn.CreateTruncating(vector1.Y) * TReturn.CreateTruncating(vector2.Y) +
            TReturn.CreateTruncating(vector1.Z) * TReturn.CreateTruncating(vector2.Z);
    }

    /// <summary>Performs a linear interpolation between two vectors based on the given weighting.</summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">A value between 0 and 1 that indicates the weight of <paramref name="value2" />.</param>
    /// <returns>The interpolated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<TFloat> Lerp<T, TFloat>(Vector3D<T> value1, Vector3D<T> value2, TFloat amount) where T : INumberBase<T> where TFloat : INumberBase<TFloat>, IFloatingPoint<TFloat>
    {
        return (value1.As<TFloat>() * (TFloat.One - amount)) + (value2.As<TFloat>() * amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<TFloat> LerpClamped<T, TFloat>(Vector3D<T> value1, Vector3D<T> value2, TFloat amount) where T : INumberBase<T> where TFloat : INumberBase<TFloat>, IFloatingPoint<TFloat>
    {
        amount = TFloat.Clamp(amount, TFloat.Zero, TFloat.One);
        return Lerp(value1, value2, amount);
    }

    /// <summary>Returns a vector whose elements are the maximum of each of the pairs of elements in two specified vectors.</summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The maximized vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Max<T>(Vector3D<T> value1, Vector3D<T> value2) where T : INumberBase<T>, IComparisonOperators<T, T, bool>
    {
        return new Vector3D<T>( // using T.Max here would add an IsNaN check
            (value1.X > value2.X) ? value1.X : value2.X, 
            (value1.Y > value2.Y) ? value1.Y : value2.Y, 
            (value1.Z > value2.Z) ? value1.Z : value2.Z
        );
    }

    /// <summary>Returns a vector whose elements are the minimum of each of the pairs of elements in two specified vectors.</summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <returns>The minimized vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Min<T>(Vector3D<T> value1, Vector3D<T> value2) where T : INumberBase<T>, IComparisonOperators<T, T, bool>
    {
        return new Vector3D<T>( // using T.Min here would add an IsNaN check
            (value1.X < value2.X) ? value1.X : value2.X, 
            (value1.Y < value2.Y) ? value1.Y : value2.Y, 
            (value1.Z < value2.Z) ? value1.Z : value2.Z
        );
    }

    /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<TReturn> Normalize<T, TReturn>(Vector3D<T> value) where T : INumberBase<T> where TReturn : INumberBase<TReturn>, IRootFunctions<TReturn>
    {
        return value.As<TReturn>() / value.Length<T, TReturn>();
    }

    /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
    /// <param name="value">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Normalize<T>(Vector3D<T> value) where T : INumberBase<T>, IRootFunctions<T>
    {
        return value / value.Length();
    }

    /// <summary>Returns the reflection of a vector off a surface that has the specified normal.</summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="normal">The normal of the surface being reflected off.</param>
    /// <returns>The reflected vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<TReturn> Reflect<T, TReturn>(Vector3D<T> vector, Vector3D<T> normal) where T : INumberBase<T> where TReturn : INumberBase<TReturn>
    {
        var dot = Dot<T, TReturn>(vector, normal);
        return vector.As<TReturn>() - (NumericConstants<TReturn>.Two * (dot * normal.As<TReturn>()));
    }

    /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
    /// <param name="value">A vector.</param>
    /// <returns>The square root vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<TReturn> Sqrt<T, TReturn>(Vector3D<T> value) where T : INumberBase<T> where TReturn : INumberBase<TReturn>, IRootFunctions<TReturn>
    {
        return new Vector3D<TReturn>(
            TReturn.Sqrt(TReturn.CreateTruncating(value.X)), 
            TReturn.Sqrt(TReturn.CreateTruncating(value.Y)), 
            TReturn.Sqrt(TReturn.CreateTruncating(value.Z))
        );
    }

    // CANNOT BE DONE
    /*
    /// <summary>Transforms a vector by a specified 4x4 matrix.</summary>
    /// <param name="position">The vector to transform.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Transform<T>(Vector3D<T> position, Matrix4X4<T> matrix) where T : INumberBase<T>
    {
        return (Vector3D<T>)Vector4D.Transform(position, matrix);
    }

    /// <summary>Transforms a vector by the specified Quaternion rotation value.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<TReturn> Transform<T, TQuat, TReturn>(Vector3D<T> value, Quaternion<TQuat> rotation) where T : INumberBase<T> where TReturn : INumberBase<TReturn> where TQuat : ITrigonometricFunctions<TQuat>, IRootFunctions<TQuat>
    {
        var  = rotation.X + rotation.X;
        var  = rotation.Y + rotation.Y;
        var  = rotation.Z + rotation.Z;var x2 = rotation.X + rotation.X;
        var y2 = rotation.Y + rotation.Y;
        var z2 = rotation.Z + rotation.Z;


        var  = rotation.X + rotation.X;
        var  = rotation.Y + rotation.Y;
        var  = rotation.Z + rotation.Z;var wx2 = TReturn.CreateTruncating(rotation.W * x2);
        var wy2 = TReturn.CreateTruncating(rotation.W * y2);
        var wz2 = TReturn.CreateTruncating(rotation.W * z2);
        var xx2 = TReturn.CreateTruncating(rotation.X * x2);
        var xy2 = TReturn.CreateTruncating(rotation.X * y2);
        var xz2 = TReturn.CreateTruncating(rotation.X * z2);
        var yy2 = TReturn.CreateTruncating(rotation.Y * y2);
        var yz2 = TReturn.CreateTruncating(rotation.Y * z2);
        var zz2 = TReturn.CreateTruncating(rotation.Z * z2);

        return new Vector3D<TReturn>(
            TReturn.Sqrt(TReturn.CreateTruncating(value.X)), 
            TReturn.Sqrt(TReturn.CreateTruncating(value.Y)), 
            TReturn.Sqrt(TReturn.CreateTruncating(value.Z))
        );

        return new Vector3D<TReturn>(
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
    // internal static Vector3D<T> TransformNormal<T>(Vector3D<T> normal, in Matrix4x4 matrix) where T : INumberBase<T>
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
    public static Vector3D<T> Remainder<T>(this Vector3D<T> left, Vector3D<T> right) where T : INumberBase<T>, IModulusOperators<T, T, T>
    {
        return new Vector3D<T>(
            left.X % right.X,
            left.Y % right.Y,
            left.Z % right.Z
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Remainder<T>(this Vector3D<T> left, T right) where T : INumberBase<T>, IModulusOperators<T, T, T>
    {
        return new Vector3D<T>(
            left.X % right,
            left.Y % right,
            left.Z % right
        );
    }
    #endregion

    #region Specializations

    /// <summary>Returns the length of this vector object.</summary>
    /// <returns>The vector's length.</returns>
    /// <altmember cref="LengthSquared{T}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Length<T>(this Vector3D<T> vec) where T : INumberBase<T>, IRootFunctions<T>
    {
        return vec.Length<T, T>();
    }

    /// <summary>Returns the length of the vector squared.</summary>
    /// <returns>The vector's length squared.</returns>
    /// <remarks>This operation offers better performance than a call to the <see cref="Length{T}" /> method.</remarks>
    /// <altmember cref="Length{T}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T LengthSquared<T>(this Vector3D<T> vec) where T : INumberBase<T>
    {
        return vec.LengthSquared<T, T>();
    }

    /// <summary>Performs a linear interpolation between two vectors based on the given weighting.</summary>
    /// <param name="value1">The first vector.</param>
    /// <param name="value2">The second vector.</param>
    /// <param name="amount">A value between 0 and 1 that indicates the weight of <paramref name="value2" />.</param>
    /// <returns>The interpolated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Lerp<T>(Vector3D<T> value1, Vector3D<T> value2, T amount) where T : INumberBase<T>, IFloatingPoint<T>
    {
        return Lerp<T, T>(value1, value2, amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> LerpClamped<T>(Vector3D<T> value1, Vector3D<T> value2, T amount) where T : INumberBase<T>, IFloatingPoint<T>
    {
        return LerpClamped<T, T>(value1, value2, amount);
    }

    /// <summary>Returns the reflection of a vector off a surface that has the specified normal.</summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="normal">The normal of the surface being reflected off.</param>
    /// <returns>The reflected vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Reflect<T>(Vector3D<T> vector, Vector3D<T> normal) where T : IFloatingPoint<T>
    {
        return Reflect<T, T>(vector, normal);
    }

    /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
    /// <param name="value">A vector.</param>
    /// <returns>The square root vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Sqrt<T>(Vector3D<T> value) where T : IFloatingPoint<T>, IRootFunctions<T>
    {
        return Sqrt<T, T>(value);
    }

    // CANNOT BE DONE
    /*
    /// <summary>Transforms a vector by the specified Quaternion rotation value.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Transform<T>(Vector3D<T> value, Quaternion<T> rotation)
        where T : IFloatingPoint<T>, ITrigonometricFunctions<T>, IRootFunctions<T>
    {
        return Transform<T, T, T>(value, rotation);
    }


    /// <summary>Transforms a vector by the specified Quaternion rotation value.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3D<T> Transform<T, TQuat>(Vector3D<T> value, Quaternion<TQuat> rotation)
        where T : IFloatingPoint<T>
        where TQuat : ITrigonometricFunctions<TQuat>, IRootFunctions<TQuat>
    {
        return Transform<T, TQuat, T>(value, rotation);
    }
    */
    #endregion

    // Equivalent implementing IHyperbolicFunctions<System.Runtime.Intrinsics.Vector3>
    public static Vector3D<T> Acosh<T>(Vector3D<T> x) where T : IHyperbolicFunctions<T> => new(T.Acosh(x.X), T.Acosh(x.Y), T.Acosh(x.Z));
    public static Vector3D<T> Asinh<T>(Vector3D<T> x) where T : IHyperbolicFunctions<T> => new(T.Asinh(x.X), T.Asinh(x.Y), T.Asinh(x.Z));
    public static Vector3D<T> Atanh<T>(Vector3D<T> x) where T : IHyperbolicFunctions<T> => new(T.Atanh(x.X), T.Atanh(x.Y), T.Atanh(x.Z));
    public static Vector3D<T> Cosh<T>(Vector3D<T> x) where T : IHyperbolicFunctions<T> => new(T.Cosh(x.X), T.Cosh(x.Y), T.Cosh(x.Z));
    public static Vector3D<T> Sinh<T>(Vector3D<T> x) where T : IHyperbolicFunctions<T> => new(T.Sinh(x.X), T.Sinh(x.Y), T.Sinh(x.Z));
    public static Vector3D<T> Tanh<T>(Vector3D<T> x) where T : IHyperbolicFunctions<T> => new(T.Tanh(x.X), T.Tanh(x.Y), T.Tanh(x.Z));

    // Equivalent implementing ITrigonometricFunctions<System.Runtime.Intrinsics.Vector3>
    public static Vector3D<T> Acos<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.Acos(x.X), T.Acos(x.Y), T.Acos(x.Z));
    public static Vector3D<T> AcosPi<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.AcosPi(x.X), T.AcosPi(x.Y), T.AcosPi(x.Z));
    public static Vector3D<T> Asin<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.Asin(x.X), T.Asin(x.Y), T.Asin(x.Z));
    public static Vector3D<T> AsinPi<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.AsinPi(x.X), T.AsinPi(x.Y), T.AsinPi(x.Z));
    public static Vector3D<T> Atan<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.Atan(x.X), T.Atan(x.Y), T.Atan(x.Z));
    public static Vector3D<T> AtanPi<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.AtanPi(x.X), T.AtanPi(x.Y), T.AtanPi(x.Z));
    public static Vector3D<T> Cos<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.Cos(x.X), T.Cos(x.Y), T.Cos(x.Z));
    public static Vector3D<T> CosPi<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.CosPi(x.X), T.CosPi(x.Y), T.CosPi(x.Z));
    public static Vector3D<T> DegreesToRadians<T>(Vector3D<T> degrees) where T : ITrigonometricFunctions<T> => new(T.DegreesToRadians(degrees.X), T.DegreesToRadians(degrees.Y), T.DegreesToRadians(degrees.Z));
    public static Vector3D<T> RadiansToDegrees<T>(Vector3D<T> radians) where T : ITrigonometricFunctions<T> => new(T.RadiansToDegrees(radians.X), T.RadiansToDegrees(radians.Y), T.RadiansToDegrees(radians.Z));
    public static Vector3D<T> Sin<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.Sin(x.X), T.Sin(x.Y), T.Sin(x.Z));
    public static Vector3D<T> SinPi<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.SinPi(x.X), T.SinPi(x.Y), T.SinPi(x.Z));
    public static Vector3D<T> Tan<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.Tan(x.X), T.Tan(x.Y), T.Tan(x.Z));
    public static Vector3D<T> TanPi<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T> => new(T.TanPi(x.X), T.TanPi(x.Y), T.TanPi(x.Z));


    public static (Vector3D<T> Sin, Vector3D<T> Cos) SinCos<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T>
    {
        var (sinX, cosX) = T.SinCos(x.X);
        var (sinY, cosY) = T.SinCos(x.Y);
        var (sinZ, cosZ) = T.SinCos(x.Z);

        return (
            new Vector3D<T>(sinX, sinY, sinZ),
            new Vector3D<T>(cosX, cosY, cosZ)
        );
    }

    public static (Vector3D<T> SinPi, Vector3D<T> CosPi) SinCosPi<T>(Vector3D<T> x) where T : ITrigonometricFunctions<T>
    {
        var (sinX, cosX) = T.SinCosPi(x.X);
        var (sinY, cosY) = T.SinCosPi(x.Y);
        var (sinZ, cosZ) = T.SinCosPi(x.Z);

        return (
            new Vector3D<T>(sinX, sinY, sinZ),
            new Vector3D<T>(cosX, cosY, cosZ)
        );
    }

    // Equivalent implementing ILogarithmicFunctions<System.Runtime.Intrinsics.Vector3>
    public static Vector3D<T> Log<T>(Vector3D<T> x) where T : ILogarithmicFunctions<T> => new(T.Log(x.X), T.Log(x.Y), T.Log(x.Z));

    public static Vector3D<T> Log<T>(Vector3D<T> x, Vector3D<T> newBase) where T : ILogarithmicFunctions<T> => new(T.Log(x.X, newBase.X), T.Log(x.Y, newBase.Y), T.Log(x.Z, newBase.Z));
    public static Vector3D<T> Log<T>(Vector3D<T> x, T newBase) where T : ILogarithmicFunctions<T> => new(T.Log(x.X, newBase), T.Log(x.Y, newBase), T.Log(x.Z, newBase));
    public static Vector3D<T> LogP1<T>(Vector3D<T> x) where T : ILogarithmicFunctions<T> => Log(x + Vector3D<T>.One);
    public static Vector3D<T> Log2<T>(Vector3D<T> x) where T : ILogarithmicFunctions<T> => new(T.Log2(x.X), T.Log2(x.Y), T.Log2(x.Z));
    public static Vector3D<T> Log2P1<T>(Vector3D<T> x) where T : ILogarithmicFunctions<T> => Log2(x + Vector3D<T>.One);
    public static Vector3D<T> Log10<T>(Vector3D<T> x) where T : ILogarithmicFunctions<T> => new(T.Log10(x.X), T.Log10(x.Y), T.Log10(x.Z));
    public static Vector3D<T> Log10P1<T>(Vector3D<T> x) where T : ILogarithmicFunctions<T> => Log10(x + Vector3D<T>.One);

    // Equivalent implementing IExponentialFunctions<System.Runtime.Intrinsics.Vector3>
    public static Vector3D<T> Exp<T>(Vector3D<T> x) where T : IExponentialFunctions<T> => new(T.Exp(x.X), T.Exp(x.Y), T.Exp(x.Z));
    public static Vector3D<T> ExpM1<T>(Vector3D<T> x) where T : IExponentialFunctions<T> => Exp(x) - Vector3D<T>.One;
    public static Vector3D<T> Exp2<T>(Vector3D<T> x) where T : IExponentialFunctions<T> => new(T.Exp2(x.X), T.Exp2(x.Y), T.Exp2(x.Z));
    public static Vector3D<T> Exp2M1<T>(Vector3D<T> x) where T : IExponentialFunctions<T> => Exp2(x) - Vector3D<T>.One;
    public static Vector3D<T> Exp10<T>(Vector3D<T> x) where T : IExponentialFunctions<T> => new(T.Exp10(x.X), T.Exp10(x.Y), T.Exp10(x.Z));
    public static Vector3D<T> Exp10M1<T>(Vector3D<T> x) where T : IExponentialFunctions<T> => Exp10(x) - Vector3D<T>.One;

    // Equivalent implementing IPowerFunctions<System.Runtime.Intrinsics.Vector3>
    public static Vector3D<T> Pow<T>(Vector3D<T> x, Vector3D<T> y) where T : IPowerFunctions<T> => new(T.Pow(x.X, y.X), T.Pow(x.Y, y.Y), T.Pow(x.Z, y.Z));
    public static Vector3D<T> Pow<T>(Vector3D<T> x, T y) where T : IPowerFunctions<T> => new(T.Pow(x.X, y), T.Pow(x.Y, y), T.Pow(x.Z, y));

    // Equivalent implementing IRootFunctions<System.Runtime.Intrinsics.Vector3>
    public static Vector3D<T> Cbrt<T>(Vector3D<T> x) where T : IRootFunctions<T> => new(T.Cbrt(x.X), T.Cbrt(x.Y), T.Cbrt(x.Z));
    public static Vector3D<T> Hypot<T>(Vector3D<T> x, Vector3D<T> y) where T : IRootFunctions<T> => new(T.Hypot(x.X, y.X), T.Hypot(x.Y, y.Y), T.Hypot(x.Z, y.Z));
    public static Vector3D<T> Hypot<T>(Vector3D<T> x, T y) where T : IRootFunctions<T> => new(T.Hypot(x.X, y), T.Hypot(x.Y, y), T.Hypot(x.Z, y));
    public static Vector3D<T> RootN<T>(Vector3D<T> x, int n) where T : IRootFunctions<T> => new(T.RootN(x.X, n), T.RootN(x.Y, n), T.RootN(x.Z, n));

    // IFloatingPoint<TSelf>
    public static Vector3D<T> Round<T>(Vector3D<T> x) where T : IFloatingPoint<T> => new(T.Round(x.X), T.Round(x.Y), T.Round(x.Z));
    public static Vector3D<T> Round<T>(Vector3D<T> x, int digits) where T : IFloatingPoint<T> => new(T.Round(x.X, digits), T.Round(x.Y, digits), T.Round(x.Z, digits));
    public static Vector3D<T> Round<T>(Vector3D<T> x, MidpointRounding mode) where T : IFloatingPoint<T> => new(T.Round(x.X, mode), T.Round(x.Y, mode), T.Round(x.Z, mode));
    public static Vector3D<T> Round<T>(Vector3D<T> x, int digits, MidpointRounding mode) where T : IFloatingPoint<T> => new(T.Round(x.X, digits, mode), T.Round(x.Y, digits, mode), T.Round(x.Z, digits, mode));
    public static Vector3D<T> Truncate<T>(Vector3D<T> x) where T : IFloatingPoint<T> => new(T.Truncate(x.X), T.Truncate(x.Y), T.Truncate(x.Z));

    // IFloatingPointIeee754<TSelf>
    public static Vector3D<T> Atan2<T>(Vector3D<T> x, Vector3D<T> y) where T : IFloatingPointIeee754<T> => new(T.Atan2(x.X, y.X), T.Atan2(x.Y, y.Y), T.Atan2(x.Z, y.Z));
    public static Vector3D<T> Atan2Pi<T>(Vector3D<T> x, Vector3D<T> y) where T : IFloatingPointIeee754<T> => new(T.Atan2Pi(x.X, y.X), T.Atan2Pi(x.Y, y.Y), T.Atan2Pi(x.Z, y.Z));
    public static Vector3D<T> Atan2<T>(Vector3D<T> x, T y) where T : IFloatingPointIeee754<T> => new(T.Atan2(x.X, y), T.Atan2(x.Y, y), T.Atan2(x.Z, y));
    public static Vector3D<T> Atan2Pi<T>(Vector3D<T> x, T y) where T : IFloatingPointIeee754<T> => new(T.Atan2Pi(x.X, y), T.Atan2Pi(x.Y, y), T.Atan2Pi(x.Z, y));
    public static Vector3D<T> BitDecrement<T>(Vector3D<T> x) where T : IFloatingPointIeee754<T> => new(T.BitDecrement(x.X), T.BitDecrement(x.Y), T.BitDecrement(x.Z));
    public static Vector3D<T> BitIncrement<T>(Vector3D<T> x) where T : IFloatingPointIeee754<T> => new(T.BitIncrement(x.X), T.BitIncrement(x.Y), T.BitIncrement(x.Z));

    public static Vector3D<T> FusedMultiplyAdd<T>(Vector3D<T> left, Vector3D<T> right, Vector3D<T> addend) where T : IFloatingPointIeee754<T> => new(T.FusedMultiplyAdd(left.X, right.X, addend.X), T.FusedMultiplyAdd(left.Y, right.Y, addend.Y), T.FusedMultiplyAdd(left.Z, right.Z, addend.Z));
    // public static Vector3D<T> Lerp<T>(Vector3D<T> value1, Vector3D<T> value2, Vector3D<T> amount) where T : IFloatingPointIeee754<T> => new(T.Lerp(value1.X, value2.X, amount.X), T.Lerp(value1.Y, value2.Y, amount.Y), T.Lerp(value1.Z, value2.Z, amount.Z));
    public static Vector3D<T> ReciprocalEstimate<T>(Vector3D<T> x) where T : IFloatingPointIeee754<T> => new(T.ReciprocalEstimate(x.X), T.ReciprocalEstimate(x.Y), T.ReciprocalEstimate(x.Z));
    public static Vector3D<T> ReciprocalSqrtEstimate<T>(Vector3D<T> x) where T : IFloatingPointIeee754<T> => new(T.ReciprocalSqrtEstimate(x.X), T.ReciprocalSqrtEstimate(x.Y), T.ReciprocalSqrtEstimate(x.Z));

    // INumber<T>
    // public static Vector3D<T> Clamp<T>(Vector3D<T> value, Vector3D<T> min, Vector3D<T> max) where T : INumber<T> => new(T.Clamp(x.X), T.Clamp(x.Y), T.Clamp(x.Z));
    public static Vector3D<T> CopySign<T>(Vector3D<T> value, Vector3D<T> sign) where T : INumber<T> => new(T.CopySign(value.X, sign.X), T.CopySign(value.Y, sign.Y), T.CopySign(value.Z, sign.Z));
    public static Vector3D<T> CopySign<T>(Vector3D<T> value, T sign) where T : INumber<T> => new(T.CopySign(value.X, sign), T.CopySign(value.Y, sign), T.CopySign(value.Z, sign));
    public static Vector3D<T> MaxNumber<T>(Vector3D<T> x, Vector3D<T> y) where T : INumber<T> => new(T.MaxNumber(x.X, y.X), T.MaxNumber(x.Y, y.Y), T.MaxNumber(x.Z, y.Z));
    public static Vector3D<T> MinNumber<T>(Vector3D<T> x, Vector3D<T> y) where T : INumber<T> => new(T.MinNumber(x.X, y.X), T.MinNumber(x.Y, y.Y), T.MinNumber(x.Z, y.Z));

    // INumberBase<T>
    // public static Vector3D<T> MaxMagnitude<T>(Vector3D<T> x, Vector3D<T> y) where T : INumberBase<T> => new(T.MaxMagnitude(x.X, y.X), T.MaxMagnitude(x.Y, y.Y), T.MaxMagnitude(x.Z, y.Z));
    // public static Vector3D<T> MaxMagnitudeNumber<T>(Vector3D<T> x, Vector3D<T> y) where T : INumberBase<T> => new(T.MaxMagnitudeNumber(x.X, y.X), T.MaxMagnitudeNumber(x.Y, y.Y), T.MaxMagnitudeNumber(x.Z, y.Z));
    // public static Vector3D<T> MinMagnitude<T>(Vector3D<T> x, Vector3D<T> y) where T : INumberBase<T> => new(T.MinMagnitude(x.X, y.X), T.MinMagnitude(x.Y, y.Y), T.MinMagnitude(x.Z, y.Z));
    // public static Vector3D<T> MinMagnitudeNumber<T>(Vector3D<T> x, Vector3D<T> y) where T : INumberBase<T> => new(T.MinMagnitudeNumber(x.X, y.X), T.MinMagnitudeNumber(x.Y, y.Y), T.MinMagnitudeNumber(x.Z, y.Z));
    // (there's no reason you would want these.)



    // IFloatingPointIeee754<TSelf>
    public static Vector3D<int> ILogB<T>(Vector3D<T> x) where T : IFloatingPointIeee754<T> => new(T.ILogB(x.X), T.ILogB(x.Y), T.ILogB(x.Z));
    public static Vector3D<T> ScaleB<T>(Vector3D<T> x, Vector3D<int> n) where T : IFloatingPointIeee754<T> => new(T.ScaleB(x.X, n.X), T.ScaleB(x.Y, n.Y), T.ScaleB(x.Z, n.Z));
    public static Vector3D<T> ScaleB<T>(Vector3D<T> x, int n) where T : IFloatingPointIeee754<T> => new(T.ScaleB(x.X, n), T.ScaleB(x.Y, n), T.ScaleB(x.Z, n));

    public static Vector3D<int> RoundToInt<T>(Vector3D<T> vector) where T : IFloatingPoint<T>
    {
        return new Vector3D<int>(
            int.CreateSaturating(T.Round(vector.X)),
            int.CreateSaturating(T.Round(vector.Y)),
            int.CreateSaturating(T.Round(vector.Z))
        );
    }

    public static Vector3D<int> FloorToInt<T>(Vector3D<T> vector) where T : IFloatingPoint<T>
    {
        return new Vector3D<int>(
            int.CreateSaturating(T.Floor(vector.X)),
            int.CreateSaturating(T.Floor(vector.Y)),
            int.CreateSaturating(T.Floor(vector.Z))
        );
    }

    public static Vector3D<int> CeilingToInt<T>(Vector3D<T> vector) where T : IFloatingPoint<T>
    {
        return new Vector3D<int>(
            int.CreateSaturating(T.Ceiling(vector.X)),
            int.CreateSaturating(T.Ceiling(vector.Y)),
            int.CreateSaturating(T.Ceiling(vector.Z))
        );
    }

    public static Vector3D<TInt> RoundToInt<T, TInt>(Vector3D<T> vector) where T : IFloatingPoint<T> where TInt : IBinaryInteger<TInt>
    {
        return new Vector3D<TInt>(
            TInt.CreateSaturating(T.Round(vector.X)),
            TInt.CreateSaturating(T.Round(vector.Y)),
            TInt.CreateSaturating(T.Round(vector.Z))
        );
    }

    public static Vector3D<TInt> FloorToInt<T, TInt>(Vector3D<T> vector) where T : IFloatingPoint<T> where TInt : IBinaryInteger<TInt>
    {
        return new Vector3D<TInt>(
            TInt.CreateSaturating(T.Floor(vector.X)),
            TInt.CreateSaturating(T.Floor(vector.Y)),
            TInt.CreateSaturating(T.Floor(vector.Z))
        );
    }

    public static Vector3D<TInt> CeilingToInt<T, TInt>(Vector3D<T> vector) where T : IFloatingPoint<T> where TInt : IBinaryInteger<TInt>
    {
        return new Vector3D<TInt>(
            TInt.CreateSaturating(T.Ceiling(vector.X)),
            TInt.CreateSaturating(T.Ceiling(vector.Y)),
            TInt.CreateSaturating(T.Ceiling(vector.Z))
        );
    }

    public static Vector3D<float> AsGeneric(this Vector3 vector)
        => Unsafe.BitCast<Vector3, Vector3D<float>>(vector);

    public static Vector3 AsNumerics(this Vector3D<float> vector)
        => Unsafe.BitCast<Vector3D<float>, Vector3>(vector);
}