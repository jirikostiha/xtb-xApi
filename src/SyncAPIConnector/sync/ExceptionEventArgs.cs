using System;

namespace xAPI.Sync
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public bool Handled { get; set; }

        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
            Handled = false;
        }
    }
}