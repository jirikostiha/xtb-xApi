using System;
using System.Windows.Input;
using xAPI.Commands;
using xAPI.Records;

namespace xAPI.Sync
{
    public class ExceptionEventArgs(Exception exception) : EventArgs
    {
        public Exception Exception { get; } = exception;

        public bool Handled { get; set; }
    }

    public class MessageEventArgs(string message) : EventArgs
    {
        public string Message { get; } = message;
    }

    public class CommandEventArgs(BaseCommand command) : EventArgs
    {
        public BaseCommand Command { get; } = command;
    }

    public class ServerEventArgs(Server server) : EventArgs
    {
        public Server Server { get; } = server;
    }

    public class TickReceivedEventArgs(StreamingTickRecord tickRecord) : EventArgs
    {
        public StreamingTickRecord TickRecord { get; } = tickRecord;
    }

    public class TradeReceivedEventArgs(StreamingTradeRecord tradeRecord) : EventArgs
    {
        public StreamingTradeRecord TradeRecord { get; } = tradeRecord;
    }

    public class BalanceReceivedEventArgs(StreamingBalanceRecord balanceRecord) : EventArgs
    {
        public StreamingBalanceRecord BalanceRecord { get; } = balanceRecord;
    }

    public class TradeStatusReceivedEventArgs(StreamingTradeStatusRecord tradeStatusRecord) : EventArgs
    {
        public StreamingTradeStatusRecord TradeStatusRecord { get; } = tradeStatusRecord;
    }

    public class ProfitReceivedEventArgs(StreamingProfitRecord profitRecord) : EventArgs
    {
        public StreamingProfitRecord ProfitRecord { get; } = profitRecord;
    }

    public class NewsReceivedEventArgs(StreamingNewsRecord newsRecord) : EventArgs
    {
        public StreamingNewsRecord NewsRecord { get; } = newsRecord;
    }

    public class KeepAliveReceivedEventArgs(StreamingKeepAliveRecord keepAliveRecord) : EventArgs
    {
        public StreamingKeepAliveRecord KeepAliveRecord { get; } = keepAliveRecord;
    }

    public class CandleReceivedEventArgs(StreamingCandleRecord candleRecord) : EventArgs
    {
        public StreamingCandleRecord CandleRecord { get; } = candleRecord;
    }
}