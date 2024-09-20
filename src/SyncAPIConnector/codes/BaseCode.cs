using System.Globalization;

namespace Xtb.XApi.Codes;

/// <summary>
/// Base class for all XApi codes.
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

    /// <inheritdoc/>
    public override string ToString() => Code.ToString(CultureInfo.InvariantCulture);
}