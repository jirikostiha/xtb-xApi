using System.Globalization;

namespace XApi.Codes;

/// <summary>
/// Base class for all xAPI codes.
/// </summary>
public class BaseCode
{
    /// <summary>
    /// Creates new base code object.
    /// </summary>
    /// <param name="code">Code represented as long value.</param>
    public BaseCode(int code)
    {
        Code = code;
    }

    /// <summary>
    /// Raw code received from the API.
    /// </summary>
    public int Code { get; set; }

    public static bool operator ==(BaseCode baseCode1, BaseCode baseCode2)
    {
        if (ReferenceEquals(baseCode1, baseCode2))
            return true;

        if ((object)baseCode1 == null || (object)baseCode2 == null)
            return false;

        return baseCode1.Code == baseCode2.Code;
    }

    public static bool operator !=(BaseCode baseCode1, BaseCode baseCode2)
    {
        return !(baseCode1 == baseCode2);
    }

    /// <inheritdoc/>
    public override bool Equals(object target)
    {
        if (target == null)
            return false;

        BaseCode baseCode = target as BaseCode;
        if ((object)baseCode == null)
            return false;

        return Code == baseCode.Code;
    }

    /// <inheritdoc/>
    public override int GetHashCode() => Code.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => Code.ToString(CultureInfo.InvariantCulture);
}