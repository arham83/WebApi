using System.Globalization;

namespace WebApplication3.GlobalExceptionHandler
{
    //The app exception is a custom exception class used to differentiate between handled and unhandled exceptions in the .NET API
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
