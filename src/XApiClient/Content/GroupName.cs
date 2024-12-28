using System.Collections.Generic;

namespace Xtb.XApiClient.Content;

public static class GroupName
{
    // Predefined groups
    /// <summary>Major</summary>
    public const string Major = "Major";

    /// <summary>Minor</summary>
    public const string Minor = "Minor";

    /// <summary>Emergings</summary>
    public const string Emergings = "Emergings";

    // Countries
    /// <summary>United States</summary>
    public const string US = Country.US;

    /// <summary>Sweden</summary>
    public const string Sweden = Country.Sweden;

    /// <summary>United Kingdom</summary>
    public const string UK = Country.UK;

    /// <summary>Spain</summary>
    public const string Spain = Country.Spain;

    /// <summary>Germany</summary>
    public const string Germany = Country.Germany;

    /// <summary>Poland</summary>
    public const string Poland = Country.Poland;

    /// <summary>France</summary>
    public const string France = Country.France;

    /// <summary>Belgium</summary>
    public const string Belgium = Country.Belgium;

    /// <summary>Finland</summary>
    public const string Finland = Country.Finland;

    /// <summary>Italy</summary>
    public const string Italy = Country.Italy;

    /// <summary>Denmark</summary>
    public const string Denmark = Country.Denmark;

    /// <summary>Netherlands</summary>
    public const string Netherlands = Country.Netherlands;

    /// <summary>Portugal</summary>
    public const string Portugal = Country.Portugal;

    /// <summary>Czech Republic</summary>
    public const string CzechRep = Country.CzechRep;

    /// <summary>Switzerland</summary>
    public const string Switzerland = Country.Switzerland;

    /// <summary>Norway</summary>
    public const string Norway = Country.Norway;

    // Regions
    /// <summary>Asia-Pacific</summary>
    public const string AsiaPacific = Region.AsiaPacific;

    /// <summary>Europe</summary>
    public const string Europe = Region.Europe;

    /// <summary>Americas</summary>
    public const string Americas = Region.Americas;

    // Investment types
    /// <summary>Cryptocurrency</summary>
    public const string Crypto = "Crypto";

    /// <summary>Exchange Traded Fund</summary>
    public const string ETF = "ETF";

    /// <summary>Exchange Traded Funds</summary>
    public const string ETFs = "ETFs";

    // Commodities
    /// <summary>Precious Metals</summary>
    public const string PreciousMetals = CommodityGroup.PreciousMetals;

    /// <summary>Agriculture</summary>
    public const string Agriculture = CommodityGroup.Agriculture;

    /// <summary>Energy</summary>
    public const string Energy = CommodityGroup.Energy;

    /// <summary>Industrial Metals</summary>
    public const string IndustrialMetals = CommodityGroup.IndustrialMetals;

    /// <summary>Livestock</summary>
    public const string Livestock = CommodityGroup.Livestock;

    // Other
    /// <summary>Other</summary>
    public const string Other = "Other";

    /// <summary>
    /// Enumerates all group values.
    /// </summary>
    /// <returns>An enumerable of all group strings.</returns>
    public static IEnumerable<string> Enumerate()
    {
        yield return Major;
        yield return Minor;
        yield return Emergings;

        foreach (var country in Country.Enumerate())
            yield return country;

        foreach (var region in Region.Enumerate())
            yield return region;

        yield return Crypto;
        yield return ETF;
        yield return ETFs;

        foreach (var commodityGroup in CommodityGroup.Enumerate())
            yield return commodityGroup;

        yield return Other;
    }
}