// Ignore Spelling: Unsubscribe

using System;
using System.Collections.Generic;
using System.Threading;
using xAPI.Records;
using xAPI.Errors;
using xAPI.Streaming;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace xAPI.Sync
{
    public class StreamingApiConnector
    {
        private Thread? _streamingReaderThread;

        private readonly IStreamingListener? _streamingListener;

        public StreamingApiConnector(IConnector connector, IStreamingListener? streamingListener = null)
        {
            Connector = connector;
            _streamingListener = streamingListener;
        }

        #region Events
        /// Event raised when a connection is established.
        /// </summary>
        public event EventHandler<ServerEventArgs>? Connected;

        /// <summary>
        /// Event raised when a tick record is received.
        /// </summary>
        public event EventHandler<TickReceivedEventArgs>? TickReceived;

        /// <summary>
        /// Event raised when a trade record is received.
        /// </summary>
        public event EventHandler<TradeReceivedEventArgs>? TradeReceived;

        /// <summary>
        /// Event raised when a balance record is received.
        /// </summary>
        public event EventHandler<BalanceReceivedEventArgs>? BalanceReceived;

        /// <summary>
        /// Event raised when a trade status record is received.
        /// </summary>
        public event EventHandler<TradeStatusReceivedEventArgs>? TradeStatusReceived;

        /// <summary>
        /// Event raised when a profit record is received.
        /// </summary>
        public event EventHandler<ProfitReceivedEventArgs>? ProfitReceived;

        /// <summary>
        /// Event raised when a news record is received.
        /// </summary>
        public event EventHandler<NewsReceivedEventArgs>? NewsReceived;

        /// <summary>
        /// Event raised when a keep alive record is received.
        /// </summary>
        public event EventHandler<KeepAliveReceivedEventArgs>? KeepAliveReceived;

        /// <summary>
        /// Event raised when a candle record is received.
        /// </summary>
        public event EventHandler<CandleReceivedEventArgs>? CandleReceived;

        /// <summary>
        /// Event raised when read streamed message.
        /// </summary>
        public event EventHandler<ExceptionEventArgs>? StreamingErrorOccurred;
        #endregion

        public IConnector Connector { get; private set; }

        public bool IsConnected => Connector.IsConnected;

        public StreamingApiConnector(Server server, string streamSessionId, IStreamingListener? streamingListner = null)
        {
            if (_streamingReaderThread is not null)
            {
                if (!_streamingReaderThread.IsAlive)
                {
                    _streamingReaderThread.Abort();
                    _streamingReaderThread = null;
                }
            }

            if (_streamingReaderThread is null)
            {
                CreateAndRunNewStreamingReaderThread();
            }
        }

        private void CreateAndRunNewStreamingReaderThread()
        {
            _streamingReaderThread = new Thread(async () =>
            {
                await ReadStreamMessage();
            })
            {
                Name = "Streaming reader",
            };

            _streamingReaderThread.Start();
        }

        /// <summary>
        /// Determines if user is logged in.
        /// </summary>
        public bool IsLoggedIn => !string.IsNullOrEmpty(StreamSessionId);
    {
            if (IsConnected)
                return false;
            var connected = Connector.Connect();
        //todo
        Thread t = new(() =>
                while (IsConnected)
        t.Start();
            return true;
        /// Reads stream message.
        /// </summary>
        private async Task ReadStreamMessage()
        {
            try
            {
                var message = await ReadMessageAsync().ConfigureAwait(false);

                if (message != null)
                {
                    var jsonBody = JsonNode.Parse(message, default);
                    if (jsonBody is null)
                    {
                        throw new APICommunicationException("Parsed json body is null.");
                    }

                    var commandName = jsonBody["command"]?.ToString();

                    if (commandName == "tickPrices")
                    {
                        StreamingTickRecord tickRecord = new();
                        tickRecord.FieldsFromJsonObject(jsonBody["data"].AsObject());

                        TickReceived?.Invoke(this, new(tickRecord));
                        _streamingListener?.ReceiveTickRecord(tickRecord);
                        if (sl != null)
                            await sl.ReceiveTickRecordAsync(tickRecord).ConfigureAwait(false);
                    }
                    else if (commandName == "trade")
                    {
                        StreamingTradeRecord tradeRecord = new();
                        tradeRecord.FieldsFromJsonObject(jsonBody["data"].AsObject());

                        TradeReceived?.Invoke(this, new(tradeRecord));
                        if (sl != null)
                            await sl.ReceiveTradeRecordAsync(tradeRecord).ConfigureAwait(false);
                    }
                    else if (commandName == "balance")
                    {
                        StreamingBalanceRecord balanceRecord = new();
                        balanceRecord.FieldsFromJsonObject(jsonBody["data"].AsObject());

                        BalanceReceived?.Invoke(this, new(balanceRecord));
                        BalanceReceived?.Invoke(this, new(balanceRecord));
                        _streamingListener?.ReceiveBalanceRecord(balanceRecord);
                        await sl.ReceiveBalanceRecordAsync(balanceRecord);
                    }
                    else if (commandName == "tradeStatus")
                    {
                        StreamingTradeStatusRecord tradeStatusRecord = new();
                        tradeStatusRecord.FieldsFromJsonObject(jsonBody["data"].AsObject());

                        TradeStatusReceived?.Invoke(this, new(tradeStatusRecord));
                        TradeStatusReceived?.Invoke(this, new(tradeStatusRecord));
                        await sl.ReceiveTradeStatusRecordAsync(tradeStatusRecord);
                        _streamingListener?.ReceiveTradeStatusRecord(tradeStatusRecord);
                    }
                    else if (commandName == "profit")
                    {
                        StreamingProfitRecord profitRecord = new();
                        profitRecord.FieldsFromJsonObject(jsonBody["data"].AsObject());

                        ProfitReceived?.Invoke(this, new(profitRecord));
                        ProfitReceived?.Invoke(this, new(profitRecord));
                        await sl.ReceiveProfitRecordAsync(profitRecord);
                        _streamingListener?.ReceiveProfitRecord(profitRecord);
                    }
                    else if (commandName == "news")
                    {
                        StreamingNewsRecord newsRecord = new();
                        newsRecord.FieldsFromJsonObject(jsonBody["data"].AsObject());

                        NewsReceived?.Invoke(this, new(newsRecord));
                        _streamingListener?.ReceiveNewsRecord(newsRecord);
                        await sl.ReceiveNewsRecordAsync(newsRecord);
                    }
                    else if (commandName == "keepAlive")
                    {
                        StreamingKeepAliveRecord keepAliveRecord = new();
                        keepAliveRecord.FieldsFromJsonObject(jsonBody["data"].AsObject());

                        KeepAliveReceived?.Invoke(this, new(keepAliveRecord));
                        KeepAliveReceived?.Invoke(this, new(keepAliveRecord));
                        await sl.ReceiveKeepAliveRecordAsync(keepAliveRecord);
                        _streamingListener?.ReceiveKeepAliveRecord(keepAliveRecord);
                    }
                    else if (commandName == "candle")
                    {
                        StreamingCandleRecord candleRecord = new();
                        candleRecord.FieldsFromJsonObject(jsonBody["data"].AsObject());

                        CandleReceived?.Invoke(this, new(candleRecord));
                        CandleReceived?.Invoke(this, new(candleRecord));
                        await sl.ReceiveCandleRecordAsync(candleRecord);
                        _streamingListener?.ReceiveCandleRecord(candleRecord);
                    }
                    else
                    {
                        throw new APICommunicationException($"Unknown streaming record received. command:'{commandName}'");
                    }
                }
            }
            catch (Exception ex)
            {
                OnStreamingErrorOccurred(ex);
            }
        }

        public void SubscribePrice(string symbol, long? minArrivalTime = null, long? maxLevel = null)
        {
            TickPricesSubscribe tickPricesSubscribe = new(symbol, StreamSessionId, minArrivalTime, maxLevel);
            Connector.WriteMessage(tickPricesSubscribe.ToString());
        }

        public void UnsubscribePrice(string symbol)
        {
            TickPricesStop tickPricesStop = new(symbol);
            Connector.WriteMessage(tickPricesStop.ToString());
        }

        public void SubscribePrices(IEnumerable<string> symbols)
        {
            foreach (string symbol in symbols)
            {
                SubscribePrice(symbol);
            }
        }

        public void UnsubscribePrices(LinkedList<string> symbols)
        {
            foreach (string symbol in symbols)
            {
                UnsubscribePrice(symbol);
            }
        }

        public void SubscribeTrades()
        {
            //check if loggedin

            TradeRecordsSubscribe tradeRecordsSubscribe = new(StreamSessionId);
            Connector.WriteMessage(tradeRecordsSubscribe.ToString());
        }

        public void UnsubscribeTrades()
        {
            TradeRecordsStop tradeRecordsStop = new();
            Connector.WriteMessage(tradeRecordsStop.ToString());
        }

        public void SubscribeBalance()
        {
            BalanceRecordsSubscribe balanceRecordsSubscribe = new(StreamSessionId);
            Connector.WriteMessage(balanceRecordsSubscribe.ToString());
        }

        public void UnsubscribeBalance()
        {
            BalanceRecordsStop balanceRecordsStop = new();
            Connector.WriteMessage(balanceRecordsStop.ToString());
        }
        public void SubscribeTradeStatus()
        {
            TradeStatusRecordsSubscribe tradeStatusRecordsSubscribe = new(StreamSessionId);
            Connector.WriteMessage(tradeStatusRecordsSubscribe.ToString());
        }

        public void UnsubscribeTradeStatus()
        {
            TradeStatusRecordsStop tradeStatusRecordsStop = new();
            Connector.WriteMessage(tradeStatusRecordsStop.ToString());
        }

        public void SubscribeProfits()
        {
            ProfitsSubscribe profitsSubscribe = new(StreamSessionId);
            Connector.WriteMessage(profitsSubscribe.ToString());
        }

        public void UnsubscribeProfits()
        {
            ProfitsStop profitsStop = new();
            Connector.WriteMessage(profitsStop.ToString());
        }

        public void SubscribeNews()
        {
            NewsSubscribe newsSubscribe = new(StreamSessionId);
            Connector.WriteMessage(newsSubscribe.ToString());
        }

        public void UnsubscribeNews()
        {
            NewsStop newsStop = new();
            Connector.WriteMessage(newsStop.ToString());
        }

        public void SubscribeKeepAlive()
        {
            KeepAliveSubscribe keepAliveSubscribe = new(StreamSessionId);
            Connector.WriteMessage(keepAliveSubscribe.ToString());
        }

        public void UnsubscribeKeepAlive()
        {
            KeepAliveStop keepAliveStop = new();
            Connector.WriteMessage(keepAliveStop.ToString());
        }

        public void SubscribeCandles(string symbol)
        {
            CandleRecordsSubscribe candleRecordsSubscribe = new(symbol, StreamSessionId);
            Connector.WriteMessage(candleRecordsSubscribe.ToString());
        }

        public void UnsubscribeCandles(string symbol)
        {
            CandleRecordsStop candleRecordsStop = new(symbol);
            Connector.WriteMessage(candleRecordsStop.ToString());
        }

        protected virtual void OnStreamingErrorOccurred(Exception ex)
        {
            var args = new ExceptionEventArgs(ex);
            StreamingErrorOccurred?.Invoke(this, args);

            if (!args.Handled)
            {
                // If the exception was not handled, rethrow it
                throw new APICommunicationException("Read streaming message failed.", ex);
            }
        }
    }
}