namespace Xtb.XApi;

public interface IBalanceRecord
{
    double? Balance { get; }

    double? Equity { get; }

    double? Margin { get; }

    double? MarginFree { get; }

    double? MarginLevel { get; }

    double? Credit { get; }
}