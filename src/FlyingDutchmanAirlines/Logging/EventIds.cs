using Microsoft.Extensions.Logging;

namespace FlyingDutchmanAirlines.Logging
{
    public static class EventIds
    {
        // BookingRepository
        public static EventId CreateBookingArgumentException = new(1000, nameof(CreateBookingArgumentException));
        public static EventId ExceptionWhilstAddingBookingToDatabase = new(1001, nameof(ExceptionWhilstAddingBookingToDatabase));
    }
}