using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApiClient;
using Xtb.XApiClient.Model;

namespace Xtb.XApi.Simulation;

/// <summary>
/// Fake connector responding as server.
/// </summary>
public class FakeConnector : IClient
{
    private readonly JsonMessageGenerator _messageGenerator;

    private readonly Timer _timer;

    public FakeConnector(JsonMessageGenerator? jsonMessageGenerator = null)
    {
        _messageGenerator = jsonMessageGenerator ?? new JsonMessageGenerator();

        _timer = new Timer(async _ =>
        {
            await ComposeFakeStreamingMessage();
        }, null, Timeout.Infinite, 1000);
    }

    public event EventHandler<EndpointEventArgs>? Connected;
    public event EventHandler<MessageEventArgs>? MessageSent;
    public event EventHandler<MessageEventArgs>? MessageReceived;
    public event EventHandler? Disconnected;

    public IPEndPoint Endpoint { get; set; }

    public bool IsConnected { get; set; }

    public bool ReceiveStreamingMessagesPeriodically { get; set; }

    public Dictionary<string, string> AllSymbols { get; private set; } = [];
    public Dictionary<long, object> AllTrades { get; private set; } = [];
    public Dictionary<long, object> TradesHistory { get; private set; } = [];

    public HashSet<string> SubscribedCommands { get; set; } = [];

