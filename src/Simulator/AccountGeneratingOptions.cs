using Xtb.XApiClient.Content;

namespace Xtb.XApi.Simulation;

public record AccountGeneratingOptions
{
    public string[] Currencies { get; set; } = [ Currency.Usd, Currency.Eur ];

    public double BalanceMin { get; set; } = 10_000;
    public double BalanceMax { get; set; } = 100_000;

    public double EquityMin { get; set; } = 5_000;
    public double EquityMax { get; set; } = 50_000;

    public double ProfitMin { get; set; } = -5_000;
    public double ProfitMax { get; set; } = 5_000;
}
