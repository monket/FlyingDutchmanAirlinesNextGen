using System;
using Microsoft.Extensions.Logging;
// ReSharper disable InconsistentNaming

namespace FlyingDutchmanAirlines.Logging
{
    public static class LoggerExtensions
    {
        private static readonly Action<ILogger, int, int, Exception?> _createBookingArgumentException = LoggerMessage.Define<int, int>(LogLevel.Error, EventIds.CreateBookingArgumentException, Messages.CreateBookingArgumentException);
        public static void CreateBookingArgumentException(this ILogger logger, int customerId, int flightNumber)
        {
            _createBookingArgumentException(logger, customerId, flightNumber, null);
        }
        private static readonly Action<ILogger, Exception> _exceptionWhilstAddingBookingToDatabase = LoggerMessage.Define(LogLevel.Error, EventIds.ExceptionWhilstAddingBookingToDatabase, Messages.ExceptionWhilstAddingBookingToDatabase);
        public static void ExceptionWhilstAddingBookingToDatabase(this ILogger logger, Exception e)
        {
            _exceptionWhilstAddingBookingToDatabase(logger, e);
        }



    }
}