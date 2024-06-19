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

namespace xAPI.Sync
{
    public class StreamingAPIConnector : Connector
    {
        #region Events

        /// <summary>
        /// Delegate called on connection establish.
        /// </summary>
        /// <param name="server">Server that the connection was made to</param>
        public delegate void OnConnectedCallback(Server server);

        /// <summary>
        /// Event raised when connection is established.
        /// </summary>
        public event OnConnectedCallback OnConnected;

        //public delegate void OnReceiveRe

        /// <summary>
        /// Delegate called on tick record arrival.
        /// </summary>
        /// <param name="tickRecord">Received tick record</param>
        public delegate void OnTick(StreamingTickRecord tickRecord);

        /// <summary>
        /// Event raised when tick is received.
        /// </summary>
        public event OnTick TickRecordReceived;

        /// <summary>
        /// Delegate called on trade record arrival.
        /// </summary>
        /// <param name="tradeRecord">Received trade record</param>
        public delegate void OnTrade(StreamingTradeRecord tradeRecord);

        /// <summary>
        /// Event raised when trade record is received.
        /// </summary>
        public event OnTrade TradeRecordReceived;

        /// <summary>
        /// Delegate called on balance record arrival.
        /// </summary>
        /// <param name="balanceRecord">Received balance record</param>
        public delegate void OnBalance(StreamingBalanceRecord balanceRecord);

        /// <summary>
        /// Event raised when balance record is received.
        /// </summary>
        public event OnBalance BalanceRecordReceived;

        /// <summary>
        /// Delegate called on trade status record arrival.
        /// </summary>
        /// <param name="tradeStatusRecord">Received trade status record</param>
        public delegate void OnTradeStatus(StreamingTradeStatusRecord tradeStatusRecord);

        /// <summary>
        /// Event raised when trade status record is received.
        /// </summary>
        public event OnTradeStatus TradeStatusRecordReceived;

        /// <summary>
        /// Delegate called on profit record arrival.
        /// </summary>
        /// <param name="profitRecord">Received profit record</param>
        public delegate void OnProfit(StreamingProfitRecord profitRecord);

        /// <summary>
        /// Event raised when profit record is received.
        /// </summary>
        public event OnProfit ProfitRecordReceived;

        /// <summary>
        /// Delegate called on news record arrival.
        /// </summary>
        /// <param name="newsRecord">Received news record</param>
        public delegate void OnNews(StreamingNewsRecord newsRecord);

        /// <summary>
        /// Event raised when news record is received.
        /// </summary>
        public event OnNews NewsRecordReceived;

        /// <summary>
        /// Delegate called on keep alive record arrival.
        /// </summary>
        /// <param name="keepAliveRecord">Received keep alive record</param>
        public delegate void OnKeepAlive(StreamingKeepAliveRecord keepAliveRecord);

        /// <summary>
        /// Event raised when keep alive record is received.
        /// </summary>
        public event OnKeepAlive KeepAliveRecordReceived;

        /// <summary>
        /// Delegate called on candle record arrival.
        /// </summary>
        /// <param name="candleRecord">Received candle record</param>
        public delegate void OnCandle(StreamingCandleRecord candleRecord);

        /// <summary>
        /// Event raised when candle record is received.
        /// </summary>
        public event OnCandle CandleRecordReceived;

        /// <summary>
        /// Event raised when read streamed message.
        /// </summary>
        public event EventHandler<ExceptionEventArgs> StreamingErrorOccurred;

        #endregion

        /// <summary>
        /// Dedicated streaming listener.
        /// </summary>
        private IStreamingListener sl;

        /// <summary>
        /// Stream session id (given on login).
        /// </summary>
        private string streamSessionId;

        /// <summary>
        /// True if streaming is running
        /// </summary>
        [Obsolete("Used only in older method")]
        private bool running = false;

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
        public StreamingAPIConnector(Server server, string streamSessionId, IStreamingListener streamingListner)
        {
            this.server = server;
            this.streamSessionId = streamSessionId;
            Connect(streamingListner, streamSessionId);
        }

        /// <summary>
        /// Connect to the streaming using given streaming listener.
        /// </summary>
        /// <param name="streamingListener">Streaming listener</param>
        public void Connect(IStreamingListener streamingListener)
        {
            Connect(streamingListener, streamSessionId);
        }

        /// <summary>
        /// Connect to the streaming.
        /// </summary>
        public void Connect()
        {
            Connect(null, streamSessionId);
        }

