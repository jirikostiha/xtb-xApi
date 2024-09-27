namespace Xtb.XApi.Simulation;

public record CalendarGeneratingOptions
{
    public string[] Countries { get; set; } = [ Country.US, Country.UK, Country.Germany, Country.France ];

}