using System;

namespace Weather_App.Services
{
    public class ExceptionApiCall : Exception
    {
        public ExceptionApiCall()
        {
        }

        public ExceptionApiCall(string? message) : base(message)
        {
        }

        public ExceptionApiCall(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
