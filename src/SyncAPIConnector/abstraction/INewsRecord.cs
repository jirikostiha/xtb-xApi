﻿using System;

namespace Xtb.XApi;

public interface INewsRecord
{
    string? Title { get; }

    string? Body { get; }

    string? Key { get; }

    DateTimeOffset? Time { get; }
}