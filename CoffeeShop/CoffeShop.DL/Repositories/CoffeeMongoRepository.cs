using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeShop.DL.Interfaces;
using CoffeShop.Models.Configurations;
using CoffeShop.Models.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CoffeShop.DL.Repositories
{
    internal class CoffeeMongoRepository : ICoffeeRepository
    {
        private readonly IOptionsMonitor<MongoDbConfiguration> _mongoDbConfiguration;
        private readonly ILogger<CoffeeMongoRepository> _logger;
        private readonly IMongoCollection<Coffee> _coffeesCollection;

        public CoffeeMongoRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoDbConfiguration,
            ILogger<CoffeeMongoRepository> logger)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            _logger = logger;

            var client = new MongoClient(_mongoDbConfiguration.CurrentValue.ConnectionString);
            var database = client.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);
            _coffeesCollection = database.GetCollection<Coffee>($"{nameof(Coffee)}s");
        }

        public async Task AddCoffeeAsync(Coffee coffee)
        {
            if (coffee == null) return;

            try
            {
                await _coffeesCollection.InsertOneAsync(coffee);
            }
            catch (Exception e)
            {
                _logger.LogError("Error adding coffee to the DB:{0}-{1}", e.Message, e.StackTrace);
            }
        }

        public async Task DeleteCoffeeAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty) return;

            try
            {
                var result = await _coffeesCollection.DeleteOneAsync(c => c.Id == id);

                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning($"No coffee found with Id: {id} to delete.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in method {nameof(DeleteCoffeeAsync)}:{e.Message}-{e.StackTrace}");
            }
        }

        public async Task<List<Coffee>> GetAllCoffeesAsync()
        {
            return await _coffeesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Coffee?> GetByIdAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty) return default;

            try
            {
                return await _coffeesCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in method {nameof(GetByIdAsync)}:{e.Message}-{e.StackTrace}");
            }

            return default;
        }
    }
}