    public void Connect()
    {
        IsConnected = true;

        if (ReceiveStreamingMessagesPeriodically)
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }
    }

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        Connect();
        return Task.CompletedTask;
    }

    public void SendMessage(string message)
    {
        SendMessageAsync(message).GetAwaiter().GetResult();
    }

    public Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        MessageSent?.Invoke(this, new MessageEventArgs(message));

        var data = JsonNode.Parse(message);
        var command = (string?)data["command"];
        var arguments = data["arguments"];
        var sessionId = data["streamSessionId"];

        if (sessionId is not null)
        {
            switch (command)
            {
                case "get" + StreamingCommandName.KeepAlive:
                    SubscribedCommands.Add(command);
                    break;

                case "stop" + StreamingCommandName.KeepAlive:
                    SubscribedCommands.Remove(StreamingCommandName.KeepAlive);
                    break;

                case "get" + StreamingCommandName.Balance:
                    SubscribedCommands.Add(command);
                    break;

                case "stop" + StreamingCommandName.Balance:
                    SubscribedCommands.Remove(StreamingCommandName.Balance);
                    break;

                case "get" + StreamingCommandName.Profit:
                    SubscribedCommands.Add(command);
                    break;

                case "stop" + StreamingCommandName.Profit:
                    SubscribedCommands.Remove(StreamingCommandName.Profit);
                    break;

                case "get" + StreamingCommandName.TickPrices:
                    SubscribedCommands.Add(command);
                    break;

                case "stop" + StreamingCommandName.TickPrices:
                    SubscribedCommands.Remove(StreamingCommandName.TickPrices);
                    break;

                case "get" + StreamingCommandName.Candle:
                    SubscribedCommands.Add(command);
                    break;

                case "stop" + StreamingCommandName.Candle:
                    SubscribedCommands.Remove(StreamingCommandName.Candle);
                    break;

                case "get" + StreamingCommandName.TradeStatus:
                    SubscribedCommands.Remove(StreamingCommandName.TradeStatus);
                    break;

                case "stop" + StreamingCommandName.TradeStatus:
                    SubscribedCommands.Add(command);
                    break;

                case "get" + StreamingCommandName.Trade:
                    SubscribedCommands.Add(command);
                    break;

                case "stop" + StreamingCommandName.Trade:
                    SubscribedCommands.Remove(StreamingCommandName.Trade);
                    break;

                case "get" + StreamingCommandName.News:
                    SubscribedCommands.Add(command);
                    break;

                case "stop" + StreamingCommandName.News:
                    SubscribedCommands.Remove(StreamingCommandName.News);
                    break;
            }
        }

        return Task.CompletedTask;
    }

    public string? ReadMessage()
    {
        return ReadMessageAsync().GetAwaiter().GetResult();
    }

    private string? _messageToBeReceived;

    public async Task<string?> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        while (_messageToBeReceived is null)
        {
            await Task.Delay(100, cancellationToken);
        }

        var message = _messageToBeReceived;
        _messageToBeReceived = null;
        MessageReceived?.Invoke(this, new MessageEventArgs(message));
        return await Task.FromResult(message);
    }

    public string SendMessageWaitResponse(string message)
    {
        return SendMessageWaitResponseAsync(message).GetAwaiter().GetResult();
    }

    public async Task<string> SendMessageWaitResponseAsync(string message, CancellationToken cancellationToken = default)
    {
        MessageSent?.Invoke(this, new MessageEventArgs(message));

        var data = JsonNode.Parse(message);
        var command = (string?)data["command"];
        var arguments = data["arguments"];

        string result = string.Empty;
        switch (command)
        {
            case PingCommand.Name:
                result = _messageGenerator.GetPingResponse();
                break;

            case ServerTimeCommand.Name:
                result = _messageGenerator.GetServerTimeResponse();
                break;

            case LoginCommand.Name:
                result = _messageGenerator.GetLoginResponse();
                break;

            case LogoutCommand.Name:
                result = _messageGenerator.GetLogoutResponse();
                break;

            case VersionCommand.Name:
                result = _messageGenerator.GetVersionResponse();
                break;

            case CurrentUserDataCommand.Name:
                result = _messageGenerator.GetCurrentUserDataResponse("USD");
                break;

            case CalendarCommand.Name:
                result = _messageGenerator.GetCalendarResponse();
                break;

            case SymbolCommand.Name:
                {
                    var symbol = (string?)arguments["symbol"];
                    result = _messageGenerator.GetSymbolResponse(symbol);
                    break;
                }

            case AllSymbolsCommand.Name:
                result = _messageGenerator.GetAllSymbolsResponse();
                break;

            case TradingHoursCommand.Name:
                {
                    var symbols = arguments["symbols"].AsArray().Select(symbol => symbol.ToString()).ToArray();
                    result = _messageGenerator.GetTradingHoursResponse(symbols);
                    break;
                }
            case ChartLastCommand.Name:
                result = _messageGenerator.GetChartLastResponse();
                break;

            case ChartRangeCommand.Name:
                result = _messageGenerator.GetChartRangeResponse();
                break;

            case ProfitCalculationCommand.Name:
                result = _messageGenerator.GetProfitCalculationResponse(10);
                break;

            case TickPricesCommand.Name:
                {
                    var symbols = arguments["symbols"].AsArray().Select(symbol => symbol.ToString()).ToArray();
                    result = _messageGenerator.GetTickPricesResponse(symbols);
                    break;
                }
            case TradeRecordsCommand.Name:
                {
                    var orders = arguments["orders"].AsArray().Select(o => (long)o).ToArray();
                    result = _messageGenerator.GetTradeRecordsResponse(orders);
                    break;
                }

            case TradesCommand.Name:
                {
                    var symbols = arguments["symbols"].AsArray().Select(symbol => symbol.ToString()).ToArray();
                    result = _messageGenerator.GetTradesResponse(symbols);
                    break;
                }

            case TradesHistoryCommand.Name:
                {
                    var symbols = arguments["symbols"].AsArray().Select(symbol => symbol.ToString()).ToArray();
                    result = _messageGenerator.GetTradesHistoryResponse(symbols);
                    break;
                }

            case TradeTransactionCommand.Name:
                {
                    var order = (long)arguments["order"];
                    result = _messageGenerator.GetTradeTransactionResponse(order);
                    break;
                }

            case TradeTransactionStatusCommand.Name:
                {
                    var order = (long)arguments["order"];
                    result = _messageGenerator.GetTradeTransactionStatusResponse(order);
                    break;
                }

            case CommissionDefCommand.Name:
                {
                    result = _messageGenerator.GetCommissionDefResponse();
                    break;
                }

            case MarginLevelCommand.Name:
                {
                    result = _messageGenerator.GetMarginLevelResponse("USD");
                    break;
                }

            case MarginTradeCommand.Name:
                {
                    result = _messageGenerator.GetMarginTradeResponse();
                    break;
                }

            case NewsCommand.Name:
                {
                    result = _messageGenerator.GetNewsResponse();
                    break;
                }

            default:
                break;
        }

        MessageReceived?.Invoke(this, new MessageEventArgs(result));
        return await Task.FromResult(result);
    }

    public void ComposeFakeStreamingMessage()
    {
        string command = string.Empty;
        switch (command)
        {
            case StreamingCommandName.KeepAlive:
                {
                    _messageGenerator.GetStreamingKeepAliveResponse();
                    break;
                }

            case StreamingCommandName.Balance:
                {
                    _messageGenerator.GetStreamingBalanceResponse();
                    break;
                }

            case StreamingCommandName.Profit:
                {
                    _messageGenerator.GetStreamingProfitsRecord();
                    break;
                }

            case StreamingCommandName.TickPrices:
                {
                    break;
                }

            case StreamingCommandName.Candle:
                {
                    _messageGenerator.GetStreamingCandlesResponse("");
                    break;
                }

            case StreamingCommandName.TradeStatus:
                {
                    _messageGenerator.GetStreamingTradeStatusResponse();
                    break;
                }

            case StreamingCommandName.Trade:
                {
                    _messageGenerator.GetStreamingTradesResponse("");
                    break;
                }

            case StreamingCommandName.News:
                {
                    //_messageGenerator.GetStreamingN("");
                    break;
                }
        }
    }

    public void Disconnect()
    {
        IsConnected = false;
    }

    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        IsConnected = false;
        return Task.CompletedTask;
    }
}
