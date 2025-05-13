using System;
using ConfigurationLibrary.Models;
using MongoDB.Driver;

namespace ConfigurationLibrary.MongoDb;

public class MongoRepository:IMongoRepository
{
    private readonly IMongoCollection<ConfigurationItem> _collection;
     public MongoRepository(string connectionString)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("ConfigurationDb");
        _collection = database.GetCollection<ConfigurationItem>("Configurations");
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<ConfigurationItem>> GetActiveItemsByAsync()
    {
        
        var items = await _collection.Find(x => x.IsActive).ToListAsync();
        //var items = await _collection.Find(_ => true).ToListAsync();

        return items;
    }

    public async Task<ConfigurationItem?> GetByIdAsync(string id)
    {
        try
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        catch
        {
            return null;
        }                       
    }

    public async Task<bool> InsertAsync(ConfigurationItem item)
    {
        try
        {
            await _collection.InsertOneAsync(item);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Insert HATASI] " + ex.Message);
            return false;
        }
    }

    public async Task<bool> UpdateAsync(ConfigurationItem item)
    {
        try
        {
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == item.Id,item);
            if(result == null)
            {
                return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
