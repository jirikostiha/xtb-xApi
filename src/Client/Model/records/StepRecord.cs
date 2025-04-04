﻿using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed record StepRecord : IBaseResponseRecord
{
    public double? FromValue { get; set; }

    public double? Step { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        FromValue = (double?)value["fromValue"];
        Step = (double?)value["step"];
    }
}