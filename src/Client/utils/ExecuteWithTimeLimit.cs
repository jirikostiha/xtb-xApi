﻿using System;
using System.Threading.Tasks;

namespace Xtb.XApi.Client.Utils;

internal static class ExecuteWithTimeLimit
{
    public static bool Execute(TimeSpan timeSpan, Action codeBlock)
    {
        try
        {
            Task task = Task.Factory.StartNew(() => codeBlock());
            task.Wait(timeSpan);
            return task.IsCompleted;
        }
        catch (AggregateException ae)
        {
            throw ae.InnerExceptions[0];
        }
    }
}