using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.Logging;
using Microsoft.Extensions.Logging;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class BookingRepository
    {
        private readonly ILogger<BookingRepository> _logger;
        private readonly FlyingDutchmanAirlinesContext _dbContext;
        
        public BookingRepository(FlyingDutchmanAirlinesContext dbContext, ILogger<BookingRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task CreateBooking(int customerId, int flightNumber)
        {
            AssertArgumentsValid(customerId, flightNumber);

            var newBooking = new Booking
            {
                CustomerId = customerId,
                FlightNumber = flightNumber
            };

            try
            {
                await _dbContext.Bookings.AddAsync(newBooking);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.ExceptionWhilstAddingBookingToDatabase(e);
                throw new FailedToAddBookingToDatabaseException();
            }

        }

        private void AssertArgumentsValid(int customerId, int flightNumber)
        {
            if (customerId >= 0 && flightNumber >= 0) return;

            _logger.CreateBookingArgumentException(customerId, flightNumber);
            throw new ArgumentException("Invalid arguments provided");
        }
    }
}