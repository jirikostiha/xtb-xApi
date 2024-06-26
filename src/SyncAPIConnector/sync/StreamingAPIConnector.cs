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

            if (server.IsSecure)
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
                    await ReadStreamMessageAsync(CancellationToken.None);
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
        private async Task ReadStreamMessageAsync(CancellationToken cancellationToken = default)
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
                            await sl.ReceiveTickRecordAsync(tickRecord, cancellationToken).ConfigureAwait(false);
                    }
                    else if (commandName == "trade")
                    {
                        StreamingTradeRecord tradeRecord = new StreamingTradeRecord();
                        tradeRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        TradeReceived?.Invoke(this, new(tradeRecord));
                        if (sl != null)
                            await sl.ReceiveTradeRecordAsync(tradeRecord, cancellationToken).ConfigureAwait(false);
                    }
                    else if (commandName == "balance")
                    {
                        StreamingBalanceRecord balanceRecord = new StreamingBalanceRecord();
                        balanceRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        BalanceReceived?.Invoke(this, new(balanceRecord));
                        if (sl != null)
                            await sl.ReceiveBalanceRecordAsync(balanceRecord, cancellationToken).ConfigureAwait(false);
                    }
                    else if (commandName == "tradeStatus")
                    {
                        StreamingTradeStatusRecord tradeStatusRecord = new StreamingTradeStatusRecord();
                        tradeStatusRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        TradeStatusReceived?.Invoke(this, new(tradeStatusRecord));
                        if (sl != null)
                            await sl.ReceiveTradeStatusRecordAsync(tradeStatusRecord, cancellationToken).ConfigureAwait(false);
                    }
                    else if (commandName == "profit")
                    {
                        StreamingProfitRecord profitRecord = new StreamingProfitRecord();
                        profitRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        ProfitReceived?.Invoke(this, new(profitRecord));
                        if (sl != null)
                            await sl.ReceiveProfitRecordAsync(profitRecord, cancellationToken).ConfigureAwait(false);
                    }
                    else if (commandName == "news")
                    {
                        StreamingNewsRecord newsRecord = new StreamingNewsRecord();
                        newsRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        NewsReceived?.Invoke(this, new(newsRecord));
                        if (sl != null)
                            await sl.ReceiveNewsRecordAsync(newsRecord, cancellationToken).ConfigureAwait(false);
                    }
                    else if (commandName == "keepAlive")
                    {
                        StreamingKeepAliveRecord keepAliveRecord = new StreamingKeepAliveRecord();
                        keepAliveRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        KeepAliveReceived?.Invoke(this, new(keepAliveRecord));
                        if (sl != null)
                            await sl.ReceiveKeepAliveRecordAsync(keepAliveRecord, cancellationToken).ConfigureAwait(false);
                    }
                    else if (commandName == "candle")
                    {
                        StreamingCandleRecord candleRecord = new StreamingCandleRecord();
                        candleRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        CandleReceived?.Invoke(this, new(candleRecord));
                        if (sl != null)
                            await sl.ReceiveCandleRecordAsync(candleRecord, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        throw new APICommunicationException($"Unknown streaming record received. command:'{commandName}'");
                    }
                }
            }
            catch (APICommunicationException ex) when (ex.InnerException.InnerException is SocketException se)
            {
                if (se.ErrorCode != (int)SocketError.OperationAborted)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                OnStreamingErrorOccurred(ex);
            }
        }

        #region subscribe, unsubscribe
        public void SubscribePrice(string symbol, long? minArrivalTime = null, long? maxLevel = null)
        {
            TickPricesSubscribe tickPricesSubscribe = new(symbol, streamSessionId, minArrivalTime, maxLevel);
            WriteMessage(tickPricesSubscribe.ToString());
        }

        public void UnsubscribePrice(string symbol)
        {
            TickPricesStop tickPricesStop = new(symbol);
            WriteMessage(tickPricesStop.ToString());
        }

        public void SubscribePrices(string[] symbols)
        {
            foreach (string symbol in symbols)
            {
                SubscribePrice(symbol);
            }
        }

        public void UnsubscribePrices(string[] symbols)
        {
            foreach (string symbol in symbols)
            {
                UnsubscribePrice(symbol);
            }
        }

        public void SubscribeTrades()
        {
            TradeRecordsSubscribe tradeRecordsSubscribe = new(streamSessionId);
            WriteMessage(tradeRecordsSubscribe.ToString());
        }

        public void UnsubscribeTrades()
        {
            TradeRecordsStop tradeRecordsStop = new();
            WriteMessage(tradeRecordsStop.ToString());
        }

        public void SubscribeBalance()
        {
            BalanceRecordsSubscribe balanceRecordsSubscribe = new(streamSessionId);
            WriteMessage(balanceRecordsSubscribe.ToString());
        }

        public void UnsubscribeBalance()
        {
            BalanceRecordsStop balanceRecordsStop = new();
            WriteMessage(balanceRecordsStop.ToString());
        }

        public void SubscribeTradeStatus()
        {
            TradeStatusRecordsSubscribe tradeStatusRecordsSubscribe = new(streamSessionId);
            WriteMessage(tradeStatusRecordsSubscribe.ToString());
        }

        public void UnsubscribeTradeStatus()
        {
            TradeStatusRecordsStop tradeStatusRecordsStop = new();
            WriteMessage(tradeStatusRecordsStop.ToString());
        }

        public void SubscribeProfits()
        {
            ProfitsSubscribe profitsSubscribe = new(streamSessionId);
            WriteMessage(profitsSubscribe.ToString());
        }

        public void UnsubscribeProfits()
        {
            ProfitsStop profitsStop = new();
            WriteMessage(profitsStop.ToString());
        }

        public void SubscribeNews()
        {
            NewsSubscribe newsSubscribe = new(streamSessionId);
            WriteMessage(newsSubscribe.ToString());
        }

        public void UnsubscribeNews()
        {
            NewsStop newsStop = new();
            WriteMessage(newsStop.ToString());
        }

        public void SubscribeKeepAlive()
        {
            KeepAliveSubscribe keepAliveSubscribe = new(streamSessionId);

            WriteMessage(keepAliveSubscribe.ToString());
        }

        public void UnsubscribeKeepAlive()
        {
            KeepAliveStop keepAliveStop = new();
            WriteMessage(keepAliveStop.ToString());
        }

        public void SubscribeCandles(string symbol)
        {
            CandleRecordsSubscribe candleRecordsSubscribe = new(symbol, streamSessionId);
            WriteMessage(candleRecordsSubscribe.ToString());
        }

        public void UnsubscribeCandles(string symbol)
        {
            CandleRecordsStop candleRecordsStop = new(symbol);
            WriteMessage(candleRecordsStop.ToString());
        }

        public async Task SubscribePriceAsync(string symbol, long? minArrivalTime = null, long? maxLevel = null, CancellationToken cancellationToken = default)
        {
            var tickPricesSubscribe = new TickPricesSubscribe(symbol, streamSessionId, minArrivalTime, maxLevel);
            await WriteMessageAsync(tickPricesSubscribe.ToString(), cancellationToken);
        }

        public async Task UnsubscribePriceAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var tickPricesStop = new TickPricesStop(symbol);
            await WriteMessageAsync(tickPricesStop.ToString(), cancellationToken);
        }

        public async Task SubscribePricesAsync(string[] symbols, CancellationToken cancellationToken = default)
        {
            foreach (string symbol in symbols)
            {
                await SubscribePriceAsync(symbol, cancellationToken: cancellationToken);
            }
        }

        public async Task UnsubscribePricesAsync(string[] symbols, CancellationToken cancellationToken = default)
        {
            foreach (string symbol in symbols)
            {
                await UnsubscribePriceAsync(symbol, cancellationToken);
            }
        }

        public async Task SubscribeTradesAsync(CancellationToken cancellationToken = default)
        {
            var tradeRecordsSubscribe = new TradeRecordsSubscribe(streamSessionId);
            await WriteMessageAsync(tradeRecordsSubscribe.ToString(), cancellationToken);
        }

        public async Task UnsubscribeTradesAsync(CancellationToken cancellationToken = default)
        {
            var tradeRecordsStop = new TradeRecordsStop();
            await WriteMessageAsync(tradeRecordsStop.ToString(), cancellationToken);
        }

        public async Task SubscribeBalanceAsync(CancellationToken cancellationToken = default)
        {
            var balanceRecordsSubscribe = new BalanceRecordsSubscribe(streamSessionId);
            await WriteMessageAsync(balanceRecordsSubscribe.ToString(), cancellationToken);
        }

        public async Task UnsubscribeBalanceAsync(CancellationToken cancellationToken = default)
        {
            var balanceRecordsStop = new BalanceRecordsStop();
            await WriteMessageAsync(balanceRecordsStop.ToString(), cancellationToken);
        }

        public async Task SubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
        {
            var tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(streamSessionId);
            await WriteMessageAsync(tradeStatusRecordsSubscribe.ToString(), cancellationToken);
        }

        public async Task UnsubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
        {
            var tradeStatusRecordsStop = new TradeStatusRecordsStop();
            await WriteMessageAsync(tradeStatusRecordsStop.ToString(), cancellationToken);
        }

        public async Task SubscribeProfitsAsync(CancellationToken cancellationToken = default)
        {
            var profitsSubscribe = new ProfitsSubscribe(streamSessionId);
            await WriteMessageAsync(profitsSubscribe.ToString(), cancellationToken);
        }

        public async Task UnsubscribeProfitsAsync(CancellationToken cancellationToken = default)
        {
            var profitsStop = new ProfitsStop();
            await WriteMessageAsync(profitsStop.ToString(), cancellationToken);
        }

        public async Task SubscribeNewsAsync(CancellationToken cancellationToken = default)
        {
            var newsSubscribe = new NewsSubscribe(streamSessionId);
            await WriteMessageAsync(newsSubscribe.ToString(), cancellationToken);
        }

        public async Task UnsubscribeNewsAsync(CancellationToken cancellationToken = default)
        {
            var newsStop = new NewsStop();
            await WriteMessageAsync(newsStop.ToString(), cancellationToken);
        }

        public async Task SubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
        {
            var keepAliveSubscribe = new KeepAliveSubscribe(streamSessionId);
            await WriteMessageAsync(keepAliveSubscribe.ToString(), cancellationToken);
        }

        public async Task UnsubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
        {
            var keepAliveStop = new KeepAliveStop();
            await WriteMessageAsync(keepAliveStop.ToString(), cancellationToken);
        }

        public async Task SubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, streamSessionId);
            await WriteMessageAsync(candleRecordsSubscribe.ToString(), cancellationToken);
        }

        public async Task UnsubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var candleRecordsStop = new CandleRecordsStop(symbol);
            await WriteMessageAsync(candleRecordsStop.ToString(), cancellationToken);
        }
        #endregion

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
                streamSessionId = null!;

                _disposed = true;
            }
        }

        ~StreamingAPIConnector()
        {
            Dispose(false);
        }
    }
}