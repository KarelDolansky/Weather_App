using System;

namespace Weather_App.Services
{
    public class ExceptionBadRequest : Exception
    {
        public ExceptionBadRequest()
        {
        }

        public ExceptionBadRequest(string? message) : base(message)
        {
        }

        public ExceptionBadRequest(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
