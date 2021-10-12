using System;
using System.Linq;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class CustomerRepository
    {
        private readonly FlyingDutchmanAirlinesContext _dbContext;

        public CustomerRepository(FlyingDutchmanAirlinesContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateCustomer(string name)
        {
            if (IsInvalidCustomerName(name))
            {
                return false;
            }

            try
            {
                var newCustomer = new Customer(name);

                await using (_dbContext)
                {
                    _dbContext.Customers.Add(newCustomer);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<Customer> GetCustomerByName(string name)
        {
            if (IsInvalidCustomerName(name))
            {
                throw new CustomerNotFoundException();
            }

            await using (_dbContext)
            {
                return await _dbContext.Customers.
                           FirstOrDefaultAsync(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase))
                       ?? throw new CustomerNotFoundException();
            }

        }

        private bool IsInvalidCustomerName(string name)
        {
            var forbidden = new[] { '!', '@', '#', '$', '%', '&', '*' };
            return string.IsNullOrWhiteSpace(name) || name.Any(c => forbidden.Contains(c));
        }
    }
}