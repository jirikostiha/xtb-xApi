namespace Xtb.XApi;

public interface IBalanceRecord
{
    /// <summary> Account balance value in account currency. </summary>
    double? Balance { get; }

    /// <summary> Account equity value in account currency. </summary>
    double? Equity { get; }

    /// <summary> Account margin requirements in account currency. </summary>
    double? Margin { get; }

    /// <summary> Account free margin value in account currency. </summary>
    double? MarginFree { get; }

    /// <summary> Margin level percentage. </summary>
    double? MarginLevel { get; }

    /// <summary> Credit in account currency. </summary>
    double? Credit { get; }
}