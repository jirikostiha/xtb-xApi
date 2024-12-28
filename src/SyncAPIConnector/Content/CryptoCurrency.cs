using System.Collections.Generic;

namespace Xtb.XApi.Content;

/// <summary>
/// CryptoCurrency group values.
/// </summary>
public static class CryptoCurrency
{
    /// <summary>Bitcoin</summary>
    public const string Btc = "BTC";

    /// <summary>Ethereum</summary>
    public const string Eth = "ETH";

    /// <summary>Bitcoin Cash</summary>
    public const string Bch = "BCH";

    /// <summary>Kusama</summary>
    public const string Ksm = "KSM";

    /// <summary>Metal</summary>
    public const string Mtl = "MTL";

    /// <summary>Kyber Network</summary>
    public const string Knc = "KNC";

    /// <summary>Galxe</summary>
    public const string Gal = "GAL";

    /// <summary>Maker</summary>
    public const string Mkr = "MKR";

    /// <summary>Chainlink</summary>
    public const string Lnk = "LNK";

    /// <summary>Internet Computer</summary>
    public const string Icp = "ICP";

    /// <summary>Fantom</summary>
    public const string Ftm = "FTM";

    /// <summary>The Graph</summary>
    public const string Grt = "GRT";

    /// <summary>Medicalchain</summary>
    public const string Mtc = "MTC";

    /// <summary>TRON</summary>
    public const string Trx = "TRX";

    /// <summary>Alchemy</summary>
    public const string Alr = "ALR";

    /// <summary>Lunyr</summary>
    public const string Lun = "LUN";

    /// <summary>Waves</summary>
    public const string Wav = "WAV";

    /// <summary>Stellar</summary>
    public const string Xlm = "XLM";

    /// <summary>Tezos</summary>
    public const string Xtz = "XTZ";

    /// <summary>Binance Coin</summary>
    public const string Bnb = "BNB";

    /// <summary>STEPN</summary>
    public const string Gmt = "GMT";

    /// <summary>Glidera</summary>
    public const string Glr = "GLR";

    /// <summary>Zcash</summary>
    public const string Zec = "ZEC";

    /// <summary>Uniswap</summary>
    public const string Uni = "UNI";

    /// <summary>Cardano</summary>
    public const string Ada = "ADA";

    /// <summary>Litecoin</summary>
    public const string Ltc = "LTC";

    /// <summary>Dogecoin</summary>
    public const string Dog = "DOG";

    /// <summary>EOS.IO</summary>
    public const string Eos = "EOS";

    /// <summary>Aave</summary>
    public const string Aav = "AAV";

    /// <summary>dYdX</summary>
    public const string Dyd = "DYD";

    /// <summary>Avalanche</summary>
    public const string Avx = "AVX";

    /// <summary>Shiba Inu</summary>
    public const string Shu = "SHU";

    /// <summary>Matrix AI Network</summary>
    public const string Man = "MAN";

    /// <summary>Ripple</summary>
    public const string Xrp = "XRP";

    /// <summary>Filecoin</summary>
    public const string Fil = "FIL";

    /// <summary>Polkadot</summary>
    public const string Dot = "DOT";

    /// <summary>Solana</summary>
    public const string Sol = "SOL";

    /// <summary>Chiliz</summary>
    public const string Chz = "CHZ";

    /// <summary>Cardano ADA Token</summary>
    public const string Ato = "ATO";

    /// <summary>Crypto.com Coin</summary>
    public const string Cro = "CRO";

    /// <summary>ApeCoin</summary>
    public const string Ape = "APE";

    /// <summary>FTX Token</summary>
    public const string Ftt = "FTT";

    /// <summary>Sandbox</summary>
    public const string Sbx = "SBX";

    /// <summary>Curve DAO Token</summary>
    public const string Crv = "CRV";

    /// <summary>VeChain</summary>
    public const string Vet = "VET";

    /// <summary>
    /// Enumerates all cryptocurrency group values.
    /// </summary>
    /// <returns>An enumerable of all cryptocurrency group strings.</returns>
    public static IEnumerable<string> Enumerate()
    {
        yield return Btc;
        yield return Eth;
        yield return Bch;
        yield return Ksm;
        yield return Mtl;
        yield return Knc;
        yield return Gal;
        yield return Mkr;
        yield return Lnk;
        yield return Icp;
        yield return Ftm;
        yield return Grt;
        yield return Mtc;
        yield return Trx;
        yield return Alr;
        yield return Lun;
        yield return Wav;
        yield return Xlm;
        yield return Xtz;
        yield return Bnb;
        yield return Gmt;
        yield return Glr;
        yield return Zec;
        yield return Uni;
        yield return Ada;
        yield return Ltc;
        yield return Dog;
        yield return Eos;
        yield return Aav;
        yield return Dyd;
        yield return Avx;
        yield return Shu;
        yield return Man;
        yield return Xrp;
        yield return Fil;
        yield return Dot;
        yield return Sol;
        yield return Chz;
        yield return Ato;
        yield return Cro;
        yield return Ape;
        yield return Ftt;
        yield return Sbx;
        yield return Crv;
        yield return Vet;
    }
}