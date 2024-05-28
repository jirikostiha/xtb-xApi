using System.Globalization;

namespace xAPI.Codes
{
    public class PROFIT_MODE : BaseCode
    {
        public const long FOREX_CODE = 5;
        public const long CFD_CODE = 6;

        public static readonly PROFIT_MODE FOREX = new(FOREX_CODE);
        public static readonly PROFIT_MODE CFD = new(CFD_CODE);

        public PROFIT_MODE(long code)
            : base(code)
        {
        }

        /// <summary> Converts to human friendly string. </summary>
        public string? ToFriendlyString() =>
            Code switch
            {
                FOREX_CODE => "forex",
                CFD_CODE => "cfd",
                _ => Code.ToString(CultureInfo.InvariantCulture),
            };
    }
}
