using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Client.Model;

namespace Xtb.XApi.Client;

public interface IStreamingListener
{
    Task ReceiveTradeRecordAsync(StreamingTradeRecord tradeRecord, CancellationToken cancellationToken = default);

    Task ReceiveTickRecordAsync(StreamingTickRecord tickRecord, CancellationToken cancellationToken = default);

    Task ReceiveBalanceRecordAsync(StreamingBalanceRecord balanceRecord, CancellationToken cancellationToken = default);

    Task ReceiveTradeStatusRecordAsync(StreamingTradeStatusRecord tradeStatusRecord, CancellationToken cancellationToken = default);

    Task ReceiveProfitRecordAsync(StreamingProfitRecord profitRecord, CancellationToken cancellationToken = default);

    Task ReceiveNewsRecordAsync(StreamingNewsRecord newsRecord, CancellationToken cancellationToken = default);

    Task ReceiveKeepAliveRecordAsync(StreamingKeepAliveRecord keepAliveRecord, CancellationToken cancellationToken = default);

    Task ReceiveCandleRecordAsync(StreamingCandleRecord candleRecord, CancellationToken cancellationToken = default);
}