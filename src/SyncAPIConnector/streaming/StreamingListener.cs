using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Records;

namespace xAPI.Streaming
{
    public interface StreamingListener
    {
        void ReceiveTradeRecord(StreamingTradeRecord tradeRecord);
        void ReceiveTickRecord(StreamingTickRecord tickRecord);
        void ReceiveBalanceRecord(StreamingBalanceRecord balanceRecord);
        void ReceiveTradeStatusRecord(StreamingTradeStatusRecord tradeStatusRecord);
        void ReceiveProfitRecord(StreamingProfitRecord profitRecord);
        void ReceiveNewsRecord(StreamingNewsRecord newsRecord);
        void ReceiveKeepAliveRecord(StreamingKeepAliveRecord keepAliveRecord);
        void ReceiveCandleRecord(StreamingCandleRecord candleRecord);
    }
}
