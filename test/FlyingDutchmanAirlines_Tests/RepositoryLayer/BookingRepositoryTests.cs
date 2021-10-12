using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class BookingRepositoryTests
    {
        private FlyingDutchmanAirlinesContextStub _dbContext;
        private ILogger<BookingRepository> _logger;
        private BookingRepository _sut;

        [TestInitialize]
        public void TestInitialize()
        {
            _logger = Mock.Of<ILogger<BookingRepository>>();

            var dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
            _dbContext = new FlyingDutchmanAirlinesContextStub(dbContextOptions);

            _sut = new BookingRepository(_dbContext, _logger);
            Assert.IsNotNull(_sut);
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            await _sut.CreateBooking(1, 2);
            var savedBooking = await _dbContext.Bookings.FirstAsync();
            Assert.AreEqual(1, savedBooking.CustomerId);
            Assert.AreEqual(2, savedBooking.FlightNumber);
        }

        [TestMethod]
        [DataRow(-1,0)]
        [DataRow(0,-1)]
        [DataRow(-1,-1)]
        public async Task CreateBooking_Failure_InvalidArguments(int customerId, int flightNumber)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(()=>_sut.CreateBooking(customerId, flightNumber));
        }

        [TestMethod]
        public async Task CreateBooking_Failure_DatabaseError()
        {
            _dbContext.OnSaveChanges = ct => throw new Exception("Database error");
            await Assert.ThrowsExceptionAsync<FailedToAddBookingToDatabaseException>(()=>_sut.CreateBooking(1, 1));
        }
    }
}