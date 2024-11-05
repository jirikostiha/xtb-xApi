namespace Xtb.XApi;

public interface IBalance
{
    double? Balance { get; }

    double? Equity { get; }

    double? Margin { get; }

    double? MarginFree { get; }

    double? MarginLevel { get; }

    double? Credit { get; }
}