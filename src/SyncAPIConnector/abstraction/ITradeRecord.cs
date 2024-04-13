namespace xAPI.Records
{
    public interface ITradeRecord
    {
        double? Close_price { get; }

        long? Close_time { get; }

        bool? Closed { get; }

        long? Cmd { get; }

        string Comment { get; }

        double? Commission { get; }

        string CustomComment { get; }

        long? Digits { get; }

        long? Expiration { get; }

        double? Margin_rate { get; }

        double? Open_price { get; }

        long? Open_time { get; }

        long? Order { get; }

        long? Order2 { get; }

        long? Position { get; }

        double? Profit { get; }

        double? Sl { get; }

        double? Storage { get; }

        string Symbol { get; }

        double? Tp { get; }

        double? Volume { get; }
    }
}