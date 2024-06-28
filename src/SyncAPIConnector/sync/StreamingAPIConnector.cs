using System.IO;
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Net.Security;

using xAPI.Utils;
using xAPI.Records;
using xAPI.Errors;
using xAPI.Responses;
using xAPI.Streaming;

using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace xAPI.Sync
{
    public class StreamingAPIConnector : Connector
    {
        #region Events

        /// <summary>
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

        private Thread? _streamingReaderThread;

        /// <summary>
        /// Dedicated streaming listener.
        /// </summary>
        private IStreamingListener sl;

        /// <summary>
        /// Stream session id (given on login).
        /// </summary>
        private string streamSessionId;

        /// <summary>
        /// Creates new StreamingAPIConnector instance based on given server data.
        /// </summary>
        /// <param name="server">Server data</param>
        public StreamingAPIConnector(Server server)
        {
            this.server = server;
            this.apiConnected = false;
        }

        /// <summary>
        /// Creates new StreamingAPIConnector instance based on given server data, stream session id and streaming listener.
        /// </summary>
        /// <param name="server">Server data</param>
        public StreamingAPIConnector(Server server, string streamSessionId, IStreamingListener? streamingListner = null)
        {
            this.server = server;
            this.streamSessionId = streamSessionId;
            this.sl = streamingListner;
        }

        /// <summary>
        /// Connect to the streaming.
        /// </summary>
        public void Connect()
        {
            if (this.streamSessionId == null)
            {
                throw new APICommunicationException("No session exists. Please login first.");
            }

            if (IsConnected())
            {
                throw new APICommunicationException("Stream already connected.");
            }

            this.apiSocket = new TcpClient(server.Address, server.StreamingPort);
            this.apiConnected = true;

            Connected?.Invoke(this, new(server));

            if (server.Secure)
            {
                SslStream ssl = new SslStream(apiSocket.GetStream(), false, new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback));
                ssl.AuthenticateAsClient(server.Address);
                apiWriteStream = new StreamWriter(ssl);
                apiReadStream = new StreamReader(ssl);
            }
            else
            {
                NetworkStream ns = apiSocket.GetStream();
                apiWriteStream = new StreamWriter(ns);
                apiReadStream = new StreamReader(ns);
            }

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
                while (IsConnected())
                {
                    await ReadStreamMessage();
                }
            })
            {
                Name = "Streaming reader",
            };

            _streamingReaderThread.Start();
        }

        /// <summary>
        /// Stream session id (member of login response). Should be set after the successful login.
        /// </summary>
        public string StreamSessionId
        {
            get { return this.streamSessionId; }
            set { this.streamSessionId = value; }
        }

        /// <summary>
        /// Reads stream message.
        /// </summary>
        private async Task ReadStreamMessage()
        {
            try
            {
                var message = await ReadMessageAsync().ConfigureAwait(false);

                if (message != null)
                {
                    JsonNode responseBody = JsonNode.Parse(message);
                    string commandName = responseBody["command"].ToString();

                    if (commandName == "tickPrices")
                    {
                        StreamingTickRecord tickRecord = new StreamingTickRecord();
                        tickRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        TickReceived?.Invoke(this, new(tickRecord));
                        if (sl != null)
                            await sl.ReceiveTickRecordAsync(tickRecord).ConfigureAwait(false);
                    }
                    else if (commandName == "trade")
                    {
                        StreamingTradeRecord tradeRecord = new StreamingTradeRecord();
                        tradeRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        TradeReceived?.Invoke(this, new(tradeRecord));
                        if (sl != null)
                            await sl.ReceiveTradeRecordAsync(tradeRecord).ConfigureAwait(false);
                    }
                    else if (commandName == "balance")
                    {
                        StreamingBalanceRecord balanceRecord = new StreamingBalanceRecord();
                        balanceRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        BalanceReceived?.Invoke(this, new(balanceRecord));
                        if (sl != null)
                            await sl.ReceiveBalanceRecordAsync(balanceRecord);
                    }
                    else if (commandName == "tradeStatus")
                    {
                        StreamingTradeStatusRecord tradeStatusRecord = new StreamingTradeStatusRecord();
                        tradeStatusRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        TradeStatusReceived?.Invoke(this, new(tradeStatusRecord));
                        if (sl != null)
                            await sl.ReceiveTradeStatusRecordAsync(tradeStatusRecord);
                    }
                    else if (commandName == "profit")
                    {
                        StreamingProfitRecord profitRecord = new StreamingProfitRecord();
                        profitRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        ProfitReceived?.Invoke(this, new(profitRecord));
                        if (sl != null)
                            await sl.ReceiveProfitRecordAsync(profitRecord);
                    }
                    else if (commandName == "news")
                    {
                        StreamingNewsRecord newsRecord = new StreamingNewsRecord();
                        newsRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        NewsReceived?.Invoke(this, new(newsRecord));
                        if (sl != null)
                            await sl.ReceiveNewsRecordAsync(newsRecord);
                    }
                    else if (commandName == "keepAlive")
                    {
                        StreamingKeepAliveRecord keepAliveRecord = new StreamingKeepAliveRecord();
                        keepAliveRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        KeepAliveReceived?.Invoke(this, new(keepAliveRecord));
                        if (sl != null)
                            await sl.ReceiveKeepAliveRecordAsync(keepAliveRecord);
                    }
                    else if (commandName == "candle")
                    {
                        StreamingCandleRecord candleRecord = new StreamingCandleRecord();
                        candleRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        CandleReceived?.Invoke(this, new(candleRecord));
                        if (sl != null)
                            await sl.ReceiveCandleRecordAsync(candleRecord);
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

        public void SubscribePrice(String symbol, long? minArrivalTime = null, long? maxLevel = null)
        {
            TickPricesSubscribe tickPricesSubscribe = new TickPricesSubscribe(symbol, streamSessionId, minArrivalTime, maxLevel);
            WriteMessage(tickPricesSubscribe.ToString());
        }

        public void UnsubscribePrice(String symbol)
        {
            TickPricesStop tickPricesStop = new TickPricesStop(symbol);
            WriteMessage(tickPricesStop.ToString());
        }

        public void SubscribePrices(IEnumerable<String> symbols)
        {
            foreach (String symbol in symbols)
            {
                SubscribePrice(symbol);
            }
        }

        public void UnsubscribePrices(LinkedList<String> symbols)
        {
            foreach (String symbol in symbols)
            {
                UnsubscribePrice(symbol);
            }
        }

        public void SubscribeTrades()
        {
            TradeRecordsSubscribe tradeRecordsSubscribe = new TradeRecordsSubscribe(streamSessionId);
            WriteMessage(tradeRecordsSubscribe.ToString());
        }

        public void UnsubscribeTrades()
        {
            TradeRecordsStop tradeRecordsStop = new TradeRecordsStop();
            WriteMessage(tradeRecordsStop.ToString());
        }

        public void SubscribeBalance()
        {
            BalanceRecordsSubscribe balanceRecordsSubscribe = new BalanceRecordsSubscribe(streamSessionId);
            WriteMessage(balanceRecordsSubscribe.ToString());
        }

        public void UnsubscribeBalance()
        {
            BalanceRecordsStop balanceRecordsStop = new BalanceRecordsStop();
            WriteMessage(balanceRecordsStop.ToString());
        }

        public void SubscribeTradeStatus()
        {
            TradeStatusRecordsSubscribe tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(streamSessionId);
            WriteMessage(tradeStatusRecordsSubscribe.ToString());
        }

        public void UnsubscribeTradeStatus()
        {
            TradeStatusRecordsStop tradeStatusRecordsStop = new TradeStatusRecordsStop();
            WriteMessage(tradeStatusRecordsStop.ToString());
        }

        public void SubscribeProfits()
        {
            ProfitsSubscribe profitsSubscribe = new ProfitsSubscribe(streamSessionId);
            WriteMessage(profitsSubscribe.ToString());
        }

        public void UnsubscribeProfits()
        {
            ProfitsStop profitsStop = new ProfitsStop();
            WriteMessage(profitsStop.ToString());
        }

        public void SubscribeNews()
        {
            NewsSubscribe newsSubscribe = new NewsSubscribe(streamSessionId);
            WriteMessage(newsSubscribe.ToString());
        }

        public void UnsubscribeNews()
        {
            NewsStop newsStop = new NewsStop();
            WriteMessage(newsStop.ToString());
        }

        public void SubscribeKeepAlive()
        {
            KeepAliveSubscribe keepAliveSubscribe = new KeepAliveSubscribe(streamSessionId);
            WriteMessage(keepAliveSubscribe.ToString());
        }

        public void UnsubscribeKeepAlive()
        {
            KeepAliveStop keepAliveStop = new KeepAliveStop();
            WriteMessage(keepAliveStop.ToString());
        }

        public void SubscribeCandles(String symbol)
        {
            CandleRecordsSubscribe candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, streamSessionId);
            WriteMessage(candleRecordsSubscribe.ToString());
        }

        public void UnsubscribeCandles(String symbol)
        {
            CandleRecordsStop candleRecordsStop = new CandleRecordsStop(symbol);
            WriteMessage(candleRecordsStop.ToString());
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

        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                base.Dispose(disposing);

                _disposed = true;
            }
        }

        ~StreamingAPIConnector()
        {
            Dispose(false);
        }
    }
}