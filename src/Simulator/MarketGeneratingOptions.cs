using System;

namespace Xtb.XApi.Simulation;

public record MarketGeneratingOptions
{
    public int[] StartTradingTime { get; set; } = [ new TimeSpan(8,0,0).Milliseconds ];

    public int[] EndTradingTime { get; set; } = [ new TimeSpan(22,0,0).Milliseconds ];

    public double SpreadMin { get; set; } = 0.01;

    public double SpreadMax { get; set; } = 1;
}
