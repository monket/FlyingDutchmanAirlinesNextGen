using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private Customer _testCustomer;
        private FlyingDutchmanAirlinesContextStub _dbContext;
        private CustomerRepository _sut;

        [TestInitialize]
        public async Task TestInitialize()
        {
            var dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman")
                .Options;
            _dbContext = new FlyingDutchmanAirlinesContextStub(dbContextOptions);

            _sut = new CustomerRepository(_dbContext);
            Assert.IsNotNull(_sut);

            _testCustomer = new Customer("Linus Torvalds");
            await _dbContext.Customers.AddAsync(_testCustomer);
            await _dbContext.SaveChangesAsync();
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("\t")]
        public async Task CreateCustomer_Failure_NameIsNullOrWhitespace(string nullOrWhitespace)
        {
            var result = await _sut.CreateCustomer(nullOrWhitespace);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow("!")]
        [DataRow("@")]
        [DataRow("#")]
        [DataRow("$")]
        [DataRow("%")]
        [DataRow("&")]
        [DataRow("*")]
        public async Task CreateCustomer_Failure_NameContainsInvalidCharacters(string invalidCharacter)
        {
            var result = await _sut.CreateCustomer($"Fred{invalidCharacter}Blogs");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Failure_DatabaseAccessError()
        {
            _sut = new CustomerRepository(null);

            var result = await _sut.CreateCustomer("Fred Blogs");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateCustomer_Success()
        {
            var result = await _sut.CreateCustomer("Fred Blogs");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("\t")]
        public async Task GetCustomerByName_Failure_NameIsNullOrWhitespace(string nullOrWhitespace)
        {
            await Assert.ThrowsExceptionAsync<CustomerNotFoundException>(() => _sut.GetCustomerByName(nullOrWhitespace));
        }

        [TestMethod]
        [DataRow("!")]
        [DataRow("@")]
        [DataRow("#")]
        [DataRow("$")]
        [DataRow("%")]
        [DataRow("&")]
        [DataRow("*")]
        public async Task GetCustomerByName_Failure_NameContainsInvalidCharacters(string invalidCharacter)
        {
            await Assert.ThrowsExceptionAsync<CustomerNotFoundException>(() => _sut.GetCustomerByName($"Fred{invalidCharacter}Blogs"));
        }

        [TestMethod]
        public async Task GetCustomerByName_Success()
        {
            var retrievedCustomer = await _sut.GetCustomerByName("Linus Torvalds");
            Assert.AreEqual(_testCustomer.Name, retrievedCustomer.Name);
        }
    }
}
