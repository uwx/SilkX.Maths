
using System.Buffers;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Unicode;

namespace GenericVector.Experimental;

public readonly partial record struct Vector5F<TScalar> :
    IVectorInternal<Vector5F<TScalar>, TScalar>,
    IFloatingPointVector<Vector5F<TScalar>, TScalar>,
    IVector5<Vector5F<TScalar>, TScalar>
    where TScalar : IBinaryFloatingPointIeee754<TScalar>
{
    internal const int ElementCount = 5;
    
    
    /// <summary>The X component of the vector.</summary>
    [DataMember]
    public TScalar X { get; }
    /// <summary>The Y component of the vector.</summary>
    [DataMember]
    public TScalar Y { get; }
    /// <summary>The Z component of the vector.</summary>
    [DataMember]
    public TScalar Z { get; }
    /// <summary>The W component of the vector.</summary>
    [DataMember]
    public TScalar W { get; }
    /// <summary>The V component of the vector.</summary>
    [DataMember]
    public TScalar V { get; }
    
    public TScalar this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.AsSpan()[index];
    }
    
    
    /// <summary>Gets the vector (1,0,0,0,0)).</summary>
    /// <value>The vector <c>(1,0,0,0,0)</c>.</value>
    public static Vector5F<TScalar> UnitX => new(TScalar.One, TScalar.Zero, TScalar.Zero, TScalar.Zero, TScalar.Zero);
    /// <summary>Gets the vector (0,1,0,0,0)).</summary>
    /// <value>The vector <c>(0,1,0,0,0)</c>.</value>
    public static Vector5F<TScalar> UnitY => new(TScalar.Zero, TScalar.One, TScalar.Zero, TScalar.Zero, TScalar.Zero);
    /// <summary>Gets the vector (0,0,1,0,0)).</summary>
    /// <value>The vector <c>(0,0,1,0,0)</c>.</value>
    public static Vector5F<TScalar> UnitZ => new(TScalar.Zero, TScalar.Zero, TScalar.One, TScalar.Zero, TScalar.Zero);
    /// <summary>Gets the vector (0,0,0,1,0)).</summary>
    /// <value>The vector <c>(0,0,0,1,0)</c>.</value>
    public static Vector5F<TScalar> UnitW => new(TScalar.Zero, TScalar.Zero, TScalar.Zero, TScalar.One, TScalar.Zero);
    /// <summary>Gets the vector (0,0,0,0,1)).</summary>
    /// <value>The vector <c>(0,0,0,0,1)</c>.</value>
    public static Vector5F<TScalar> UnitV => new(TScalar.Zero, TScalar.Zero, TScalar.Zero, TScalar.Zero, TScalar.One);
    
    /// <summary>Gets a vector whose 5 elements are equal to zero.</summary>
    /// <value>A vector whose  elements are equal to zero (that is, it returns the vector <c>(0,0,0,0,0)</c>.</value>
    public static Vector5F<TScalar> Zero => new(TScalar.Zero);
    
    /// <summary>Gets a vector whose 5 elements are equal to one.</summary>
    /// <value>Returns <see cref="Vector5F{TScalar}" />.</value>
    /// <remarks>A vector whose  elements are equal to one (that is, it returns the vector <c>(1,1,1,1,1)</c>.</remarks>
    public static Vector5F<TScalar> One => new(TScalar.One);
    
    
    /// <summary>Creates a vector whose elements have the specified values.</summary>
    /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
    /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
    /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
    /// <param name="w">The value to assign to the <see cref="W" /> field.</param>
    /// <param name="v">The value to assign to the <see cref="V" /> field.</param>
    public Vector5F(TScalar x, TScalar y, TScalar z, TScalar w, TScalar v)
    {
        Unsafe.SkipInit(out this);
    
        X = x;
        Y = y;
        Z = z;
        W = w;
        V = v;
    }
    
    /// <summary>Creates a new <see cref="Vector5F{TScalar}" /> object whose  elements have the same value.</summary>
    /// <param name="value">The value to assign to all  elements.</param>
    public Vector5F(TScalar value) : this(value, value, value, value, value)
    {
    }
    
    /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{TScalar}" />. The span must contain at least 2 elements.</summary>
    /// <param name="values">The span of elements to assign to the vector.</param>
    public Vector5F(ReadOnlySpan<TScalar> values)
    {
        Unsafe.SkipInit(out this);
    
        ArgumentOutOfRangeException.ThrowIfLessThan(values.Length, ElementCount, nameof(values));
    
        this = Unsafe.ReadUnaligned<Vector5F<TScalar>>(ref Unsafe.As<TScalar, byte>(ref MemoryMarshal.GetReference(values)));
    }
    
    
    /// <summary>Creates a new <see cref="Vector5F{TScalar}" /> object from the specified <see cref="Vector5i{TScalar}" /> object Z and a W and a V component.</summary>
    /// <param name="value">The vector to use for the Scriban.Runtime.ScriptRange components.</param>
    /// <param name="z">The Z component.</param>
    /// <param name="w">The W component.</param>
    /// <param name="v">The V component.</param>
    public Vector5F(Vector2D<TScalar> value, TScalar z, TScalar w, TScalar v) : this(value.Xvalue.Yvalue.Zvalue.Wvalue.Vvalue.Xvalue.Yvalue.Zvalue.Wvalue.V, z, w, v)
    {
    }
    /// <summary>Creates a new <see cref="Vector5F{TScalar}" /> object from the specified <see cref="Vector5i{TScalar}" /> object W and a V component.</summary>
    /// <param name="value">The vector to use for the Scriban.Runtime.ScriptRange components.</param>
    /// <param name="w">The W component.</param>
    /// <param name="v">The V component.</param>
    public Vector5F(Vector3D<TScalar> value, TScalar w, TScalar v) : this(value.Xvalue.Yvalue.Zvalue.Wvalue.Vvalue.Xvalue.Yvalue.Zvalue.Wvalue.V, , w, v)
    {
    }
    /// <summary>Creates a new <see cref="Vector5F{TScalar}" /> object from the specified <see cref="Vector5i{TScalar}" /> object V component.</summary>
    /// <param name="value">The vector to use for the Scriban.Runtime.ScriptRange components.</param>
    /// <param name="v">The V component.</param>
    public Vector5F(Vector4D<TScalar> value, TScalar v) : this(value.Xvalue.Yvalue.Zvalue.Wvalue.Vvalue.Xvalue.Yvalue.Zvalue.Wvalue.V, , , v)
    {
    }
    
    public static Vector5F<TScalar> operator +(Vector5F<TScalar> left, Vector5F<TScalar> right) => SpeedHelpers2.Add<Vector5F<TScalar>, TScalar>(left, right);
    public static Vector5F<TScalar> operator -(Vector5F<TScalar> left, Vector5F<TScalar> right) => SpeedHelpers2.Subtract<Vector5F<TScalar>, TScalar>(left, right);
    public static Vector5F<TScalar> operator *(Vector5F<TScalar> left, Vector5F<TScalar> right) => SpeedHelpers2.Multiply<Vector5F<TScalar>, TScalar>(left, right);
    public static Vector5F<TScalar> operator /(Vector5F<TScalar> left, Vector5F<TScalar> right) => SpeedHelpers2.Divide<Vector5F<TScalar>, TScalar>(left, right);
    public static Vector5F<TScalar> operator %(Vector5F<TScalar> left, Vector5F<TScalar> right) => SpeedHelpers2.Remainder<Vector5F<TScalar>, TScalar>(left, right);
    
    public static Vector5F<TScalar> operator *(Vector5F<TScalar> left, TScalar right) => SpeedHelpers2.Multiply(left, right);
    public static Vector5F<TScalar> operator /(Vector5F<TScalar> left, TScalar right) => SpeedHelpers2.Divide(left, right);
    public static Vector5F<TScalar> operator %(Vector5F<TScalar> left, TScalar right) => SpeedHelpers2.Remainder(left, right);
    
    public static Vector5F<TScalar> operator *(TScalar left, Vector5F<TScalar> right) => right * left;
    
    public static Vector5F<TScalar> operator -(Vector5F<TScalar> value) => SpeedHelpers2.Negate<Vector5F<TScalar>, TScalar>(value);
    public static Vector5F<TScalar> operator +(Vector5F<TScalar> value) => value;
    
    public static Vector5F<TScalar> operator &(Vector5F<TScalar> left, Vector5F<TScalar> right) => SpeedHelpers2.BitwiseAnd<Vector5F<TScalar>, TScalar>(left, right);
    public static Vector5F<TScalar> operator |(Vector5F<TScalar> left, Vector5F<TScalar> right) => SpeedHelpers2.BitwiseOr<Vector5F<TScalar>, TScalar>(left, right);
    public static Vector5F<TScalar> operator ^(Vector5F<TScalar> left, Vector5F<TScalar> right) => SpeedHelpers2.BitwiseXor<Vector5F<TScalar>, TScalar>(left, right);
    public static Vector5F<TScalar> operator ~(Vector5F<TScalar> value) => SpeedHelpers2.BitwiseNot<Vector5F<TScalar>, TScalar>(value);
    
    // public static bool operator ==(Vector5F<TScalar> left, Vector5F<TScalar> right) => left.Equals(right);
    // public static bool operator !=(Vector5F<TScalar> left, Vector5F<TScalar> right) => !(left == right);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public override string ToString() => ToString("G", null);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) => ToString(format, null);
    
    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
    
        Span<char> initialBuffer = stackalloc char[Math.Min((2 + (ElementCount - 1) + (separator.Length * (ElementCount - 1)) + (ElementCount * 2)), 256)];
    
        // We can't use an interpolated string here because it won't allow us to pass `format`
        var handler = new DefaultInterpolatedStringHandler(
            4 + (separator.Length * 2),
            ElementCount,
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
        handler.AppendLiteral(separator);
        handler.AppendLiteral(" ");
        handler.AppendFormatted(W, format);
        handler.AppendLiteral(separator);
        handler.AppendLiteral(" ");
        handler.AppendFormatted(V, format);
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
            ElementCount,
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
            handler.AppendLiteral(separator) &&
            handler.AppendLiteral(" ") &&
            handler.AppendFormatted(W, formatString) &&
            handler.AppendLiteral(separator) &&
            handler.AppendLiteral(" ") &&
            handler.AppendFormatted(V, formatString) &&
            handler.AppendLiteral(">");
    
        return destination.TryWrite(ref handler, out charsWritten);
    }
    
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        // Possible fast path for failure case:
        // if (destination.Length < 4) return false;
    
        var separator = NumberFormatInfo.GetInstance(provider).NumberGroupSeparator;
    
        // We can't use an interpolated string here because it won't allow us to pass `format`
        var handler = new Utf8.TryWriteInterpolatedStringHandler(
            4 + (separator.Length * 2),
            ElementCount,
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
            handler.AppendLiteral(separator) &&
            handler.AppendLiteral(" ") &&
            handler.AppendFormatted(W, formatString) &&
            handler.AppendLiteral(separator) &&
            handler.AppendLiteral(" ") &&
            handler.AppendFormatted(V, formatString) &&
            handler.AppendLiteral(">");
    
        return Utf8.TryWrite(utf8Destination, ref handler, out bytesWritten);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5F<TScalar> Parse(string s, IFormatProvider? provider)
        => Parse(s.AsSpan(), provider);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5F<TScalar> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => TryParse(s, provider, out var result) ? result : throw new ArgumentException($"Failed to parse {nameof(Vector5F)}<{typeof(TScalar)}>");
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector5F<TScalar> Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
        => TryParse(utf8Text, provider, out var result) ? result : throw new ArgumentException($"Failed to parse {nameof(Vector5F)}<{typeof(TScalar)}>");
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse(string? s, IFormatProvider? provider, out Vector5F<TScalar> result)
        => TryParse(s.AsSpan(), provider, out result);
    
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Vector5F<TScalar> result)
    {
        result = default;
    
        if (s[0] != '<') return false;
        if (s[^1] != '>') return false;
    
        var separator = NumberFormatInfo.GetInstance(provider).NumberGroupSeparator;
    
        s = s[1..^1];
    
        TScalar? x, y, z, w, v;
    
        {
            if (s.Length == 0) return false;
    
            var nextNumber = s.IndexOf(separator);
            if (nextNumber == -1)
            {
                return false;
            }
    
            if (!TScalar.TryParse(s[..nextNumber],  provider, out x)) return false;
    
            s = s[(nextNumber + separator.Length)..];
        }
        {
            if (s.Length == 0) return false;
    
            var nextNumber = s.IndexOf(separator);
            if (nextNumber == -1)
            {
                return false;
            }
    
            if (!TScalar.TryParse(s[..nextNumber],  provider, out y)) return false;
    
            s = s[(nextNumber + separator.Length)..];
        }
        {
            if (s.Length == 0) return false;
    
            var nextNumber = s.IndexOf(separator);
            if (nextNumber == -1)
            {
                return false;
            }
    
            if (!TScalar.TryParse(s[..nextNumber],  provider, out z)) return false;
    
            s = s[(nextNumber + separator.Length)..];
        }
        {
            if (s.Length == 0) return false;
    
            var nextNumber = s.IndexOf(separator);
            if (nextNumber == -1)
            {
                return false;
            }
    
            if (!TScalar.TryParse(s[..nextNumber],  provider, out w)) return false;
    
            s = s[(nextNumber + separator.Length)..];
        }
    
        {
            if (s.Length == 0) return false;
    
            if (!TScalar.TryParse(s, provider, out v)) return false;
        }
    
        result = new Vector5F<TScalar>(x, y, z, w, v);
        return true;
    }
    
    public static bool TryParse(ReadOnlySpan<byte> s, IFormatProvider? provider, out Vector5F<TScalar> result)
    {
        var separator = NumberFormatInfo.GetInstance(provider).NumberGroupSeparator;
    
        Span<byte> separatorSpan = stackalloc byte[Encoding.UTF8.GetByteCount(separator)];
        if (Utf8.FromUtf16(separator, separatorSpan, out var charsRead, out var bytesWritten, isFinalBlock: true) != OperationStatus.Done)
        {
            result = default;
            return false;
        }
    
        result = default;
    
        if (s[0] != (byte)'<') return false;
        if (s[^1] != (byte)'>') return false;
    
        s = s[1..^1];
    
        TScalar? x, y, z, w, v;
    
        {
            if (s.Length == 0) return false;
    
            var nextNumber = s.IndexOf(separatorSpan);
            if (nextNumber == -1)
            {
                return false;
            }
    
            if (!TScalar.TryParse(s[..nextNumber], provider, out x)) return false;
    
            s = s[(nextNumber + separatorSpan.Length)..];
        }
        {
            if (s.Length == 0) return false;
    
            var nextNumber = s.IndexOf(separatorSpan);
            if (nextNumber == -1)
            {
                return false;
            }
    
            if (!TScalar.TryParse(s[..nextNumber], provider, out y)) return false;
    
            s = s[(nextNumber + separatorSpan.Length)..];
        }
        {
            if (s.Length == 0) return false;
    
            var nextNumber = s.IndexOf(separatorSpan);
            if (nextNumber == -1)
            {
                return false;
            }
    
            if (!TScalar.TryParse(s[..nextNumber], provider, out z)) return false;
    
            s = s[(nextNumber + separatorSpan.Length)..];
        }
        {
            if (s.Length == 0) return false;
    
            var nextNumber = s.IndexOf(separatorSpan);
            if (nextNumber == -1)
            {
                return false;
            }
    
            if (!TScalar.TryParse(s[..nextNumber], provider, out w)) return false;
    
            s = s[(nextNumber + separatorSpan.Length)..];
        }
    
        {
            if (s.Length == 0) return false;
    
            if (!TScalar.TryParse(s, provider, out v)) return false;
        }
    
        result = new Vector5F<TScalar>(x, y, z, w, v);
        return true;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public ReadOnlySpan<TScalar>.Enumerator GetEnumerator() => this.AsSpan().GetEnumerator();
    IEnumerator<TScalar> IEnumerable<TScalar>.GetEnumerator()
    {
        yield return X;
        yield return Y;
        yield return Z;
        yield return W;
        yield return V;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TScalar>)this).GetEnumerator();
    
    public bool Equals(Vector5F<TScalar> other) => SpeedHelpers2.Equal<Vector5F<TScalar>, TScalar>(this, other);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVectorInternal<Vector5F<TScalar>, TScalar>.CreateInternal(TScalar x, TScalar y, TScalar z, TScalar w, TScalar v) => new(x, y, z, w, v);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public override int GetHashCode() => HashCode.Combine(X, Y, Z, W, V);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] TScalar IVector<Vector5F<TScalar>, TScalar>.LengthSquared() => Vector5F.LengthSquared(this);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] Vector5F<TScalar> IVectorEquatable<Vector5F<TScalar>, TScalar>.ScalarsEqual(Vector5F<TScalar> other) => SpeedHelpers2.EqualIntoVector<Vector5F<TScalar>, TScalar>(this, other);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static ReadOnlySpan<TScalar> IVector<Vector5F<TScalar>, TScalar>.AsSpan(Vector5F<TScalar> vec) => vec.AsSpan();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVector<Vector5F<TScalar>, TScalar>.GetUnitVector(uint dimension) => dimension switch
    {
        0 => UnitX,
        1 => UnitY,
        2 => UnitZ,
        3 => UnitW,
        4 => UnitV,
        _ => throw new ArgumentOutOfRangeException(nameof(dimension), dimension, "dimension must be >= 0, <= 4")
    };
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVector<Vector5F<TScalar>, TScalar>.Clamp(Vector5F<TScalar> value1, Vector5F<TScalar> min, Vector5F<TScalar> max) => Vector5F.Clamp(value1, min, max);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVector<Vector5F<TScalar>, TScalar>.Max(Vector5F<TScalar> value1, Vector5F<TScalar> value2) => Vector5F.Max(value1, value2);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVector<Vector5F<TScalar>, TScalar>.Min(Vector5F<TScalar> value1, Vector5F<TScalar> value2) => Vector5F.Min(value1, value2);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVector<Vector5F<TScalar>, TScalar>.Abs(Vector5F<TScalar> value) => Vector5F.Abs(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static TScalar IVector<Vector5F<TScalar>, TScalar>.DistanceSquared(Vector5F<TScalar> value1, Vector5F<TScalar> value2) => Vector5F.DistanceSquared(value1, value2);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static TScalar IVector<Vector5F<TScalar>, TScalar>.Dot(Vector5F<TScalar> vector1, Vector5F<TScalar> vector2) => Vector5F.Dot(vector1, vector2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static void IVector<Vector5F<TScalar>, TScalar>.CopyTo(Vector5F<TScalar> vector, TScalar[] array) => vector.CopyTo(array);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static void IVector<Vector5F<TScalar>, TScalar>.CopyTo(Vector5F<TScalar> vector, TScalar[] array, int index) => vector.CopyTo(array, index);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static void IVector<Vector5F<TScalar>, TScalar>.CopyTo(Vector5F<TScalar> vector, Span<TScalar> destination) => vector.CopyTo(destination);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static bool IVector<Vector5F<TScalar>, TScalar>.TryCopyTo(Vector5F<TScalar> vector, Span<TScalar> destination) => vector.TryCopyTo(destination);
    
    static bool IVector<Vector5F<TScalar>, TScalar>.TryConvertFromChecked<TOther, TOtherScalar>(TOther value, out Vector5F<TScalar> result)
    {
        if (TOther.Count < ElementCount)
        {
            result = default;
            return false;
        }
    
        // For Silk.NET.Maths-provided vectors, where the scalars are the exact same type, and the size is at least as
        // large as the vector type being converted to, we can safely do a bitcast.
        if (value is IVectorInternal && typeof(TScalar) == typeof(TOtherScalar))
        {
            result = Unsafe.As<TOther, Vector5F<TScalar>>(ref value);
            return true;
        }
    
        if (
            !ShamelessExploit.TryConvertChecked<TOtherScalar, TScalar>(value[0], out var x) ||
            !ShamelessExploit.TryConvertChecked<TOtherScalar, TScalar>(value[1], out var y) ||
            !ShamelessExploit.TryConvertChecked<TOtherScalar, TScalar>(value[2], out var z) ||
            !ShamelessExploit.TryConvertChecked<TOtherScalar, TScalar>(value[3], out var w) ||
            !ShamelessExploit.TryConvertChecked<TOtherScalar, TScalar>(value[4], out var v)
        )
        {
            result = default;
            return false;
        }
    
        result = new Vector5F<TScalar>(x, y, z, w, v);
        return true;
    }
    static bool IVector<Vector5F<TScalar>, TScalar>.TryConvertFromSaturating<TOther, TOtherScalar>(TOther value, out Vector5F<TScalar> result)
    {
        if (TOther.Count < ElementCount)
        {
            result = default;
            return false;
        }
    
        // For Silk.NET.Maths-provided vectors, where the scalars are the exact same type, and the size is at least as
        // large as the vector type being converted to, we can safely do a bitcast.
        if (value is IVectorInternal && typeof(TScalar) == typeof(TOtherScalar))
        {
            result = Unsafe.As<TOther, Vector5F<TScalar>>(ref value);
            return true;
        }
    
        if (
            !ShamelessExploit.TryConvertSaturating<TOtherScalar, TScalar>(value[0], out var x) ||
            !ShamelessExploit.TryConvertSaturating<TOtherScalar, TScalar>(value[1], out var y) ||
            !ShamelessExploit.TryConvertSaturating<TOtherScalar, TScalar>(value[2], out var z) ||
            !ShamelessExploit.TryConvertSaturating<TOtherScalar, TScalar>(value[3], out var w) ||
            !ShamelessExploit.TryConvertSaturating<TOtherScalar, TScalar>(value[4], out var v)
        )
        {
            result = default;
            return false;
        }
    
        result = new Vector5F<TScalar>(x, y, z, w, v);
        return true;
    }
    static bool IVector<Vector5F<TScalar>, TScalar>.TryConvertFromTruncating<TOther, TOtherScalar>(TOther value, out Vector5F<TScalar> result)
    {
        if (TOther.Count < ElementCount)
        {
            result = default;
            return false;
        }
    
        // For Silk.NET.Maths-provided vectors, where the scalars are the exact same type, and the size is at least as
        // large as the vector type being converted to, we can safely do a bitcast.
        if (value is IVectorInternal && typeof(TScalar) == typeof(TOtherScalar))
        {
            result = Unsafe.As<TOther, Vector5F<TScalar>>(ref value);
            return true;
        }
    
        if (
            !ShamelessExploit.TryConvertTruncating<TOtherScalar, TScalar>(value[0], out var x) ||
            !ShamelessExploit.TryConvertTruncating<TOtherScalar, TScalar>(value[1], out var y) ||
            !ShamelessExploit.TryConvertTruncating<TOtherScalar, TScalar>(value[2], out var z) ||
            !ShamelessExploit.TryConvertTruncating<TOtherScalar, TScalar>(value[3], out var w) ||
            !ShamelessExploit.TryConvertTruncating<TOtherScalar, TScalar>(value[4], out var v)
        )
        {
            result = default;
            return false;
        }
    
        result = new Vector5F<TScalar>(x, y, z, w, v);
        return true;
    }
    static bool IVector<Vector5F<TScalar>, TScalar>.TryConvertToChecked<TOther, TOtherScalar>(Vector5F<TScalar> value, [MaybeNullWhen(false)] out TOther result)
    {
        if (
            !ShamelessExploit.TryConvertChecked<TScalar, TOtherScalar>(value.X, out var x) ||
            !ShamelessExploit.TryConvertChecked<TScalar, TOtherScalar>(value.Y, out var y) ||
            !ShamelessExploit.TryConvertChecked<TScalar, TOtherScalar>(value.Z, out var z) ||
            !ShamelessExploit.TryConvertChecked<TScalar, TOtherScalar>(value.W, out var w) ||
            !ShamelessExploit.TryConvertChecked<TScalar, TOtherScalar>(value.V, out var v)
        )
        {
            result = default;
            return false;
        }
    
        result = new Vector5F<TOtherScalar>(x, y, z, w, v);
        return true;
    }
    static bool IVector<Vector5F<TScalar>, TScalar>.TryConvertToSaturating<TOther, TOtherScalar>(Vector5F<TScalar> value, [MaybeNullWhen(false)] out TOther result)
    {
        if (
            !ShamelessExploit.TryConvertSaturating<TScalar, TOtherScalar>(value.X, out var x) ||
            !ShamelessExploit.TryConvertSaturating<TScalar, TOtherScalar>(value.Y, out var y) ||
            !ShamelessExploit.TryConvertSaturating<TScalar, TOtherScalar>(value.Z, out var z) ||
            !ShamelessExploit.TryConvertSaturating<TScalar, TOtherScalar>(value.W, out var w) ||
            !ShamelessExploit.TryConvertSaturating<TScalar, TOtherScalar>(value.V, out var v)
        )
        {
            result = default;
            return false;
        }
    
        result = new Vector5F<TOtherScalar>(x, y, z, w, v);
        return true;
    }
    static bool IVector<Vector5F<TScalar>, TScalar>.TryConvertToTruncating<TOther, TOtherScalar>(Vector5F<TScalar> value, [MaybeNullWhen(false)] out TOther result)
    {
        if (
            !ShamelessExploit.TryConvertTruncating<TScalar, TOtherScalar>(value.X, out var x) ||
            !ShamelessExploit.TryConvertTruncating<TScalar, TOtherScalar>(value.Y, out var y) ||
            !ShamelessExploit.TryConvertTruncating<TScalar, TOtherScalar>(value.Z, out var z) ||
            !ShamelessExploit.TryConvertTruncating<TScalar, TOtherScalar>(value.W, out var w) ||
            !ShamelessExploit.TryConvertTruncating<TScalar, TOtherScalar>(value.V, out var v)
        )
        {
            result = default;
            return false;
        }
    
        result = new Vector5F<TOtherScalar>(x, y, z, w, v);
        return true;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVector<Vector5F<TScalar>, TScalar>.Create(TScalar scalar) => new(scalar);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVector<Vector5F<TScalar>, TScalar>.Create(ReadOnlySpan<TScalar> values) => new(values);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IVector5<Vector5F<TScalar>, TScalar>.Create(TScalar x, TScalar y, TScalar z, TScalar w, TScalar v) => new(x, y, z, w, v);


    #region Float-specific code

    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> INumberVector<Vector5F<TScalar>, TScalar>.CopySign(Vector5F<TScalar> value, Vector5F<TScalar> sign) => Vector5F.CopySign(value, sign);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> INumberVector<Vector5F<TScalar>, TScalar>.CopySign(Vector5F<TScalar> value, TScalar sign) => Vector5F.CopySign(value, sign);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> INumberVector<Vector5F<TScalar>, TScalar>.Sign(Vector5F<TScalar> value) => Vector5F.Sign(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Normalize(Vector5F<TScalar> value) => Vector5F.Normalize(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Lerp(Vector5F<TScalar> value1, Vector5F<TScalar> value2, Vector5F<TScalar> amount) => Vector5F.Lerp(value1, value2, amount);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.LerpClamped(Vector5F<TScalar> value1, Vector5F<TScalar> value2, Vector5F<TScalar> amount) => Vector5F.LerpClamped(value1, value2, amount);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Reflect(Vector5F<TScalar> vector, Vector5F<TScalar> normal) => Vector5F.Reflect(vector, normal);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Sqrt(Vector5F<TScalar> value) => Vector5F.Sqrt(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Acosh(Vector5F<TScalar> x) => Vector5F.Acosh(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Asinh(Vector5F<TScalar> x) => Vector5F.Asinh(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Atanh(Vector5F<TScalar> x) => Vector5F.Atanh(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Cosh(Vector5F<TScalar> x) => Vector5F.Cosh(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Sinh(Vector5F<TScalar> x) => Vector5F.Sinh(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Tanh(Vector5F<TScalar> x) => Vector5F.Tanh(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Acos(Vector5F<TScalar> x) => Vector5F.Acos(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.AcosPi(Vector5F<TScalar> x) => Vector5F.AcosPi(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Asin(Vector5F<TScalar> x) => Vector5F.Asin(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.AsinPi(Vector5F<TScalar> x) => Vector5F.AsinPi(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Atan(Vector5F<TScalar> x) => Vector5F.Atan(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.AtanPi(Vector5F<TScalar> x) => Vector5F.AtanPi(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Cos(Vector5F<TScalar> x) => Vector5F.Cos(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.CosPi(Vector5F<TScalar> x) => Vector5F.CosPi(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.DegreesToRadians(Vector5F<TScalar> degrees) => Vector5F.DegreesToRadians(degrees);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.RadiansToDegrees(Vector5F<TScalar> radians) => Vector5F.RadiansToDegrees(radians);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Sin(Vector5F<TScalar> x) => Vector5F.Sin(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.SinPi(Vector5F<TScalar> x) => Vector5F.SinPi(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Tan(Vector5F<TScalar> x) => Vector5F.Tan(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.TanPi(Vector5F<TScalar> x) => Vector5F.TanPi(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static (Vector5F<TScalar> Sin, Vector5F<TScalar> Cos) IFloatingPointVector<Vector5F<TScalar>, TScalar>.SinCos(Vector5F<TScalar> x) => Vector5F.SinCos(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static (Vector5F<TScalar> SinPi, Vector5F<TScalar> CosPi) IFloatingPointVector<Vector5F<TScalar>, TScalar>.SinCosPi(Vector5F<TScalar> x) => Vector5F.SinCosPi(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Log(Vector5F<TScalar> x) => Vector5F.Log(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Log(Vector5F<TScalar> x, Vector5F<TScalar> newBase) => Vector5F.Log(x, newBase);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Log(Vector5F<TScalar> x, TScalar newBase) => Vector5F.Log(x, newBase);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.LogP1(Vector5F<TScalar> x) => Vector5F.LogP1(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Log2(Vector5F<TScalar> x) => Vector5F.Log2(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Log2P1(Vector5F<TScalar> x) => Vector5F.Log2P1(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Log10(Vector5F<TScalar> x) => Vector5F.Log10(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Log10P1(Vector5F<TScalar> x) => Vector5F.Log10P1(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Exp(Vector5F<TScalar> x) => Vector5F.Exp(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.ExpM1(Vector5F<TScalar> x) => Vector5F.ExpM1(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Exp2(Vector5F<TScalar> x) => Vector5F.Exp2(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Exp2M1(Vector5F<TScalar> x) => Vector5F.Exp2M1(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Exp10(Vector5F<TScalar> x) => Vector5F.Exp10(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Exp10M1(Vector5F<TScalar> x) => Vector5F.Exp10M1(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Pow(Vector5F<TScalar> x, Vector5F<TScalar> y) => Vector5F.Pow(x, y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Pow(Vector5F<TScalar> x, TScalar y) => Vector5F.Pow(x, y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Cbrt(Vector5F<TScalar> x) => Vector5F.Cbrt(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Hypot(Vector5F<TScalar> x, Vector5F<TScalar> y) => Vector5F.Hypot(x, y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Hypot(Vector5F<TScalar> x, TScalar y) => Vector5F.Hypot(x, y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.RootN(Vector5F<TScalar> x, int n) => Vector5F.RootN(x, n);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Round(Vector5F<TScalar> x) => Vector5F.Round(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Round(Vector5F<TScalar> x, int digits) => Vector5F.Round(x, digits);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Round(Vector5F<TScalar> x, MidpointRounding mode) => Vector5F.Round(x, mode);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Round(Vector5F<TScalar> x, int digits, MidpointRounding mode) => Vector5F.Round(x, digits, mode);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Truncate(Vector5F<TScalar> x) => Vector5F.Truncate(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Atan2(Vector5F<TScalar> x, Vector5F<TScalar> y) => Vector5F.Atan2(x, y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Atan2Pi(Vector5F<TScalar> x, Vector5F<TScalar> y) => Vector5F.Atan2Pi(x, y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Atan2(Vector5F<TScalar> x, TScalar y) => Vector5F.Atan2(x, y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.Atan2Pi(Vector5F<TScalar> x, TScalar y) => Vector5F.Atan2Pi(x, y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.BitDecrement(Vector5F<TScalar> x) => Vector5F.BitDecrement(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.BitIncrement(Vector5F<TScalar> x) => Vector5F.BitIncrement(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.FusedMultiplyAdd(Vector5F<TScalar> left, Vector5F<TScalar> right, Vector5F<TScalar> addend) => Vector5F.FusedMultiplyAdd(left, right, addend);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.ReciprocalEstimate(Vector5F<TScalar> x) => Vector5F.ReciprocalEstimate(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.ReciprocalSqrtEstimate(Vector5F<TScalar> x) => Vector5F.ReciprocalSqrtEstimate(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.ScaleB(Vector5F<TScalar> x, Vector2D<int> n) => Vector5F.ScaleB(x, n);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] static Vector5F<TScalar> IFloatingPointVector<Vector5F<TScalar>, TScalar>.ScaleB(Vector5F<TScalar> x, int n) => Vector5F.ScaleB(x, n);

    #endregion
}