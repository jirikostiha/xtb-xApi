using System.Collections.Generic;

namespace Xtb.XApiClient.Content;

/// <summary>
/// Country group values.
/// </summary>
public static class Country
{
    /// <summary>United States</summary>
    public const string US = "US";

    /// <summary>Sweden</summary>
    public const string Sweden = "Sweden";

    /// <summary>United Kingdom</summary>
    public const string UK = "UK";

    /// <summary>Spain</summary>
    public const string Spain = "Spain";

    /// <summary>Germany</summary>
    public const string Germany = "Germany";

    /// <summary>Poland</summary>
    public const string Poland = "Poland";

    /// <summary>France</summary>
    public const string France = "France";

    /// <summary>Belgium</summary>
    public const string Belgium = "Belgium";

    /// <summary>Finland</summary>
    public const string Finland = "Finland";

    /// <summary>Italy</summary>
    public const string Italy = "Italy";

    /// <summary>Denmark</summary>
    public const string Denmark = "Denmark";

    /// <summary>Netherlands</summary>
    public const string Netherlands = "Netherlands";

    /// <summary>Portugal</summary>
    public const string Portugal = "Portugal";

    /// <summary>Czech Republic</summary>
    public const string CzechRep = "Czech Rep.";

    /// <summary>Switzerland</summary>
    public const string Switzerland = "Switzerland";

    /// <summary>Norway</summary>
    public const string Norway = "Norway";

    /// <summary>
    /// Enumerates all country group values.
    /// </summary>
    /// <returns>An enumerable of all country group strings.</returns>
    public static IEnumerable<string> Enumerate()
    {
        yield return US;
        yield return Sweden;
        yield return UK;
        yield return Spain;
        yield return Germany;
        yield return Poland;
        yield return France;
        yield return Belgium;
        yield return Finland;
        yield return Italy;
        yield return Denmark;
        yield return Netherlands;
        yield return Portugal;
        yield return CzechRep;
        yield return Switzerland;
        yield return Norway;
    }
}