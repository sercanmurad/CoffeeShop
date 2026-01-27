using CoffeShop.DL.Interfaces;
using CoffeShop.Models.Configurations;
using CoffeShop.Models.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CoffeShop.DL.Repositories
{
    public class CustomerMongoRepository : ICustomerRepository
    {
        private readonly IOptionsMonitor<MongoDbConfiguration> _mongoDbConfiguration;
        private readonly ILogger<CustomerMongoRepository> _logger;
        private readonly IMongoCollection<Customer> _customersCollection;

        public CustomerMongoRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoDbConfiguration,
            ILogger<CustomerMongoRepository> logger)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            _logger = logger;

            var client = new MongoClient(_mongoDbConfiguration.CurrentValue.ConnectionString);
            var database = client.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);
            _customersCollection = database.GetCollection<Customer>($"{nameof(Customer)}s");
        }

        public void AddCustomer(Customer customer)
        {
            if (customer == null) return;

            try
            {
                _customersCollection.InsertOne(customer);
            }
            catch (Exception e)
            {
                _logger.LogError("Error adding Customer to DB: {0} - {1}", e.Message, e.StackTrace);
            }
        }

        public void DeleteCustomer(Guid id)
        {
            if (id == Guid.Empty) return;

            try
            {
                var result = _customersCollection.DeleteOne(c => c.Id == id);

                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning($"No customer found with id: {id} to delete.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in method {nameof(DeleteCustomer)}: {e.Message} - {e.StackTrace}");
            }
        }

        public List<Customer> GetAllCustomers()
        {
            try
            {
                return _customersCollection.Find(_ => true).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in method {nameof(GetAllCustomers)}: {e.Message} - {e.StackTrace}");
            }

            return new List<Customer>();
        }

        public Customer? GetById(Guid id)
        {
            if (id == Guid.Empty) return default;

            try
            {
                return _customersCollection.Find(c => c.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in method {nameof(GetById)}: {e.Message} - {e.StackTrace}");
            }

            return default;
        }
    }
}
