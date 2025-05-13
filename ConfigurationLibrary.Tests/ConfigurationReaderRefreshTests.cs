using System.Reflection;
using ConfigurationLibrary.Models;
using ConfigurationLibrary.MongoDb;
using Moq;

namespace ConfigurationLibrary.Tests;

public class ConfigurationReaderRefreshTests
{
    [Fact]
    public async Task RefreshDataAsync_Should_Update_Cache_With_Latest_Data()
    {
        var mockRepo = new Mock<IMongoRepository>();
        string applicationName = "SERVICE-A";
        mockRepo.Setup(repo => repo.GetActiveItemsByAsync(applicationName))
                .ReturnsAsync(new List<ConfigurationItem>
                {
                    new ConfigurationItem
                    {
                        Name = "RefreshedKey",
                        Value = "456",
                        Type = "string",
                        ApplicationName = "SERVICE-A",
                        IsActive = true
                    }
                });

        var reader = new ConfigurationReader("SERVICE-A", "mongodb://fake", 1000);

        var repoField = typeof(ConfigurationReader)
            .GetField("_repository", BindingFlags.NonPublic | BindingFlags.Instance);
        repoField!.SetValue(reader, mockRepo.Object);

        var method = typeof(ConfigurationReader)
            .GetMethod("RefreshDataAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        var task = (Task)method!.Invoke(reader, null)!;
        await task;

        var cacheField = typeof(ConfigurationReader)
            .GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);
        var cache = (List<ConfigurationItem>)cacheField!.GetValue(reader)!;

        Assert.NotNull(cache);
        Assert.Single(cache);
        Assert.Equal("RefreshedKey", cache[0].Name);
        Assert.Equal("456", cache[0].Value);
    }
}
