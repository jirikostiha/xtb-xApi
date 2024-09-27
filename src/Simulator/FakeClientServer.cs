using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Commands;

namespace Xtb.XApi.Simulation;

/// <summary>
/// Fake client responding as server.
/// </summary>
public class FakeClientServer : IClient
{
    private readonly JsonMessageGenerator _messageGenerator;

    public FakeClientServer()
    {
        _messageGenerator = new JsonMessageGenerator();
    }

    public event EventHandler<EndpointEventArgs>? Connected;
    public event EventHandler<MessageEventArgs>? MessageSent;
    public event EventHandler<MessageEventArgs>? MessageReceived;
    public event EventHandler? Disconnected;

    public IPEndPoint Endpoint { get; set; }

    public bool IsConnected { get; set; }

    public Dictionary<string, string> AllSymbols { get; private set; } = [];
    public Dictionary<long, object> AllTrades { get; private set; } = [];
    public Dictionary<long, object> TradesHistory { get; private set; } = [];

    public void Connect()
    {
        ConnectAsync().GetAwaiter().GetResult();
    }

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        IsConnected = true;
        return Task.CompletedTask;
    }

    public void SendMessage(string message)
    {
        SendMessageAsync(message).GetAwaiter().GetResult();
    }

    public Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        MessageSent?.Invoke(this, new MessageEventArgs(message));
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
                result = _messageGenerator.GetTradesResponse();
                break;

            case TradesHistoryCommand.Name:
                result = _messageGenerator.GetTradesHistoryResponse();
                break;

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
                    result = _messageGenerator.GetMarginLevelResponse();
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

    public void Disconnect()
    {
        IsConnected = false;
    }
}
