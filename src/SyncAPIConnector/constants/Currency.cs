using System.Collections.Generic;

namespace xAPI;

/// <summary>
/// Currency group values.
/// </summary>
public static class Currency
{
    /// <summary>United States Dollar</summary>
    public const string Usd = "USD";

    /// <summary>Euro</summary>
    public const string Eur = "EUR";

    /// <summary>Swedish Krona</summary>
    public const string Sek = "SEK";

    /// <summary>British Pound Sterling</summary>
    public const string Gbp = "GBP";

    /// <summary>Polish Zloty</summary>
    public const string Pln = "PLN";

    /// <summary>Danish Krone</summary>
    public const string Dkk = "DKK";

    /// <summary>Czech Koruna</summary>
    public const string Czk = "CZK";

    /// <summary>Swiss Franc</summary>
    public const string Chf = "CHF";

    /// <summary>Norwegian Krone</summary>
    public const string Nok = "NOK";

    /// <summary>South African Rand</summary>
    public const string Zar = "ZAR";

    /// <summary>Australian Dollar</summary>
    public const string Aud = "AUD";

    /// <summary>New Zealand Dollar</summary>
    public const string Nzd = "NZD";

    /// <summary>Japanese Yen</summary>
    public const string Jpy = "JPY";

    /// <summary>Canadian Dollar</summary>
    public const string Cad = "CAD";

    /// <summary>
    /// Enumerates all currency group values.
    /// </summary>
    /// <returns>An enumerable of all currency group strings.</returns>
    public static IEnumerable<string> Enumerate()
    {
        yield return Usd;
        yield return Eur;
        yield return Sek;
        yield return Gbp;
        yield return Pln;
        yield return Dkk;
        yield return Czk;
        yield return Chf;
        yield return Nok;
        yield return Zar;
        yield return Aud;
        yield return Nzd;
        yield return Jpy;
        yield return Cad;
    }
}