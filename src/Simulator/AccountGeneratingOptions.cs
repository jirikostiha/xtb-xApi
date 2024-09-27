namespace Xtb.XApi.Simulation;

public record AccountGeneratingOptions
{
    public string[] Currencies { get; set; } = [ Currency.Usd, Currency.Eur ];

    public double BalanceMin { get; set; } = 100_000;

    public double BalanceMax { get; set; } = 100_000;
}
