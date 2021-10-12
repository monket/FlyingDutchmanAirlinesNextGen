namespace FlyingDutchmanAirlines.Logging
{
    public class Messages
    {
        // BookingRepository
        public static readonly string CreateBookingArgumentException = "Argument exception in CreateBooking: customerId {customerId}, flightNumber {flightNumber}";
        public static readonly string ExceptionWhilstAddingBookingToDatabase = "Exception whilst adding Booking to database.  See inner exception for details";
    }
}