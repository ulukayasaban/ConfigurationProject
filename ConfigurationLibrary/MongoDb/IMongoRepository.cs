using System;
using ConfigurationLibrary.Models;

namespace ConfigurationLibrary.MongoDb;

public interface IMongoRepository
{
    Task<List<ConfigurationItem>> GetActiveItemsByAsync();//
    Task<bool> InsertAsync(ConfigurationItem item);//

    Task<bool> UpdateAsync(ConfigurationItem item);//
    Task<ConfigurationItem?> GetByIdAsync(string id);//
    Task<bool> DeleteAsync(string id);//


}
