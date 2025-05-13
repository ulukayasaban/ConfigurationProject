using ConfigurationLibrary.Models;
using ConfigurationLibrary.MongoDb;
using System.Timers;

namespace ConfigurationLibrary;

public class ConfigurationReader
{
    private readonly string _applicationName;
    private readonly IMongoRepository _repository;
    private readonly System.Timers.Timer _refreshTimer;
    private List<ConfigurationItem> _cache = new();
    public ConfigurationReader(string applicationName, string connectionString, int refreshIntervalMs)
    {
        _applicationName = applicationName;
        _repository = new MongoRepository(connectionString);

        LoadInitialData().Wait(); 

        
        _refreshTimer = new System.Timers.Timer(refreshIntervalMs);
        _refreshTimer.Elapsed += async (sender, args) => await RefreshDataAsync();
        _refreshTimer.AutoReset = true;     
        _refreshTimer.Enabled = true;       
    }

    
    private async Task LoadInitialData()
    {
        try
        {
            _cache = await _repository.GetActiveItemsByAsync(); 
            
        }
        catch(Exception ex)
        {
            
            Console.WriteLine(ex.Message);
        }

    }

    
    private async Task RefreshDataAsync()
    {
        try
        {
            var latestData = await _repository.GetActiveItemsByAsync();
            _cache = latestData;
        }
        catch(Exception ex)
        {
            
            Console.WriteLine(ex.Message);
        }

    }

    
    public T GetValue<T>(string key)
    {
        var item = _cache.FirstOrDefault(x => x.Name == key);

        if (item == null)
        {
            throw new KeyNotFoundException($"'{key}' adında bir konfigürasyon bulunamadı.");
        }

        try
        {
            return (T)Convert.ChangeType(item.Value, typeof(T));
        }
        catch
        {
            throw new InvalidCastException($"Anahtar '{key}' için '{typeof(T).Name}' türüne dönüştürme hatası.");
        }
    }
}
