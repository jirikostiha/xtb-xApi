using System.Threading.Tasks;
using xAPI.Records;

namespace xAPI.Streaming
{
    public interface IStreamingListener
    {
        Task ReceiveTradeRecordAsync(StreamingTradeRecord tradeRecord);

        Task ReceiveTickRecordAsync(StreamingTickRecord tickRecord);

        Task ReceiveBalanceRecordAsync(StreamingBalanceRecord balanceRecord);

        Task ReceiveTradeStatusRecordAsync(StreamingTradeStatusRecord tradeStatusRecord);

        Task ReceiveProfitRecordAsync(StreamingProfitRecord profitRecord);

        Task ReceiveNewsRecordAsync(StreamingNewsRecord newsRecord);

        Task ReceiveKeepAliveRecordAsync(StreamingKeepAliveRecord keepAliveRecord);

        Task ReceiveCandleRecordAsync(StreamingCandleRecord candleRecord);
    }
}