        /// <summary>
        /// Connect to the streaming using given streaming listener.
        /// </summary>
        /// <param name="streamingListener">Streaming listener</param>
        /// <param name="streamSessionId">Stream session id</param>
        public void Connect(IStreamingListener streamingListener, string streamSessionId)
        {
            this.streamSessionId = streamSessionId;

            if (this.streamSessionId == null)
            {
                throw new APICommunicationException("No session exists. Please login first.");
            }

            if (Connected())
            {
                throw new APICommunicationException("Stream already connected.");
            }

            this.sl = streamingListener;

            this.apiSocket = new TcpClient(server.Address, server.StreamingPort);
            this.apiConnected = true;

            if (OnConnected != null)
                OnConnected.Invoke(this.server);

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

            Thread t = new Thread(delegate ()
            {
                while (Connected())
                {
                    ReadStreamMessage();
                }
            });

            t.Start();
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
        /// Creates new StreamingAPIConnector object.
        /// </summary>
        /// <param name="sl">Streaming listener</param>
        /// <param name="ip">IP address</param>
        /// <param name="port">Streaming port</param>
        /// <param name="lr">Login response</param>
        /// <param name="secure">Secure</param>
        [Obsolete("Use StreamingAPIConnector(Server server) instead")]
        private StreamingAPIConnector(IStreamingListener sl, string ip, int port, LoginResponse lr, bool secure)
        {
            this.running = true;
            this.sl = sl;
            this.streamSessionId = lr.StreamSessionId;
            apiSocket = new System.Net.Sockets.TcpClient(ip, port);

            if (secure)
            {
                SslStream ssl = new SslStream(apiSocket.GetStream());
                ssl.AuthenticateAsClient(ip);
                apiWriteStream = new StreamWriter(ssl);
                apiReadStream = new StreamReader(ssl);
            }
            else
            {
                NetworkStream ns = apiSocket.GetStream();
                apiWriteStream = new StreamWriter(ns);
                apiReadStream = new StreamReader(ns);
            }

            Thread t = new Thread(delegate ()
            {
                while (running)
                {
                    ReadStreamMessage();
                    Thread.Sleep(50);
                }
            });
            t.Start();
        }

        [Obsolete("Use StreamingAPIConnector(Server server) instead")]
        private StreamingAPIConnector(IStreamingListener sl, string ip, int port, LoginResponse lr)
            : this(sl, ip, port, lr, false)
        {
        }

        [Obsolete("Use StreamingAPIConnector(Server server) instead")]
        public StreamingAPIConnector(IStreamingListener sl, Server dt, LoginResponse lr)
            : this(sl, dt.Address, dt.StreamingPort, lr, dt.Secure)
        {
        }

        /// <summary>
        /// Reads stream message.
        /// </summary>
        private void ReadStreamMessage()
        {
            try
            {
                String message = ReadMessage();

                if (message != null)
                {
                    JsonNode responseBody = JsonNode.Parse(message);
                    string commandName = responseBody["command"].ToString();

                    if (commandName == "tickPrices")
                    {
                        StreamingTickRecord tickRecord = new StreamingTickRecord();
                        tickRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        if (TickRecordReceived != null)
                            TickRecordReceived.Invoke(tickRecord);
                        if (sl != null)
                            sl.ReceiveTickRecord(tickRecord);
                    }
                    else if (commandName == "trade")
                    {
                        StreamingTradeRecord tradeRecord = new StreamingTradeRecord();
                        tradeRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        if (TradeRecordReceived != null)
                            TradeRecordReceived.Invoke(tradeRecord);
                        if (sl != null)
                            sl.ReceiveTradeRecord(tradeRecord);
                    }
                    else if (commandName == "balance")
                    {
                        StreamingBalanceRecord balanceRecord = new StreamingBalanceRecord();
                        balanceRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        if (BalanceRecordReceived != null)
                            BalanceRecordReceived.Invoke(balanceRecord);
                        if (sl != null)
                            sl.ReceiveBalanceRecord(balanceRecord);
                    }
                    else if (commandName == "tradeStatus")
                    {
                        StreamingTradeStatusRecord tradeStatusRecord = new StreamingTradeStatusRecord();
                        tradeStatusRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        if (TradeStatusRecordReceived != null)
                            TradeStatusRecordReceived.Invoke(tradeStatusRecord);
                        if (sl != null)
                            sl.ReceiveTradeStatusRecord(tradeStatusRecord);
                    }
                    else if (commandName == "profit")
                    {
                        StreamingProfitRecord profitRecord = new StreamingProfitRecord();
                        profitRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        if (ProfitRecordReceived != null)
                            ProfitRecordReceived.Invoke(profitRecord);
                        if (sl != null)
                            sl.ReceiveProfitRecord(profitRecord);
                    }
                    else if (commandName == "news")
                    {
                        StreamingNewsRecord newsRecord = new StreamingNewsRecord();
                        newsRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        if (NewsRecordReceived != null)
                            NewsRecordReceived.Invoke(newsRecord);
                        if (sl != null)
                            sl.ReceiveNewsRecord(newsRecord);
                    }
                    else if (commandName == "keepAlive")
                    {
                        StreamingKeepAliveRecord keepAliveRecord = new StreamingKeepAliveRecord();
                        keepAliveRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        if (KeepAliveRecordReceived != null)
                            KeepAliveRecordReceived.Invoke(keepAliveRecord);
                        if (sl != null)
                            sl.ReceiveKeepAliveRecord(keepAliveRecord);
                    }
                    else if (commandName == "candle")
                    {
                        StreamingCandleRecord candleRecord = new StreamingCandleRecord();
                        candleRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                        if (CandleRecordReceived != null)
                            CandleRecordReceived.Invoke(candleRecord);
                        if (sl != null)
                            sl.ReceiveCandleRecord(candleRecord);
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

        [Obsolete("Use SubscribeTradeStatus instead")]
        public void SubscribeReqStatus()
        {
            SubscribeTradeStatus();
        }

        [Obsolete("Use UnsubscribeTradeStatus instead")]
        public void UnsubscribeReqStatus()
        {
            TradeStatusRecordsStop reqStatusRecordsStop = new TradeStatusRecordsStop();
            WriteMessage(reqStatusRecordsStop.ToString());
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